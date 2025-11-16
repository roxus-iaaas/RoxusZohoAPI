using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Services.MicrosoftGraph;
using System.Threading.Tasks;
using System;
using RoxusZohoAPI.Models.MicrosoftGraph;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.SharePoint;

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

        [HttpPost("search-files")]
        public async Task<IActionResult> SearchFilesInFolder([FromBody]
            SearchFilesInFolderRequest request)
        {
            var apiResult = new ApiResultDto<string>();
            try
            {
                apiResult = await _sharePointService.SearchFilesInFolder(request);
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

        [HttpPost("search-folders")]
        public async Task<IActionResult> SearchFolders([FromBody]
            SearchFoldersByNameRequest request)
        {
            var apiResult = new ApiResultDto<string>();
            try
            {
                apiResult = await _sharePointService.SearchFoldersByName(request);
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

        [HttpPost("create-folder")]
        public async Task<IActionResult> CreateFolderInFolder([FromBody]
            CreateSharePointFolderRequest request)
        {
            var apiResult = new ApiResultDto<string>();
            try
            {
                apiResult = await _sharePointService.CreateFolderInFolder(request);
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
