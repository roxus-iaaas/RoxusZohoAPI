using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Zoho;
using RoxusZohoAPI.Repositories;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho
{
    public class ZohoAuthService : IZohoAuthService
    {
        private readonly IRoxusLoggingRepository _roxusRepository;
        private readonly HttpClient _httpClient = new HttpClient(
         new HttpClientHandler()
         {
             AutomaticDecompression = DecompressionMethods.GZip
         });

        public ZohoAuthService(IRoxusLoggingRepository roxusRepository)
        {
            _roxusRepository = roxusRepository;
            _httpClient.Timeout = new TimeSpan(0, 0, 1800);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<AppConfiguration> GetAccessToken(string apiKey)
        {
            ApiLogging apiLogging = null;
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

                string endpoint = $"{authEndpoint}?refresh_token={refreshToken}&client_id={clientId}&client_secret={clientSecret}&grant_type={grantType}";

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint);

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();
                    var responseObj = JsonConvert.DeserializeObject<ZohoTokenResponse>(responseData);
                    var currentTime = DateTimeOffset.Now;

                    string accessToken = responseObj.access_token;
                    string newExpiredTime = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow.AddMinutes(20));

                    appConfig.AccessToken = accessToken;
                    appConfig.ExpiredTime = newExpiredTime;

                    await _roxusRepository.UpdateTokenAndExpiredTime(appConfig.Id, accessToken, newExpiredTime);

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
                        HttpMethod = "POST",
                        ApiName = ZohoConstants.ZCRM_GET_ACCESS_TOKEN,
                        Endpoint = endpoint
                    };

                    return appConfig;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                apiLogging.Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging.Response = responseData;
                            apiLogging.Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode;
                        }
                    }
                }
                return appConfig;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }
    
    }

}
