using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using System.Threading.Tasks;

namespace RoxusWebAPI.Services.Zoho.ZohoProjects
{
    public interface IZohoProjectService
    {

        Task<ApiResultDto<SearchProjectResponse>> SearchProjectInPortal(string apiKey, string searchTerm);
        
        Task<ApiResultDto<GetAllProjectsInPortalResponse>> GetAllProjectsInPortal(string apiKey);
        
        Task<ApiResultDto<CreateProjectResponse>> CreateProject(string apiKey, ProjectForCreation projectForCreation);
        
        Task<TaskModel> GetTaskDetails(string apiKey, string taskId, string projectId);
        
        Task<ProjectModel> GetProjectDetails(string apiKey, string projectId);
        
        Task<ApiResultDto<string>> InsertTaskToTable(string apiKey, string taskId, string projectId);
        
        Task<ApiResultDto<CheckTaskValidResponse>> CheckTaskValid(string apiKey, string taskId, string projectId);
        
        Task<ApiResultDto<string>> DeleteTaskFromTable(string apiKey, string taskId, string projectId);

        Task<ApiResultDto<AddEventResponse>> AddEvent(string apiKey, string projectId, AddEventRequest addEventRequest);

    }
}
