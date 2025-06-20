using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.PureFinance.Airtable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.PureFinance
{

    public interface IPureFinanceCustomService
    {

        Task<ApiResultDto<string>> SyncFromAirtable2Pipedrive(AirtablePayload airtablePayload);

        Task<ApiResultDto<string>> MassUpdateTable();

    }

}
