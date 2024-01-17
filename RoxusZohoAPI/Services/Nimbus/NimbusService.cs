using Newtonsoft.Json;
using RoxusZohoAPI.Entities.TrenchesReportDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Nimbus;
using RoxusZohoAPI.Models.Zoho;
using RoxusZohoAPI.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Nimbus
{
    public class NimbusService : INimbusService
    {

        private readonly ITrenchesReportingRepository _trenchesRepository;
        private const string TrenchesUsername = "TrenchesLaw";
        private const string TrenchesPassword = "290347EB-FB6E-4AA0-BDAB-7079EC0D3334";
        private const string SiteInventory1Endpoint = "https://trencheslawapi.nimbusmaps.co.uk/api/SiteInventory/uprncheck?uprn=";

        public NimbusService(ITrenchesReportingRepository trenchesRepository)
        {
            _trenchesRepository = trenchesRepository;
        }

        public async Task<ApiResultDto<SiteInventory1Response>> SiteInventory1_LFL(long uprn)
        {
            LFLNimbusRequest nimbusRequest = null;
            var apiResult = new ApiResultDto<SiteInventory1Response>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string endpoint = $"{SiteInventory1Endpoint}{uprn}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{TrenchesUsername}:{TrenchesPassword}");
                string apiKey = Convert.ToBase64String(plainTextBytes);

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {apiKey}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    nimbusRequest = new LFLNimbusRequest()
                    {
                        UPRN = uprn,
                        Name = "Site Inventory #1",
                        Endpoint = endpoint,
                        Response = string.Empty,
                        Created = DateTime.UtcNow,
                        StatusCode = (int)response.StatusCode + " " + response.StatusCode,
                    };
                    apiResult.Code = ResultCode.NoContent;
                    return apiResult;
                }

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<SiteInventory1Response>(responseData);
                apiResult.Data = responseObj;

                // HANDLE LOGGING TO DATABASE
                nimbusRequest = new LFLNimbusRequest()
                {
                    UPRN = uprn,
                    Name = "Site Inventory #1",
                    Endpoint = endpoint,
                    Response = responseData,
                    Created = DateTime.UtcNow,
                    StatusCode = (int)response.StatusCode + " " + response.StatusCode,
                };

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using StreamReader reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        nimbusRequest.Response = responseData;
                        nimbusRequest.StatusCode = (int)badResponse.StatusCode + " " + badResponse.StatusCode;
                    }
                }
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            finally
            {
                if (nimbusRequest != null)
                {
                    await _trenchesRepository.CreateLFLNimbusRequest(nimbusRequest);
                }
            }
        }

        public async Task<ApiResultDto<SiteInventory1Response>> SiteInventory1_OR(long uprn)
        {
            ORNimbusRequest nimbusRequest = null;
            var apiResult = new ApiResultDto<SiteInventory1Response>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            string endpoint = string.Empty;

            try
            {
                endpoint = $"{SiteInventory1Endpoint}{uprn}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{TrenchesUsername}:{TrenchesPassword}");
                string apiKey = Convert.ToBase64String(plainTextBytes);

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {apiKey}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    apiResult.Code = ResultCode.NoContent;
                    return apiResult;
                }

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<SiteInventory1Response>(responseData);
                apiResult.Data = responseObj;

                // HANDLE LOGGING TO DATABASE
                nimbusRequest = new ORNimbusRequest()
                {
                    UPRN = uprn,
                    Name = "Site Inventory #1",
                    Endpoint = endpoint,
                    Response = responseData,
                    Created = DateTime.UtcNow,
                    StatusCode = (int)response.StatusCode + " " + response.StatusCode,
                };

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using StreamReader reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        nimbusRequest = new ORNimbusRequest()
                        {
                            UPRN = uprn,
                            Name = "Site Inventory #1",
                            Endpoint = endpoint,
                            Created = DateTime.UtcNow,
                        };
                        nimbusRequest.Response = responseData;
                        nimbusRequest.StatusCode = (int)badResponse.StatusCode + " " + badResponse.StatusCode;
                    }
                }
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            catch (Exception ex)
            {
                nimbusRequest = new ORNimbusRequest()
                {
                    UPRN = uprn,
                    Name = "Site Inventory #1",
                    Endpoint = endpoint,
                    Created = DateTime.UtcNow,
                    Response = "No results found",
                    StatusCode = (int) HttpStatusCode.NotFound + " " + HttpStatusCode.NotFound
                };
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            finally
            {
                if (nimbusRequest != null)
                {
                    await _trenchesRepository.CreateORNimbusRequest(nimbusRequest);
                }
            }
        }

    }
}
