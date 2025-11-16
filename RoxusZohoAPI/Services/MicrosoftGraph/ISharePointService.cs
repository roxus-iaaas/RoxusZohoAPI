using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.MicrosoftGraph;
using RoxusZohoAPI.Models.SharePoint;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.MicrosoftGraph
{

    public interface ISharePointService
    {

        Task<ApiResultDto<string>> 
            DownloadFile(DownloadSharePointFileRequest request);

        Task<ApiResultDto<string>> SearchFilesInFolder
            (SearchFilesInFolderRequest request);

        Task<ApiResultDto<string>> SearchFoldersByName
            (SearchFoldersByNameRequest request);

        Task<ApiResultDto<string>> CreateFolderInFolder
            (CreateSharePointFolderRequest request);

    }

}
