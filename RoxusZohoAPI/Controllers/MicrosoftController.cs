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
using RoxusZohoAPI.Services.MicrosoftGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{
    [Route("api/microsoft")]
    [ApiController]
    public class MicrosoftController : ControllerBase
    {

        private readonly IMicrosoftGraphAuthService _microsoftGraphAuthService;

        public MicrosoftController(IMicrosoftGraphAuthService microsoftGraphAuthService)
        {
            _microsoftGraphAuthService = microsoftGraphAuthService;
        }

        #region Microsoft Graph

        [HttpGet("token")]
        public async Task<IActionResult> GetAccessToken()
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new AppConfiguration();
            try
            {
                apiResult = await _microsoftGraphAuthService.GetAccessToken(apiKey);
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        #endregion

    }
}
