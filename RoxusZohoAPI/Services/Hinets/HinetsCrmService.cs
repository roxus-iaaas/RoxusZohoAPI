using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets;
using RoxusZohoAPI.Services.Zoho;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Hinets
{
    public class HinetsCrmService : IHinetsCrmService
    {
        private readonly IZohoAuthService _zohoAuthService;
        private const string DealModule = "Deals";
        private string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public HinetsCrmService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<RelatedPurchaseOrdersResponse>> GetRelatedPurchaseOrders(string apiKey, string dealId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<RelatedPurchaseOrdersResponse>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{DealModule}/{dealId}/Purchase_Orders";
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
                    var responseObj = JsonConvert.DeserializeObject<RelatedPurchaseOrdersResponse>(responseData);
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
                    ApiName = ZohoConstants.ZCRM_GET_DEAL_RELATED_PURCHASE_ORDERS,
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
                        using var reader = new StreamReader(responseStream);
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
                            ApiName = ZohoConstants.ZCRM_GET_DEAL_RELATED_PURCHASE_ORDERS,
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApiResultDto<string>> DownloadAttachmentsFromDeal(string apiKey, 
            string dealId, string attachmentName, string attachmentId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<string>();
            string filePath = $"{DocumentPath}/{attachmentName}";
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{DealModule}/{dealId}/Attachments/{attachmentId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient(
                             new HttpClientHandler()
                             {
                                 AutomaticDecompression = DecompressionMethods.GZip
                             });
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var byteArray = await response.Content.ReadAsByteArrayAsync();
                    File.WriteAllBytes(filePath, byteArray);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = filePath;
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
                    ApplicationName = appConfig.Platform,
                    CustomerName = appConfig.CustomerName,
                    ApplicationId = appConfig.Id,
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = ZohoConstants.ZCRM_DOWNLOAD_DEAL_ATTACHMENT,
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
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ApplicationName = appConfig.Platform,
                            CustomerName = appConfig.CustomerName,
                            ApplicationId = appConfig.Id,
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = ZohoConstants.ZCRM_DOWNLOAD_DEAL_ATTACHMENT,
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ApiResultDto<GetListOfAttachments>> GetDealAttachments(string apiKey, string dealId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<GetListOfAttachments>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{DealModule}/{dealId}/Attachments";
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
                    var responseObj = JsonConvert.DeserializeObject<GetListOfAttachments>(responseData);
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
                    ApiName = ZohoConstants.ZCRM_GET_DEAL_ATTACHMENTS,
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
                        using var reader = new StreamReader(responseStream);
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
                            ApiName = ZohoConstants.ZCRM_GET_DEAL_ATTACHMENTS,
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApiResultDto<int>> GetNumberOfProjects(string apiKey, string dealId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<int>() 
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400,
                Data = 0
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
                endpoint = $"{appConfig.EndPoint}/{DealModule}/{dealId}/Zoho_Projects";
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
                    var responseObj = JsonConvert.DeserializeObject<RelatedProjectsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    int projectCount = 0;
                    foreach (var relatedProject in responseObj.data)
                    {
                        if (!string.IsNullOrEmpty(relatedProject.name))
                        {
                            projectCount++;
                        }
                    }
                    apiResult.Data = projectCount;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                    apiResult.Data = 0;
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
                    ApiName = ZohoConstants.ZCRM_GET_DEAL_RELATED_PROJECT,
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
                        using var reader = new StreamReader(responseStream);
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
                            ApiName = ZohoConstants.ZCRM_GET_DEAL_RELATED_PROJECT,
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApiResultDto<UploadResponse>> UploadAttachmentsToDeal(string apiKey, string dealId, string fileName, string filePath)
        {
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
                endpoint = $"{appConfig.EndPoint}/Deals/{dealId}/Attachments";
                var byteArray = File.ReadAllBytes(filePath);
                var pdfContent = new ByteArrayContent(byteArray);
                var form = new MultipartFormDataContent();
                form.Add(pdfContent, "file", fileName);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");
                using (var response = await httpClient.PostAsync(endpoint, form))
                {
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
                    return apiResult;
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }
    }
}
