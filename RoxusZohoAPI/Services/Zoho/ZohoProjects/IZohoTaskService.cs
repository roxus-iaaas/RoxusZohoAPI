using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusWebAPI.Services.Zoho.ZohoProjects
{
    public interface IZohoTaskService
    {

        Task<ApiResultDto<GetAllTasksResponse>> GetAllTasksInProject(string apiKey, string projectId);

        Task<ApiResultDto<TaskDetail>> GetTaskDetails(string apiKey, string projectId, string taskId);

        Task<ApiResultDto<CreateTaskResponse>> CreateTask(string apiKey, string projectId, TaskForCreation taskForCreation);

        Task<ApiResultDto<CreateTaskResponse>> UpdateTaskForTrenches(
            string apiKey, string projectId, string taskId, TaskForUpdation taskForUpdation);

        Task<ApiResultDto<CreateTaskResponse>> UpdateTaskForHinets(
            string apiKey, string projectId, string taskId, TaskForUpdation taskForUpdation);

        Task<ApiResultDto<string>> DeleteTask(string apiKey, string projectId, string taskId);

        Task<ApiResultDto<SearchTasksResponse>> SearchTasksInProjects
            (string apiKey, string projectId, string searchTerm);

    }
}
