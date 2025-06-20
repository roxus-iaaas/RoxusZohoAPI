using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.PureFinance.Airtable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.PureFinance
{

    public interface IAirtableService
    {

        Task<string> RefreshToken();

        Task<ApiResultDto<RefreshWebhookResponse>> RefreshWebhook(string baseId, string webhookId);

        Task<ApiResultDto<ListWebhookPayloadsResponse>>
            ListWebhookPayloads(string baseId, string webhookId, int cursor = 0);

    }

}
