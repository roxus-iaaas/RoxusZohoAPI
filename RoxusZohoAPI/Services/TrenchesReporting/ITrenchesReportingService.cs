using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoxusZohoAPI.Entities.TrenchesReportDB;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.TrenchesReporting;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.TrenchesReporting
{

    public interface ITrenchesReportingService
    {

        public Task<ApiResultDto<string>> UpsertSyncingRecords(SyncingRecord syncingRecord);

        public Task<ApiResultDto<string>> SyncingTaskDelete(string taskUrl);

        public Task<ApiResultDto<string>> HandleLightningFibre();

        public Task<ApiResultDto<string>> HandleOpenReach();

        public Task<ApiResultDto<string>> HandleOpenReach1402();

        public Task<ApiResultDto<string>> HandleOpenReachAccounts(string apiKey);

        public Task<ApiResultDto<string>> HandleOROwnershipLinking(string apiKey);

        public Task<ApiResultDto<string>> CreateAccessAgreement(AccessAgreementRequest accessAgreementRequest);

        public Task<ApiResultDto<string>> HandleLetterReference(string apiKey, string openreachId);

        public Task<ApiResultDto<bool>> HandleRegisterPurchase(RegisterPurchaseRequest purchaseRequest);

        public Task<ApiResultDto<string>> CheckPrimarySM(string smNumber);

    }

}
