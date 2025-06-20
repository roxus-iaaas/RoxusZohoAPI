using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Services.MicrosoftGraph;
using System.Threading.Tasks;
using System;
using RoxusZohoAPI.Models.MicrosoftGraph;
using RoxusZohoAPI.Models.Common;

namespace RoxusZohoAPI.Controllers
{

    [Route("api/sharepoint")]
    [ApiController]
    public class SharePointController : ControllerBase
    {

        private readonly ISharePointService _sharePointService;

        public SharePointController(ISharePointService sharePointService)
        {
            _sharePointService = sharePointService;
        }

        [HttpPost("download")]
        public async Task<IActionResult> GetAccessToken([FromBody] 
            DownloadSharePointFileRequest request)
        {
            var apiResult = new ApiResultDto<string>();
            try
            {
                apiResult = await _sharePointService.DownloadFile(request);
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

    }

}
