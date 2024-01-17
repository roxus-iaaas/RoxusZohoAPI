using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Hinets
{
    public interface IHinetsBooksService
    {

        #region Purchase Order

        Task<ApiResultDto<PurchaseOrderResponse>> GetPoById(string apiKey, string poId);

        Task<ApiResultDto<UpdatePoResponse>> UpdatePurchaseOrder(string apiKey, string poId, object updateRequest);

        Task<ApiResultDto<SearchPurchaseOrderResponse>> SearchPurchaseOrdersByReference(string apiKey, string referenceNumber);

        Task<ApiResultDto<string>> DownloadPoPDF(string apiKey, string poId, string poNumber);

        #endregion

        #region Estimate

        Task<ApiResultDto<EstimateResponse>> GetEstimateById(string apiKey, string estimateId);

        Task<ApiResultDto<SearchEstimateResponse>> SearchEstimateByDealId(string apiKey, string dealId);

        Task<ApiResultDto<UpdateEstimateResponse>> UpdateEstimate(string apiKey, string estimateId, object updateRequest);

        Task<ApiResultDto<string>> DownloadEstimatePDF(string apiKey, string estimateId, string estimateNumber);

        #endregion

        #region Items

        Task<ApiResultDto<GetItemsResponse>> GetActiveItems(string apiKey, int? page);

        Task<ApiResultDto<GetItemsResponse>> GetCompositeItems(string apiKey, int? page);

        Task<ApiResultDto<GetCompositeItemByIdResponse>> GetCompositeItemById(string apiKey, string compositeId);

        Task<ApiResultDto<UpdateItemResponse>> UpdateItem(string apiKey, string itemId, ItemForUpdate itemForUpdate);

        #endregion

    }
}
