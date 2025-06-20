using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho.ZohoDesk
{
    public interface IZohoDeskService
    {

        Task<ApiResultDto<CreateTicketResponse>> CreateTicket
            (string apiKey, CreateTicketRequest createTicketRequest);

    }
}
