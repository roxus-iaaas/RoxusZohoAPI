using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.Custom;
using RoxusZohoAPI.Models.Zoho.ZohoBooks;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using RoxusZohoAPI.Services.Hinets;
using RoxusZohoAPI.Services.ThorneWidgery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{
    [Route("api/thornewidgery")]
    [ApiController]
    public class ThorneWidgeryController : ControllerBase
    {
        private readonly ITWService _twService;

        public ThorneWidgeryController(ITWService twService)
        {
            _twService = twService;
        }

        [HttpGet("monitor-records/{numberOfReports}")]
        public async Task<IActionResult> GetMonitorQuery(int numberOfReports)
        {
            var apiResult = new ApiResultDto<string>();
            try
            {
                apiResult = await _twService.MonitorTWProcess(numberOfReports);
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

    }
}
