
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public interface IZohoUprnService
    {

        Task<ApiResultDto<UprnResponse>> GetUprnById(string apiKey, string usrnId);

        Task<ApiResultDto<UpsertResponse<LinkingResponse>>> UPRN_LinkingWithTitles(string apiKey, string uprnId, string titleIds);

    }
}
