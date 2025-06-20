using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoBooks;
using RoxusZohoAPI.Services.Zoho;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Hinets
{
    public class HinetsBooksService : IHinetsBooksService
    {
        private readonly IZohoAuthService _zohoAuthService;
        private const string PurchaseOrderModule = "purchaseorders";
        private const string EstimateModule = "estimates";
        private const string CompositeItemModule = "compositeitems";
        private const string ItemModule = "items";
        private string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public HinetsBooksService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
            if (!Directory.Exists(DocumentPath))
            {
                Directory.CreateDirectory(DocumentPath);
            }
        }

        #region Purchase Order
        public async Task<ApiResultDto<PurchaseOrderResponse>> GetPoById(string apiKey, string poId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<PurchaseOrderResponse>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{PurchaseOrderModule}/{poId}?organization_id={appConfig.TenantId}";
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
                    var responseObj = JsonConvert.DeserializeObject<PurchaseOrderResponse>(responseData);

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
                    ApiName = ZohoConstants.ZBOOKS_GET_PURCHASEORDER_BY_ID,
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
                            ApiName = ZohoConstants.ZBOOKS_GET_PURCHASEORDER_BY_ID,
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

        public async Task<ApiResultDto<UpdatePoResponse>> UpdatePurchaseOrder(string apiKey, string poId, object updateRequest)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpdatePoResponse>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                string requestBody = JsonConvert.SerializeObject(updateRequest);
                endpoint = $"{appConfig.EndPoint}/{PurchaseOrderModule}/{poId}?organization_id={appConfig.TenantId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Put,
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
                    var responseObj = JsonConvert.DeserializeObject<UpdatePoResponse>(responseData);

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
                    HttpMethod = "PUT",
                    ApiName = ZohoConstants.ZBOOKS_UPDATE_PURCHASE_ORDER,
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
                            HttpMethod = "PUT",
                            ApiName = ZohoConstants.ZBOOKS_UPDATE_PURCHASE_ORDER,
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

        public async Task<ApiResultDto<string>> DownloadPoPDF(string apiKey, string poId, string poNumber)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<string>();
            string filePath = $"{DocumentPath}/{poNumber}.pdf";
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{PurchaseOrderModule}/pdf?organization_id={appConfig.TenantId}&purchaseorder_ids={poId}";
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
                    ApiName = ZohoConstants.ZBOOKS_DOWNLOAD_PURCHASE_ORDER_PDF,
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
                            ApiName = ZohoConstants.ZBOOKS_DOWNLOAD_PURCHASE_ORDER_PDF,
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

        #endregion

        #region Estimate
        public async Task<ApiResultDto<EstimateResponse>> GetEstimateById(string apiKey, string estimateId)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<EstimateResponse>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{EstimateModule}/{estimateId}?organization_id={appConfig.TenantId}";
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
                    var responseObj = JsonConvert.DeserializeObject<EstimateResponse>(responseData);
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
                return null;
            }
        }

        public async Task<ApiResultDto<SearchEstimateResponse>> SearchEstimateByDealId(string apiKey, string dealId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<SearchEstimateResponse>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{EstimateModule}?organization_id={appConfig.TenantId}&zcrm_potential_id={dealId}";
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
                    var responseObj = JsonConvert.DeserializeObject<SearchEstimateResponse>(responseData);

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
                    ApiName = ZohoConstants.ZBOOKS_SEARCH_ESTIMATES,
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
                            ApiName = ZohoConstants.ZBOOKS_SEARCH_ESTIMATES,
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

        public async Task<ApiResultDto<UpdateEstimateResponse>> UpdateEstimate(string apiKey, string estimateId, object updateRequest)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpdateEstimateResponse>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{EstimateModule}/{estimateId}?organization_id={appConfig.TenantId}";
                string requestBody = JsonConvert.SerializeObject(updateRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Put,
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
                    var responseObj = JsonConvert.DeserializeObject<UpdateEstimateResponse>(responseData);

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
                return null;
            }
        }

        public async Task<ApiResultDto<string>> DownloadEstimatePDF(string apiKey, string estimateId, string estimateNumber)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<string>();
            if (string.IsNullOrEmpty(estimateNumber))
            {
                estimateNumber = "TEST ESTIMATE";
            }
            string filePath = $"{DocumentPath}/{estimateNumber}.pdf";
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/{EstimateModule}/pdf?organization_id={appConfig.TenantId}&estimate_ids={estimateId}";
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
                    ApiName = ZohoConstants.ZBOOKS_DOWNLOAD_PURCHASE_ORDER_PDF,
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
                            ApiName = ZohoConstants.ZBOOKS_DOWNLOAD_PURCHASE_ORDER_PDF,
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

        public async Task<ApiResultDto<SearchPurchaseOrderResponse>> SearchPurchaseOrdersByReference(string apiKey, string referenceNumber)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<SearchPurchaseOrderResponse>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{PurchaseOrderModule}?organization_id={appConfig.TenantId}&reference_number={referenceNumber}";
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
                    var responseObj = JsonConvert.DeserializeObject<SearchPurchaseOrderResponse>(responseData);

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
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Items

        public async Task<ApiResultDto<GetItemsResponse>> GetActiveItems(string apiKey, int? page)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<GetItemsResponse>()
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
                endpoint = $"{appConfig.EndPoint}/{ItemModule}/?organization_id={appConfig.TenantId}&is_combo_product=false&status=active";
                if (page.HasValue)
                {
                    endpoint += $"&page={page.Value}";
                }

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
                    var responseObj = JsonConvert.DeserializeObject<GetItemsResponse>(responseData);

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
                return null;
            }
        }

        public async Task<ApiResultDto<UpdateItemResponse>> UpdateItem(string apiKey, string itemId, ItemForUpdate itemForUpdate)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpdateItemResponse>()
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
                endpoint = $"{appConfig.EndPoint}/{ItemModule}/{itemId}?organization_id={appConfig.TenantId}";

                string requestBody = JsonConvert.SerializeObject(itemForUpdate);
                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8)
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
                    var responseObj = JsonConvert.DeserializeObject<UpdateItemResponse>(responseData);

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
                return null;
            }
        }

        public async Task<ApiResultDto<GetItemsResponse>> GetCompositeItems(string apiKey, int? page)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<GetItemsResponse>()
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
                endpoint = $"{appConfig.EndPoint}/{ItemModule}/?organization_id={appConfig.TenantId}&is_combo_product=true&status=active";
                if (page.HasValue)
                {
                    endpoint += $"&page={page.Value}";
                }

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
                    var responseObj = JsonConvert.DeserializeObject<GetItemsResponse>(responseData);

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
                return null;
            }
        }

        public async Task<ApiResultDto<GetCompositeItemByIdResponse>> GetCompositeItemById(string apiKey, string compositeId)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<GetCompositeItemByIdResponse>()
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
                endpoint = $"{appConfig.EndPoint}/compositeitems/{compositeId}?organization_id={appConfig.TenantId}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetCompositeItemByIdResponse>(responseData);

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
                return null;
            }
        }

        #endregion

    }
}
