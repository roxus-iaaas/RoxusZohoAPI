using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Hinets
{
    public interface IHinetsCrmService
    {
        Task<ApiResultDto<RelatedPurchaseOrdersResponse>> GetRelatedPurchaseOrders(string apiKey, string dealId);

        Task<ApiResultDto<GetListOfAttachments>> GetDealAttachments(string apiKey, string dealId);

        Task<ApiResultDto<string>> DownloadAttachmentsFromDeal(string apiKey, string dealId,
            string attachmentName, string attachmentId);

        Task<ApiResultDto<int>> GetNumberOfProjects(string apiKey, string dealId);

        Task<ApiResultDto<UploadResponse>> UploadAttachmentsToDeal(string apiKey, string dealId, string fileName, string filePath);

    }
}
