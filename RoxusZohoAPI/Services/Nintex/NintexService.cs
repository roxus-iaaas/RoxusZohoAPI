using Newtonsoft.Json;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Nintex;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Nintex
{

    public class NintexService : INintexService
    {

        public async Task<ApiResultDto<AddTask244Response>> AddTask244(AddTask244Request addTaskRequest)
        {

            var apiResult = new ApiResultDto<AddTask244Response>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                var endpoint = "https://roxusrpaapi.azurewebsites.net/api/tasks244";
                string apiKey = "Um94dXMuQ29uc29sZTpSb3h1c0AyMDI1IQ==";

                string requestBody = JsonConvert.SerializeObject(addTaskRequest);

                var request = new HttpRequestMessage(
                          HttpMethod.Post,
                          endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {apiKey}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiResult = JsonConvert.DeserializeObject<ApiResultDto<AddTask244Response>>(responseData);
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
