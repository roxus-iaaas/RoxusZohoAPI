using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using RoxusZohoAPI.Repositories;
using RoxusZohoAPI.Services.Zoho;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Threading;

namespace RoxusWebAPI.Services.Zoho.ZohoProjects
{
    public class ZohoProjectService : IZohoProjectService
    {
        private readonly IRoxusLoggingRepository _roxusRepository;
        private readonly IZohoAuthService _zohoAuthService;

        public const string GetProjectUrl = "https://projectsapi.zoho.eu/restapi/portal/20069538644/projects/{projectId}/";
        public const string GetTaskUrl = "https://projectsapi.zoho.eu/restapi/portal/20069538644/projects/{projectId}/tasks/{taskId}/";

        private readonly HttpClient _httpClient = new HttpClient(
         new HttpClientHandler()
         {
             AutomaticDecompression = DecompressionMethods.GZip
         });

        public string[] CTV_VALID_STATUS = { "Not Started", "Letter 1", "Letter 2", "Letter 3" };

        public ZohoProjectService(IRoxusLoggingRepository roxusRepository, IZohoAuthService zohoAuthService)
        {
            _roxusRepository = roxusRepository;
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<SearchProjectResponse>> SearchProjectInPortal(string apiKey, string searchTerm)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<SearchProjectResponse>()
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

                string encodedSearchTerm = HttpUtility.UrlEncode(searchTerm);

                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/search?search_term={encodedSearchTerm}&module=projects";

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
                        var responseObj = JsonConvert.DeserializeObject<SearchProjectResponse>(responseData);

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
                        ApiName = ZohoConstants.ZPRJ_SEARCH_PROJECTS_IN_PORTAL,
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
                                ApiName = ZohoConstants.ZPRJ_SEARCH_PROJECTS_IN_PORTAL,
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

        public async Task<ApiResultDto<GetAllProjectsInPortalResponse>> GetAllProjectsInPortal(string apiKey)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<GetAllProjectsInPortalResponse>()
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

                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/";

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
                        var responseObj = JsonConvert.DeserializeObject<GetAllProjectsInPortalResponse>(responseData);

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
                        ApiName = ZohoConstants.ZPRJ_SEARCH_PROJECTS_IN_PORTAL,
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
                                ApiName = ZohoConstants.ZPRJ_SEARCH_PROJECTS_IN_PORTAL,
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

        public async Task<ApiResultDto<CreateProjectResponse>> CreateProject(string apiKey, ProjectForCreation projectForCreation)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CreateProjectResponse>()
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

                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/";

                // Handle Endpoint for Project
                var builder = new UriBuilder(endpoint);
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["name"] = projectForCreation.name;
                query["description"] = projectForCreation.description;
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
                        var responseObj = JsonConvert.DeserializeObject<CreateProjectResponse>(responseData);

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
                        ApiName = ZohoConstants.ZPRJ_CREATE_PROJECT,
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
                                ApiName = ZohoConstants.ZPRJ_CREATE_PROJECT,
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

        public async Task<TaskModel> GetTaskDetails(string apiKey, string taskId, string projectId)
        {
            try
            {
                var appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                using (var httpClient = new HttpClient())
                {
                    string endpoint = GetTaskUrl.Replace("{projectId}", projectId).Replace("{taskId}", taskId);
                    var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                    using (var response = await httpClient.SendAsync(request,
                        HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        var stream = await response.Content.ReadAsStreamAsync();
                        // Convert stream to string
                        var reader = new StreamReader(stream);
                        string responseString = reader.ReadToEnd();
                        var responseContent = JsonConvert.DeserializeObject<TaskModel>(responseString);
                        return responseContent;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ProjectModel> GetProjectDetails(string apiKey, string projectId)
        {
            try
            {
                var appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                using (var httpClient = new HttpClient())
                {
                    string endpoint = GetProjectUrl.Replace("{projectId}", projectId);
                    var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                    using (var response = await httpClient.SendAsync(request,
                        HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        var stream = await response.Content.ReadAsStreamAsync();
                        // Convert stream to string
                        var reader = new StreamReader(stream);
                        string responseString = reader.ReadToEnd();
                        var responseContent = JsonConvert.DeserializeObject<ProjectModel>(responseString);
                        return responseContent;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApiResultDto<string>> InsertTaskToTable(string apiKey, string taskId, string projectId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_ITD_ERROR
            };
            try
            {
                #region STEP 1: Get Project Details
                var projectModels = await GetProjectDetails(apiKey, projectId);
                Thread.Sleep(1000);
                if (projectModels == null)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_ITD_P01;
                    return apiResult;
                }
                var projectModel = projectModels.projects[0];
                #endregion

                #region STEP 2: Get Task Details
                var taskModels = await GetTaskDetails(apiKey, taskId, projectId);
                Thread.Sleep(1000);
                if (taskModels == null)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_ITD_T01;
                    return apiResult;
                }
                var taskModel = taskModels.tasks[0];
                #endregion

                #region STEP 3: Convert Task Model to Task Entity

                TrenchesTask taskEntity = null;
                bool isUpdate = true;
                taskEntity = await _roxusRepository.GetTrenchesTaskByTaskIdAndProjectId(taskId, projectId);

                if (taskEntity == null)
                {
                    taskEntity = new TrenchesTask();
                    taskEntity.CreatedTime = DateTime.UtcNow;
                    isUpdate = false;
                }

                if (taskEntity.CreatedTime == null)
                {
                    taskEntity.CreatedTime = DateTime.UtcNow;
                }
                taskEntity.TaskId = taskId;
                taskEntity.TaskName = taskModel.name;
                taskEntity.ProjectId = projectId;
                taskEntity.ProjectName = projectModel.name;
                taskEntity.TaskListId = taskModel.tasklist.id;
                taskEntity.TaskListName = taskModel.tasklist.name;
                taskEntity.Owner = taskModel.details.owners[0].name;
                taskEntity.Status = taskModel.status.name;
                taskEntity.LastUpdatedTime = DateTime.UtcNow;

                if (taskModel.start_date_long != null)
                {
                    double startDateLong = Convert.ToDouble(taskModel.start_date_long);
                    taskEntity.StartTime = DateTimeHelpers.UnixTimestampToDateTime(startDateLong, false);
                }

                if (taskModel.end_date_long != null)
                {
                    double dueTimeLong = Convert.ToDouble(taskModel.end_date_long);
                    taskEntity.DueTime = DateTimeHelpers.UnixTimestampToDateTime(dueTimeLong, false);
                }

                if (taskModel.created_time_long != null)
                {
                    double createdTimeLong = Convert.ToDouble(taskModel.created_time_long);
                    taskEntity.ZohoCreatedTime = DateTimeHelpers.UnixTimestampToDateTime(createdTimeLong, false);
                }

                if (taskModel.last_updated_time_long != null)
                {
                    double modifiedTimeLong = Convert.ToDouble(taskModel.last_updated_time_long);
                    taskEntity.ZohoModifiedTime = DateTimeHelpers.UnixTimestampToDateTime(modifiedTimeLong, false);
                }

                foreach (var item in taskModel.custom_fields)
                {
                    switch (item.label_name)
                    {
                        case "Title Number":
                            taskEntity.TitleNumber = item.value;
                            break;
                        case "Title_Type":
                            taskEntity.TitleType = item.value;
                            break;
                        case "CRM Title":
                            taskEntity.CRMTitle = item.value;
                            break;
                        case "Wayleave Template":
                            taskEntity.WayleaveTemplate = item.value;
                            break;
                        case "Letter 1 Date":
                            taskEntity.Letter1Date = item.value;
                            break;
                        case "Letter 1 Status":
                            taskEntity.Letter1Status = item.value;
                            break;
                        case "Letter 2 Date":
                            taskEntity.Letter2Date = item.value;
                            break;
                        case "Letter 2 Status":
                            taskEntity.Letter2Status = item.value;
                            break;
                        case "Letter 3 Date":
                            taskEntity.Letter3Date = item.value;
                            break;
                        case "Letter 3 Status":
                            taskEntity.Letter3Status = item.value;
                            break;
                        case "Letter 4 Date":
                            taskEntity.Letter4Date = item.value;
                            break;
                        case "Letter 4 Status":
                            taskEntity.Letter4Status = item.value;
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region STEP 4: Insert data to SQL
                await _roxusRepository.UpsertTrenchesTaskToSQL(taskEntity, isUpdate);
                #endregion

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_ITD_SUCCESS;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message}\n{ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> DeleteTaskFromTable(string apiKey, string taskId, string projectId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_DTD_ERROR
            };

            try
            {
                #region STEP 1: Get task from SQL
                var taskEntity = await _roxusRepository.GetTrenchesTaskByTaskIdAndProjectId(taskId, projectId);
                #endregion

                #region STEP 2: Delete task from SQL
                if (taskEntity != null)
                {
                    await _roxusRepository.DeleteTaskFromDb(taskEntity);
                }
                #endregion

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_DTD_SUCCESS;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message}\n{ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<CheckTaskValidResponse>> CheckTaskValid(string apiKey, string taskId, string projectId)
        {
            var apiResult = new ApiResultDto<CheckTaskValidResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_ITD_ERROR
            };
            var taskValid = new CheckTaskValidResponse
            {
                IsValid = false
            };
            apiResult.Data = taskValid;

            try
            {

                #region STEP 1: Get Project Details
                var projectModels = await GetProjectDetails(apiKey, projectId);
                if (projectModels == null)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_ITD_P01;
                    return apiResult;
                }
                var projectModel = projectModels.projects[0];
                #endregion

                #region STEP 2: Get Task Details
                var taskModels = await GetTaskDetails(apiKey, taskId, projectId);
                if (taskModels == null)
                {
                    // DELETE TASK FROM TABLE
                    var taskInDb = await _roxusRepository.GetTrenchesTaskByTaskIdAndProjectId(taskId, projectId);
                    if (taskInDb != null)
                    {
                        await _roxusRepository.DeleteTaskFromDb(taskInDb);
                    }
                    apiResult.Message = ZohoConstants.CUSTOM_ITD_T01;
                    return apiResult;
                }
                var taskModel = taskModels.tasks[0];
                #endregion

                #region STEP 3: Convert Task Model to Task Entity
                TrenchesTask taskEntity = null;
                bool isUpdate = true;
                taskEntity = await _roxusRepository.GetTrenchesTaskByTaskIdAndProjectId(taskId, projectId);

                if (taskEntity == null)
                {
                    taskEntity = new TrenchesTask();
                    taskEntity.CreatedTime = DateTime.UtcNow;
                    isUpdate = false;
                }

                if (taskEntity.CreatedTime == null)
                {
                    taskEntity.CreatedTime = DateTime.UtcNow;
                }
                taskEntity.TaskId = taskId;
                taskEntity.TaskName = taskModel.name;
                taskEntity.ProjectId = projectId;
                taskEntity.ProjectName = projectModel.name;
                taskEntity.TaskListId = taskModel.tasklist.id;
                taskEntity.TaskListName = taskModel.tasklist.name;
                taskEntity.Owner = taskModel.details.owners[0].name;
                taskEntity.Status = taskModel.status.name;
                taskEntity.LastUpdatedTime = DateTime.UtcNow;

                if (taskModel.start_date_long != null)
                {
                    double startDateLong = Convert.ToDouble(taskModel.start_date_long);
                    taskEntity.StartTime = DateTimeHelpers.UnixTimestampToDateTime(startDateLong, false);
                }

                if (taskModel.end_date_long != null)
                {
                    double dueTimeLong = Convert.ToDouble(taskModel.end_date_long);
                    taskEntity.DueTime = DateTimeHelpers.UnixTimestampToDateTime(dueTimeLong, false);
                }

                if (taskModel.created_time_long != null)
                {
                    double createdTimeLong = Convert.ToDouble(taskModel.created_time_long);
                    taskEntity.ZohoCreatedTime = DateTimeHelpers.UnixTimestampToDateTime(createdTimeLong, false);
                }

                if (taskModel.last_updated_time_long != null)
                {
                    double modifiedTimeLong = Convert.ToDouble(taskModel.end_date_long);
                    taskEntity.ZohoModifiedTime = DateTimeHelpers.UnixTimestampToDateTime(modifiedTimeLong, false);
                }

                foreach (var item in taskModel.custom_fields)
                {
                    switch (item.label_name)
                    {
                        case "Title Number":
                            taskEntity.TitleNumber = item.value;
                            break;
                        case "Title_Type":
                            taskEntity.TitleType = item.value;
                            break;
                        case "CRM Title":
                            taskEntity.CRMTitle = item.value;
                            break;
                        case "Wayleave Template":
                            taskEntity.WayleaveTemplate = item.value;
                            break;
                        case "Letter 1 Date":
                            taskEntity.Letter1Date = item.value;
                            break;
                        case "Letter 1 Status":
                            taskEntity.Letter1Status = item.value;
                            break;
                        case "Letter 2 Date":
                            taskEntity.Letter2Date = item.value;
                            break;
                        case "Letter 2 Status":
                            taskEntity.Letter2Status = item.value;
                            break;
                        case "Letter 3 Date":
                            taskEntity.Letter3Date = item.value;
                            break;
                        case "Letter 3 Status":
                            taskEntity.Letter3Status = item.value;
                            break;
                        case "Letter 4 Date":
                            taskEntity.Letter4Date = item.value;
                            break;
                        case "Letter 4 Status":
                            taskEntity.Letter4Status = item.value;
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region STEP 4: Insert data to SQL
                await _roxusRepository.UpsertTrenchesTaskToSQL(taskEntity, isUpdate);
                #endregion

                #region STEP 5: Handle check task valid for send letter
                string taskName = taskEntity.TaskName;
                if (!taskName.StartsWith("[Automation]") && !taskName.StartsWith("[PIA]"))
                {
                    apiResult.Message = ZohoConstants.CUSTOM_CTV_T01;
                    return apiResult;
                }

                if (taskModel.end_date_long == null)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_CTV_D01;
                    return apiResult;
                }

                if (DateTime.UtcNow <= taskEntity.DueTime)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_CTV_D02;
                    return apiResult;
                }

                string taskStatus = taskEntity.Status;
                if (!CTV_VALID_STATUS.Contains(taskStatus))
                {
                    apiResult.Message = ZohoConstants.CUSTOM_CTV_S01;
                    return apiResult;
                }

                if (taskName.StartsWith("[PIA]") && taskStatus == "Letter 3")
                {
                    apiResult.Message = ZohoConstants.CUSTOM_CTV_PIAL3;
                    return apiResult;
                }

                switch (taskStatus)
                {
                    case "Not Started":
                        taskValid.NextAction = "Send Letter 1";
                        taskValid.IsValid = true;
                        apiResult.Data = taskValid;
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.CUSTOM_CTV_SUCCESS;
                        return apiResult;
                    case "Letter 1":
                        if (string.IsNullOrEmpty(taskEntity.Letter1Date) || string.IsNullOrEmpty(taskEntity.Letter1Status))
                        {
                            apiResult.Message = ZohoConstants.CUSTOM_CTV_CSL2;
                            return apiResult;
                        }
                        taskValid.NextAction = "Send Letter 2";
                        taskValid.IsValid = true;
                        apiResult.Data = taskValid;
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.CUSTOM_CTV_SUCCESS;
                        return apiResult;
                    case "Letter 2":
                        if (string.IsNullOrEmpty(taskEntity.Letter1Date) || string.IsNullOrEmpty(taskEntity.Letter1Status)
                            || string.IsNullOrEmpty(taskEntity.Letter2Date) || string.IsNullOrEmpty(taskEntity.Letter2Status))
                        {
                            apiResult.Message = ZohoConstants.CUSTOM_CTV_CSL3;
                            return apiResult;
                        }
                        taskValid.NextAction = "Send Letter 3";
                        taskValid.IsValid = true;
                        apiResult.Data = taskValid;
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.CUSTOM_CTV_SUCCESS;
                        return apiResult;
                    case "Letter 3":
                        if (string.IsNullOrEmpty(taskEntity.Letter1Date) || string.IsNullOrEmpty(taskEntity.Letter1Status)
                            || string.IsNullOrEmpty(taskEntity.Letter2Date) || string.IsNullOrEmpty(taskEntity.Letter2Status)
                            || string.IsNullOrEmpty(taskEntity.Letter3Date) || string.IsNullOrEmpty(taskEntity.Letter3Status))
                        {
                            apiResult.Message = ZohoConstants.CUSTOM_CTV_CSL4;
                            return apiResult;
                        }
                        taskValid.NextAction = "Send Letter 4";
                        taskValid.IsValid = true;
                        apiResult.Data = taskValid;
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.CUSTOM_CTV_SUCCESS;
                        return apiResult;
                    default:
                        break;
                }
                #endregion

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message}\n{ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<AddEventResponse>> AddEvent(string apiKey, string projectId, AddEventRequest addEventRequest)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<AddEventResponse>()
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

                endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/events/";

                // Handle Endpoint for Project
                var builder = new UriBuilder(endpoint);
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["title"] = addEventRequest.title;
                query["date"] = addEventRequest.date;
                query["hour"] = addEventRequest.hour;
                query["minutes"] = addEventRequest.minutes;
                query["ampm"] = addEventRequest.ampm;
                query["duration_hour"] = addEventRequest.duration_hour;
                query["duration_mins"] = addEventRequest.duration_mins;
                query["participants"] = addEventRequest.participants;
                // query["repeat"] = addEventRequest.repeat;

                builder.Query = query.ToString();
                string url = builder.ToString();

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           url);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    // response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        var responseObj = JsonConvert.DeserializeObject<AddEventResponse>(responseData);

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
                        ApiName = ZohoConstants.ZPRJ_CREATE_PROJECT,
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
                                ApiName = ZohoConstants.ZPRJ_CREATE_PROJECT,
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
    }
}
