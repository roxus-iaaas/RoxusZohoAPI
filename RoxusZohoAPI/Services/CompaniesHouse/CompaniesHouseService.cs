using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.CompanyHouse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace RoxusZohoAPI.Services.CompaniesHouse
{
    public class CompaniesHouseService : ICompaniesHouseService
    {
        public async Task<ApiResultDto<SearchCompaniesResponse>> GetCompanies(string query)
        {
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<SearchCompaniesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {

                endpoint = $"{CommonConstants.CompaniesHouseEndpoint}";
                endpoint += HttpUtility.UrlEncode(query);

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {CommonConstants.CompaniesHouseKey}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SearchCompaniesResponse>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_200;
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
