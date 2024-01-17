using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public class ZohoNoteService : IZohoNoteService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public ZohoNoteService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateNote(string apiKey, UpsertRequest<NoteForCreation> noteRequest)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);
                endpoint = $"{appConfig.EndPoint}/Notes";
                string requestBody = JsonConvert.SerializeObject(noteRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
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

    }
}
