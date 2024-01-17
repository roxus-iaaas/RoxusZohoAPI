using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using RoxusZohoAPI.Repositories;
using RoxusZohoAPI.Services.Zoho;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Hinets
{
    public class HinetsProjectsService : IHinetsProjectsService
    {
        private readonly IZohoAuthService _zohoAuthService;

        public HinetsProjectsService(IHinetsRepository hinetsRepository, IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<GetFolderResponse>> GetProjectFolders(string apiKey, string projectId)
        {
            var apiResult = new ApiResultDto<GetFolderResponse>() { 
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                var appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/folders/";
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
                    var responseObj = JsonConvert.DeserializeObject<GetFolderResponse>(responseData);
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

        public async Task<ApiResultDto<string>> UploadFileToProject(string apiKey, string filePath, string fileName, string projectId, string folderId = null)
        {
            var apiResult = new ApiResultDto<string>();
            try
            {
                var appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/documents/";
                var requestContent = new MultipartFormDataContent();
                var byteArray = File.ReadAllBytes(filePath);
                var pdfContent = new ByteArrayContent(byteArray);
                pdfContent.Headers.ContentType =
                    MediaTypeHeaderValue.Parse("application/pdf");
                requestContent.Add(pdfContent, "uploaddoc", fileName);
                if (!string.IsNullOrEmpty(folderId))
                {
                    var folderContent = new StringContent(folderId);
                    requestContent.Add(folderContent, "folder_id");
                }
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = requestContent
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
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = "Upload File successfully";
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

        public async Task<ApiResultDto<GetAllDocumentsResponse>> GetAllDocuments(string apiKey, string projectId, string folderId)
        {
            var apiResult = new ApiResultDto<GetAllDocumentsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                var appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/documents/";
                if (!string.IsNullOrEmpty(folderId))
                {
                    endpoint += $"?folder_id={folderId}";
                }
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");
                using var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetAllDocumentsResponse>(responseData);
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

        public async Task<ApiResultDto<string>> DeleteDocument(string apiKey, string projectId, string documentId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                var appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/documents/{documentId}/";
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");
                using var response = await httpClient.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
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

        public async Task<ApiResultDto<HinetsProjectDetailsResponse>> GetHinetsProjectDetails(string apiKey, string projectId)
        {
            var apiResult = new ApiResultDto<HinetsProjectDetailsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                var appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{appConfig.EndPoint}/portal/{appConfig.TenantId}/projects/{projectId}/";
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
                    var responseObj = JsonConvert.DeserializeObject<HinetsProjectDetailsResponse>(responseData);
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
