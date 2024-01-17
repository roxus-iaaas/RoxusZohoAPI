using Microsoft.EntityFrameworkCore;
using RoxusZohoAPI.Contexts;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Repositories
{
    public class RoxusLoggingRepository : IRoxusLoggingRepository
    {
        private readonly RoxusContext _roxusContext;

        public RoxusLoggingRepository(RoxusContext context)
        {
            _roxusContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region API Logging

        public async Task CreateApiLogging(ApiLogging apiLogging)
        {
            if (apiLogging == null)
            {
                return;
            }
            _roxusContext.ApiLoggings.Add(apiLogging);
            await _roxusContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppConfiguration>> GetAllAppConfigurations()
        {
            return await _roxusContext.AppConfigurations.ToListAsync();
        }

        public async Task<AppConfiguration> GetAppConfigurationById(string id)
        {
            return await _roxusContext.AppConfigurations
                .Where(a => a.Id.ToLower() == id.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<AppConfiguration> GetAppConfigurationByApiKey(string apiKey)
        {
            string decoded = TokenHelpers.Base64Decode(apiKey);

            if (!decoded.Contains(":"))
            {
                return null;
            }

            string userName = decoded.Split(":")[0];
            string password = decoded.Split(":")[1];

            return await _roxusContext.AppConfigurations
                .Where(a => a.Username.Equals(userName) && a.Password.Equals(password))
                .FirstOrDefaultAsync();
        }

        public async Task<AppConfiguration> GetAppConfigurationByCustomerNameAndPlatform(string customerName, string platform)
        {
            return await _roxusContext.AppConfigurations
                .Where(a => a.CustomerName.Equals(customerName) && a.Platform.Equals(platform))
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAccessTokenAndExpiredTime(string id, string accessToken, string newExpiredTime, string refreshToken = "")
        {
            try
            {
                var appConfig = await _roxusContext.AppConfigurations.FirstOrDefaultAsync(a => a.Id == id);
                appConfig.AccessToken = accessToken;
                appConfig.ExpiredTime = newExpiredTime;
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    appConfig.RefreshToken = refreshToken;
                }
                await _roxusContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Trenches Task

        public async Task<TrenchesTask> GetTrenchesTaskByTaskIdAndProjectId(string taskId, string projectId)
        {
            return await _roxusContext.TrenchesTasks
                .Where(t => t.TaskId == taskId && t.ProjectId == projectId)
                .FirstOrDefaultAsync();
        }

        public async Task UpsertTrenchesTaskToSQL(TrenchesTask trenchesTask, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                _roxusContext.TrenchesTasks.Add(trenchesTask);
            }
            await _roxusContext.SaveChangesAsync();
        }

        public async Task DeleteTaskFromDb(TrenchesTask trenchesTask)
        {
            _roxusContext.TrenchesTasks.Remove(trenchesTask);
            await _roxusContext.SaveChangesAsync();
        }

        #endregion Trenches Task

        #region API Transaction Queue

        public async Task<ApiTransactionQueue> GetTransactionQueueById(int id)
        {
            return await _roxusContext.ApiTransactionQueues.Where(q => q.Id == id).FirstOrDefaultAsync();
        }

        public async Task InsertApiTransactionQueue(ApiTransactionQueue transactionQueue)
        {
            _roxusContext.ApiTransactionQueues.Add(transactionQueue);
            await _roxusContext.SaveChangesAsync();
        }

        public async Task UpdateApiTransactionQueue(ApiTransactionQueue transactionQueue)
        {
            await _roxusContext.SaveChangesAsync();
        }

        #endregion

        #region Docmail Records

        public async Task<IEnumerable<DocmailRecord>> GetDocmailRecordBySMId(string smId)
        {
            return await _roxusContext.DocmailRecords.Where(d => d.OpenreachId == smId
                && !string.IsNullOrEmpty(d.BlobUrl)).ToListAsync();
        }

        public async Task<IEnumerable<DocmailRecord>> GetDocmailRecordByMailingReference(string smNumber)
        {

            return await _roxusContext.DocmailRecords.Where(d => EF.Functions.Like(d.MailingReference, $"%{smNumber}%")
                && !string.IsNullOrEmpty(d.BlobUrl)).ToListAsync();
        }

        #endregion

    }
}
