using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoBooks;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Contxtus
{
    public class ContxtusService : IContxtusService
    {

        public async Task<ApiResultDto<string>> CreateIntegrationLog(IntegrationLogForCreation logForCreation)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ContxtusConstants.CreateIntegrationLog_400,
            };

            try
            {
                string requestBody = JsonConvert.SerializeObject(logForCreation);
                string endpoint = $"{ContxtusConstants.RoxusWebApiEndpoint}/api/integration-logs";
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ContxtusConstants.CreateIntegrationLog_200;
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
