using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RoxusZohoAPI.Entities.TrenchesReportDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.TrenchesReporting;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using RoxusZohoAPI.Services.TrenchesReporting;
using System;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{
    [Route("api/trenches-reporting")]
    [ApiController]
    public class TrenchesReportingController : ControllerBase
    {
        private readonly ITrenchesReportingService _trenchesService;
        private readonly IMapper _mapper;

        public TrenchesReportingController(ITrenchesReportingService trenchesService, IMapper mapper)
        {
            _trenchesService = trenchesService;
            _mapper = mapper;
        }

        [HttpPost("syncing")]
        public async Task<IActionResult> CreateSyncingRecord([FromBody] SyncingForCreation syncingForCreation)
        {
            try
            {
                var syncingEntity = _mapper.Map<SyncingRecord>(syncingForCreation);
                var apiResult = await _trenchesService.UpsertSyncingRecords(syncingEntity);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
                }
            }
            catch (Exception)
            {
                return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
            }
        }

        [HttpGet("syncing/task-delete")]
        public async Task<IActionResult> CreateTaskDeleteSyncing()
        {
            var apiResult = new ApiResultDto<string>();

            var emailContent = new EmailContent
            {
                Body = "",
                Subject = "Test Zoho Webhook",
                Clients = "help@roxus.io",
                Email = CommonConstants.Email_Username,
                Password = CommonConstants.Email_Password,
                SmtpPort = CommonConstants.SmtpPort,
                SmtpServer = CommonConstants.Outlook_Email_SmtpServer
            };
            try
            {
                string taskUrl = Request.Query["taskUrl"];
                emailContent.Body = $"{taskUrl}";
                
                apiResult = await _trenchesService.SyncingTaskDelete(taskUrl);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
                }
            }
            catch (Exception ex)
            {
                emailContent.Body += $"<br><br><b>Exception Details</b>:<br>{ex.Message}<br>{ex.StackTrace}";
                await EmailHelpers.SendEmail(emailContent);
                return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
            }
        }

        [HttpPost("handle-lfl")]
        public async Task<IActionResult> HandleLFL()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = TrenchesReportingConstants.MSG_OR_OWNERSHIP_400
            };

            try
            {
                apiResult = await _trenchesService.HandleLightningFibre();
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
                return BadRequest(TrenchesReportingConstants.MSG_OR_OWNERSHIP_400);
            }
        }

        [HttpPost("handle-or")]
        public async Task<IActionResult> HandleOpenReach()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                apiResult = await _trenchesService.HandleOpenReach();
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
                return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
            }
        }

        [HttpPost("handle-or-1402")]
        public async Task<IActionResult> HandleOpenReach1402()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                apiResult = await _trenchesService.HandleOpenReach1402();
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
                return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
            }
        }

        [HttpPost("or-ownerships")]
        public async Task<IActionResult> HandleOROwnerships()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                apiResult = await _trenchesService.HandleOpenReachAccounts("dHJlbmNoZXNsYXc6em9ob2NybQ==");
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
                return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
            }
        }

        [HttpPost("or-ownership-linking")]
        public async Task<IActionResult> HandleOROwnershipLinking()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                apiResult = await _trenchesService
                    .HandleOROwnershipLinking("dHJlbmNoZXNsYXc6em9ob2NybQ==");
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
                return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
            }
        }

        [HttpPost("or-access-agreement")]
        public async Task<IActionResult> CreateAccessAgreement
            ([FromBody] AccessAgreementRequest accessAgreementRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                apiResult = await _trenchesService
                    .CreateAccessAgreement(accessAgreementRequest);
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
                return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
            }
        }

        [HttpGet("handle-letter-reference/{openreachId}")]
        public async Task<IActionResult> HandleLetterReference(string openreachId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
                apiResult = await _trenchesService.HandleLetterReference(apiKey, openreachId);
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
                return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
            }
        }

        [HttpPost("check-primary-sm")]
        public async Task<IActionResult> CheckPrimarySM([FromBody] CheckPrimarySMRequest checkRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string openreachNumber = checkRequest.OpenreachNumber;

                string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
                apiResult = await _trenchesService.CheckPrimarySM(openreachNumber);
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
            }
        }

        [HttpPost("check-register-purchase")]
        public async Task<IActionResult> HandleRegisterPurchase([FromBody] RegisterPurchaseRequest registerRequest)
        {
            var apiResult = new ApiResultDto<bool>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                apiResult = await _trenchesService.HandleRegisterPurchase(registerRequest);
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(TrenchesReportingConstants.MSG_SYNCING_400);
            }
        }

    }
}
