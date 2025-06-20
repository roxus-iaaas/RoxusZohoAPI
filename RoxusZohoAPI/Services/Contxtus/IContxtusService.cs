using RoxusZohoAPI.Models.Common;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Contxtus
{

    public interface IContxtusService
    {

        Task<ApiResultDto<string>> CreateIntegrationLog(IntegrationLogForCreation logForCreation);

    }

}
