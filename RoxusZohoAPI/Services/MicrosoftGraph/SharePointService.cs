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
                bool exactMatch = request.ExactMatch;
                bool firstLevel = request.FirstLevel;

                if (string.IsNullOrEmpty(folderId) && string.IsNullOrEmpty(folderPath))
                {
                    apiResult.Message = "RoxusError! Folder Id / Folder Path is mandatory";
                    return apiResult;
                }

                // ---------- customer config ----------
                string siteId = string.Empty, username = string.Empty, password = string.Empty;
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

                // ---------- STEP 1: get access token ----------
                string accessToken = string.Empty;
                using (var httpAuth = new HttpClient())
                {
                    string apiKey = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                    httpAuth.DefaultRequestHeaders.Add("Authorization", $"Basic {apiKey}");

                    var tokenResp = await httpAuth.GetAsync("https://roxuszohoapi.azurewebsites.net/api/microsoft/token");
                    if (tokenResp.StatusCode == HttpStatusCode.OK)
                    {
                        var raw = await tokenResp.Content.ReadAsStringAsync();
                        var cfg = JsonConvert.DeserializeObject<AppConfiguration>(raw);
                        accessToken = cfg?.AccessToken ?? string.Empty;
                    }
                }

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Message = "RoxusError! Cannot get Microsoft Graph access token";
                    return apiResult;
                }

                // ---------- helpers for safe encoding ----------
                string EncodePathSegments(string path)
                {
                    if (string.IsNullOrWhiteSpace(path)) return string.Empty;
                    return string.Join("/",
                        path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(seg => Uri.EscapeDataString(seg)));
                }

                // Graph's search(q='...') expects the value inside single quotes.
                // If folderName contains single-quote, double it (Graph escaping).
                string safeName = (folderName ?? string.Empty).Replace("'", "''");
                // encode content so spaces/special chars become %20, etc.
                string encodedName = Uri.EscapeDataString(safeName);

                // ---------- STEP 2: build search endpoint safely ----------
                string baseUrl = $"https://graph.microsoft.com/v1.0/sites/{Uri.EscapeDataString(siteId)}/drive/";
                string searchEndpoint;

                if (!string.IsNullOrEmpty(folderId))
                {
                    // items/{id}/search(q='...') and request parentReference via $select
                    // Note: folderId is usually an ID (GUID). Escape it to be safe.
                    searchEndpoint =
                        $"{baseUrl}items/{Uri.EscapeDataString(folderId)}/search(q='{encodedName}')?$select=id,name,size,parentReference";
                }
                else
                {
                    // root:/{path}:/search(q='...') -> encode path segments but keep slashes
                    var safePath = (folderPath ?? string.Empty).Trim('/');
                    var encodedPath = EncodePathSegments(safePath);
                    searchEndpoint =
                        $"{baseUrl}root:/{encodedPath}:/search(q='{encodedName}')?$select=id,name,size,parentReference";
                }

                // ---------- create HttpClient and perform search ----------
                using (var http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                    var searchResp = await http.GetAsync(searchEndpoint);
                    if (!searchResp.IsSuccessStatusCode)
                    {
                        apiResult.Message = $"RoxusError! Graph search failed: {(int)searchResp.StatusCode} {searchResp.ReasonPhrase}";
                        return apiResult;
                    }

                    var searchJson = await searchResp.Content.ReadAsStringAsync();
                    var searchObj = JsonConvert.DeserializeObject<SearchSharePointFolderResponse>(searchJson);

                    if (searchObj?.value == null || searchObj.value.Length == 0)
                    {
                        apiResult.Message = "RoxusError! No folders found.";
                        return apiResult;
                    }

                    string output = string.Empty;
                    string normalizedTargetPath = (folderPath ?? string.Empty).Trim('/').ToLowerInvariant();

                    foreach (var item in searchObj.value)
                    {
                        // skip files (size>0 indicates file in your prior code)
                        if (item.size > 0)
                            continue;

                        string itemName = item.name ?? string.Empty;

                        // name match first (cheap)
                        bool nameMatch = exactMatch
                            ? itemName.Equals(folderName ?? string.Empty, StringComparison.InvariantCultureIgnoreCase)
                            : itemName.IndexOf(folderName ?? string.Empty, StringComparison.InvariantCultureIgnoreCase) >= 0;

                        if (!nameMatch)
                            continue;

                        // parentReference from search response (we requested it with $select)
                        var parentRef = item?.parentReference;
                        string parentIdFromSearch = parentRef?.id;
                        string parentPathFromSearch = parentRef?.path ?? string.Empty;

                        // Normalize parentPathFromSearch -> remove leading "/drive/root:" and leading slash
                        if (!string.IsNullOrEmpty(parentPathFromSearch))
                            parentPathFromSearch = parentPathFromSearch.Replace("/drive/root:", "").TrimStart('/');

                        // ---------- MODE: FolderId ----------
                        if (!string.IsNullOrEmpty(folderId))
                        {
                            // Must be direct child of folderId for firstLevel==true,
                            // and also to restrict scope (Graph search may return outer items)
                            if (!string.Equals(parentIdFromSearch, folderId, StringComparison.OrdinalIgnoreCase))
                                continue;

                            // if firstLevel is required, parentId check above already ensures direct child.
                            // Build full path from parentPathFromSearch + item.name
                            string fullPath = string.IsNullOrEmpty(parentPathFromSearch)
                                ? itemName
                                : $"{parentPathFromSearch}/{itemName}";

                            string line = $"{itemName}{CommonConstants.ColumnDelimiter}{item.id}{CommonConstants.ColumnDelimiter}{fullPath}";
                            output = string.IsNullOrEmpty(output) ? line : $"{output}{CommonConstants.RowDelimiter}{line}";
                            continue;
                        }

                        // ---------- MODE: FolderPath ----------
                        // Build itemPath from parentPathFromSearch + itemName
                        string itemPathBuilt = string.IsNullOrEmpty(parentPathFromSearch)
                            ? itemName
                            : $"{parentPathFromSearch}/{itemName}";

                        if (firstLevel)
                        {
                            // parentPath = itemPathBuilt minus last segment
                            string parentPath = itemPathBuilt.Contains("/")
                                ? string.Join("/", itemPathBuilt.Split('/').Reverse().Skip(1).Reverse())
                                : string.Empty;

                            string normalizedParent = parentPath.Trim('/').ToLowerInvariant();
                            if (!string.Equals(normalizedParent, normalizedTargetPath, StringComparison.OrdinalIgnoreCase))
                                continue;
                        }

                        // Passed all filters — add result
                        string line2 = $"{itemName}{CommonConstants.ColumnDelimiter}{item.id}{CommonConstants.ColumnDelimiter}{itemPathBuilt}";
                        output = string.IsNullOrEmpty(output) ? line2 : $"{output}{CommonConstants.RowDelimiter}{line2}";
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
            catch (Exception ex)
            {
                apiResult.Message = $"RoxusError! {ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }


        public async Task<ApiResultDto<string>> CreateFolderInFolder(CreateSharePointFolderRequest request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = $"Create SharePoint folder {request.FolderName} FAILED"
            };

            try
            {
                string folderId = request.FolderId;
                string folderName = request.FolderName;
                string customerName = request.CustomerName;

                if (string.IsNullOrEmpty(folderId))
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

                #region STEP 2: Call API to create folder

                string createFolderEndpoint = $"https://graph.microsoft.com/v1.0/sites/{siteId}" +
                    $"/drive/items/{folderId}/children";

                string requestBody = "{\"name\":\"" + folderName + "\",\"folder\":{}}";

                var createFoldersRequest = new HttpRequestMessage(
                           HttpMethod.Post,
                           createFolderEndpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using (var createFolderResponse = httpClient.SendAsync(createFoldersRequest,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    var createFolderResult = createFolderResponse.Result;
                    // createFolderResult.EnsureSuccessStatusCode();
                    var stream = createFolderResult.Content.ReadAsStreamAsync().Result;

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (createFolderResult.StatusCode == HttpStatusCode.OK
                        || createFolderResult.StatusCode == HttpStatusCode.Created)
                    {
                        var responseObj = JsonConvert.DeserializeObject<CreateSharePointFolderResponse>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = $"Create SharePoint folder {folderName} SUCCESSFULLY";
                        apiResult.Data = responseObj.id;
                    }
                    else
                    {
                        apiResult.Data = responseData;
                    }
                }
                #endregion

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
