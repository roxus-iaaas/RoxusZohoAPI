using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Nimbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Nimbus
{
    public interface INimbusService
    {

        Task<ApiResultDto<SiteInventory1Response>> SiteInventory1_LFL(long uprn);

        Task<ApiResultDto<SiteInventory1Response>> SiteInventory1_OR(long uprn);

    }
}
