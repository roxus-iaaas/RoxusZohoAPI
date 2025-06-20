using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoDesk;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using RoxusZohoAPI.Services.Zoho.ZohoDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ZohoDesksController : ControllerBase
    {
        private readonly IZohoDeskService _zohoDeskService;

        public ZohoDesksController(IZohoDeskService zohoDeskService)
        {
            _zohoDeskService = zohoDeskService;
        }

        #region TICKETS

        [HttpPost("tickets")]
        public async Task<IActionResult> CreateTicket(
            [FromBody] CreateTicketRequest createTicketRequest)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = "cm94dXM6em9ob2Rlc2s=";
            }    

            var apiResult = new ApiResultDto<CreateTicketResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                if (string.IsNullOrEmpty(createTicketRequest.departmentId))
                {
                    createTicketRequest.departmentId = "475647000000006907";
                }
                if (string.IsNullOrEmpty(createTicketRequest.contactId))
                {
                    createTicketRequest.contactId = "475647000009226001";
                }
                if (string.IsNullOrEmpty(createTicketRequest.status))
                {
                    createTicketRequest.status = "Open";
                }
                if (string.IsNullOrEmpty(createTicketRequest.classification))
                {
                    createTicketRequest.classification = "Problem";
                }

                apiResult = await _zohoDeskService.CreateTicket(apiKey, createTicketRequest);
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

    }
}
