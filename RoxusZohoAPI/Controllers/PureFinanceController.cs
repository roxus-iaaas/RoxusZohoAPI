using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.PureFinance.Airtable;
using RoxusZohoAPI.Models.Zoho.Custom;
using RoxusZohoAPI.Models.Zoho.ZohoBooks;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using RoxusZohoAPI.Services.Hinets;
using RoxusZohoAPI.Services.PureFinance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{
    [Route("api/pure-finance")]
    [ApiController]
    public class PureFinanceController : ControllerBase
    {

        private readonly IAirtableService _airtableService;
        private readonly IPureFinanceCustomService _customService;

        public PureFinanceController(IAirtableService airtableService, 
            IPureFinanceCustomService customService)
        {
            _airtableService = airtableService;
            _customService = customService;
        }

        #region Custom Functions

        [HttpPost("custom/sync-to-pipedrive")]
        public async Task<IActionResult> SyncToPipedrive([FromBody] AirtablePayload requestBody)
        {
            try
            {

                var apiResult = await _customService.SyncFromAirtable2Pipedrive(requestBody);

                return Ok(apiResult);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("airtable/token")]
        public async Task<IActionResult> GetAirtableToken()
        {
            var apiResultDto = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = PureFinanceConstants.Airtable_GetToken_400,
                Data = null
            };

            try
            {

                string accessToken = await _airtableService.RefreshToken();
                if (!string.IsNullOrEmpty(accessToken))
                {
                    apiResultDto.Code = ResultCode.OK;
                    apiResultDto.Message = PureFinanceConstants.Airtable_GetToken_200;
                    apiResultDto.Data = accessToken;
                }

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

        [HttpPost("airtable/webhook/{baseId}/{webhookId}")]
        public async Task<IActionResult> RefreshWebhook(string baseId, string webhookId)
        {
            var apiResultDto = new ApiResultDto<RefreshWebhookResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = PureFinanceConstants.Airtable_RefreshWebhook_400,
                Data = null
            };

            try
            {

                apiResultDto = await _airtableService
                    .RefreshWebhook(baseId, webhookId);

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

    }
}
