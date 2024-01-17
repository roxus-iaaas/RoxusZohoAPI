using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.Custom;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho
{
    public interface IZohoCustomService
    {

        public Task<ApiResultDto<string>> RemoveRedundantOccupier(string apiKey, string titleId);

        public Task<ApiResultDto<string>> CreateUnregisteredTitle(string apiKey, string usrnId);

        public Task<ApiResultDto<string>> CreateUnregisteredTask(string crmKey, string projectKey, string titleId);

        public Task<ApiResultDto<string>> ProcessPostcardMailing(string crmKey, string projectKey, string taskUrl, string credential);

        public Task<ApiResultDto<string>> ProcessSendingLetter(SendLetterRequest sendLetterRequest);

        public Task<ApiResultDto<string>> OR_ProcessSendingLetter(SendLetterRequest sendLetterRequest);

        public Task<ApiResultDto<string>> OR_SyncFromTaskToCRM(string projectId, string taskId);

        public Task<ApiResultDto<string>> OR_MassSyncFromTaskToCRM();

        public Task<ApiResultDto<string>> OR_SyncFromCRMToDB(string openreachId);

        public Task<ApiResultDto<string>> OR_MassSyncFromCRMToDB();

        public Task<ApiResultDto<string>> OR_MassUpdateTasksToRemoved();

        public Task<ApiResultDto<string>> InsertTransactionQueue(ApiTransactionQueue transactionQueue);

        public Task<ApiResultDto<string>> HandleTransactionQueue(int queueId);

        public Task<ApiResultDto<string>> HandleReturnedSM();

        public Task<ApiResultDto<string>> GetLetterDocumentForOpenreach(string openreachId);

        public Task<ApiResultDto<string>> MassUpdateTasks();

        #region Temporary Function

        public Task<string> OR_SyncFromDBToProject();

        #endregion

    }
}
