using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.CompanyHouse;
using RoxusZohoAPI.Models.Zoho.Custom;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using RoxusZohoAPI.Services.CompaniesHouse;
using RoxusZohoAPI.Services.Zoho;
using RoxusZohoAPI.Services.Zoho.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ZohoCrmController : ControllerBase
    {
        private readonly IZohoUsrnService _zohoUsrnService;
        private readonly IZohoTitleService _zohoTitleService;
        private readonly IZohoContactService _zohoContactService;
        private readonly IZohoAccountService _zohoAccountService;
        private readonly IZohoNoteService _zohoNoteService;
        private readonly IZohoAuthService _zohoAuthService;
        private readonly ICompaniesHouseService _companiesHouseService;

        public ZohoCrmController(IZohoUsrnService zohoUsrnService, IZohoTitleService zohoTitleService, IZohoContactService zohoContactService, IZohoAccountService zohoAccountService, IZohoNoteService zohoNoteService, ICompaniesHouseService companiesHouseService = null, IZohoAuthService zohoAuthService = null)
        {
            _zohoUsrnService = zohoUsrnService;
            _zohoTitleService = zohoTitleService;
            _zohoContactService = zohoContactService;
            _zohoAccountService = zohoAccountService;
            _zohoNoteService = zohoNoteService;
            _companiesHouseService = companiesHouseService;
            _zohoAuthService = zohoAuthService;
        }

        #region Token

        [HttpPost("token")]
        public async Task<IActionResult> GetAccessToken()
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                var getTokenResult = await _zohoAuthService.GetAccessToken(apiKey);

                if (getTokenResult == null)
                {
                    return BadRequest(apiResult);
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.MSG_200;
                apiResult.Data = getTokenResult.AccessToken;

                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        #endregion

        #region USRN
        [HttpGet("usrns/{usrnId}")]
        public async Task<IActionResult> GetUsrnById(string usrnId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<UsrnResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoUsrnService.GetUsrnById(apiKey, usrnId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.NoContent:
                        return NoContent();
                    default:
                        return BadRequest(apiResult.Message);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult.Message);
            }
        }
        #endregion

        #region Title
        [HttpGet("titles/{titleId}")]
        public async Task<IActionResult> GetTitleById(string titleId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            var apiResult = new ApiResultDto<TitleResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoTitleService.GetTitleById(apiKey, titleId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.NoContent:
                        return NoContent();
                    default:
                        return BadRequest(apiResult.Message);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult.Message);
            }
        }

        [HttpPost("titles/upsert")]
        public async Task<IActionResult> UpsertTitle(TitleForCreation titleForCreation)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoTitleService.UpsertTitle(apiKey, titleForCreation);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.Unauthorize:
                        return Unauthorized(apiResult.Message);
                    default:
                        return BadRequest(apiResult.Message);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult.Message);
            }
        }
        [HttpPut("titles/{titleId}")]
        public async Task<IActionResult> UpdateTitle(string titleId, [FromBody] UpdateRequest updateRequest)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoTitleService.UpdateTitle(apiKey, titleId, updateRequest);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.NoContent:
                        return NoContent();
                    default:
                        return BadRequest(apiResult.Message);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult.Message);
            }
        }
        [HttpPut("titles/linking-uprn")]
        public async Task<IActionResult> TitleLinkingUprn(Title_LinkingUprnRequest title_LinkingUprn)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            var apiResult = new ApiResultDto<UpsertResponse<LinkingResponse>>()
            {
                Code = ResultCode.OK,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoTitleService.Title_LinkingWithUPRNs(apiKey, title_LinkingUprn.id, title_LinkingUprn.uprnIds);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.Unauthorize:
                        return Unauthorized(apiResult.Message);
                    default:
                        return BadRequest(apiResult.Message);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult.Message);
            }
        }

        [HttpPost("titles/upload-file")]
        public async Task<IActionResult> TitleUploadFile(Title_UploadFileRequest uploadRequest)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<UploadResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoTitleService.Title_UploadAttachments(apiKey, uploadRequest.TitleId, 
                    uploadRequest.FileName, uploadRequest.FileContent);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.Unauthorize:
                        return Unauthorized(apiResult.Message);
                    default:
                        return BadRequest(apiResult.Message);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult.Message);
            }
        }
        #endregion

        #region Contact

        [HttpPost("contacts/search-by-email")]
        public async Task<IActionResult> SearchContactByEmail(SearchEmailRequest emailRequest)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<ContactResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoContactService.SearchContactByEmail(apiKey, emailRequest.email);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpPost("contacts/search-by-name")]
        public async Task<IActionResult> SearchContactByName(SearchContactByNameRequest nameRequest)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<ContactResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoContactService.SearchContactByFirstNameAndLastName
                    (apiKey, nameRequest.FirstName, nameRequest.LastName);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpPut("contacts/{contactId}")]
        public async Task<IActionResult> UpdateContact(string contactId, ContactForUpdate contactForUpdate)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<UpdateResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoContactService.UpdateContact(apiKey, contactId, contactForUpdate);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpGet("contacts/{contactId}")]
        public async Task<IActionResult> GetContactById(string contactId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<ContactResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoContactService.GetContactById(apiKey, contactId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        #endregion

        #region Account

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<GetResponse<AccountResponse>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoAccountService.GetAccounts(apiKey, "");
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpGet("accounts/{accountId}")]
        public async Task<IActionResult> GetAccountById(string accountId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<AccountResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoAccountService.GetAccountById(apiKey, accountId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpPost("accounts/search-by-company-number")]
        public async Task<IActionResult> SearchAccountByCompanyNumber
            ([FromBody] SearchAccountByCompanyNumberRequest searchAccountByCompanyNumber)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<SearchResponse<AccountResponse>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoAccountService
                    .SearchAccountByCompanyRegistration(apiKey, searchAccountByCompanyNumber.CompanyNumber);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpPost("accounts/search-by-company-name")]
        public async Task<IActionResult> SearchAccountByCompanyName
            ([FromBody] SearchAccountByCompanyNameRequest searchAccountByCompanyName)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<SearchResponse<AccountResponse>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoAccountService
                    .SearchAccountByAccountName(apiKey, searchAccountByCompanyName.CompanyName);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpPost("accounts/search-by-coql")]
        public async Task<IActionResult> SearchAccountByCOQL
            ([FromBody] SearchAccountByCompanyNameRequest searchAccountByCompanyName)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<SearchResponse<AccountResponse>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string accountName = searchAccountByCompanyName.CompanyName;
                string coql = $"select Account_Name, Website, Industry from Accounts  where Account_Name = '{accountName}' and Territories = '4368622000008404531'";

                var coqlRequest = new CoqlRequest()
                {
                    select_query = coql
                };

                apiResult = await _zohoAccountService
                    .SearchAccountByCOQL(apiKey, coqlRequest);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpPut("accounts/{accountId}")]
        public async Task<IActionResult> UpdateAccount(string accountId, AccountForUpdate accountForUpdate)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<UpdateResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoAccountService.UpdateAccount(apiKey, accountId, accountForUpdate);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        #endregion

        #region Note

        [HttpPost("notes")]
        public async Task<IActionResult> CreateNote([FromBody] NoteForCreation noteForCreation)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                /*
                var noteForCreation = new NoteForCreation()
                {
                    Note_Title = noteTitle,
                    Note_Content = noteContent,
                    Parent_Id = openreachId,
                    se_module = "Openreach"
                };
                */

                var upsertRequest = new UpsertRequest<NoteForCreation>();
                upsertRequest.data.Add(noteForCreation);
                var createNoteResult = await _zohoNoteService.CreateNote(apiKey, upsertRequest);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        #endregion

        #region Company House

        [HttpPost("companieshouse")]
        public async Task<IActionResult> SearchCompaniesHouse([FromBody] SearchCompaniesRequest searchCompaniesRequest)
        {
            var apiResult = new ApiResultDto<SearchCompaniesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {

                string query = searchCompaniesRequest.Query;
                if (string.IsNullOrEmpty(query))
                {
                    return BadRequest(apiResult);
                }

                apiResult = await _companiesHouseService.GetCompanies(query);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        #endregion

    }
}
