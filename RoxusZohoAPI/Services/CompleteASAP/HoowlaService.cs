using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.CompleteASAP.Hoowla;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets;
using System.IO;
using System.Net.Http;
using System.Net;
using System;
using System.Threading.Tasks;
using RoxusZohoAPI.Repositories;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using RoxusZohoAPI.Models.PureFinance.Pipedrive;
using System.Text;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using System.Linq;

namespace RoxusZohoAPI.Services.CompleteASAP
{

    public class HoowlaService : IHoowlaService
    {

        private readonly IRoxusLoggingRepository _loggingRepository;

        public HoowlaService(IRoxusLoggingRepository loggingRepository)
        {
            _loggingRepository = loggingRepository;
        }

        public async Task<ApiResultDto<CasesViewACaseResponse>> CasesViewACase(string caseId)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesViewACaseResponse>();
            try
            {

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/cases/info?id={caseId}" +
                    $"&key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject<CasesViewACaseResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Cases - View a Case",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Cases - View a Case",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }
        }

        public async Task<ApiResultDto<CasesUpdateManyCustomFieldsResponse>> 
            CasesUpdateManyCustomFields(string caseId, CasesUpdateManyCustomFieldsRequest updateRequest)
        {

            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesUpdateManyCustomFieldsResponse>();
            try
            {

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/custom-fields/many?" +
                    $"case={caseId}&key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";
                
                string requestBody = JsonConvert.SerializeObject(updateRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
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
                    var responseObj = JsonConvert.DeserializeObject
                        <CasesUpdateManyCustomFieldsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "PUT",
                    ApiName = "Cases - Update Many Custom Fields",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Cases - Update Many Custom Fields",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<CasesListCaseTypesResponse>> CasesListCaseTypes()
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesListCaseTypesResponse>();
            try
            {

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/types?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject
                        <List<HoowlaCaseTypeDetails>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    var casesListCaseTypesResponse = new CasesListCaseTypesResponse
                    {
                        CaseTypes = responseObj
                    };
                    apiResult.Data = casesListCaseTypesResponse;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Cases - List Case Types",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Cases - List Case Types",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }
        }

        public async Task<ApiResultDto<CasesListCasesForUserResponse>> CasesListCasesForUser
            (CasesListCasesForUserRequest listCasesRequest)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesListCasesForUserResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLC4U_400
            };

            try
            {

                if (string.IsNullOrEmpty(listCasesRequest.Query))
                {
                    return apiResult;
                }

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/cases/?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}" +
                    $"&query={listCasesRequest.Query}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject
                        <List<HoowlaListCase>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CompleteASAPConstants.CLC4U_200;

                    apiResult.Data = new CasesListCasesForUserResponse()
                    {
                        Cases = responseObj
                    };
                    return apiResult;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                    return apiResult;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Cases - List Cases for User",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Cases - List Cases for User",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }
            }
        }

        public async Task<ApiResultDto<CasesListCustomFieldsResponse>> CasesListCustomFields(string caseId)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesListCustomFieldsResponse>();
            try
            {

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/custom-fields/?case={caseId}" +
                    $"&key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject<List<HoowlaCustomFieldDetails>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    var casesListCustomFieldsResponse = new CasesListCustomFieldsResponse
                    {
                        CustomFields = responseObj
                    };
                    apiResult.Data = casesListCustomFieldsResponse;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Cases - List Custom Fields",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Cases - List Custom Fields",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }
        }

        public async Task<ApiResultDto<CasesListNotesResponse>> CasesListNotes(string caseId)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesListNotesResponse>();
            try
            {

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/notes/?case={caseId}" +
                    $"&key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject<List<Models.CompleteASAP.Hoowla.HoowlaNoteDetails>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    var casesListNotesResponse = new CasesListNotesResponse
                    {
                        Notes = responseObj
                    };
                    apiResult.Data = casesListNotesResponse;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Cases - List Notes",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Cases - List Notes",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }
        }

        public async Task<ApiResultDto<CasesListTasksResponse>> CasesListTasks(string caseId)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesListTasksResponse>();
            try
            {

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/tasks/?case={caseId}" +
                    $"&key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject<List<Models.CompleteASAP.Hoowla.HoowlaTaskDetails>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    var casesListNotesResponse = new CasesListTasksResponse
                    {
                        Tasks = responseObj
                    };
                    apiResult.Data = casesListNotesResponse;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Cases - List Tasks",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Cases - List Tasks",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }
        }

        public async Task<ApiResultDto<CasesCreateANewCaseResponse>>CasesCreateANewCase(CasesCreateANewCaseRequest casesCreateANewCaseRequest)
        {

            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesCreateANewCaseResponse>();

            try
            {
                if (casesCreateANewCaseRequest.case_type_id == 3) 
                    casesCreateANewCaseRequest.case_name = null;

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/cases?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                string requestBody = JsonConvert.SerializeObject(casesCreateANewCaseRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    var responseObj = JsonConvert.DeserializeObject
                        <CasesCreateANewCaseResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "POST",
                    ApiName = "Cases - Create a new Case",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "POST",
                            ApiName = "Cases - Create a new Case",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<CasesAddPersonToACaseResponse>> CasesAddPersonToACase(CasesAddPersonToACaseRequest casesAddPersonToACaseRequest)
        {

            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesAddPersonToACaseResponse>();

            try
            {

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/people?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                string requestBody = JsonConvert.SerializeObject(casesAddPersonToACaseRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    var responseObj = JsonConvert.DeserializeObject
                        <CasesAddPersonToACaseResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "POST",
                    ApiName = "Cases - Add Person to a Case",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "POST",
                            ApiName = "Cases - Add Person to a Case",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<CasesCreateANoteResponse>> CasesCreateANote(CasesCreateANoteRequest casesCreateANoteRequest)
        {

            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesCreateANoteResponse>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/notes?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                string requestBody = JsonConvert.SerializeObject(casesCreateANoteRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    var responseObj = JsonConvert.DeserializeObject
                        <CasesCreateANoteResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "POST",
                    ApiName = "Cases - Create a Note",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "POST",
                            ApiName = "Cases - Create a Note",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<CasesUpdateACaseResponse>> CasesUpdateACase(string caseId, CasesUpdateACaseRequest casesUpdateACaseRequest)
        {

            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesUpdateACaseResponse>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/cases?id={caseId}&" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                string requestBody = JsonConvert.SerializeObject(casesUpdateACaseRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    var responseObj = JsonConvert.DeserializeObject
                        <CasesUpdateACaseResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "PUT",
                    ApiName = "Cases - Update a Case",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "PUT",
                            ApiName = "Cases - Update a Case",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<CasesUpdateTheCaseWorkerResponse>> CasesUpdateTheCaseWorker(string caseId, CasesUpdateTheCaseWorkerRequest casesUpdateTheCaseWorkerRequest)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesUpdateTheCaseWorkerResponse>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/cases/case-worker?id={caseId}&" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                string requestBody = JsonConvert.SerializeObject(casesUpdateTheCaseWorkerRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    var responseObj = JsonConvert.DeserializeObject
                        <CasesUpdateTheCaseWorkerResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "PUT",
                    ApiName = "Cases - Update the Case Worker",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "PUT",
                            ApiName = "Cases - Update the Case Worker",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<CasesUpdateATaskResponse>> CasesUpdateATask(CasesUpdateATaskRequest casesUpdateATaskRequest)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesUpdateATaskResponse>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/tasks?id={casesUpdateATaskRequest.id}&" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                string requestBody = JsonConvert.SerializeObject(casesUpdateATaskRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    var responseObj = JsonConvert.DeserializeObject
                        <CasesUpdateATaskResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "PUT",
                    ApiName = "Cases - Update a Task",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "PUT",
                            ApiName = "Cases - Update a Task",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<List<CasesListDocumentEntitiesResponse>>> CasesListDocumentEntities(string caseId)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<List<CasesListDocumentEntitiesResponse>>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/entities?case={caseId}&" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

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
                    var responseObj = JsonConvert.DeserializeObject
                        <List<CasesListDocumentEntitiesResponse>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Cases - List Document Entities",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Cases - List Document Entities",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<CasesUpdateADocumentEntityResponse>> CasesUpdateADocumentEntity(CasesUpdateADocumentEntityRequest casesUpdateADocumentEntityRequest)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesUpdateADocumentEntityResponse>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/entities?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                string requestBody = JsonConvert.SerializeObject(casesUpdateADocumentEntityRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject <CasesUpdateADocumentEntityResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "PUT",
                    ApiName = "Cases - Update a Document Entity",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "PUT",
                            ApiName = "Cases - Update a Document Entity",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<CasesCompleteATaskResponse>> CasesCompleteATask(string taskId)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesCompleteATaskResponse>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/tasks/complete?id={taskId}&" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

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
                    var responseObj = JsonConvert.DeserializeObject
                        <CasesCompleteATaskResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Cases - Complete a Task",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Cases - Complete a Task",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<CasesUpdateTheFeeEarnerResponse>> CasesUpdateTheFeeEarner(string caseId, CasesUpdateTheFeeEarnerRequest casesUpdateTheFeeEarnerRequest)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<CasesUpdateTheFeeEarnerResponse>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/cases/fee-earner?id={caseId}&" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";
                
                string requestBody = JsonConvert.SerializeObject(casesUpdateTheFeeEarnerRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    var responseObj = JsonConvert.DeserializeObject
                        <CasesUpdateTheFeeEarnerResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "PUT",
                    ApiName = "Cases - Update the Fee Earner",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "PUT",
                            ApiName = "Cases - Update the Fee Earner",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<List<CasesGetBillableInfoByCaseResponse>>> CasesGetBillableInfoByCase(string caseId)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<List<CasesGetBillableInfoByCaseResponse>>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/cases/billable?case={caseId}&" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

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
                    var responseObj = JsonConvert.DeserializeObject
                        <List<CasesGetBillableInfoByCaseResponse>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Cases - Gets billable information by case number",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Cases - Gets billable information by case number",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<List<QuoteGetCompanyCustomQuoteSituationsResponse>>> QuoteGetCompanyCustomQuoteSituations ()
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<List<QuoteGetCompanyCustomQuoteSituationsResponse>>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/quote/situation?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

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
                    var responseObj = JsonConvert.DeserializeObject
                        <List<QuoteGetCompanyCustomQuoteSituationsResponse>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Quote - Get Company Custom Quote Situations",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Quote - Get Company Custom Quote Situations",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<QuoteCalcCreateAQuoteForAPanelResponse>> QuoteCalcCreateAQuoteForAPanel (QuoteCalcCreateAQuoteForAPanelRequest quoteCalcCreateAQuoteForAPanelRequest)
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<QuoteCalcCreateAQuoteForAPanelResponse>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/quote-calc/panel?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                string requestBody = JsonConvert.SerializeObject(quoteCalcCreateAQuoteForAPanelRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    var responseObj = JsonConvert.DeserializeObject
                        <QuoteCalcCreateAQuoteForAPanelResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "POST",
                    ApiName = "QuoteCalc - Create a Quote for a Panel",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "POST",
                            ApiName = "QuoteCalc - Create a Quote for a Panel",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<List<UsersListUsersEmployeesResponse>>> UsersListUsersEmployees()
        {
            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<List<UsersListUsersEmployeesResponse>>();

            try
            {
                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/users/employees?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

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
                    var responseObj = JsonConvert.DeserializeObject
                        <List<UsersListUsersEmployeesResponse>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                }

                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "Users - List Users/Employees",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "Users - List Users/Employees",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<GetPersonByIdResponse>> GetPersonById(string personId)
        {

            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<GetPersonByIdResponse>();
            try
            {

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/people/person?id={personId}" +
                    $"&key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject<GetPersonByIdResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "People - Get a Person Card",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "People - Get a Person Card",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }
        }

        public async Task<ApiResultDto<GetPersonByEmailResponse>> GetPersonByEmail (GetPersonByEmailRequest getPersonByEmailRequest)
        {

            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<GetPersonByEmailResponse>();
            try
            {

                string email = getPersonByEmailRequest.Email;

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/people/person/byemail?email={email}" +
                    $"&key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject<List<EmailPersonDetails>>(responseData);
                    var getPersonsByEmailResponse = new GetPersonByEmailResponse();
                    getPersonsByEmailResponse.EmailPersons = responseObj;

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = getPersonsByEmailResponse;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "People - Get Person Card(s) by Email",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "People - Get Person Card(s) by Email",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<GetPersonByEmailV2Response>> GetPersonByEmailV2(GetPersonByEmailV2Request getPersonByEmailV2Request)
        {

            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<GetPersonByEmailV2Response>();
            try
            {

                string email = getPersonByEmailV2Request.Email;

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/people/person/byemail?email={email}" +
                    $"&key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject<List<EmailPersonDetailsV2>>(responseData);

                    foreach (var obj in responseObj)
                    {

                        if (obj.case_history != null)
                        {
                            obj.case_number = obj.case_history.Count > 0 ? obj.case_history.Count : 0;
                        }    
                        obj.case_history = null; // Clear case history to avoid sending unnecessary data
                    }
                    responseObj = responseObj.OrderByDescending(x => x.case_number).ToList();

                    var getPersonsByEmailV2Response = new GetPersonByEmailV2Response();
                    getPersonsByEmailV2Response.EmailPersons = responseObj;

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = getPersonsByEmailV2Response;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "GET",
                    ApiName = "People - Get Person Card(s) by Email v2",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "GET",
                            ApiName = "People - Get Person Card(s) by Email v2",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<PeopleCreateAPersonCardResponse>> PeopleCreateAPersonCard(PeopleCreateAPersonCardRequest createPersonCardRequest)
        {

            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<PeopleCreateAPersonCardResponse>();

            try
            {

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/people/person?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                string requestBody = JsonConvert.SerializeObject(createPersonCardRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    var responseObj = JsonConvert.DeserializeObject
                        <PeopleCreateAPersonCardResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "POST",
                    ApiName = "People - Create a Person Card",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "POST",
                            ApiName = "People - Create a Person Card",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }

        }

        public async Task<ApiResultDto<PeopleAddRelationshipToPersonResponse>> PeopleAddRelationshipToPerson(PeopleAddRelationshipToPersonRequest addRelationshipRequest)
        {

            ApiLogging apiLogging = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<PeopleAddRelationshipToPersonResponse>();

            try
            {

                endpoint = $"{CompleteASAPConstants.HoowlaApiEndpointV2}/people/person/relationships?" +
                    $"key={CompleteASAPConstants.HoowlaApiKey}&user={CompleteASAPConstants.HoowlaRoxusEmail}";

                string requestBody = JsonConvert.SerializeObject(addRelationshipRequest);

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
                    var responseObj = JsonConvert.DeserializeObject
                        <PeopleAddRelationshipToPersonResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                // HANDLE LOGGING TO DATABASE
                apiLogging = new ApiLogging()
                {
                    Id = Guid.NewGuid().ToString(),
                    Response = responseData,
                    ApplicationName = "Hoowla",
                    CustomerName = "CompleteASAP",
                    Status = (int)response.StatusCode + " " + response.StatusCode,
                    CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                    HttpMethod = "POST",
                    ApiName = "People - Add Relationship to Person",
                    Endpoint = endpoint
                };
                return apiResult;
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using var reader = new StreamReader(responseStream);
                        string responseData = reader.ReadToEnd();
                        apiLogging = new ApiLogging()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Response = responseData,
                            ApplicationName = "Hoowla",
                            CustomerName = "CompleteASAP",
                            Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                            CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                            HttpMethod = "POST",
                            ApiName = "People - Create a Person Card",
                            Endpoint = endpoint
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (apiLogging != null)
                {
                    await _loggingRepository.CreateApiLogging(apiLogging);
                }

            }
        }

    }

}
