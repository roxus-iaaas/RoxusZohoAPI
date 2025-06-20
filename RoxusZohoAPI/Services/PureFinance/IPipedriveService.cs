using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.PureFinance.Pipedrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.PureFinance
{

    public interface IPipedriveService
    {

        #region Notes

        Task<ApiResultDto<CreateNoteResponse>> CreateNote
            (CreateNoteRequest createDealRequest);

        #endregion

        #region Deals

        Task<ApiResultDto<CreateDealResponse>> CreateDeal
            (CreateDealRequest createDealRequest);

        #endregion

        #region Persons

        Task<ApiResultDto<SearchPersonsResponse>> SearchPersonsByEmail(string email);

        Task<ApiResultDto<CreatePersonResponse>> CreatePerson
            (CreatePersonRequest createPersonRequest);

        #endregion

    }

}
