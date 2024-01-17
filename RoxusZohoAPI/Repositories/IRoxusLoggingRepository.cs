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

        Task UpdateAccessTokenAndExpiredTime(string id, string accessToken, string newExpiredTime, string refreshToken = "");

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

    }
}
