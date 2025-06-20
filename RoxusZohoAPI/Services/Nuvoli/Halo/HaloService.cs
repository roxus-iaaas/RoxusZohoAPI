using Newtonsoft.Json;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Nuvoli.Halo;
using RoxusZohoAPI.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Nuvoli.Halo
{

    public class HaloService : IHaloService
    {

        private readonly IRoxusLoggingRepository _roxusRepository;

        public HaloService(IRoxusLoggingRepository roxusRepository)
        {
            _roxusRepository = roxusRepository;
        }

        #region Token

        public async Task<string> 
            GetAccessToken()
        {

            try
            {

                string username = "nuvoli";
                string password = "haloservicedesk";

                string encodedKey = StringHelpers.Base64Encode($"{username}:{password}");

                var appConfiguration = await _roxusRepository.GetAppConfigurationByApiKey(encodedKey);

                string expiredStr = appConfiguration.ExpiredTime;
                string accessToken = appConfiguration.AccessToken;

                if (!string.IsNullOrWhiteSpace(expiredStr))
                {
                    var expiredTime = DateTimeHelpers.ConvertStringToDateTime(expiredStr);
                    int compare = DateTime.Compare(DateTime.UtcNow, expiredTime.AddMinutes(30));
                    if (compare < 0 && !string.IsNullOrWhiteSpace(appConfiguration.AccessToken))
                    {
                        return accessToken;
                    }
                }

                string haloEndpoint = appConfiguration.AuthEndPoint;
                string clientId = appConfiguration.ClientId;
                string clientSecret = appConfiguration.ClientSecret;

                string requestTokenEndpoint = $"{haloEndpoint}";

                var formData = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "scope", "all:standard" }
                };

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           requestTokenEndpoint)
                {
                    Content = new FormUrlEncodedContent(formData)
                };

                using var httpClient = new HttpClient();

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetAccessTokenResponse>(responseData);
                    accessToken = responseObj.access_token;

                    string newExpiredTime = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow.AddMinutes(20));

                    appConfiguration.AccessToken = accessToken;
                    appConfiguration.ExpiredTime = newExpiredTime;

                    await _roxusRepository.UpdateTokenAndExpiredTime(appConfiguration.Id, accessToken, newExpiredTime);

                    return accessToken;
                }

                return null;

            }
            catch (Exception)
            {

                return null;
            
            }

        }

        #endregion

        #region Tickets

        public async Task<ApiResultDto<GetTicketByIdResponse>> 
            GetTicketById(string ticketId)
        {

            var apiResult = new ApiResultDto<GetTicketByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {

                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;

                }    

                string getTicketByIdEndpoint = $"{NuvoliConstants.Halo_Endpoint}/api/Tickets/{ticketId}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           getTicketByIdEndpoint);

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetTicketByIdResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }

        }

        public async Task<ApiResultDto<GetTicketActionsResponse>> 
            GetTicketActions(string ticketId)
        {
            var apiResult = new ApiResultDto<GetTicketActionsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {

                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;

                }

                string getTicketByIdEndpoint = $"{NuvoliConstants.Halo_Endpoint}/api/Actions?ticket_id={ticketId}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           getTicketByIdEndpoint);

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetTicketActionsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }
        }

        public async Task<ApiResultDto<UpdateTicketResponse>> UpdateTicket
            (UpdateTicketRequest ticketRequest)
        {

            var apiResult = new ApiResultDto<UpdateTicketResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {

                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;

                }

                var requestBody = new List<UpdateTicketRequest>
                {
                    ticketRequest
                };

                string requestBodyStr = JsonConvert.SerializeObject(requestBody);

                string updateTicketEndpoint = $"{NuvoliConstants.Halo_Endpoint}/api/Tickets";

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           updateTicketEndpoint)
                {
                    Content = new StringContent(requestBodyStr, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpdateTicketResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }

        }

        public async Task<ApiResultDto<string>> ExecuteTicketActions(string requestBody)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            string responseData = string.Empty;

            try
            {

                string accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {

                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;

                }

                string executeTicketActionEndpoint = $"{NuvoliConstants.Halo_Endpoint}/api/actions";

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           executeTicketActionEndpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseData;
                }
                else
                {
                    apiResult.Data = responseData;
                }    

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }
        }

        public async Task<ApiResultDto<GetCannedTextByIdResponse>>
            GetCannedTextById(string cannedTextId)
        {

            var apiResult = new ApiResultDto<GetCannedTextByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {

                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;

                }

                string getCannedTextByIdEndpoint = $"{NuvoliConstants.Halo_Endpoint}/api/CannedText/{cannedTextId}";

                var request = new HttpRequestMessage(
                            HttpMethod.Get,
                            getCannedTextByIdEndpoint);

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using var response = await httpClient.SendAsync(request,
                            HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetCannedTextByIdResponse>(responseData);
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

        public async Task<ApiResultDto<ListUsersResponse>> 
            ListUsers(ListUsersRequest listUsersRequest)
        {

            var apiResult = new ApiResultDto<ListUsersResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {

                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;

                }

                string username = listUsersRequest.Username;
                string siteId = listUsersRequest.SiteId;

                string searchUsersEndpoint = 
                    $"{NuvoliConstants.Halo_Endpoint}/api/users?search={username}" +
                        $"&site_id={siteId}&pageinate=true&page_no=1&page_size=100";

                var request = new HttpRequestMessage(
                            HttpMethod.Get,
                            searchUsersEndpoint);

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using var response = await httpClient.SendAsync(request,
                            HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<ListUsersResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<CreateUserResponse>> CreateUser(CreateUserData createUserData)
        {

            var apiResult = new ApiResultDto<CreateUserResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {

                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;

                }

                string searchUsersEndpoint =
                    $"{NuvoliConstants.Halo_Endpoint}/api/users";

                var createUserRequest = new List<CreateUserData>();
                createUserRequest.Add(createUserData);

                var request = new HttpRequestMessage(
                            HttpMethod.Post,
                            searchUsersEndpoint)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(createUserRequest), 
                        Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using var response = await httpClient.SendAsync(request,
                            HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<CreateUserResponse>(responseData);
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

        public async Task<ApiResultDto<ListAssetsResponse>> ListAssets(ListAssetsRequest listAssetsRequest)
        {
            var apiResult = new ApiResultDto<ListAssetsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {

                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;

                }

                string inventoryNumber = listAssetsRequest.InventoryNumber;
                string clientId = listAssetsRequest.ClientId;

                string searchUsersEndpoint =
                    $"{NuvoliConstants.Halo_Endpoint}/api/asset?" +
                        $"&includedetails=true" +
                        $"&client_id={clientId}&inventory_number={inventoryNumber}" +
                        $"&pageinate=true&page_no=1&page_size=100";

                var request = new HttpRequestMessage(
                            HttpMethod.Get,
                            searchUsersEndpoint);

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using var response = await httpClient.SendAsync(request,
                            HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<ListAssetsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<CreateChildTicketResponse>> CreateChildTicket(CreateChildTicketData createChildTicketRequest)
        {

            var apiResult = new ApiResultDto<CreateChildTicketResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;

                }

                string searchUsersEndpoint =
                    $"{NuvoliConstants.Halo_Endpoint}/api/Tickets";

                var createChildTicket = new List<CreateChildTicketData>
                {
                    createChildTicketRequest
                };

                string requestBody = JsonConvert.SerializeObject(createChildTicket);

                var request = new HttpRequestMessage(
                            HttpMethod.Post,
                            searchUsersEndpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using var response = await httpClient.SendAsync(request,
                            HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<CreateChildTicketResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<UpsertAssetResponse>> UpsertAsset(
            UpsertAssetData upsertAssetRequest)
        {

            var apiResult = new ApiResultDto<UpsertAssetResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {

                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;

                }

                string upsertAssetEndpoint =
                    $"{NuvoliConstants.Halo_Endpoint}/api/Asset";

                var upsertAsset = new List<UpsertAssetData>
                {
                    upsertAssetRequest
                };

                string requestBody = JsonConvert.SerializeObject(upsertAsset);

                var request = new HttpRequestMessage(
                            HttpMethod.Post,
                            upsertAssetEndpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                using var response = await httpClient.SendAsync(request,
                            HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertAssetResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        #endregion

    }

}
