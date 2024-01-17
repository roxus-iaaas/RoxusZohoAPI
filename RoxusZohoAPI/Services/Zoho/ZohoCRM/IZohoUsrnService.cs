using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public interface IZohoUsrnService
    {
        Task<ApiResultDto<UsrnResponse>> GetUsrnById(string apiKey, string usrnId);
        Task<ApiResultDto<UpsertResponse<LinkingResponse>>> USRN_LinkingWithTitle(string apiKey, string usrnId, string titleId);
    }
}
