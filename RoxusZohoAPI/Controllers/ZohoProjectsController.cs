using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RoxusWebAPI.Services.Zoho.ZohoProjects;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ZohoProjectsController : ControllerBase
    {
        private readonly IZohoProjectService _zohoProjectService;
        private readonly IZohoTasklistService _zohoTasklistService;
        private readonly IZohoTaskService _zohoTaskService;

        public ZohoProjectsController(IZohoProjectService zohoProjectService, IZohoTasklistService zohoTasklistService, IZohoTaskService zohoTaskService)
        {
            _zohoProjectService = zohoProjectService;
            _zohoTasklistService = zohoTasklistService;
            _zohoTaskService = zohoTaskService;
        }

        #region PROJECT
        [HttpGet("projects/search")]
        public async Task<IActionResult> SearchProjectsInPortal([FromBody] SearchRequest searchRequest)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            var apiResult = new ApiResultDto<SearchProjectResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoProjectService.SearchProjectInPortal(apiKey, searchRequest.SearchTerm);
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
        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjectsInPortal()
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            var apiResult = new ApiResultDto<GetAllProjectsInPortalResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoProjectService.GetAllProjectsInPortal(apiKey);
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

        #region TASKLIST
        [HttpGet("projects/{projectId}/tasklists/{flag}")]
        public async Task<IActionResult> GetAllTasklistsInProject(string projectId, string flag)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            var apiResult = new ApiResultDto<GetAllTasklistsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                if (!string.IsNullOrWhiteSpace(flag))
                {
                    apiResult = await _zohoTasklistService.GetAllTasklistsInProject(apiKey, projectId, flag);
                }
                else
                {
                    apiResult = await _zohoTasklistService.GetAllTasklistsInProject(apiKey, projectId);
                }

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
        [HttpPost("projects/{projectId}/tasklists")]
        public async Task<IActionResult> CreateTasklist(string projectId, TasklistForCreation tasklistForCreation)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            var apiResult = new ApiResultDto<CreateTasklistResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                if (!string.IsNullOrWhiteSpace(tasklistForCreation.flag))
                {
                    tasklistForCreation.flag = "external";
                }
                apiResult = await _zohoTasklistService.CreateTasklist(apiKey, projectId, tasklistForCreation);

                switch (apiResult.Code)
                {
                    case ResultCode.Created:
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

        #region TASK
        [HttpGet("projects/{projectId}/tasks")]
        public async Task<IActionResult> GetAllTasksInProject(string projectId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            var apiResult = new ApiResultDto<GetAllTasksResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoTaskService.GetAllTasksInProject(apiKey, projectId);
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
        [HttpGet("projects/{projectId}/tasks/{taskId}")]
        public async Task<IActionResult> GetTaskById(string projectId, string taskId)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");
            var apiResult = new ApiResultDto<TaskDetail>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoTaskService.GetTaskDetails(apiKey, projectId, taskId);

                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    case ResultCode.NoContent:
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
        [HttpPost("projects/{projectId}/tasks")]
        public async Task<IActionResult> CreateTask(string projectId, TaskForCreation taskForCreation)
        {
            string apiKey = Request.Headers[HeaderNames.Authorization].ToString().Replace("Basic ", "");

            var apiResult = new ApiResultDto<CreateTaskResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                apiResult = await _zohoTaskService.CreateTask(apiKey, projectId, taskForCreation);

                switch (apiResult.Code)
                {
                    case ResultCode.Created:
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
