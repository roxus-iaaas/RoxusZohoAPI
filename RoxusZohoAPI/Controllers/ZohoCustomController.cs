using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RoxusWebAPI.Services.Zoho.ZohoProjects;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho;
using RoxusZohoAPI.Models.Zoho.Custom;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using RoxusZohoAPI.Repositories;
using RoxusZohoAPI.Services.Zoho;
using RoxusZohoAPI.Services.Zoho.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{
    [Route("api/custom")]
    [ApiController]
    public class ZohoCustomController : ControllerBase
    {
        private readonly IZohoCustomService _zohoCustomService;
        private readonly IZohoProjectService _zohoProjectService;

        public ZohoCustomController(IZohoCustomService zohoCustomService, IZohoProjectService zohoProjectService, 
            IRoxusLoggingRepository roxusLoggingRepository)
        {
            _zohoCustomService = zohoCustomService;
            _zohoProjectService = zohoProjectService;
        }

        // CUSTOM

        [HttpPost("create-title-from-usrn/{usrnId}")]
        public async Task<IActionResult> CreateUnregisteredFromUsrn(string usrnId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoCustomService.CreateUnregisteredTitle(apiKey, usrnId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.Unauthorize:
                        return Unauthorized(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpPost("create-unregistered-task/{titleId}")]
        public async Task<IActionResult> CreateUnregisteredTask(string titleId, [FromBody] ZohoKeyRequest keyRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoCustomService.CreateUnregisteredTask(keyRequest.crmKey, keyRequest.projectKey, titleId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.Unauthorize:
                        return Unauthorized(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }
        
        [HttpPost("process-postcard-mailing")]
        public async Task<IActionResult> ProcessPostcardMailing([FromBody] PostcardRequest postcardRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = "FAILED",
            };
            try
            {
                string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
                apiResult = await _zohoCustomService.ProcessPostcardMailing(
                    postcardRequest.crmKey, postcardRequest.projectKey, postcardRequest.taskUrl, apiKey);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.Unauthorize:
                        return Unauthorized(apiResult);
                    default:
                        return BadRequest(apiResult.Message);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }
        
        [HttpPost("trenches-task/{projectId}/{taskId}")]
        public async Task<IActionResult> InsertTrenchesTaskToDb(string projectId, string taskId, [FromBody] ZohoKeyRequest keyRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoProjectService.InsertTaskToTable(keyRequest.projectKey, taskId, projectId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.Unauthorize:
                        return Unauthorized(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }
        
        [HttpDelete("trenches-task/{projectId}/{taskId}")]
        public async Task<IActionResult> DeleteTrenchesTaskFromDb(string projectId, string taskId, [FromBody] ZohoKeyRequest keyRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoProjectService.DeleteTaskFromTable(keyRequest.projectKey, taskId, projectId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.Unauthorize:
                        return Unauthorized(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult.Message);
            }
        }
        
        [HttpPost("trenches-task/{projectId}/{taskId}/check-letter-valid")]
        public async Task<IActionResult> CheckTaskValidForSendingLetter(string projectId, string taskId, [FromBody] ZohoKeyRequest keyRequest)
        {
            var apiResult = new ApiResultDto<CheckTaskValidResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoProjectService.CheckTaskValid(keyRequest.projectKey, taskId, projectId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.Unauthorize:
                        return Unauthorized(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult.Message);
            }
        }

        [HttpGet("trenches/send-letter")]
        public async Task<IActionResult> ProcessSendingLetter()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {

                string projectId = Request.Query["projectId"];
                string tasklistId = Request.Query["tasklistId"];
                string taskId = Request.Query["taskId"];

                var apiTransactionQueue = new ApiTransactionQueue()
                {
                    ProjectId = projectId,
                    TasklistId = tasklistId,
                    TaskId = taskId,
                    Status = 0,
                    TransactionName = "Send Letter",
                    TransactionType = 2,
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                };

                apiResult = await _zohoCustomService.InsertTransactionQueue(apiTransactionQueue);
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return Ok(apiResult);
            }
        }

        [HttpGet("trenches/send-openreach-letter")]
        public async Task<IActionResult> ProcessSendingORLetter()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string projectId = Request.Query["projectId"];
                string tasklistId = Request.Query["tasklistId"];
                string taskId = Request.Query["taskId"];

                var apiTransactionQueue = new ApiTransactionQueue()
                {
                    ProjectId = projectId,
                    TasklistId = tasklistId,
                    TaskId = taskId,
                    Status = 0,
                    TransactionName = "Send Openreach Letter",
                    TransactionType = 1,
                    Created = DateTime.UtcNow,
                };

                apiResult = await _zohoCustomService.InsertTransactionQueue(apiTransactionQueue);
                return Ok(apiResult);

            }
            catch (Exception)
            {
                return Ok(apiResult);
            }
        }

        [HttpGet("trenches/send-openreach-letter-directly")]
        public async Task<IActionResult> ProcessSendingORLetterDirectly()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string projectId = Request.Query["projectId"];
                string tasklistId = Request.Query["tasklistId"];
                string taskId = Request.Query["taskId"];

                var sendLetterRequest = new SendLetterRequest()
                {
                    projectId = projectId,
                    taskId = taskId,
                    tasklistId = tasklistId,
                    crmKey = ZohoConstants.Trenches_CRM_Key,
                    projectKey = ZohoConstants.Trenches_Project_Key
                };

                apiResult = await _zohoCustomService.OR_ProcessSendingLetter(sendLetterRequest);
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return Ok(apiResult);
            }
        }

        [HttpGet("trenches/openreach-sync-task-to-crm/{projectId}/{taskId}")]
        public async Task<IActionResult> OR_SyncFromTaskToCRM(string projectId, string taskId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            try
            {
                apiResult = await _zohoCustomService.OR_SyncFromTaskToCRM(projectId, taskId);
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return Ok(apiResult);
            }
        }

        [HttpGet("trenches/openreach-mass-sync-task-to-crm")]
        public async Task<IActionResult> OR_MassSyncFromTaskToCRM()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            try
            {
                apiResult = await _zohoCustomService.OR_MassSyncFromTaskToCRM();
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return Ok(apiResult);
            }
        }

        [HttpGet("trenches/openreach-sync-db-to-project")]
        public async Task<IActionResult> OR_SyncDBToProject()
        {
            try
            {
                string syncResult = await _zohoCustomService.OR_SyncFromDBToProject();
                return Ok(syncResult);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + " - " + ex.StackTrace);
            }
        }

        [HttpGet("trenches/openreach-sync-crm-to-db/{openreachId}")]
        public async Task<IActionResult> OR_SyncCRMToDB(string openreachId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoCustomService.OR_SyncFromCRMToDB(openreachId);
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return Ok(apiResult);
            }
        }

        [HttpGet("trenches/openreach-mass-sync-crm-to-db")]
        public async Task<IActionResult> OR_MassSyncCRMToDB(string openreachId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoCustomService.OR_MassSyncFromCRMToDB();
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return Ok(apiResult);
            }
        }

        [HttpGet("trenches/openreach-mass-update-tasks-to-removed")]
        public async Task<IActionResult> OR_MassUpdateTasksToRemoved(string openreachId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoCustomService.OR_MassUpdateTasksToRemoved();
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return Ok(apiResult);
            }
        }

        [HttpGet("trenches/handle-transaction-queue/{queueId}")]
        public async Task<IActionResult> OR_HandleTransactionQueue(string queueId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                int.TryParse(queueId, out int queue);
                apiResult = await _zohoCustomService.HandleTransactionQueue(queue);
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return Ok(apiResult);
            }
        }

        [HttpPost("trenches/handle-returned-sm")]
        public async Task<IActionResult> OR_HandleReturnedSM()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoCustomService.HandleReturnedSM();
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return Ok(apiResult);
            }
        }

        [HttpPost("trenches/handle-register-purchase")]
        public async Task<IActionResult> HandleRegisterPurchase ()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoCustomService.HandleReturnedSM();
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return Ok(apiResult);
            }
        }

        [HttpPost("trenches/attach-letter-documents/{openreachId}")]
        public async Task<IActionResult> AttachLetterDocuments(string openreachId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {

                apiResult = await _zohoCustomService.GetLetterDocumentForOpenreach(openreachId);
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
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return Ok(apiResult);
            }
        }

    }
}
