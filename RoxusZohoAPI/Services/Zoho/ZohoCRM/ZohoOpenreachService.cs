using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
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
    public class ZohoOpenreachService : IZohoOpenreachService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public ZohoOpenreachService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<OpenreachResponse>> GetOpenreachById(string apiKey, string openreachId)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<OpenreachResponse>()
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

                endpoint = $"{appConfig.EndPoint}/Openreach/{openreachId}";

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
                    var jsonData = JObject.Parse(responseData);
                    var responseObj = JsonConvert.DeserializeObject<OpenreachResponse>(jsonData["data"][0].ToString());

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
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<SearchResponse<OpenreachResponse>>> SearchOpenreachByOpenreachNumber(string apiKey, string openreachNumber)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<SearchResponse<OpenreachResponse>>()
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

                string encodedOpenreachNumber = HttpUtility.UrlEncode(openreachNumber);
                endpoint = $"{appConfig.EndPoint}/Openreach/search?criteria=(Openreach_SM:equals:{encodedOpenreachNumber})";

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
                    var responseObj = JsonConvert.DeserializeObject<SearchResponse<OpenreachResponse>>(responseData);

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
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<UpdateResponse>> UpdateOpenreach(string apiKey, string openreachId, OpenreachForUpdate openreachForUpdate)
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

                endpoint = $"{appConfig.EndPoint}/Openreach/{openreachId}";
                var updateRequest = new UpdateRequest();
                updateRequest.data.Add(openreachForUpdate);

                string requestBody = JsonConvert.SerializeObject(updateRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await httpClient.SendAsync(request,
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
        }

        public async Task<ApiResultDto<RelatedAccountsResponse>> GetOpenreachRelatedAccount(string apiKey, string openreachId)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<RelatedAccountsResponse>()
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

                endpoint = $"{appConfig.EndPoint}/Openreach/{openreachId}/Accounts7";

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
                    var responseObj = JsonConvert.DeserializeObject<RelatedAccountsResponse>(responseData);

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
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<RelatedTitlesResponse>> GetOpenreachRelatedTitles(string apiKey, string openreachId)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<RelatedTitlesResponse>()
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

                endpoint = $"{appConfig.EndPoint}/Openreach/{openreachId}/Titles6";

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
                    var responseObj = JsonConvert.DeserializeObject<RelatedTitlesResponse>(responseData);

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
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }
    }
}
