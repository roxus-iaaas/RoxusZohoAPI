using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using System.Threading.Tasks;

namespace RoxusWebAPI.Services.Zoho.ZohoProjects
{
    public interface IZohoTasklistService
    {
        Task<ApiResultDto<GetAllTasklistsResponse>> GetAllTasklistsInProject(string apiKey, string projectId, string flag = "external");
        Task<ApiResultDto<CreateTasklistResponse>> CreateTasklist(string apiKey, string projectId, TasklistForCreation tasklistForCreation);

    }
}
