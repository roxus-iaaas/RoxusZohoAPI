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
using RoxusZohoAPI.Models.SharePoint;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

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

        public async Task<ApiResultDto<string>> SearchFilesInFolder(SearchFilesInFolderRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = "Search Files in Folder FAILED"
            };


            try
            {

                string folderId = request.FolderId;
                string fileName = request.FileName;
                string customerName = request.CustomerName;

                if (string.IsNullOrEmpty(fileName))
                {
                    apiResult.Message = "RoxusError! File Name is mandatory";
                    return apiResult;
                }    

                string siteId = string.Empty;
                string username = string.Empty;
                string password = string.Empty;

                if (customerName == "Enviromontel")
                {
                    siteId = EnviromontelConstants.AutomationSiteId;
                    username = EnviromontelConstants.MGUserName;
                    password = EnviromontelConstants.MGPassword;
                }
                else if (customerName == "Nuvoli")
                {
                    siteId = NuvoliConstants.AutomationSiteId;
                    username = NuvoliConstants.MGUserName;
                    password = NuvoliConstants.MGPassword;
                }

                byte[] stringBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
                string apiKey = Convert.ToBase64String(stringBytes);

                #region STEP 1: Call API to get Microsoft Graph Access Token

                string accessToken = string.Empty;

                string accessEndpoint = "https://roxuszohoapi.azurewebsites.net/api/microsoft/token";
                var accessRequest = new HttpRequestMessage(
                           HttpMethod.Get,
                           accessEndpoint);

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {apiKey}");

                using (var accessResponse = httpClient.SendAsync(accessRequest,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    var responseResult = accessResponse.Result;
                    var stream = responseResult.Content.ReadAsStreamAsync().Result;

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (responseResult.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<AppConfiguration>(responseData);
                        accessToken = responseObj.AccessToken;
                    }
                }

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Data = "Error! Cannot get Microsoft Graph access token";
                    return apiResult;
                }

                #endregion

                #region STEP 2: Call API to search file in folder
                string searchFileInFolderEndpoint = $"https://graph.microsoft.com/v1.0/sites/{siteId}" +
                    $"/drive/items/{folderId}/search(q='{fileName}')";

                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                // Initialize the allItems list
                var allItems = new List<JObject>();

                string output = string.Empty;

                HttpResponseMessage response = httpClient.GetAsync(searchFileInFolderEndpoint).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject jsonResponse = JObject.Parse(jsonString);

                    foreach (var item in jsonResponse["value"])
                    {
                        allItems.Add((JObject)item);
                    }
                }

                foreach (var item in allItems)
                {

                    string itemId = item["id"]?.ToString();
                    string itemName = item["name"]?.ToString();

                    if (!itemName.Contains(fileName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }    

                    bool isFolder = item["folder"] != null; // Check if the "folder" property exists

                    string itemType = isFolder ? "Folder" : "File";

                    if (itemType == "File")
                    {

                        if (string.IsNullOrEmpty(output))
                        {
                            output = $"{itemName}{CommonConstants.ColumnDelimiter}" +
                                $"{itemId}";
                        }
                        else
                        {
                            output += $"{CommonConstants.RowDelimiter}" +
                                $"{itemName}{CommonConstants.ColumnDelimiter}{itemId}";
                        }
                    }

                }

                if (string.IsNullOrEmpty(output))
                {
                    apiResult.Message = "RoxusError! There is no file found with the following name";
                    return apiResult;
                }    

                #endregion

                apiResult.Code = ResultCode.OK;
                apiResult.Message = "Search Files in Folder SUCCESSFULLY";
                apiResult.Data = output;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"RoxusError! {ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> SearchFoldersByName(SearchFoldersByNameRequest request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = "Search SharePoint Folders FAILED"
            };

            try
            {
                string folderId = request.FolderId;
                string folderName = request.FolderName;
                string customerName = request.CustomerName;
                string folderPath = request.FolderPath;
                var exactMatch = request.ExactMatch;
                var firstLevel = request.FirstLevel;

                if (string.IsNullOrEmpty(folderId) && string.IsNullOrEmpty(folderPath))
                {
                    apiResult.Message = "RoxusError! Folder Id / Folder Path is mandatory";
                    return apiResult;
                }

                string siteId = string.Empty;
                string username = string.Empty;
                string password = string.Empty;

                if (customerName == "Enviromontel")
                {
                    siteId = EnviromontelConstants.AutomationSiteId;
                    username = EnviromontelConstants.MGUserName;
                    password = EnviromontelConstants.MGPassword;
                }
                else if (customerName == "Nuvoli")
                {
                    siteId = NuvoliConstants.AutomationSiteId;
                    username = NuvoliConstants.MGUserName;
                    password = NuvoliConstants.MGPassword;
                }

                byte[] stringBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
                string apiKey = Convert.ToBase64String(stringBytes);

                #region STEP 1: Get Microsoft Graph Access Token
                string accessToken = string.Empty;
                string accessEndpoint = "https://roxuszohoapi.azurewebsites.net/api/microsoft/token";
                var accessRequest = new HttpRequestMessage(HttpMethod.Get, accessEndpoint);

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {apiKey}");

                using (var accessResponse = await httpClient.SendAsync(accessRequest))
                {
                    string responseData = await accessResponse.Content.ReadAsStringAsync();
                    if (accessResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<AppConfiguration>(responseData);
                        accessToken = responseObj.AccessToken;
                    }
                }

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Message = "RoxusError! Cannot get Microsoft Graph access token";
                    return apiResult;
                }
                #endregion

                // STEP 2: Build search endpoint
                string endpointBase = $"https://graph.microsoft.com/v1.0/sites/{siteId}/drive/";
                string searchEndpoint;

                if (!string.IsNullOrEmpty(folderId))
                    searchEndpoint = $"{endpointBase}items/{folderId}/search(q='{folderName}')";
                else
                    searchEndpoint = $"{endpointBase}root:/{folderPath}:/search(q='{folderName}')";

                var searchFoldersRequest = new HttpRequestMessage(HttpMethod.Get, searchEndpoint);
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using (var searchFoldersResponse = await httpClient.SendAsync(searchFoldersRequest))
                {
                    searchFoldersResponse.EnsureSuccessStatusCode();
                    string responseData = await searchFoldersResponse.Content.ReadAsStringAsync();

                    if (searchFoldersResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<SearchSharePointFolderResponse>(responseData);
                        var responseValue = responseObj.value;

                        if (responseValue == null || responseValue.Length == 0)
                        {
                            apiResult.Message = "RoxusError! No folders found.";
                            return apiResult;
                        }

                        string output = string.Empty;
                        string normalizedTargetPath = (folderPath ?? "").TrimStart('/').TrimEnd('/').ToLower();

                        foreach (var item in responseValue)
                        {
                            // Only folders
                            if (item.size > 0)
                                continue;

                            string itemName = item.name;

                            // Pre-filter by folder name
                            bool nameMatch = exactMatch == true
                                ? itemName.Equals(folderName, StringComparison.InvariantCultureIgnoreCase)
                                : itemName.Contains(folderName, StringComparison.InvariantCultureIgnoreCase);

                            if (!nameMatch)
                                continue;

                            // Fetch full details to get parentReference.path
                            string detailEndpoint = $"https://graph.microsoft.com/v1.0/sites/{siteId}/drive/items/{item.id}";
                            var detailRequest = new HttpRequestMessage(HttpMethod.Get, detailEndpoint);
                            detailRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                            string itemPath = string.Empty;
                            using (var detailResponse = await httpClient.SendAsync(detailRequest))
                            {
                                if (!detailResponse.IsSuccessStatusCode)
                                    continue;

                                string detailData = await detailResponse.Content.ReadAsStringAsync();
                                var detailObj = JsonConvert.DeserializeObject<SearchSharePointFolderItem>(detailData);

                                itemPath = detailObj.parentReference?.path ?? string.Empty;
                                itemPath = itemPath.Replace("/drive/root:", "").TrimStart('/');
                                itemPath = string.IsNullOrEmpty(itemPath) ? itemName : $"{itemPath}/{itemName}";
                            }

                            // First-level filter: normalize paths by removing leading slash
                            if (firstLevel)
                            {
                                string parentPath = "";
                                if (itemPath.Contains("/"))
                                    parentPath = string.Join("/", itemPath.Split('/').Reverse().Skip(1).Reverse());

                                string normalizedParent = parentPath.TrimStart('/').TrimEnd('/').ToLower();
                                if (normalizedParent != normalizedTargetPath)
                                    continue;
                            }

                            // Add to output
                            string line = $"{itemName}{CommonConstants.ColumnDelimiter}{item.id}{CommonConstants.ColumnDelimiter}{itemPath}";
                            output = string.IsNullOrEmpty(output) ? line : $"{output}{CommonConstants.RowDelimiter}{line}";
                        }

                        if (string.IsNullOrEmpty(output))
                        {
                            apiResult.Message = "RoxusError! No matching folder found at the specified level.";
                            return apiResult;
                        }

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = "Search SharePoint Folders SUCCESSFULLY";
                        apiResult.Data = output;
                        return apiResult;
                    }
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"RoxusError! {ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }



    }

}
