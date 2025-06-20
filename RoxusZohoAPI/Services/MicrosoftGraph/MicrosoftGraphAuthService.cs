using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Models.MicrosoftGraph;
using RoxusZohoAPI.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace RoxusZohoAPI.Services.MicrosoftGraph
{

    public class MicrosoftGraphAuthService : IMicrosoftGraphAuthService
    {
        private readonly IRoxusLoggingRepository _roxusRepository;

        public MicrosoftGraphAuthService(IRoxusLoggingRepository roxusRepository)
        {
            _roxusRepository = roxusRepository;
        }

        public async Task<AppConfiguration> GetAccessToken(string apiKey)
        {
            AppConfiguration appConfig = null;
            try
            {
                appConfig = await _roxusRepository.GetAppConfigurationByApiKey(apiKey);
                if (appConfig == null)
                {
                    throw new Exception($"Invalid API Key.");
                }

                string expiredStr = appConfig.ExpiredTime;
                if (!string.IsNullOrWhiteSpace(expiredStr))
                {
                    var expiredTime = DateTimeHelpers.ConvertStringToDateTime(expiredStr);
                    int compare = DateTime.Compare(DateTime.UtcNow, expiredTime.AddMinutes(30));
                    if (compare < 0 && !string.IsNullOrWhiteSpace(appConfig.AccessToken))
                    {
                        return appConfig;
                    }
                }

                string authEndpoint = appConfig.AuthEndPoint;
                string refreshToken = appConfig.RefreshToken;
                string clientId = appConfig.ClientId;
                string clientSecret = appConfig.ClientSecret;
                string grantType = "refresh_token";
                string redirectUri = appConfig.RedirectUri;
                if (string.IsNullOrEmpty(redirectUri))
                {
                    redirectUri = "https://roxus.io/";
                }    

                string endpoint = authEndpoint;

                var dict = new Dictionary<string, string>
                {
                    { "client_id", clientId },
                    { "scope", "offline_access Sites.Manage.All" },
                    { "refresh_token", refreshToken },
                    { "redirect_uri", redirectUri },
                    { "grant_type", grantType },
                    { "client_secret", clientSecret }
                };

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new FormUrlEncodedContent(dict)
                };

                var httpClient = new HttpClient();

                using (var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    // response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();
                    var responseObj = JsonConvert.DeserializeObject<MicrosoftTokenResponse>(responseData);
                    var currentTime = DateTimeOffset.Now;

                    string accessToken = responseObj.access_token;
                    string newRefreshToken = responseObj.refresh_token;
                    string newExpiredTime = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow.AddMinutes(20));

                    appConfig.AccessToken = accessToken;
                    appConfig.ExpiredTime = newExpiredTime;
                    appConfig.RefreshToken = newRefreshToken;

                    await _roxusRepository.UpdateTokenAndExpiredTime(appConfig.Id, accessToken, newExpiredTime);

                    return appConfig;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
