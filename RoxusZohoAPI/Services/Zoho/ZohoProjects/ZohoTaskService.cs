using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using RoxusZohoAPI.Repositories;
using RoxusZohoAPI.Services.Zoho;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace RoxusWebAPI.Services.Zoho.ZohoProjects
{
    public class ZohoTaskService : IZohoTaskService
    {
        private readonly IRoxusLoggingRepository _roxusRepository;
        private readonly IZohoAuthService _zohoAuthService;

        private readonly HttpClient _httpClient = new HttpClient(
         new HttpClientHandler()
         {
             AutomaticDecompression = DecompressionMethods.GZip
         });

        public ZohoTaskService(IRoxusLoggingRepository roxusRepository, IZohoAuthService zohoAuthService)
        {
            _roxusRepository = roxusRepository;
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<CreateTaskResponse>> CreateTask(string apiKey, string projectId, TaskForCreation taskForCreation)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CreateTaskResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/tasks/";

                // Handle Endpoint for Task
                var builder = new UriBuilder(endpoint);
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["name"] = taskForCreation.name;
                if (!string.IsNullOrWhiteSpace(taskForCreation.tasklist_id))
                {
                    query["tasklist_id"] = taskForCreation.tasklist_id;
                }
                query["custom_fields"] = taskForCreation.CreateRequestString();
                
                builder.Query = query.ToString();
                string url = builder.ToString();

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           url);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        var responseObj = JsonConvert.DeserializeObject<CreateTaskResponse>(responseData);

                        apiResult.Code = ResultCode.Created;
                        apiResult.Message = ZohoConstants.MSG_201;
                        apiResult.Data = responseObj;
                    }

                    // HANDLE LOGGING TO DATABASE
                    apiLogging = new ApiLogging()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Response = responseData,
                        ApplicationName = appConfig.Platform,
                        CustomerName = appConfig.CustomerName,
                        ApplicationId = appConfig.Id,
                        Status = (int)response.StatusCode + " " + response.StatusCode,
                        CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                        HttpMethod = "POST",
                        ApiName = ZohoConstants.ZPRJ_CREATE_TASK,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "POST",
                                ApiName = ZohoConstants.ZPRJ_CREATE_TASK,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<GetAllTasksResponse>> GetAllTasksInProject(string apiKey, string projectId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<GetAllTasksResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/tasks/";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<GetAllTasksResponse>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_200;
                    }

                    // HANDLE LOGGING TO DATABASE
                    apiLogging = new ApiLogging()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Response = responseData,
                        ApplicationName = appConfig.Platform,
                        CustomerName = appConfig.CustomerName,
                        ApplicationId = appConfig.Id,
                        Status = (int)response.StatusCode + " " + response.StatusCode,
                        CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                        HttpMethod = "GET",
                        ApiName = ZohoConstants.ZPRJ_GET_ALL_TASKS_IN_PROJECT,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "GET",
                                ApiName = ZohoConstants.ZPRJ_GET_ALL_TASKS_IN_PROJECT,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<TaskDetail>> GetTaskDetails(string apiKey, string projectId, string taskId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<TaskDetail>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/tasks/{taskId}/";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonResponse = JObject.Parse(responseData);
                        var responseObj = JsonConvert.DeserializeObject<TaskDetail>(jsonResponse["tasks"][0].ToString());

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_200;
                    }

                    // HANDLE LOGGING TO DATABASE
                    apiLogging = new ApiLogging()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Response = responseData,
                        ApplicationName = appConfig.Platform,
                        CustomerName = appConfig.CustomerName,
                        ApplicationId = appConfig.Id,
                        Status = (int)response.StatusCode + " " + response.StatusCode,
                        CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                        HttpMethod = "GET",
                        ApiName = ZohoConstants.ZPRJ_GET_TASK_DETAILS,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "GET",
                                ApiName = ZohoConstants.ZPRJ_GET_TASK_DETAILS,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<CreateTaskResponse>> UpdateTaskForTrenches(
            string apiKey, string projectId, string taskId, TaskForUpdation taskForUpdation)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CreateTaskResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/tasks/{taskId}/";

                // Handle Endpoint for Task
                var builder = new UriBuilder(endpoint);
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["custom_fields"] = taskForUpdation.CreateRequestString();
                if (!string.IsNullOrEmpty(taskForUpdation.end_date))
                {
                    query["end_date"] = taskForUpdation.end_date;
                }
                if (!string.IsNullOrEmpty(taskForUpdation.custom_status))
                {
                    query["custom_status"] = taskForUpdation.custom_status;
                }

                builder.Query = query.ToString();
                string url = builder.ToString();

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           url);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<CreateTaskResponse>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    // HANDLE LOGGING TO DATABASE
                    apiLogging = new ApiLogging()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Response = responseData,
                        ApplicationName = appConfig.Platform,
                        CustomerName = appConfig.CustomerName,
                        ApplicationId = appConfig.Id,
                        Status = (int)response.StatusCode + " " + response.StatusCode,
                        CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                        HttpMethod = "POST",
                        ApiName = ZohoConstants.ZPRJ_UPDATE_TASK,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "POST",
                                ApiName = ZohoConstants.ZPRJ_UPDATE_TASK,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<CreateTaskResponse>> UpdateTaskForHinets(string apiKey, string projectId, string taskId, TaskForUpdation taskForUpdation)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CreateTaskResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/tasks/{taskId}/";

                // Handle Endpoint for Task
                var builder = new UriBuilder(endpoint);
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                if (!string.IsNullOrEmpty(taskForUpdation.start_date))
                {
                    query["start_date"] = taskForUpdation.start_date;
                }
                if (!string.IsNullOrEmpty(taskForUpdation.end_date))
                {
                    query["end_date"] = taskForUpdation.end_date;
                }

                builder.Query = query.ToString();
                string url = builder.ToString();

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           url);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<CreateTaskResponse>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    // HANDLE LOGGING TO DATABASE
                    apiLogging = new ApiLogging()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Response = responseData,
                        ApplicationName = appConfig.Platform,
                        CustomerName = appConfig.CustomerName,
                        ApplicationId = appConfig.Id,
                        Status = (int)response.StatusCode + " " + response.StatusCode,
                        CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                        HttpMethod = "POST",
                        ApiName = ZohoConstants.ZPRJ_UPDATE_TASK,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "POST",
                                ApiName = ZohoConstants.ZPRJ_UPDATE_TASK,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<string>> DeleteTask(string apiKey, string projectId, string taskId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/tasks/{taskId}/";

                var request = new HttpRequestMessage(
                           HttpMethod.Delete,
                           endpoint);
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                    }

                    // HANDLE LOGGING TO DATABASE
                    apiLogging = new ApiLogging()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Response = responseData,
                        ApplicationName = appConfig.Platform,
                        CustomerName = appConfig.CustomerName,
                        ApplicationId = appConfig.Id,
                        Status = (int)response.StatusCode + " " + response.StatusCode,
                        CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                        HttpMethod = "DELETE",
                        ApiName = ZohoConstants.ZPRJ_DELETE_TASK,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "DELETE",
                                ApiName = ZohoConstants.ZPRJ_DELETE_TASK,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<SearchTasksResponse>> SearchTasksInProjects(string apiKey, string projectId, string searchTerm)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<SearchTasksResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/search?search_term={searchTerm}&module=tasks";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<SearchTasksResponse>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_200;
                    }

                    // HANDLE LOGGING TO DATABASE
                    apiLogging = new ApiLogging()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Response = responseData,
                        ApplicationName = appConfig.Platform,
                        CustomerName = appConfig.CustomerName,
                        ApplicationId = appConfig.Id,
                        Status = (int)response.StatusCode + " " + response.StatusCode,
                        CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                        HttpMethod = "GET",
                        ApiName = ZohoConstants.ZPRJ_SEARCH_TASKS_IN_PROJECT,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "GET",
                                ApiName = ZohoConstants.ZPRJ_SEARCH_TASKS_IN_PROJECT,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }
    }
}
