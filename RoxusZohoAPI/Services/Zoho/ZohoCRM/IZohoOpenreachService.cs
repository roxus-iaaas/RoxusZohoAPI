using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public interface IZohoOpenreachService
    {
        Task<ApiResultDto<SearchResponse<OpenreachResponse>>> SearchOpenreachByOpenreachNumber
            (string apiKey, string openreachNumber);

        Task<ApiResultDto<OpenreachResponse>> GetOpenreachById(string apiKey, string openreachId);

        Task<ApiResultDto<UpdateResponse>> UpdateOpenreach(string apiKey, string openreachId, OpenreachForUpdate openreachForUpdate);

        Task<ApiResultDto<RelatedTitlesResponse>> GetOpenreachRelatedTitles(string apiKey, string openreachId);

        Task<ApiResultDto<RelatedAccountsResponse>> GetOpenreachRelatedAccount(string apiKey, string openreachId);

    }
}
