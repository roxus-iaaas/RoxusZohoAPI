using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.MicrosoftGraph;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using Azure;

namespace RoxusZohoAPI.Services.MicrosoftGraph
{

    public class SharePointService : ISharePointService
    {

        private readonly IMicrosoftGraphAuthService _authService;

        public SharePointService(IMicrosoftGraphAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ApiResultDto<string>> DownloadFile(DownloadSharePointFileRequest request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = SharePointConstants.DownloadFile_400
            };

            try
            {

                string fileId = request.FileId;
                string customerName = request.CustomerName;

                string siteId = string.Empty;
                string username = string.Empty;
                string password = string.Empty;

                if (customerName == "Nuvoli")
                {
                    siteId = NuvoliConstants.AutomationSiteId;
                    username = NuvoliConstants.MGUserName;
                    password = NuvoliConstants.MGPassword;
                }

                byte[] stringBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
                string apiKey = Convert.ToBase64String(stringBytes);

                #region STEP 1: Call API to get Microsoft Graph Access Token

                var appConfiguration = await _authService.GetAccessToken(apiKey);
                string accessToken = appConfiguration.AccessToken;

                #endregion

                #region STEP 2: Call API to download file

                string downloadFileEndpoint = $"https://graph.microsoft.com/v1.0/sites/{siteId}" +
                    $"/drive/items/{fileId}/content";

                var downloadFileRequest = new HttpRequestMessage(
                           HttpMethod.Get,
                           downloadFileEndpoint);

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using (var downloadFileResponse = await httpClient.SendAsync(downloadFileRequest,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    downloadFileResponse.EnsureSuccessStatusCode();

                    byte[] fileBytes = await downloadFileResponse.Content.ReadAsByteArrayAsync();
                    string base64String = Convert.ToBase64String(fileBytes);
                    apiResult.Data = base64String;
                    apiResult.Message = SharePointConstants.DownloadFile_200;
                    apiResult.Code = ResultCode.OK;
                }

                #endregion

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
