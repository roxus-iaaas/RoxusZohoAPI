using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Nintex;
using RoxusZohoAPI.Models.Nuvoli.Halo;
using RoxusZohoAPI.Models.Zoho.ZohoDesk;
using RoxusZohoAPI.Services.Contxtus;
using RoxusZohoAPI.Services.Nintex;
using RoxusZohoAPI.Services.Nuvoli;
using RoxusZohoAPI.Services.Nuvoli.Halo;
using RoxusZohoAPI.Services.Zoho.ZohoDesk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{

    [Route("api/nuvoli")]
    [ApiController]
    public class NuvoliController : ControllerBase
    {

        private readonly IHaloService _haloService;
        private readonly INuvoliCustomService _nuvoliCustomService;
        private readonly INintexService _nintexService;
        private readonly IContxtusService _contxtusService;
        private readonly IZohoDeskService _zohoDeskService;

        public NuvoliController(IHaloService haloService,
            INuvoliCustomService nuvoliCustomService,
            INintexService nintexService,
            IContxtusService contxtusService,
            IZohoDeskService zohoDeskService)
        {
            _haloService = haloService;
            _nuvoliCustomService = nuvoliCustomService;
            _nintexService = nintexService;
            _contxtusService = contxtusService;
            _zohoDeskService = zohoDeskService;
        }

        #region Halo

        [HttpGet("halo/tickets/{ticketId}")]
        public async Task<IActionResult> GetTicketById(string ticketId)
        {

            var apiResult = new ApiResultDto<GetTicketByIdResponse>();

            try
            {

                apiResult = await _nuvoliCustomService.GetTicketById(ticketId);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPost("halo/tickets/extract-details")]
        public IActionResult ExtractTicketDetails
            (ExtractTicketDetailsRequest extractRequest)
        {

            var apiResult = new ApiResultDto<ExtractTicketDetailsResponse>();

            try
            {

                string ticketDetails = extractRequest.TicketDetails;

                apiResult = _nuvoliCustomService.ExtractTicketDetails(ticketDetails);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPut("halo/tickets")]
        public async Task<IActionResult> UpdateTicket(
            [FromBody] UpdateTicketRequest updateTicketRequest)
        {

            var apiResult = new ApiResultDto<UpdateTicketResponse>();

            try
            {

                apiResult = await _nuvoliCustomService.UpdateTicket(updateTicketRequest);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("halo/tickets/{ticketId}/actions")]
        public async Task<IActionResult> GetTicketActions(
            string ticketId)
        {

            var apiResult = new ApiResultDto<GetTicketActionsResponse>();

            try
            {

                apiResult = await _nuvoliCustomService.GetTicketActions(ticketId);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }

        }

        [HttpPost("halo/tickets/actions")]
        public async Task<IActionResult> ExecuteActions()
        {

            var apiResult = new ApiResultDto<string>();

            try
            {

                string content = "";

                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    content = await reader.ReadToEndAsync();
                }

                //string requestBody = JsonConvert.SerializeObject(requestBodyObj);

                apiResult = await _nuvoliCustomService.ExecuteAction(content);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }

        }

        [HttpGet("halo/canned-text/{cannedTextId}")]
        public async Task<IActionResult> GetCannedTextById(
            string cannedTextId)
        {

            var apiResult = new ApiResultDto<GetCannedTextByIdResponse>();

            try
            {

                apiResult = await _nuvoliCustomService.GetCannedTextById(cannedTextId);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }

        }

        #endregion

        #region Custom Functions

        [HttpPost("custom/send-email-v2")]
        public async Task<IActionResult> SendEmailV2(
            [FromBody] HandleSendEmailRequest sendEmailRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                apiResult = await _nuvoliCustomService
                    .HandleSendEmail(sendEmailRequest);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }    
                    

                return BadRequest(apiResult);

            }
            catch (Exception ex)
            {
                return BadRequest(apiResult);

            }
        }

        [HttpPost("custom/handle-nhs-starter-phase-1")]
        public async Task<IActionResult> HandleNuvoliPhase1()
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
                Subject = $"[Nuvoli] NHS Starter Phase 1",
                Clients = "hoang.tran@roxus.io,misha@roxus.io,ofer.einy@roxus.io"
            };

            try {


                var ticketPayload = JsonConvert.DeserializeObject<TicketPayload>(content);

                string ticketId = ticketPayload.ticket.id.ToString();

                var addTask244Request = new AddTask244Request()
                {
                    query = "mutation AddTask($task: AddTaskInput!) { addTask(task: $task) { id name queueId tenantId createdAt state } }",
                };

                var parentVariables = new Variables244();
                var task = new Task244()
                {
                    queueId = "0652e693-4d52-4fa0-a52e-87902f09e210",
                    name = $"[NHS Starter - Phase 1] Ticket {ticketId}",
                    wizardCustomName = "NHSNewStarterMainWizardPhase1",
                    priority = 1,
                    tenantId = "1",
                };

                var variables = new List<Variable244>();
                var variable = new Variable244()
                {
                    name = "TicketId",
                    value = ticketId
                };

                variables.Add(variable);
                task.variables = variables;

                parentVariables.task = task;

                addTask244Request.variables = parentVariables;

                apiResult = await _nintexService.AddTask244(addTask244Request);

                var inputSummary = new
                {
                    TicketId = ticketId,
                    WizardCustomId = "NHSNewStarterMainWizardPhase1"
                };
                var addTask244 = apiResult.Data;

                if (addTask244 == null)
                {

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
                        subject = $"[Nuvoli] NHS Starter Phase 2 - Ticket: {ticketId}"
                    };

                    var createTicketResponse = await _zohoDeskService
                        .CreateTicket("cm94dXM6em9ob2Rlc2s=", createTicketRequest);

                    return BadRequest(apiResult);
                }    

                var integrationLog = new IntegrationLogForCreation()
                {
                    InputSummary = JsonConvert.SerializeObject(inputSummary),
                    Input1 = ticketId,
                    Input2 = "NHSNewStarterMainWizardPhase1",
                    CompanyId = "B54B99A4-B5B0-492C-8C08-4C2EE277B48B",
                    DepartmentId = "2153B8D6-5E62-4FD2-834F-3E8B05D3B4A2",
                    PlatformName = "Halo",
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
        }

        [HttpPost("custom/handle-nhs-starter-phase-2")]
        public async Task<IActionResult> HandleNuvoliPhase2()
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

            string emailBody = $"Hi Hoang,<br><br>Please review the request body:<br>{content}";

            var emailContent = new EmailContent()
            {
                Subject = $"[Nuvoli] NHS Starter Phase 2",
                Clients = "hoang.tran@roxus.io,misha@roxus.io,ofer.einy@roxus.io"
            };

            try
            {

                var ticketPayload = JsonConvert.DeserializeObject<TicketPayload>(content);

                string ticketId = ticketPayload.ticket.id.ToString();

                var addTask244Request = new AddTask244Request()
                {
                    query = "mutation AddTask($task: AddTaskInput!) { addTask(task: $task) { id name queueId tenantId createdAt state } }",
                };

                var parentVariables = new Variables244();
                var task = new Task244()
                {
                    queueId = "0652e693-4d52-4fa0-a52e-87902f09e210",
                    name = $"[NHS Starter - Phase 2] Ticket {ticketId}",
                    wizardCustomName = "NHSNewStarterMainWizardPhase2",
                    priority = 1,
                    tenantId = "1",
                };

                var variables = new List<Variable244>();
                var variable = new Variable244()
                {
                    name = "TicketId",
                    value = ticketId
                };

                variables.Add(variable);
                task.variables = variables;

                parentVariables.task = task;

                addTask244Request.variables = parentVariables;

                apiResult = await _nintexService.AddTask244(addTask244Request);

                var inputSummary = new
                {
                    TicketId = ticketId,
                    WizardCustomId = "NHSNewStarterMainWizardPhase2"
                };
                var addTask244 = apiResult.Data;

                if (addTask244 == null)
                {

                    // Send Eror Email
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
                        subject = $"[Nuvoli] NHS Starter Phase 2 - Ticket: {ticketId}"
                    };

                    var createTicketResponse = await _zohoDeskService
                        .CreateTicket("cm94dXM6em9ob2Rlc2s=", createTicketRequest);

                    return BadRequest(apiResult);
                }

                var integrationLog = new IntegrationLogForCreation()
                {
                    InputSummary = JsonConvert.SerializeObject(inputSummary),
                    Input1 = ticketId,
                    Input2 = "NHSNewStarterMainWizardPhase2",
                    CompanyId = "B54B99A4-B5B0-492C-8C08-4C2EE277B48B",
                    DepartmentId = "2153B8D6-5E62-4FD2-834F-3E8B05D3B4A2",
                    PlatformName = "Halo",
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
                return BadRequest();

            }
        }

        [HttpPost("custom/handle-o2-email-phase-2/{ticketId}")]
        public async Task<IActionResult> HandleEmailUpdatePhase2(string ticketId)
        {

            var apiResult = new ApiResultDto<ExtractPhase2Response>();

            try
            {

                apiResult = await _nuvoliCustomService.HandleEmailUpdatePhase2(ticketId);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }

        }

        [HttpPost("custom/handle-asset-phase-2/{ticketId}")]
        public async Task<IActionResult> HandleAssetPhase2(string ticketId)
        {
            var apiResult = new ApiResultDto<string>();

            try
            {

                apiResult = await _nuvoliCustomService.HandleAssetPhase2(ticketId);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }


        #endregion

    }

}
