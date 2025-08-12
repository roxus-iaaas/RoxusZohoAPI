using Microsoft.AspNetCore.Mvc;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Services.CompleteASAP;
using RoxusZohoAPI.Services.PureFinance;
using System.Threading.Tasks;
using System;
using RoxusZohoAPI.Models.CompleteASAP.Hoowla;
using System.Collections.Generic;
using Newtonsoft.Json;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Models.Nintex;
using RoxusZohoAPI.Models.Nuvoli.Halo;
using RoxusZohoAPI.Models.Zoho.ZohoDesk;
using System.IO;
using System.Text;
using RoxusZohoAPI.Services.Nintex;
using RoxusZohoAPI.Services.Zoho.ZohoDesk;
using RoxusZohoAPI.Services.Contxtus;

namespace RoxusZohoAPI.Controllers
{

    [Route("api/asap")]
    [ApiController]
    public class CompleteASAPController : ControllerBase
    {

        private readonly IHoowlaService _hoowlaService;
        private readonly INintexService _nintexService;
        private readonly IZohoDeskService _zohoDeskService;
        private readonly IContxtusService _contxtusService;

        public CompleteASAPController(IHoowlaService hoowlaService,
            INintexService nintexService, IZohoDeskService zohoDeskService, IContxtusService contxtusService)
        {
            _hoowlaService = hoowlaService;
            _nintexService = nintexService;
            _zohoDeskService = zohoDeskService;
            _contxtusService = contxtusService;
        }

        [HttpGet("person/{personId}")]
        public async Task<IActionResult> GetPersonById(string personId)
        {
            var apiResultDto = new ApiResultDto<GetPersonByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.GPI_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.GetPersonById(personId);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }
        }

        [HttpPost("people/person")]
        public async Task<IActionResult> PeopleCreateAPersonCard
            ([FromBody] PeopleCreateAPersonCardRequest request)
        {
            var apiResultDto = new ApiResultDto<PeopleCreateAPersonCardResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.PCAPC_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.PeopleCreateAPersonCard(request);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }
        }

        [HttpPost("people/person/relationships")]
        public async Task<IActionResult> PeopleAddRelationshipToPerson
            ([FromBody] PeopleAddRelationshipToPersonRequest addRelationshipRequest)
        {
            var apiResultDto = new ApiResultDto<PeopleAddRelationshipToPersonResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.PARP_400,
                Data = null
            };
            try
            {
                apiResultDto = await _hoowlaService.PeopleAddRelationshipToPerson(addRelationshipRequest);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }
                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        #region Cases
        [HttpGet("case/{caseId}")]
        public async Task<IActionResult> CasesViewACase(string caseId)
        {

            var apiResultDto = new ApiResultDto<CasesViewACaseResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CVAC_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesViewACase(caseId);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }
        }

        [HttpPut("case/{caseId}/custom-fields/many")]
        public async Task<IActionResult> CasesUpdateManyCustomFields(string caseId,
            [FromBody] CasesUpdateManyCustomFieldsRequest updateRequest)
        {

            var apiResultDto = new ApiResultDto<CasesUpdateManyCustomFieldsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CUMCF_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService
                    .CasesUpdateManyCustomFields(caseId, updateRequest);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpPost("cases/info/user")]
        public async Task<IActionResult> CasesListCasesForUser
            ([FromBody] CasesListCasesForUserRequest listCasesForUserRequest)
        {

            var apiResultDto = new ApiResultDto<CasesListCasesForUserResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLC4U_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesListCasesForUser
                    (listCasesForUserRequest);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpGet("case/case-types")]
        public async Task<IActionResult> CasesListCaseTypes()
        {

            var apiResultDto = new ApiResultDto<CasesListCaseTypesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLCT_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesListCaseTypes();

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpGet("case/{caseId}/custom-fields")]
        public async Task<IActionResult> CasesListCustomFields(string caseId)
        {

            var apiResultDto = new ApiResultDto<CasesListCustomFieldsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLN_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesListCustomFields(caseId);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpGet("case/{caseId}/notes")]
        public async Task<IActionResult> CasesListNotes(string caseId)
        {

            var apiResultDto = new ApiResultDto<CasesListNotesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLN_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesListNotes(caseId);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpGet("case/{caseId}/tasks")]
        public async Task<IActionResult> CasesListTasks(string caseId)
        {

            var apiResultDto = new ApiResultDto<CasesListTasksResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLN_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesListTasks(caseId);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpPost("cases/create-case")]
        public async Task<IActionResult> CasesCreateANewCase([FromBody] CasesCreateANewCaseRequest request)
        {
            var apiResultDto = new ApiResultDto<CasesCreateANewCaseResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CCNC_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesCreateANewCase(request);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpPost("cases/add-person")]
        public async Task<IActionResult> CasesAddPersonToACase([FromBody] CasesAddPersonToACaseRequest request)
        {
            var apiResultDto = new ApiResultDto<CasesAddPersonToACaseResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CAPTC_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesAddPersonToACase(request);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpPost("cases/create-note")]
        public async Task<IActionResult> CasesCreateANote([FromBody] CasesCreateANoteRequest request)
        {
            var apiResultDto = new ApiResultDto<CasesCreateANoteResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CCN_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesCreateANote(request);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpPut("cases/{caseId}/update-case")]
        public async Task<IActionResult> CasesUpdateACase(string caseId, [FromBody] CasesUpdateACaseRequest request)
        {
            var apiResultDto = new ApiResultDto<CasesUpdateACaseResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CUC_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesUpdateACase(caseId, request);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpPut("cases/{caseId}/update-case-worker")]
        public async Task<IActionResult> CasesUpdateTheCaseWorker(string caseId, [FromBody] CasesUpdateTheCaseWorkerRequest request)
        {
            var apiResultDto = new ApiResultDto<CasesUpdateTheCaseWorkerResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CUCW_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesUpdateTheCaseWorker(caseId, request);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpPut("cases/update-task")]
        public async Task<IActionResult> CasesUpdateATask([FromBody] CasesUpdateATaskRequest request)
        {
            var apiResultDto = new ApiResultDto<CasesUpdateATaskResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CUT_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesUpdateATask(request);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpGet("cases/{caseId}/entities")]
        public async Task<IActionResult> CasesListDocumentEntities(string caseId)
        {
            var apiResultDto = new ApiResultDto<List<CasesListDocumentEntitiesResponse>>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLDE_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesListDocumentEntities(caseId);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpPut("cases/update-entity")]
        public async Task<IActionResult> CasesUpdateADocumentEntity([FromBody] CasesUpdateADocumentEntityRequest request)
        {
            var apiResultDto = new ApiResultDto<CasesUpdateADocumentEntityResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CUDE_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesUpdateADocumentEntity(request);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpPut("cases/tasks/{taskId}/complete-task")]
        public async Task<IActionResult> CasesCompleteATask(string taskId)
        {
            var apiResultDto = new ApiResultDto<CasesCompleteATaskResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CCT_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesCompleteATask(taskId);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpPut("cases/{caseId}/update-fee-earner")]
        public async Task<IActionResult> CasesUpdateTheFeeEarner(string caseId, [FromBody] CasesUpdateTheFeeEarnerRequest casesUpdateTheFeeEarnerRequest)
        {
            var apiResultDto = new ApiResultDto<CasesUpdateTheFeeEarnerResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CUTFE_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesUpdateTheFeeEarner(caseId, casesUpdateTheFeeEarnerRequest);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpGet("cases/{caseId}/list-billable-info")]
        public async Task<IActionResult> CasesGetBillableInfoByCase(string caseId)
        {
            var apiResultDto = new ApiResultDto<List<CasesGetBillableInfoByCaseResponse>>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CGBIBC_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesGetBillableInfoByCase(caseId);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }
        #endregion

        #region Quote

        [HttpGet("quote/situation")]
        public async Task<IActionResult> QuoteGetCompanyCustomQuoteSituations()
        {
            var apiResultDto = new ApiResultDto<List<QuoteGetCompanyCustomQuoteSituationsResponse>>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CGBIBC_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.QuoteGetCompanyCustomQuoteSituations();
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        #endregion

        #region Quote Calc

        [HttpPost("quote-calc/panel")]
        public async Task<IActionResult> QuoteCalcCreateAQuoteForAPanel([FromBody] QuoteCalcCreateAQuoteForAPanelRequest quoteCalcCreateAQuoteForAPanelRequest)
        {
            var apiResultDto = new ApiResultDto<QuoteCalcCreateAQuoteForAPanelResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.QCCQFP_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.QuoteCalcCreateAQuoteForAPanel(quoteCalcCreateAQuoteForAPanelRequest);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        #endregion

        #region Users

        [HttpGet("users/list-users-employees")]
        public async Task<IActionResult> UsersListUsersEmployees()
        {
            var apiResultDto = new ApiResultDto<List<UsersListUsersEmployeesResponse>>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.QCCQFP_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.UsersListUsersEmployees();
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        #endregion

        #region Person

        [HttpPost("person/byemail")]
        public async Task<IActionResult> GetPersonByEmail
            ([FromBody] GetPersonByEmailRequest getPersonByEmailRequest)
        {
            var apiResultDto = new ApiResultDto<GetPersonByEmailResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = PureFinanceConstants.Airtable_GetToken_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.GetPersonByEmail(getPersonByEmailRequest);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }
        }

        [HttpPost("person/byemail-v2")]
        public async Task<IActionResult> GetPersonByEmailV2([FromBody] GetPersonByEmailV2Request getPersonByEmailV2Request)
        {
            var apiResultDto = new ApiResultDto<GetPersonByEmailV2Response>()
            {
                Code = ResultCode.BadRequest,
                Message = PureFinanceConstants.Airtable_GetToken_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.GetPersonByEmailV2(getPersonByEmailV2Request);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }
        }

        [HttpPost("land-registry/property-desc")]
        public async Task<IActionResult> LandRegPerformEnquiryByPropertyDesc([FromBody] LandRegPerformEnquiryByPropertyDescRequest requestObj)
        {
            var apiResultDto = new ApiResultDto<List<LandRegPerformEnquiryByPropertyDescResponse>>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.LRPEBPD_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.LandRegPerformEnquiryByPropertyDesc(requestObj);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        #endregion

        #region Nintex API

        [HttpPost("nintex/spo-case-setup/{caseGUID}")]
        public async Task<IActionResult> TriggerSPOCaseSetup(string caseGUID)
        {

            var apiResult = new ApiResultDto<AddTask244Response>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            string content = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                content = await reader.ReadToEndAsync();
            }

            string emailBody = $"Hi Roxus,<br><br>Please review the request body:<br>{content}";

            var emailContent = new EmailContent()
            {
                Subject = $"[CompleteASAP] SPO Case Setup Trigger",
                Clients = "hoang.tran@roxus.io,misha@roxus.io,ofer.einy@roxus.io"
            };

            try
            {

                var addTask244Request = new AddTask244Request()
                {
                    query = "mutation AddTask($task: AddTaskInput!) { addTask(task: $task) { id name queueId tenantId createdAt state } }",
                };

                var parentVariables = new Variables244();
                var task = new Task244()
                {
                    queueId = "0652e693-4d52-4fa0-a52e-87902f09e210",
                    name = $"[CompleteASAP] SPO Case Setup {caseGUID}",
                    wizardCustomName = "CASSPOCaseSetup",
                    priority = 1,
                    tenantId = "1",
                };

                var variables = new List<Variable244>();
                var variable = new Variable244()
                {
                    name = "caseGUID",
                    value = caseGUID
                };

                variables.Add(variable);
                task.variables = variables;

                parentVariables.task = task;

                addTask244Request.variables = parentVariables;

                apiResult = await _nintexService.AddTask244(addTask244Request);

                var inputSummary = new
                {
                    TicketId = caseGUID,
                    WizardCustomId = "CASSPOCaseSetup"
                };
                var addTask244 = apiResult.Data;

                if (addTask244 == null)
                {

                    apiResult.Message = "Cannot create Task on Nintex 24.4";

                    // Send Error Email
                    emailBody += $"<br>Create Task Result: {JsonConvert.SerializeObject(apiResult)}";
                    emailContent.Body = emailBody;
                    await EmailHelpers.SendEmail(emailContent);

                    // Create Ticket
                    var createTicketRequest = new CreateTicketRequest()
                    {
                        //channel = "",
                        contactId = "475647000017068001",
                        classification = "Problem",
                        departmentId = "475647000000006907",
                        description = emailBody,
                        email = "misha@roxus.io",
                        priority = "High",
                        status = "Open",
                        subject = $"[CompleteASAP] SPO Case Setup - Case GUID: {caseGUID}"
                    };

                    var createTicketResponse = await _zohoDeskService
                        .CreateTicket("cm94dXM6em9ob2Rlc2s=", createTicketRequest);

                    return BadRequest(apiResult);
                }

                var integrationLog = new IntegrationLogForCreation()
                {
                    InputSummary = JsonConvert.SerializeObject(inputSummary),
                    Input1 = caseGUID,
                    Input2 = "CASSPOCaseSetup",
                    CompanyId = "0424F17F-A0C0-47F1-892F-439C3A907F46",
                    DepartmentId = "5806341F-86B9-4980-9DB3-F159858EA9D0",
                    PlatformName = "Hoowla",
                    OutputSummary = JsonConvert.SerializeObject(addTask244),
                };

                var createIntegrationLogResponse =
                    await _contxtusService.CreateIntegrationLog(integrationLog);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);

            }
            catch (Exception ex)
            {

                emailBody += $"<br>Create Task Result: {JsonConvert.SerializeObject(apiResult)}<br>" +
                    $"Exception: {ex.Message} - {ex.StackTrace}";
                emailContent.Body = emailBody;
                await EmailHelpers.SendEmail(emailContent);
                return BadRequest(apiResult);
            }

            #endregion
        }
    }

}
