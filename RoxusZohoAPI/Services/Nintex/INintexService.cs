using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Nintex;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Nintex
{

    public interface INintexService
    {

        Task<ApiResultDto<AddTask244Response>> AddTask244(AddTask244Request request);

    }

}
