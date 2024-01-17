using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public interface IZohoAccountService
    {

        Task<ApiResultDto<GetResponse<AccountResponse>>> GetAccounts(string apiKey, string sortCriteria);

        Task<ApiResultDto<AccountResponse>> GetAccountById(string apiKey, string accountId);

        Task<ApiResultDto<SearchResponse<AccountResponse>>> SearchAccountByCompanyRegistration(string apiKey, string companyNumber);

        Task<ApiResultDto<SearchResponse<AccountResponse>>> SearchAccountByAccountName(string apiKey, string companyName);

        Task<ApiResultDto<SearchResponse<AccountResponse>>> SearchAccountByCOQL(string apiKey, CoqlRequest coqlRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateAccount(string apiKey, UpsertRequest<AccountForCreation> accountRequest);

        Task<ApiResultDto<UploadResponse>> Account_UploadAttachments(string apiKey, string accountId, string fileName, string fileContent);

        Task<ApiResultDto<Account_RelatedContacts>> Account_GetRelatedContacts(string apiKey, string accountId);

        Task<ApiResultDto<UpdateResponse>> UpdateAccount(string apiKey, string accountId, AccountForUpdate accountRequest);

    }
}
