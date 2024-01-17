using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RoxusZohoAPI.Contexts;
using RoxusZohoAPI.Entities.TrenchesReportDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Repositories
{

    public class TrenchesReportingRepository : ITrenchesReportingRepository
    {
        private readonly TrenchesContext _trenchesContext;

        public TrenchesReportingRepository(TrenchesContext trenchesContext)
        {
            _trenchesContext = trenchesContext;
        }

        public async Task<SyncingRecord> GetSyncingRecord(SyncingRecord syncingRecord)
        {
            return await _trenchesContext.SyncingRecords
                .Where(sr => sr.Action == syncingRecord.Action && sr.ModuleId == syncingRecord.ModuleId 
                && sr.ModuleName == syncingRecord.ModuleName && sr.Status == SyncingStatus.Pending)
                .FirstOrDefaultAsync();
        }

        public async Task CreateSyncingRecord(SyncingRecord syncingRecord)
        {
            _trenchesContext.SyncingRecords.Add(syncingRecord);
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task UpdateSyncingRecord(SyncingRecord syncingRecord)
        {
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task<ZohoTask> GetTaskByTaskIdAndProjectId(string taskId, string projectId)
        {
            return await _trenchesContext.ZohoTasks
                .Where(t => t.TaskId == taskId && t.ProjectId == projectId)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteTaskRecord(ZohoTask task)
        {
            _trenchesContext.ZohoTasks.Remove(task);
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task UpdateTaskRecord(ZohoTask task)
        {
            await _trenchesContext.SaveChangesAsync();
        }

        #region Lightning Fibre

        public async Task<List<LFLAddressData>> GetLFLAddressDatasByType(string type)
        {
            return await _trenchesContext.LFLAddressDatas.Where(a => a.Type == type)
                .OrderBy(a => a.UPRN).Skip(6264).Take(300).ToListAsync();
        }

        public async Task<LFLNimbusRequest> GetLFLNimbusRequest(long UPRN)
        {
            return await _trenchesContext.LFLNimbusRequests.Where(r => r.UPRN == UPRN).FirstOrDefaultAsync();
        }

        public async Task CreateLFLNimbusRequest(LFLNimbusRequest nimbusRequest)
        {
            _trenchesContext.LFLNimbusRequests.Add(nimbusRequest);
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task<LFLUPRNsFreeholdTitles> GetLFLFreeholdByUPRNAndTitle(long UPRN, string titleNumber)
        {
            return await _trenchesContext.LFLUPRNsFreeholdTitles
                .Where(f => f.UPRN == UPRN && f.FreeholdTitle == titleNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<LFLUPRNsLeaseholdTitles> GetLFLLeaseholdByUPRNAndTitle(long UPRN, string titleNumber)
        {
            return await _trenchesContext.LFLUPRNsLeaseholdTitles
                .Where(l => l.UPRN == UPRN && l.LeaseholdTitle == titleNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<LFLUPRNsFreeholdTitles> GetLFLFreeholdByUPRN(long UPRN)
        {
            return await _trenchesContext.LFLUPRNsFreeholdTitles
                .Where(l => l.UPRN == UPRN)
                .FirstOrDefaultAsync();
        }

        public async Task<LFLUPRNsLeaseholdTitles> GetLFLLeaseholdByUPRN(long UPRN)
        {
            return await _trenchesContext.LFLUPRNsLeaseholdTitles
                .Where(l => l.UPRN == UPRN)
                .FirstOrDefaultAsync();
        }

        public async Task CreateLFLFreehold(LFLUPRNsFreeholdTitles freeholdTitle)
        {
            _trenchesContext.LFLUPRNsFreeholdTitles.Add(freeholdTitle);
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task CreateLFLLeasehold(LFLUPRNsLeaseholdTitles leaseholdTitle)
        {
            _trenchesContext.LFLUPRNsLeaseholdTitles.Add(leaseholdTitle);
            await _trenchesContext.SaveChangesAsync();
        }

        #endregion

        #region Openreach

        public async Task<List<ORPrimaryData>> GetAllORPrimaryRecords()
        {
            return await _trenchesContext.ORPrimaryDatas.ToListAsync();
        }

        public async Task<List<ORPrimaryData>> GetORPrimaryRecordsByParentUPRN(long parentUPRN)
        {
            return await _trenchesContext.ORPrimaryDatas
                .Where(o => o.Parent_UPRN == parentUPRN).ToListAsync();
        }

        public async Task<ORPrimaryData> GetORPrimaryDataByOpenreachNumber(string openreachNumber)
        {
            return await _trenchesContext.ORPrimaryDatas
               .Where(o => o.Openreach_SM == openreachNumber).FirstOrDefaultAsync();
        }

        public async Task UpdatePrimaryData(ORPrimaryData primaryData)
        {
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task MassUpdateORRecords(List<ORPrimaryData> orRecords)
        {
            _trenchesContext.UpdateRange(orRecords);
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task<List<long>> GetORDistinctParentUPRN()
        {
            var orRecords = await _trenchesContext.ORPrimaryDatas
                .Where(o => o.Nimbus_UPRN_Processed == 0)
                .OrderBy(o => o.Parent_UPRN)
                .ToListAsync();

            return orRecords.Select(o => o.Parent_UPRN).Distinct().ToList();
        }

        public async Task<ORNimbusRequest> GetORNimbusRequest(long UPRN)
        {
            return await _trenchesContext.ORNimbusRequests
                .Where(r => r.UPRN == UPRN)
                .FirstOrDefaultAsync();
        }

        public async Task CreateORNimbusRequest(ORNimbusRequest nimbusRequest)
        {
            _trenchesContext.ORNimbusRequests.Add(nimbusRequest);
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task<ORUPRNsFreeholdTitles> GetORFreeholdByUPRN(long UPRN)
        {
            return await _trenchesContext.ORUPRNsFreeholdTitles
                .Where(l => l.UPRN == UPRN)
                .FirstOrDefaultAsync();
        }

        public async Task<ORUPRNsFreeholdTitles> GetORFreeholdByUPRNAndTitle(long UPRN, string titleNumber)
        {
            return await _trenchesContext.ORUPRNsFreeholdTitles
                .Where(o => o.UPRN == UPRN && o.FreeholdTitle == titleNumber)
                .FirstOrDefaultAsync();
        }

        public async Task CreateORFreehold(ORUPRNsFreeholdTitles freeholdTitle)
        {
            _trenchesContext.ORUPRNsFreeholdTitles.Add(freeholdTitle);
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task<ORUPRNsLeaseholdTitles> GetORLeaseholdByUPRN(long UPRN)
        {
            return await _trenchesContext.ORUPRNsLeaseholdTitles
                .Where(l => l.UPRN == UPRN)
                .FirstOrDefaultAsync();
        }

        public async Task<ORUPRNsLeaseholdTitles> GetORLeaseholdByUPRNAndTitle(long UPRN, string titleNumber)
        {
            return await _trenchesContext.ORUPRNsLeaseholdTitles
                .Where(l => l.UPRN == UPRN && l.LeaseholdTitle == titleNumber)
                .FirstOrDefaultAsync();
        }

        public async Task CreateORLeasehold(ORUPRNsLeaseholdTitles leaseholdTitle)
        {
            _trenchesContext.ORUPRNsLeaseholdTitles.Add(leaseholdTitle);
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task<List<ViewCorporateOwnership>> GetOROwnerships()
        {
            var ownerships = new List<ViewCorporateOwnership>();
            using (var command = _trenchesContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @"SELECT * FROM View_SM_Primary_UPRN_Related_FreeHold_AllOwnership ORDER BY Title_Number ASC OFFSET 2344 ROWS";
                command.CommandText = @"SELECT        t1.Title_Number, t1.Tenure, t1.Property_Address, t1.District, t1.County, t1.Region, t1.Postcode, t1.Multiple_Address_Indicator, t1.Price_Paid, t1.Proprietor_Name_1, t1.Company_Registration_No_1, t1.Proprietorship_Category_1, 
                         t1.Proprietor_1_Address_1, t1.Proprietor_1_Address_2, t1.Proprietor_1_Address_3, t1.Proprietor_Name_2, t1.Company_Registration_No_2, t1.Proprietorship_Category_2, t1.Proprietor_2_Address_1, t1.Proprietor_2_Address_2, 
                         t1.Proprietor_2_Address_3, t1.Proprietor_Name_3, t1.Company_Registration_No_3, t1.Proprietorship_Category_3, t1.Proprietor_3_Address_1, t1.Proprietor_3_Address_2, t1.Proprietor_3_Address_3, t1.Proprietor_Name_4, 
                         t1.Company_Registration_No_4, t1.Proprietorship_Category_4, t1.Proprietor_4_Address_1, t1.Proprietor_4_Address_2, t1.Proprietor_4_Address_3, t1.Date_Proprietor_Added, t1.Additional_Proprietor_Indicator
FROM            dbo.Overseas_Corporate_Ownership AS t1 INNER JOIN
                             (SELECT DISTINCT FreeholdTitle AS Title_Number
                               FROM            dbo.OR_UPRNsFreeholdTitles
                               WHERE        (FreeholdTitle NOT IN
                                                             (SELECT DISTINCT LeaseholdTitle
                                                               FROM            dbo.OR_UPRNsLeaseholdTitles)) AND (UPRN IN
                                                             (SELECT DISTINCT Parent_UPRN
                                                               FROM            dbo.OR_PrimaryData WHERE Phase = 2))) AS t2 ON t2.Title_Number = t1.Title_Number
								AND Proprietor_Name_1 IN ('C P M SECURITIES LIMITED','HONITON FREEHOLD LIMITED','JD WETHERSPOON PLC','RETIREMENT CARE (BH) LIMITED','RETIREMENT CARE (BH) LIMITED','QDIME LIMITED','ACENT INVESTMENTS LIMITED','P G ENTERPRISES LIMITED','PREMIER GROUND RENTS NO 1 LIMITED')
UNION
SELECT        t1.Title_Number, t1.Tenure, t1.Property_Address, t1.District, t1.County, t1.Region, t1.Postcode, t1.Multiple_Address_Indicator, t1.Price_Paid, t1.Proprietor_Name_1, t1.Company_Registration_No_1, t1.Proprietorship_Category_1, 
                         t1.Proprietor_1_Address_1, t1.Proprietor_1_Address_2, t1.Proprietor_1_Address_3, t1.Proprietor_Name_2, t1.Company_Registration_No_2, t1.Proprietorship_Category_2, t1.Proprietor_2_Address_1, t1.Proprietor_2_Address_2, 
                         t1.Proprietor_2_Address_3, t1.Proprietor_Name_3, t1.Company_Registration_No_3, t1.Proprietorship_Category_3, t1.Proprietor_3_Address_1, t1.Proprietor_3_Address_2, t1.Proprietor_3_Address_3, t1.Proprietor_Name_4, 
                         t1.Company_Registration_No_4, t1.Proprietorship_Category_4, t1.Proprietor_4_Address_1, t1.Proprietor_4_Address_2, t1.Proprietor_4_Address_3, t1.Date_Proprietor_Added, t1.Additional_Proprietor_Indicator
FROM            dbo.Corporate_Ownership AS t1 INNER JOIN
                             (SELECT DISTINCT FreeholdTitle AS Title_Number
                               FROM            dbo.OR_UPRNsFreeholdTitles
                               WHERE        (FreeholdTitle NOT IN
                                                             (SELECT DISTINCT LeaseholdTitle
                                                               FROM            dbo.OR_UPRNsLeaseholdTitles)) AND (UPRN IN
                                                             (SELECT DISTINCT Parent_UPRN
                                                               FROM            dbo.OR_PrimaryData WHERE Phase = 2))) AS t2 ON t2.Title_Number = t1.Title_Number
															   AND Proprietor_Name_1 IN ('C P M SECURITIES LIMITED','HONITON FREEHOLD LIMITED','JD WETHERSPOON PLC','RETIREMENT CARE (BH) LIMITED','RETIREMENT CARE (BH) LIMITED','QDIME LIMITED','ACENT INVESTMENTS LIMITED','P G ENTERPRISES LIMITED','PREMIER GROUND RENTS NO 1 LIMITED')";

                command.CommandType = CommandType.Text;

                _trenchesContext.Database.OpenConnection();

                using var result = await command.ExecuteReaderAsync();
                while (result.Read())
                {
                    ownerships.Add(new ViewCorporateOwnership()
                    {
                        Title_Number = StringHelpers.NullToString(result["Title_Number"]),
                        Tenure = StringHelpers.NullToString(result["Tenure"]),
                        Property_Address = StringHelpers.NullToString(result["Property_Address"]),
                        District = StringHelpers.NullToString(result["District"]),
                        County = StringHelpers.NullToString(result["County"]),
                        Region = StringHelpers.NullToString(result["Region"]),
                        Postcode = StringHelpers.NullToString(result["Postcode"]),
                        Multiple_Address_Indicator = StringHelpers.NullToString(result["Multiple_Address_Indicator"]),
                        Price_Paid = StringHelpers.NullToString(result["Price_Paid"]),
                        Proprietor_Name_1 = StringHelpers.NullToString(result["Proprietor_Name_1"]),
                        Company_Registration_No_1 = StringHelpers.NullToString(result["Company_Registration_No_1"]),
                        Proprietorship_Category_1 = StringHelpers.NullToString(result["Proprietorship_Category_1"]),
                        Proprietor_1_Address_1 = StringHelpers.NullToString(result["Proprietor_1_Address_1"]),
                        Proprietor_1_Address_2 = StringHelpers.NullToString(result["Proprietor_1_Address_2"]),
                        Proprietor_1_Address_3 = StringHelpers.NullToString(result["Proprietor_1_Address_3"]),
                        Proprietor_Name_2 = StringHelpers.NullToString(result["Proprietor_Name_2"]),
                        Company_Registration_No_2 = StringHelpers.NullToString(result["Company_Registration_No_2"]),
                        Proprietorship_Category_2 = StringHelpers.NullToString(result["Proprietorship_Category_2"]),
                        Proprietor_2_Address_1 = StringHelpers.NullToString(result["Proprietor_2_Address_1"]),
                        Proprietor_2_Address_2 = StringHelpers.NullToString(result["Proprietor_2_Address_2"]),
                        Proprietor_2_Address_3 = StringHelpers.NullToString(result["Proprietor_2_Address_3"]),
                        Proprietor_Name_3 = StringHelpers.NullToString(result["Proprietor_Name_3"]),
                        Company_Registration_No_3 = StringHelpers.NullToString(result["Company_Registration_No_3"]),
                        Proprietorship_Category_3 = StringHelpers.NullToString(result["Proprietorship_Category_3"]),
                        Proprietor_3_Address_1 = StringHelpers.NullToString(result["Proprietor_3_Address_1"]),
                        Proprietor_3_Address_2 = StringHelpers.NullToString(result["Proprietor_3_Address_2"]),
                        Proprietor_3_Address_3 = StringHelpers.NullToString(result["Proprietor_3_Address_3"]),
                        Proprietor_Name_4 = StringHelpers.NullToString(result["Proprietor_Name_4"]),
                        Company_Registration_No_4 = StringHelpers.NullToString(result["Company_Registration_No_4"]),
                        Proprietorship_Category_4 = StringHelpers.NullToString(result["Proprietorship_Category_4"]),
                        Proprietor_4_Address_1 = StringHelpers.NullToString(result["Proprietor_4_Address_1"]),
                        Proprietor_4_Address_2 = StringHelpers.NullToString(result["Proprietor_4_Address_2"]),
                        Proprietor_4_Address_3 = StringHelpers.NullToString(result["Proprietor_4_Address_3"]),
                        Date_Proprietor_Added = StringHelpers.NullToString(result["Date_Proprietor_Added"]),
                        Additional_Proprietor_Indicator = StringHelpers.NullToString(result["Additional_Proprietor_Indicator"]),
                    }); 
                }

                _trenchesContext.Database.CloseConnection();
            }
            return ownerships;
        }

        public async Task<List<OROwnerLinking>> GetAllOROwnerLinkings()
        {
            return await _trenchesContext.OROwnerLinkings.ToListAsync();
        }

        public async Task<List<OROwnerLinking>> GetUnhandledOROwnerLinkings()
        {
            return await _trenchesContext.OROwnerLinkings
                .Where(l => l.OwnerId == null)
                .OrderBy(l => l.TitleNumber).ToListAsync();
        }

        public async Task<List<OROwnerLinking>> GetOROwnerLinkingsByOpenreachNumber(string openreachNumber)
        {
            return await _trenchesContext.OROwnerLinkings
                .Where(l => l.OpenreachNumber == openreachNumber && l.OwnerId != null)
                .OrderBy(l => l.TitleNumber).ToListAsync();
        }

        public async Task<List<OROwnerLinking>> GetOROwnerLinkingsByOwnerId(string ownerId)
        {
            return await _trenchesContext.OROwnerLinkings
                .Where(l => l.OwnerId == ownerId && l.OwnerId != null)
                .ToListAsync();
        }

        public async Task<List<OROwnerLinking>> GetOROwnerLinkingsByLetterReference(string letterReference)
        {
            return await _trenchesContext.OROwnerLinkings
                .Where(l => l.LetterReference == letterReference && l.OwnerId != null)
                .OrderBy(l => l.TitleNumber).ToListAsync();
        }

        public async Task<string> GetLetterReferenceByOpenreachNumber(string openreachNumber)
        {
            var ownerLinking = await _trenchesContext.OROwnerLinkings
                .Where(l => l.OpenreachNumber == openreachNumber 
                    && l.LetterReference != null && l.OwnerId != null)
                .FirstOrDefaultAsync();

            if (ownerLinking == null)
            {
                return string.Empty;
            }

            return ownerLinking.LetterReference;
        }

        public async Task<List<string>> GetDistinctOwnerIdsByOpenreachNumber(string openreachNumber)
        {
            var distinctOwners = await _trenchesContext.OROwnerLinkings
                .Where(l => l.OpenreachNumber == openreachNumber && l.OwnerId != null)
                .Select(l => l.OwnerId)
                .Distinct()
                .ToListAsync();

            return distinctOwners;
        }

        public async Task UpdateOROwnerLinking(OROwnerLinking ownerLinking)
        {
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task CreateORTask(ORTask orTask)
        {
            _trenchesContext.ORTasks.Add(orTask);
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task UpdateORTask(ORTask orTask)
        {
            await _trenchesContext.SaveChangesAsync();
        }

        public async Task<ORTask> GetORTaskByTaskId(string taskId)
        {
            return await _trenchesContext.ORTasks.Where(t => t.TaskId == taskId)
                .FirstOrDefaultAsync();
        }

        #endregion

        #region Corporate Ownership

        public async Task<CorporateOwnership> GetOwnershipByTitleNumber(string titleNumber)
        {
            return await _trenchesContext.CorporateOwnerships.Where(t => t.Title_Number == titleNumber)
                .FirstOrDefaultAsync();
        }

        #endregion

    }
}
