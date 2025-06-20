using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Nuvoli.Halo;
using RoxusZohoAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Nuvoli.Halo
{

    public interface IHaloService
    {

        #region Token

        Task<string> GetAccessToken();

        #endregion
        
        Task<ApiResultDto<GetTicketByIdResponse>> GetTicketById
            (string ticketId);

        Task<ApiResultDto<GetTicketActionsResponse>> GetTicketActions 
            (string ticketId);

        Task<ApiResultDto<UpdateTicketResponse>> UpdateTicket
            (UpdateTicketRequest ticketRequest);

        Task<ApiResultDto<string>> ExecuteTicketActions
            (string requestBody);

        Task<ApiResultDto<GetCannedTextByIdResponse>> GetCannedTextById
            (string cannedTextId);

        Task<ApiResultDto<ListUsersResponse>> ListUsers
            (ListUsersRequest listUsersRequest);

        Task<ApiResultDto<CreateUserResponse>> CreateUser
            (CreateUserData createUserData);

        Task<ApiResultDto<ListAssetsResponse>> ListAssets
            (ListAssetsRequest listAssetsRequest);

        Task<ApiResultDto<UpsertAssetResponse>> UpsertAsset
            (UpsertAssetData upsertAssetRequest);

        Task<ApiResultDto<CreateChildTicketResponse>> CreateChildTicket
            (CreateChildTicketData createChildTicketRequest);

    }

}
