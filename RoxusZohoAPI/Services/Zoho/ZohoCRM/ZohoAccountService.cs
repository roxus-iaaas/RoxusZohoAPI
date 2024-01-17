using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using RoxusZohoAPI.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public class ZohoAccountService : IZohoAccountService
    {
        private readonly IZohoAuthService _zohoAuthService;
        private readonly IRoxusLoggingRepository _roxusRepository;
        private const string AccountModule = "Accounts";

        private readonly HttpClient _httpClient = new HttpClient(
         new HttpClientHandler()
         {
             AutomaticDecompression = DecompressionMethods.GZip
         });

        public ZohoAccountService(IZohoAuthService zohoAuthService, IRoxusLoggingRepository roxusRepository)
        {
            _zohoAuthService = zohoAuthService;
            _roxusRepository = roxusRepository;
            _httpClient.Timeout = new TimeSpan(0, 0, 1800);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<ApiResultDto<GetResponse<AccountResponse>>> GetAccounts(string apiKey, string sortCriteria = "")
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<GetResponse<AccountResponse>>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                if (string.IsNullOrEmpty(sortCriteria))
                {
                    sortCriteria = "sort_by=Account_Name&sort_order=asc";
                }
                
                endpoint = $"{appConfig.EndPoint}/{AccountModule}?{sortCriteria}";

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
                        var responseObj = JsonConvert.DeserializeObject<GetResponse<AccountResponse>>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_204;
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
                        ApiName = ZohoConstants.ZCRM_GET_ALL_ACCOUNTS,
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
                                ApiName = ZohoConstants.ZCRM_GET_ALL_ACCOUNTS,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<AccountResponse>> GetAccountById(string apiKey, string accountId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<AccountResponse>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/{AccountModule}/{accountId}";

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
                    var reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonData = JObject.Parse(responseData);
                        var responseObj = JsonConvert.DeserializeObject<AccountResponse>(jsonData["data"][0].ToString());

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_204;
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
                        ApiName = ZohoConstants.ZCRM_GET_ACCOUNT_BY_ID,
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
                        using (var reader = new StreamReader(responseStream))
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
                                ApiName = ZohoConstants.ZCRM_GET_ACCOUNT_BY_ID,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<SearchResponse<AccountResponse>>> SearchAccountByCompanyRegistration(string apiKey, string companyNumber)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<SearchResponse<AccountResponse>>()
            {
                Message = ZohoConstants.MSG_400,
                Code = ResultCode.BadRequest
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

                string encodedCompanyNumber = HttpUtility.UrlEncode(companyNumber);
                endpoint = $"{appConfig.EndPoint}/{AccountModule}/search?criteria=(Company_Registration:equals:{encodedCompanyNumber})";

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
                    var reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<SearchResponse<AccountResponse>>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_204;
                    }
                    return apiResult;
                }
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
            }
        }

        public async Task<ApiResultDto<SearchResponse<AccountResponse>>> SearchAccountByAccountName(string apiKey, string companyName)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<SearchResponse<AccountResponse>>()
            {
                Message = ZohoConstants.MSG_400,
                Code = ResultCode.BadRequest
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

                string encodedCompanyName = HttpUtility.UrlEncode(companyName);
                encodedCompanyName = encodedCompanyName.Replace("(", "\\(").Replace(")", "\\)");
                endpoint = $"{appConfig.EndPoint}/{AccountModule}/search?criteria=(Account_Name:equals:{encodedCompanyName})";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SearchResponse<AccountResponse>>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<SearchResponse<AccountResponse>>> SearchAccountByCOQL(string apiKey, CoqlRequest coqlRequest)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<SearchResponse<AccountResponse>>()
            {
                Message = ZohoConstants.MSG_400,
                Code = ResultCode.BadRequest
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

                endpoint = $"{appConfig.EndPoint}/coql";
                string requestBody = JsonConvert.SerializeObject(coqlRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SearchResponse<AccountResponse>>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateAccount(string apiKey, UpsertRequest<AccountForCreation> accountRequest)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                endpoint = $"{appConfig.EndPoint}/{AccountModule}";
                string requestBody = JsonConvert.SerializeObject(accountRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }

        }

        public async Task<ApiResultDto<UploadResponse>> Account_UploadAttachments(string apiKey, string accountId, string fileName, string fileContent)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UploadResponse>()
            {
                Code = ResultCode.OK,
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
                endpoint = $"{appConfig.EndPoint}/{AccountModule}/{accountId}/Attachments";
                byte[] fileBytes = Convert.FromBase64String(fileContent);
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), "file", fileName);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");
                using var response = await httpClient.PostAsync(endpoint, form);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<UploadResponse>(responseData);

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
                    HttpMethod = "POST",
                    ApiName = ZohoConstants.ZCRM_ACCOUNT_UPLOAD_ATTACHMENT,
                    Endpoint = endpoint
                };
                return apiResult;

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
                                ApiName = ZohoConstants.ZCRM_ACCOUNT_UPLOAD_ATTACHMENT,
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

        public async Task<ApiResultDto<UpdateResponse>> UpdateAccount(string apiKey, string accountId, AccountForUpdate accountForUpdate)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpdateResponse>()
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

                endpoint = $"{appConfig.EndPoint}/{AccountModule}/{accountId}";
                var updateRequest = new UpdateRequest();
                updateRequest.data.Add(accountForUpdate);

                string requestBody = JsonConvert.SerializeObject(updateRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    // response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<UpdateResponse>(responseData);
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
                        HttpMethod = "PUT",
                        ApiName = ZohoConstants.ZCRM_UPDATE_CONTACT,
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
                                HttpMethod = "PUT",
                                ApiName = ZohoConstants.ZCRM_UPDATE_CONTACT,
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

        public async Task<ApiResultDto<Account_RelatedContacts>> Account_GetRelatedContacts(string apiKey, string accountId)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<Account_RelatedContacts>()
            {
                Message = ZohoConstants.MSG_400,
                Code = ResultCode.BadRequest
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
                
                endpoint = $"{appConfig.EndPoint}/{AccountModule}/{accountId}/Contacts";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<Account_RelatedContacts>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
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
