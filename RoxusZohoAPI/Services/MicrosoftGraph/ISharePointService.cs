using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.MicrosoftGraph;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.MicrosoftGraph
{

    public interface ISharePointService
    {

        Task<ApiResultDto<string>> 
            DownloadFile(DownloadSharePointFileRequest request);

    }

}
