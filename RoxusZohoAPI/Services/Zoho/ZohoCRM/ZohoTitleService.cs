using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using RoxusZohoAPI.Repositories;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public class ZohoTitleService : IZohoTitleService
    {
        private readonly IRoxusLoggingRepository _roxusRepository;
        private readonly IZohoAuthService _zohoAuthService;


        private readonly HttpClient _httpClient = new HttpClient(
         new HttpClientHandler()
         {
             AutomaticDecompression = DecompressionMethods.GZip
         });

        private const string TitleModule = "Titles";

        public ZohoTitleService(IRoxusLoggingRepository roxusRepository, IZohoAuthService zohoAuthService)
        {
            _roxusRepository = roxusRepository;
            _zohoAuthService = zohoAuthService;
            _httpClient.Timeout = new TimeSpan(0, 0, 1800);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<ApiResultDto<TitleResponse>> GetTitleById(string apiKey, string titleId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<TitleResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/{TitleModule}/{titleId}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();
                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = JObject.Parse(responseData);
                        var responseObj = JsonConvert.DeserializeObject<TitleResponse>(json["data"][0].ToString());

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_200;
                    }
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
                        HttpMethod = "GET",
                        ApiName = ZohoConstants.ZCRM_GET_USRN_BY_ID,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "GET",
                                ApiName = ZohoConstants.ZCRM_GET_TITLE_BY_ID,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpsertTitle(string apiKey, TitleForCreation titleForCreation)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/{TitleModule}/upsert";
                var upserModel = new UpsertRequest<TitleForCreation>();
                upserModel.data.Add(titleForCreation);
                upserModel.duplicate_check_fields.Add("Name");
                string requestBody = JsonConvert.SerializeObject(upserModel);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    // response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

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
                        ApiName = ZohoConstants.ZCRM_UPSERT_TITLE,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "POST",
                                ApiName = ZohoConstants.ZCRM_UPSERT_TITLE,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpsertTitleForOpenreach(string apiKey, OpenreachTitleForCreation titleForCreation)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/{TitleModule}/upsert";
                var upserModel = new UpsertRequest<OpenreachTitleForCreation>();
                upserModel.data.Add(titleForCreation);
                upserModel.duplicate_check_fields.Add("Name");
                string requestBody = JsonConvert.SerializeObject(upserModel);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    // response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

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
                        ApiName = ZohoConstants.ZCRM_UPSERT_TITLE,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "POST",
                                ApiName = ZohoConstants.ZCRM_UPSERT_TITLE,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateTitle(string apiKey, string titleId, UpdateRequest updateRequest)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/{TitleModule}/{titleId}";
                string requestBody = JsonConvert.SerializeObject(updateRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    // response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

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
                        HttpMethod = "PUT",
                        ApiName = ZohoConstants.ZCRM_UPDATE_TITLE,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "PUT",
                                ApiName = ZohoConstants.ZCRM_UPDATE_TITLE,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<UpsertResponse<LinkingResponse>>> Title_LinkingWithUPRNs(string apiKey, string titleId, string uprnIds)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpsertResponse<LinkingResponse>>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/{TitleModule}";

                // Handle Object for Linking
                var upsertRequest = new UpsertRequest<Title_LinkingUPRNs>();
                var titleLinkingUprn = new Title_LinkingUPRNs();
                titleLinkingUprn.id = titleId;
                string[] uprns = uprnIds.Split(",");
                foreach (var uprn in uprns)
                {
                    var relatedUprn = new Title_RelatedUprn()
                    {
                        UPRN = uprn
                    };
                    titleLinkingUprn.Related_UPRN.Add(relatedUprn);
                }
                upsertRequest.data.Add(titleLinkingUprn);

                string requestBody = JsonConvert.SerializeObject(upsertRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<UpsertResponse<LinkingResponse>>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_200;
                    }

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
                        HttpMethod = "PUT",
                        ApiName = ZohoConstants.ZCRM_TITLE_LINKING_WITH_UPRN,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "PUT",
                                ApiName = ZohoConstants.ZCRM_TITLE_LINKING_WITH_UPRN,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<UpsertResponse<LinkingResponse>>> Title_LinkingWithUSRN(string apiKey, string titleId, string usrnId)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpsertResponse<LinkingResponse>>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{TitleModule}";
                // Handle Object for Linking
                var upsertRequest = new UpsertRequest<Title_LinkingUSRN>();
                var titleLinkingUsrn = new Title_LinkingUSRN();
                titleLinkingUsrn.id = titleId;
                titleLinkingUsrn.USRN = usrnId;
                upsertRequest.data.Add(titleLinkingUsrn);

                string requestBody = JsonConvert.SerializeObject(upsertRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");
                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();
                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<UpsertResponse<LinkingResponse>>(responseData);
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_200;
                    }
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
                        HttpMethod = "PUT",
                        ApiName = ZohoConstants.ZCRM_TITLE_LINKING_WITH_USRN,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "PUT",
                                ApiName = ZohoConstants.ZCRM_TITLE_LINKING_WITH_USRN,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<UpsertResponse<LinkingResponse>>> Title_LinkingWithOwners(string apiKey, string titleId, string contactIds, string accountIds)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UpsertResponse<LinkingResponse>>();
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                endpoint = $"{appConfig.EndPoint}/{TitleModule}";

                // Handle Object for Linking
                var upsertRequest = new UpsertRequest<Title_LinkingOwners>();
                var titleLinkingOwners = new Title_LinkingOwners();
                titleLinkingOwners.id = titleId;
                string[] contacts = contactIds.Split(",");
                int contactLength = contacts.Length;
                string[] accounts = contactIds.Split(",");
                int accountLength = accounts.Length;

                if (contactLength <= accountLength)
                {
                    for (int i = 0; i < contactLength; i++)
                    {
                        var relatedContact = new Title_RelatedContacts();
                        relatedContact.Contact_Name = contacts[i];
                        relatedContact.Account_Name = accounts[i];
                        titleLinkingOwners.Related_Contacts.Add(relatedContact);
                    }
                    for (int i = contactLength; i < accountLength; i++)
                    {
                        var relatedContact = new Title_RelatedContacts();
                        relatedContact.Account_Name = accounts[i];
                        titleLinkingOwners.Related_Contacts.Add(relatedContact);
                    }
                }
                else if (contactLength > accountLength)
                {
                    for (int i = 0; i < accountLength; i++)
                    {
                        var relatedContact = new Title_RelatedContacts();
                        relatedContact.Contact_Name = contacts[i];
                        relatedContact.Account_Name = accounts[i];
                        titleLinkingOwners.Related_Contacts.Add(relatedContact);
                    }
                    for (int i = accountLength; i < contactLength; i++)
                    {
                        var relatedContact = new Title_RelatedContacts();
                        relatedContact.Contact_Name = contacts[i];
                        titleLinkingOwners.Related_Contacts.Add(relatedContact);
                    }
                }

                upsertRequest.data.Add(titleLinkingOwners);
                string requestBody = JsonConvert.SerializeObject(upsertRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<UpsertResponse<LinkingResponse>>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_200;
                    }

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
                        HttpMethod = "PUT",
                        ApiName = ZohoConstants.ZCRM_TITLE_LINKING_WITH_OWNERS,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "PUT",
                                ApiName = ZohoConstants.ZCRM_TITLE_LINKING_WITH_UPRN,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<UploadResponse>> Title_UploadAttachments(string apiKey, string titleId, string fileName,string fileContent)
        {
            ApiLogging apiLogging = null;
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<UploadResponse>() 
            {
                Code = ResultCode.OK,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }
                endpoint = $"{appConfig.EndPoint}/{TitleModule}/{titleId}/Attachments";
                byte[] fileBytes = Convert.FromBase64String(fileContent);
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), "file", fileName);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");
                using (var response = await _httpClient.PostAsync(endpoint, form))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();
                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<UploadResponse>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        apiResult.Code = ResultCode.NoContent;
                        apiResult.Message = ZohoConstants.MSG_200;
                    }

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
                        ApiName = ZohoConstants.ZCRM_TITLE_UPLOAD_ATTACHMENT,
                        Endpoint = endpoint
                    };
                    return apiResult;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse badResponse = (HttpWebResponse)ex.Response;
                using (Stream responseStream = badResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseData = reader.ReadToEnd();
                            apiLogging = new ApiLogging()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Response = responseData,
                                ApplicationName = appConfig.Platform,
                                CustomerName = appConfig.CustomerName,
                                ApplicationId = appConfig.Id,
                                Status = (int)badResponse.StatusCode + " " + badResponse.StatusCode,
                                CreatedDate = DateTimeHelpers.ConvertDateTimeToString(DateTime.UtcNow),
                                HttpMethod = "POST",
                                ApiName = ZohoConstants.ZCRM_TITLE_UPLOAD_ATTACHMENT,
                                Endpoint = endpoint
                            };
                        }
                    }
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
                await _roxusRepository.CreateApiLogging(apiLogging);
            }
        }

        public async Task<ApiResultDto<SearchResponse<SearchTitleResponse>>> 
            SearchTitleByTitleNumber(string apiKey, string titleNumber)
        {
            AppConfiguration appConfig = null;
            string endpoint = string.Empty;
            var apiResult = new ApiResultDto<SearchResponse<SearchTitleResponse>>()
            {
                Message = ZohoConstants.MSG_400,
                Code = ResultCode.BadRequest
            };
            try
            {
                appConfig = await _zohoAuthService.GetAccessToken(apiKey);

                if (appConfig == null)
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = ZohoConstants.MSG_401;
                    return apiResult;
                }

                string encodedTitleNumber = HttpUtility.UrlEncode(titleNumber);
                endpoint = $"{appConfig.EndPoint}/{TitleModule}/search?criteria=(Name:equals:{encodedTitleNumber})";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {appConfig.AccessToken}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SearchResponse<SearchTitleResponse>>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZohoConstants.MSG_204;
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
            }
        }
        
    }
}
