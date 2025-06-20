using Newtonsoft.Json;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.PureFinance.Pipedrive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RoxusZohoAPI.Services.PureFinance
{

    public class PipedriveService : IPipedriveService
    {

        #region Deals

        public async Task<ApiResultDto<CreateDealResponse>> CreateDeal(CreateDealRequest createDealRequest)
        {

            var apiResult = new ApiResultDto<CreateDealResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string createPersonEndpoint = $"{PureFinanceConstants.PipedriveEndpointV1}/deals?" +
                    $"api_token={PureFinanceConstants.PipedriveApiKey}";

                string requestBody = JsonConvert.SerializeObject(createDealRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           createPersonEndpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),
                };
                using var httpClient = new HttpClient();

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
                    var responseObj = JsonConvert.DeserializeObject<CreateDealResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.StackTrace;
                return apiResult;
            }

        }

        #endregion

        #region Note

        public async Task<ApiResultDto<CreateNoteResponse>> CreateNote(CreateNoteRequest createDealRequest)
        {

            var apiResult = new ApiResultDto<CreateNoteResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string createPersonEndpoint = $"{PureFinanceConstants.PipedriveEndpointV1}/notes?" +
                    $"api_token={PureFinanceConstants.PipedriveApiKey}";

                string requestBody = JsonConvert.SerializeObject(createDealRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           createPersonEndpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),
                };
                using var httpClient = new HttpClient();

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
                    var responseObj = JsonConvert.DeserializeObject<CreateNoteResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.StackTrace;
                return apiResult;
            }

        }

        #endregion

        #region Person

        public async Task<ApiResultDto<CreatePersonResponse>> CreatePerson(CreatePersonRequest createPersonRequest)
        {

            var apiResult = new ApiResultDto<CreatePersonResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string createPersonEndpoint = $"{PureFinanceConstants.PipedriveEndpointV1}/persons?" +
                    $"api_token={PureFinanceConstants.PipedriveApiKey}";

                string requestBody = JsonConvert.SerializeObject(createPersonRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           createPersonEndpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),
                };
                using var httpClient = new HttpClient();

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
                    var responseObj = JsonConvert.DeserializeObject<CreatePersonResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.StackTrace;
                return apiResult;
            }

        }

        public async Task<ApiResultDto<SearchPersonsResponse>> SearchPersonsByEmail(string email)
        {

            var apiResult = new ApiResultDto<SearchPersonsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string searchPersonsEndpoint = $"{PureFinanceConstants.PipedriveEndpointV1}/persons/search?" +
                    $"term=" + HttpUtility.HtmlEncode(email) +
                    $"&fields=email&exact_match=true" +
                    $"&api_token={PureFinanceConstants.PipedriveApiKey}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           searchPersonsEndpoint);

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
                    var responseObj = JsonConvert.DeserializeObject<SearchPersonsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.StackTrace;
                return apiResult;
            }

        }

        #endregion

    }

}
