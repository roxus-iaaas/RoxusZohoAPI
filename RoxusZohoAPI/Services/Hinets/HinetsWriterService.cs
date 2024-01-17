using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.Custom;
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
    public class HinetsWriterService : IHinetsWriterService
    {

        private readonly IZohoAuthService _zohoAuthService;
        private string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public HinetsWriterService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<string>> MergeAndDownloadDocument(UploadAccommodationRequest accommodationRequest)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<string>();
            string fileName = $"Accommodation - {accommodationRequest.DealName}";
            string filePath = $"{DocumentPath}/{fileName}.pdf";
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(accommodationRequest.ZohoWriterApiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
 
                endpoint = $"{appConfig.EndPoint}/documents/{HinetsConstants.AccommondationDocumentId}/merge";
                var mergeContent = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("output_format", "pdf"),
                    new KeyValuePair<string, string>("merge_data", accommodationRequest.MergeData),
                    new KeyValuePair<string, string>("filename", fileName)
                };
                var request = new HttpRequestMessage(
                            HttpMethod.Post,
                            endpoint)
                {
                    Content = new FormUrlEncodedContent(mergeContent)
                };
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
    }
}
