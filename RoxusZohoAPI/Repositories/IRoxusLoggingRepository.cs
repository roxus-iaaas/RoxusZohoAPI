using RoxusZohoAPI.Entities.RoxusDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Repositories
{
    public interface IRoxusLoggingRepository
    {

        #region App Configuration

        Task<IEnumerable<AppConfiguration>> GetAllAppConfigurations();

        Task<AppConfiguration> GetAppConfigurationById(string id);

        Task<AppConfiguration> GetAppConfigurationByCustomerNameAndPlatform(string customerName, string platform);

        Task<AppConfiguration> GetAppConfigurationByApiKey(string apiKey);

        Task UpdateTokenAndExpiredTime(string id, string accessToken, string newExpiredTime, string refreshToken = "");

        #endregion

        #region API Logging

        Task CreateApiLogging(ApiLogging apiLogging);

        #endregion

        #region Trenches Task

        Task UpsertTrenchesTaskToSQL(TrenchesTask trenchesTask, bool isUpdate = false);

        Task<TrenchesTask> GetTrenchesTaskByTaskIdAndProjectId(string taskId, string projectId);

        Task DeleteTaskFromDb(TrenchesTask trenchesTask);

        #endregion

        #region API Transaction Queue

        Task<ApiTransactionQueue> GetTransactionQueueById(int id);

        Task InsertApiTransactionQueue(ApiTransactionQueue transactionQueue);

        Task UpdateApiTransactionQueue(ApiTransactionQueue transactionQueue);

        #endregion

        #region Docmail Records

        Task<IEnumerable<DocmailRecord>> GetDocmailRecordBySMId(string smId);

        Task<IEnumerable<DocmailRecord>> GetDocmailRecordByMailingReference(string smNumber);

        #endregion

        #region Integration Logs

        Task<IEnumerable<IntegrationLog>> GetIntegrationLogs();

        Task<IntegrationLog> GetLatestIntegrationLogByCursor
            (string customerName, string platformName);

        Task<IEnumerable<IntegrationLog>> GetEmptyIntegrationLogs();

        Task CreateIntegrationLog(IntegrationLog integrationLog);

        Task UpdateIntegrationLog(int currentCursor, string output, string result
            , string input1 = "", string input2 = "", string input3 = "", string input4 = "", string input5 = ""
            , string input6 = "", string input7 = "", string input8 = "", string input9 = "", string input10 = "");

        #endregion

    }
}
