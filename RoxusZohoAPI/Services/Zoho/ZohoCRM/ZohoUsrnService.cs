using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Repositories;
using RoxusZohoAPI.Services.Zoho;
using RoxusZohoAPI.Services.Zoho.ZohoCRM;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public class ZohoUsrnService : IZohoUsrnService
    {
        private readonly IRoxusLoggingRepository _roxusRepository;
        private readonly IZohoAuthService _zohoAuthService;

        private readonly HttpClient _httpClient = new HttpClient(
         new HttpClientHandler()
         {
             AutomaticDecompression = DecompressionMethods.GZip
         });

        private const string UsrnModule = "USRN";

        public ZohoUsrnService(IRoxusLoggingRepository roxusRepository, IZohoAuthService zohoAuthService)
        {
            _roxusRepository = roxusRepository;
            _zohoAuthService = zohoAuthService;
            _httpClient.Timeout = new TimeSpan(0, 0, 1800);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<ApiResultDto<UsrnResponse>> GetUsrnById(string apiKey, string usrnId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UsrnResponse>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/{UsrnModule}/{usrnId}";

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
                        var json = JObject.Parse(responseData);
                        var responseObj = JsonConvert.DeserializeObject<UsrnResponse>(json["data"][0].ToString());

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
                        ApiName = ZohoConstants.ZCRM_GET_USRN_BY_ID,
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
                                ApiName = ZohoConstants.ZCRM_GET_USRN_BY_ID,
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

        public async Task<ApiResultDto<UpsertResponse<LinkingResponse>>> USRN_LinkingWithTitle(string apiKey, string usrnId, string titleId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpsertResponse<LinkingResponse>>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/{UsrnModule}";

                // Handle Object for Linking
                var upsertRequest = new UpsertRequest<USRN_LinkingTitle>();
                var usrnLinkingTitle = new USRN_LinkingTitle();
                usrnLinkingTitle.id = usrnId;
                usrnLinkingTitle.Title_Number = titleId;
                upsertRequest.data.Add(usrnLinkingTitle);

                string requestBody = JsonConvert.SerializeObject(upsertRequest);

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
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<UpsertResponse<LinkingResponse>>(responseData);

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
                        HttpMethod = "PUT",
                        ApiName = ZohoConstants.ZCRM_USRN_LINKING_WITH_TITLE,
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
                                ApiName = ZohoConstants.ZCRM_USRN_LINKING_WITH_TITLE,
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
