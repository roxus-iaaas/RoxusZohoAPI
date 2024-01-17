using RoxusZohoAPI.Entities.TrenchesReportDB;
using RoxusZohoAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Repositories
{
    public interface ITrenchesReportingRepository
    {
        Task<SyncingRecord> GetSyncingRecord(SyncingRecord syncingRecord);

        Task CreateSyncingRecord(SyncingRecord syncingRecord);

        Task UpdateSyncingRecord(SyncingRecord syncingRecord);

        public Task<ZohoTask> GetTaskByTaskIdAndProjectId(string taskId, string projectId);

        public Task DeleteTaskRecord(ZohoTask task);

        public Task UpdateTaskRecord(ZohoTask task);

        #region Lightning Fibre

        public Task<List<LFLAddressData>> GetLFLAddressDatasByType(string type);

        public Task<LFLNimbusRequest> GetLFLNimbusRequest(long UPRN);

        public Task CreateLFLNimbusRequest(LFLNimbusRequest nimbusRequest);

        public Task<LFLUPRNsFreeholdTitles> GetLFLFreeholdByUPRN(long UPRN);

        public Task<LFLUPRNsFreeholdTitles> GetLFLFreeholdByUPRNAndTitle(long UPRN, string titleNumber);

        public Task CreateLFLFreehold(LFLUPRNsFreeholdTitles freeholdTitle);

        public Task<LFLUPRNsLeaseholdTitles> GetLFLLeaseholdByUPRN(long UPRN);

        public Task<LFLUPRNsLeaseholdTitles> GetLFLLeaseholdByUPRNAndTitle(long UPRN, string titleNumber);

        public Task CreateLFLLeasehold(LFLUPRNsLeaseholdTitles leaseholdTitle);

        #endregion

        #region Openreach

        public Task<List<ORPrimaryData>> GetAllORPrimaryRecords();

        public Task<ORPrimaryData> GetORPrimaryDataByOpenreachNumber(string openreachNumber);

        public Task<List<ORPrimaryData>> GetORPrimaryRecordsByParentUPRN(long parentUPRN);

        public Task UpdatePrimaryData(ORPrimaryData primaryData);  

        public Task<List<long>> GetORDistinctParentUPRN();

        public Task MassUpdateORRecords(List<ORPrimaryData> orRecords);

        public Task<ORNimbusRequest> GetORNimbusRequest(long UPRN);

        public Task CreateORNimbusRequest(ORNimbusRequest nimbusRequest);

        public Task<ORUPRNsFreeholdTitles> GetORFreeholdByUPRN(long UPRN);

        public Task<ORUPRNsFreeholdTitles> GetORFreeholdByUPRNAndTitle(long UPRN, string titleNumber);

        public Task CreateORFreehold(ORUPRNsFreeholdTitles freeholdTitle);

        public Task<ORUPRNsLeaseholdTitles> GetORLeaseholdByUPRN(long UPRN);

        public Task<ORUPRNsLeaseholdTitles> GetORLeaseholdByUPRNAndTitle(long UPRN, string titleNumber);

        public Task CreateORLeasehold(ORUPRNsLeaseholdTitles leaseholdTitle);

        public Task<List<ViewCorporateOwnership>> GetOROwnerships();

        public Task<List<OROwnerLinking>> GetAllOROwnerLinkings();

        public Task<List<OROwnerLinking>> GetUnhandledOROwnerLinkings();

        public Task<List<OROwnerLinking>> GetOROwnerLinkingsByOpenreachNumber(string openreachNumber);

        public Task<List<OROwnerLinking>> GetOROwnerLinkingsByOwnerId(string ownerId);

        public Task<List<OROwnerLinking>> GetOROwnerLinkingsByLetterReference(string letterReference);

        public Task<string> GetLetterReferenceByOpenreachNumber(string openreachNumber);

        public Task<List<string>> GetDistinctOwnerIdsByOpenreachNumber(string openreachNumber);

        public Task UpdateOROwnerLinking(OROwnerLinking ownerLinking);

        public Task<ORTask> GetORTaskByTaskId(string taskId);

        public Task CreateORTask(ORTask orTask);

        public Task UpdateORTask(ORTask orTask);

        #endregion

        #region Corporate Ownership

        public Task<CorporateOwnership> GetOwnershipByTitleNumber(string titleNumber);

        #endregion

    }
}
