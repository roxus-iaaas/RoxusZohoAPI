using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Nuvoli.Halo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Nuvoli
{
    
    public interface INuvoliCustomService
    {

        #region Halo

        Task<ApiResultDto<GetTicketByIdResponse>> GetTicketById
            (string ticketId);

        Task<ApiResultDto<GetTicketActionsResponse>> GetTicketActions
            (string ticketId);

        ApiResultDto<ExtractTicketDetailsResponse>
            ExtractTicketDetails(string ticketDetails);

        Task<ApiResultDto<UpdateTicketResponse>>
            UpdateTicket(UpdateTicketRequest updateTicketRequest);

        Task<ApiResultDto<string>>
            ExecuteAction(string requestBody);

        Task<ApiResultDto<GetCannedTextByIdResponse>> GetCannedTextById
            (string cannedTextId);

        #endregion

        #region Custom Functions

        Task<ApiResultDto<string>> HandleNuvoliStarterPhase1
            (string ticketId);

        Task<ApiResultDto<ExtractPhase2Response>>
            HandleEmailUpdatePhase2(string ticketId);

        Task<ApiResultDto<string>>
            HandleAssetPhase2(string ticketId);

        Task<ApiResultDto<string>>
            HandleSendEmail(HandleSendEmailRequest sendEmailRequest);
        
        #endregion

    }

}
