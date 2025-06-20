using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.PureFinance.Airtable;
using RoxusZohoAPI.Repositories;
using RoxusZohoAPI.Services.Zoho;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.PureFinance
{

    public class AirtableService : IAirtableService
    {

        private readonly IZohoAuthService _zohoAuthService;
        private readonly IRoxusLoggingRepository _roxusRepository;

        public AirtableService(IZohoAuthService zohoAuthService, 
            IRoxusLoggingRepository roxusRepository)
        {
            _zohoAuthService = zohoAuthService;
            _roxusRepository = roxusRepository;
        }

        public async Task<string> RefreshToken()
        {

            AppConfiguration appConfig = null;
            string accessToken = null;

            try
            {

                string apiKey = TokenHelpers.Base64Encode("PureFinance:Airtable");
                appConfig = await _roxusRepository.GetAppConfigurationByApiKey(apiKey);
                if (appConfig == null)
                {
                    return null;
                }

                string expiredStr = appConfig.ExpiredTime;
                if (!string.IsNullOrWhiteSpace(expiredStr))
                {
                    var expiredTime = DateTimeHelpers.ConvertStringToDateTime(expiredStr);
                    int compare = DateTime.Compare(DateTime.UtcNow, expiredTime);
                    if (compare < 0 && !string.IsNullOrWhiteSpace(appConfig.AccessToken))
                    {
                        return appConfig.AccessToken;
                    }
                }

                string refreshEndpoint = appConfig.AuthEndPoint;
                string clientId = appConfig.ClientId;
                string clientSecret = appConfig.ClientSecret;
                string refreshToken = appConfig.RefreshToken;

                string basicAuth = TokenHelpers.Base64Encode($"{clientId}:{clientSecret}");
                
                using var httpClient = new HttpClient();

                var dict = new Dictionary<string, string>();
                dict.Add("grant_type", "refresh_token");
                dict.Add("refresh_token", refreshToken);
                dict.Add("client_id", clientId);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           refreshEndpoint)
                {
                    Content = new FormUrlEncodedContent(dict)
                };

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {basicAuth}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to strings
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<RefreshTokenResponse>(responseData);
                    accessToken = responseObj.access_token;
                    string newRefreshToken = responseObj.refresh_token;
                    string newExpiredTime = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow.AddMinutes(20));

                    await _roxusRepository.UpdateTokenAndExpiredTime(appConfig.Id, accessToken, newExpiredTime, newRefreshToken);

                }

                return accessToken;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<ApiResultDto<RefreshWebhookResponse>> RefreshWebhook(string baseId, string webhookId)
        {

            var apiResult = new ApiResultDto<RefreshWebhookResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = PureFinanceConstants.Airtable_RefreshWebhook_400
            };

            try
            {

                string accessToken = await RefreshToken();

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Message = "Cannot get Airtable Access Token";
                    return apiResult;
                }

                string refreshWebhookEndpoint = $"https://api.airtable.com/v0/bases/{baseId}"
                    + $"/webhooks/{webhookId}/refresh";

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           refreshWebhookEndpoint);

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to strings
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<RefreshWebhookResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = PureFinanceConstants.Airtable_RefreshWebhook_200;
                    apiResult.Data = responseObj;
                    return apiResult;
                }

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<ListWebhookPayloadsResponse>> ListWebhookPayloads
            (string baseId, string webhookId, int cursor = 0)
        {
            var apiResult = new ApiResultDto<ListWebhookPayloadsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await RefreshToken();

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Message = "Cannot get Airtable Access Token";
                    return apiResult;
                }

                string refreshWebhookEndpoint = $"https://api.airtable.com/v0/bases/{baseId}"
                    + $"/webhooks/{webhookId}/payloads";

                if (cursor > 0)
                {
                    refreshWebhookEndpoint += $"?cursor={cursor}";
                }

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           refreshWebhookEndpoint);

                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to strings
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<ListWebhookPayloadsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                    return apiResult;
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
