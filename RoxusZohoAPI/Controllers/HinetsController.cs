using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.Custom;
using RoxusZohoAPI.Models.Zoho.ZohoBooks;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using RoxusZohoAPI.Services.Hinets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{
    [Route("api/hi-nets")]
    [ApiController]
    public class HinetsController : ControllerBase
    {
        private readonly IHinetsCrmService _hinetsCrmService;
        private readonly IHinetsBooksService _hinetsBookService;
        private readonly IHinetsProjectsService _hinetsProjectsService;
        private readonly IHinetsCustomService _hinetsCustomService;

        public HinetsController(IHinetsBooksService hinetsBookService, IHinetsProjectsService hinetsProjectsService, IHinetsCustomService hinetsCustomService, IHinetsCrmService hinetsCrmService)
        {
            _hinetsBookService = hinetsBookService;
            _hinetsProjectsService = hinetsProjectsService;
            _hinetsCustomService = hinetsCustomService;
            _hinetsCrmService = hinetsCrmService;
        }

        #region ZohoCRM
        [HttpGet("deals/{dealId}/Attachments")]
        public async Task<IActionResult> GetDealAttachments(string dealId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<GetListOfAttachments>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsCrmService.GetDealAttachments(apiKey, dealId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.NoContent:
                        return NoContent();
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpGet("deals/{dealId}/Zoho_Projects")]
        public async Task<IActionResult> GetDealRelatedProjects(string dealId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<int>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400,
                Data = 0
            };
            try
            {
                apiResult = await _hinetsCrmService.GetNumberOfProjects(apiKey, dealId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.NoContent:
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

        #region ZohoBooks

        #region Purchase Orders
        [HttpGet("purchaseorders/{poId}")]
        public async Task<IActionResult> GetPurchaseOrderById(string poId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<PurchaseOrderResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsBookService.GetPoById(apiKey, poId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.NoContent:
                        return NoContent();
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }
        [HttpGet("purchaseorders/pdf/{poId}")]
        public async Task<IActionResult> DownloadPoPdf(string poId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsBookService.DownloadPoPDF(apiKey, poId, "");
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.NoContent:
                        return NoContent();
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

        #region Estimates
        [HttpGet("estimates/{estimateId}")]
        public async Task<IActionResult> GetEstimateById(string estimateId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<EstimateResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsBookService.GetEstimateById(apiKey, estimateId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.NoContent:
                        return NoContent();
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpGet("estimates/search-by-dealid/{dealId}")]
        public async Task<IActionResult> SearchEstimateByDealId(string dealId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<SearchEstimateResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsBookService.SearchEstimateByDealId(apiKey, dealId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.NoContent:
                        return NoContent();
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
        #endregion

        #region Zoho Projects
        #region Projects
        /*
        [HttpPost("projects/{projectId}/upload-document")]
        public async Task<IActionResult> UploadDocumentToProject(string projectId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsProjectsService.UploadFileToProject(apiKey, "", projectId);
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
        */
        #endregion
        #endregion

        #region Custom Functions

        [HttpPost("custom/upload-attachments")]
        public async Task<IActionResult> UploadAttachmentsToProject([FromBody] UploadDocumentsToProject uploadDocuments)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsCustomService.UploadDrawingsToProject(uploadDocuments);
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

        [HttpPost("custom/upload-estimate")]
        public async Task<IActionResult> UploadEstimateToProject([FromBody] UploadDocumentsToProject uploadDocuments)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsCustomService.UploadEstimateToProject(uploadDocuments);
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

        [HttpPost("custom/upload-estimate-to-deal")]
        public async Task<IActionResult> UploadApprovedEstimateToDeal([FromBody] UploadApprovedEstimateToDeal uploadDocuments)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsCustomService.UploadApprovedEstimateToDeal(uploadDocuments);
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

        [HttpPost("custom/upload-pos")]
        public async Task<IActionResult> UploadPosToProject([FromBody] UploadDocumentsToProject uploadDocuments)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsCustomService.UploadPoToProject(uploadDocuments);
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

        [HttpGet("custom/projects/{projectId}/update-task-dates")]
        public async Task<IActionResult> UpdateTaskDates(string projectId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                var updateTaskDates = new UpdateTaskDates()
                {
                    ProjectId = projectId,
                    ZohoProjectsApiKey = "aGktbmV0czp6b2hvcHJvamVjdHM="
                };
                apiResult = await _hinetsCustomService.UpdateTaskDates(updateTaskDates);
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

        [HttpGet("custom/projects/{projectId}/reupload-po-documents")]
        public async Task<IActionResult> ReUploadPoDocuments(string projectId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                var uploadDocuments = new UploadDocumentsToProject()
                {
                    ProjectId = projectId,
                    ZohoProjectsApiKey = "aGktbmV0czp6b2hvcHJvamVjdHM=",
                    ZohoCrmApiKey = "aGktbmV0czp6b2hvY3Jt",
                    ZohoBooksApiKey = "aGktbmV0czp6b2hvYm9va3M="
                };
                apiResult = await _hinetsCustomService.ReUploadPoDocuments(uploadDocuments);
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

        [HttpPost("custom/upload-accommodation")]
        public async Task<IActionResult> UploadAccomodation([FromBody] UploadAccommodationRequest accommodationRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsCustomService
                    .DownloadAccommodationDocumentsAndUploadToProject(accommodationRequest);
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

        [HttpPost("custom/create-excel-and-email")]
        public async Task<IActionResult> CreateItemsExcelFile([FromBody] GetItemsRequest itemsRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _hinetsCustomService.CreateItemsExcelFileAndSendToAndy(itemsRequest);
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

        [HttpPost("custom/update-item-price")]
        public async Task<IActionResult> UpdateItemPrice([FromBody] HinetsUpdateItemRequest updateItemRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            try
            {
                string updatedContents = updateItemRequest.UpdatedContents;
                apiResult = await _hinetsCustomService.UpdateItemPrice(updatedContents);
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

        [HttpGet("custom/create-event-for-task")]
        public async Task<IActionResult> CreateEventForTask()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string projectId = Request.Query["projectId"];
                string taskId = Request.Query["taskId"];

                var request = new AddProjectEventForTask()
                {
                    projectId = projectId,
                    taskId = taskId
                };

                apiResult = await _hinetsCustomService.AddProjectEventForTask(request);
                return Ok(apiResult);

            }
            catch (Exception)
            {
                return Ok(apiResult);
            }
        }

        #endregion

    }
}
