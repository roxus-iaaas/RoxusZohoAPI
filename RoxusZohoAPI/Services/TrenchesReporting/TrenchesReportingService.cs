using Newtonsoft.Json;
using RoxusZohoAPI.Entities.TrenchesReportDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Nimbus;
using RoxusZohoAPI.Models.TrenchesReporting;
using RoxusZohoAPI.Models.Zoho.Custom;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using RoxusZohoAPI.Repositories;
using RoxusZohoAPI.Services.CompaniesHouse;
using RoxusZohoAPI.Services.Nimbus;
using RoxusZohoAPI.Services.Zoho.ZohoCRM;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.TrenchesReporting
{
    public class TrenchesReportingService : ITrenchesReportingService
    {
        private readonly ITrenchesReportingRepository _trenchesRepository;
        private readonly IRoxusLoggingRepository _roxusLoggingRepository;
        private readonly INimbusService _nimbusService;
        private readonly ICompaniesHouseService _companiesHouseService;
        private readonly IZohoAccountService _zohoAccountService;
        private readonly IZohoTitleService _zohoTitleService;
        private readonly IZohoOpenreachService _zohoOpenreachService;
        private readonly IZohoContactService _zohoContactService;
        private readonly IZohoNoteService _zohoNoteService;

        public TrenchesReportingService(ITrenchesReportingRepository trenchesRepository, 
            IRoxusLoggingRepository roxusLoggingRepository, INimbusService nimbusService, 
            IZohoAccountService zohoAccountService, ICompaniesHouseService companiesHouseService, 
            IZohoTitleService zohoTitleService, IZohoOpenreachService zohoOpenreachService, 
            IZohoContactService zohoContactService, IZohoNoteService zohoNoteService)
        {
            _trenchesRepository = trenchesRepository;
            _roxusLoggingRepository = roxusLoggingRepository;
            _nimbusService = nimbusService;
            _zohoAccountService = zohoAccountService;
            _companiesHouseService = companiesHouseService;
            _zohoTitleService = zohoTitleService;
            _zohoOpenreachService = zohoOpenreachService;
            _zohoContactService = zohoContactService;
            _zohoNoteService = zohoNoteService;
        }

        private string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public async Task<ApiResultDto<string>> SyncingTaskDelete(string taskUrl)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = TrenchesReportingConstants.MSG_SYNCING_400
            };

            try
            {
                var taskSplits = taskUrl.Split("/");
                int length = taskSplits.Length;
                string projectId = taskSplits[length - 3];
                string taskId = taskSplits[length - 1];

                #region STEP 1: Delete Task from ZohoTask table
                var taskEntity = await _trenchesRepository.GetTaskByTaskIdAndProjectId(taskId, projectId);
                if (taskEntity != null)
                {
                    await _trenchesRepository.DeleteTaskRecord(taskEntity);
                }
                #endregion

                #region STEP 2: Delete Task from TrenchesTask table
                var trenchesTask = await _roxusLoggingRepository.GetTrenchesTaskByTaskIdAndProjectId(taskId, projectId);
                if (trenchesTask != null)
                {
                    await _roxusLoggingRepository.DeleteTaskFromDb(trenchesTask);
                }
                #endregion

                var syncingRecord = new SyncingRecord()
                {
                    ModuleName = "Task",
                    TaskUrl = taskUrl,
                    Action = SyncingAction.Delete,
                    Status = SyncingStatus.Completed,
                    ZohoDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                };

                apiResult.Code = ResultCode.OK;
                apiResult.Message = TrenchesReportingConstants.MSG_SYNCING_200;
                await _trenchesRepository.CreateSyncingRecord(syncingRecord);

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message}\n{ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> UpsertSyncingRecords(SyncingRecord syncingRecord)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = TrenchesReportingConstants.MSG_SYNCING_400
            };

            try
            {
                var syncingEntity = await _trenchesRepository.GetSyncingRecord(syncingRecord);
                if (syncingEntity != null)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = TrenchesReportingConstants.MSG_SYNCING_EXIST;
                    return apiResult;
                }

                syncingRecord.CreatedDate = DateTime.UtcNow;
                syncingRecord.ModifiedDate = DateTime.UtcNow;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = TrenchesReportingConstants.MSG_SYNCING_200;
                await _trenchesRepository.CreateSyncingRecord(syncingRecord);

                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> HandleLightningFibre()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {

                // STEP 1: Get All Address Data with Type F - MDU
                var lflRecords = await _trenchesRepository.GetLFLAddressDatasByType("LFL - MDU");

                // STEP 2: Loop all the Lightning Records and handle the data
                foreach (var lflRecord in lflRecords)
                {
                    // Step 2.1: Check if UPRN is already been requested
                    long uprn = lflRecord.UPRN;

                    var nimbusRequest = await _trenchesRepository.GetLFLNimbusRequest(uprn);
                    if (nimbusRequest != null)
                    {
                        continue;
                    }

                    // Step 2.2: Check if Freehold table already has record
                    var freeholdRecord = await _trenchesRepository.GetLFLFreeholdByUPRN(uprn);
                    if (freeholdRecord != null)
                    {
                        continue;
                    }

                    // Step 2.3: Check if Freehold table already has record
                    var leaseholdRecord = await _trenchesRepository.GetORLeaseholdByUPRN(uprn);
                    if (freeholdRecord != null)
                    {
                        continue;
                    }

                    // Step 2.4: Call Nimbus API to get data
                    var siteInventoryResponse = await _nimbusService.SiteInventory1_LFL(uprn);
                    if (siteInventoryResponse.Code != ResultCode.OK)
                    {
                        continue;
                    }
                    var siteInventoryDetails = siteInventoryResponse.Data;
                    var siteResults = siteInventoryDetails.results;

                    var freeholdSets = new HashSet<string>();
                    var leaseholdSets = new HashSet<string>();
                    foreach (var siteResult in siteResults)
                    {
                        string freeholdTitle = siteResult.freeholdTitleNumber;
                        bool corporatelyOwned = siteResult.corporatelyOwned;
                        if (!string.IsNullOrEmpty(freeholdTitle))
                        {
                            string freeholdStr = $"{uprn}|{freeholdTitle}|{corporatelyOwned}";
                            freeholdSets.Add(freeholdStr);
                        }
                        var leaseholds = siteResult.leaseholdTitleNumbers;
                        if (leaseholds != null)
                        {
                            foreach (var leasehold in leaseholds)
                            {
                                var uprns = leasehold.uprNs;
                                string leaseholdTitle = leasehold.leasehold;
                                if (string.IsNullOrEmpty(leaseholdTitle))
                                {
                                    continue;
                                }
                                if (uprns == null || uprns.Count == 0)
                                {
                                    continue;
                                }
                                foreach (var leaseUprn in uprns)
                                {
                                    if (!string.IsNullOrEmpty(leaseUprn))
                                    {
                                        if (!string.IsNullOrEmpty(freeholdTitle))
                                        {
                                            string freeholdStr = $"{leaseUprn}|{freeholdTitle}|{corporatelyOwned}";
                                            freeholdSets.Add(freeholdStr);
                                        }
                                        string leaseholdStr = $"{leaseUprn}|{leaseholdTitle}";
                                        leaseholdSets.Add(leaseholdStr);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var freeholdMap in freeholdSets)
                    {
                        var freeholdSplits = freeholdMap.Split('|');
                        string freeUprn = freeholdSplits[0];
                        string freeTitle = freeholdSplits[1];
                        string freeCorporatelyOwned = freeholdSplits[2];
                        var freeRecord = await _trenchesRepository.GetLFLFreeholdByUPRNAndTitle(long.Parse(freeUprn), freeTitle);
                        if (freeRecord != null)
                        {
                            continue;
                        }
                        var freeholdForCreate = new LFLUPRNsFreeholdTitles()
                        {
                            UPRN = long.Parse(freeUprn),
                            FreeholdTitle = freeTitle,
                            Ownership = freeCorporatelyOwned.Equals("True", StringComparison.InvariantCultureIgnoreCase) ? "C" : "P",
                            Created = DateTime.UtcNow
                        };
                        await _trenchesRepository.CreateLFLFreehold(freeholdForCreate);
                    }

                    foreach (var leaseholdMap in leaseholdSets)
                    {
                        var leaseholdSplits = leaseholdMap.Split('|');
                        string leaseUprn = leaseholdSplits[0];
                        string leaseTitle = leaseholdSplits[1];
                        var leaseRecord = await _trenchesRepository.GetORLeaseholdByUPRNAndTitle(long.Parse(leaseUprn), leaseTitle);
                        if (leaseRecord != null)
                        {
                            continue;
                        }
                        var leaseholdForCreate = new LFLUPRNsLeaseholdTitles()
                        {
                            UPRN = long.Parse(leaseUprn),
                            LeaseholdTitle = leaseTitle,
                            Created = DateTime.UtcNow
                        };
                        await _trenchesRepository.CreateLFLLeasehold(leaseholdForCreate);
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> HandleOpenReach()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {
                
                // STEP 1: Get All Address Data with Type F - MDU
                var parentUPRNs = await _trenchesRepository.GetORDistinctParentUPRN();

                // STEP 2: Loop all the Lightning Records and handle the data
                foreach (var parentUPRN in parentUPRNs)
                {
                    // Step 2.1: Check if UPRN is already been requested
                    var nimbusRequest = await _trenchesRepository.GetORNimbusRequest(parentUPRN);
                    string response = string.Empty;
                    if (nimbusRequest != null)
                    {
                        response = nimbusRequest.Response;
                    }

                    // Step 2.2: Check if Freehold table already has record
                    var freeholdRecord = await _trenchesRepository.GetORFreeholdByUPRN(parentUPRN);
                    if (freeholdRecord != null)
                    {
                        continue;
                    }

                    // Step 2.3: Check if Freehold table already has record
                    var leaseholdRecord = await _trenchesRepository.GetORLeaseholdByUPRN(parentUPRN);
                    if (leaseholdRecord != null)
                    {
                        continue;
                    }

                    SiteInventory1Response siteInventoryDetails = null;

                    // Step 2.4: Call Nimbus API to get data
                    if (string.IsNullOrEmpty(response))
                    {
                        var siteInventoryResponse = await _nimbusService.SiteInventory1_OR(parentUPRN);
                        if (siteInventoryResponse.Code != ResultCode.OK)
                        {
                            continue;
                        }
                        siteInventoryDetails = siteInventoryResponse.Data;
                    }
                    else
                    {
                        siteInventoryDetails = JsonConvert.DeserializeObject<SiteInventory1Response>(response);
                    }

                    var siteResults = siteInventoryDetails.results;

                    var freeholdSets = new HashSet<string>();
                    var leaseholdSets = new HashSet<string>();

                    foreach (var siteResult in siteResults)
                    {
                        string freeholdTitle = siteResult.freeholdTitleNumber;
                        bool corporatelyOwned = siteResult.corporatelyOwned;
                        if (!string.IsNullOrEmpty(freeholdTitle))
                        {
                            string freeholdStr = $"{parentUPRN}|{freeholdTitle}|{corporatelyOwned}";
                            freeholdSets.Add(freeholdStr);
                        }
                        var leaseholds = siteResult.leaseholdTitleNumbers;
                        if (leaseholds != null)
                        {
                            foreach (var leasehold in leaseholds)
                            {
                                var uprns = leasehold.uprNs;
                                string leaseholdTitle = leasehold.leasehold;
                                if (string.IsNullOrEmpty(leaseholdTitle))
                                {
                                    continue;
                                }
                                if (uprns == null || uprns.Count == 0)
                                {
                                    continue;
                                }
                                foreach (var leaseUprn in uprns)
                                {
                                    if (!string.IsNullOrEmpty(leaseUprn))
                                    {
                                        if (!string.IsNullOrEmpty(freeholdTitle))
                                        {
                                            string freeholdStr = $"{leaseUprn}|{freeholdTitle}|{corporatelyOwned}";
                                            freeholdSets.Add(freeholdStr);
                                            string leaseholdStr = $"{leaseUprn}|{leaseholdTitle}|{freeholdTitle}";
                                            leaseholdSets.Add(leaseholdStr);
                                        }
                                        else
                                        {
                                            string leaseholdStr = $"{leaseUprn}|{leaseholdTitle}|";
                                            leaseholdSets.Add(leaseholdStr);
                                        }
                                        
                                    }
                                }
                            }
                        }
                    }

                    foreach (var freeholdMap in freeholdSets)
                    {
                        var freeholdSplits = freeholdMap.Split('|');
                        string freeUprn = freeholdSplits[0];
                        string freeTitle = freeholdSplits[1];
                        string freeCorporatelyOwned = freeholdSplits[2];
                        var freeRecord = await _trenchesRepository.GetORFreeholdByUPRNAndTitle(long.Parse(freeUprn), freeTitle);
                        if (freeRecord != null)
                        {
                            continue;
                        }
                        var freeholdForCreate = new ORUPRNsFreeholdTitles()
                        {
                            ParentUPRN = parentUPRN,
                            UPRN = long.Parse(freeUprn),
                            FreeholdTitle = freeTitle,
                            Ownership = freeCorporatelyOwned.Equals("True", StringComparison.InvariantCultureIgnoreCase) ? "C" : "P",
                            Created = DateTime.UtcNow
                        };
                        await _trenchesRepository.CreateORFreehold(freeholdForCreate);
                    }

                    foreach (var leaseholdMap in leaseholdSets)
                    {
                        var leaseholdSplits = leaseholdMap.Split('|');
                        string leaseUprn = leaseholdSplits[0];
                        string leaseTitle = leaseholdSplits[1];
                        string freeholdTitle = leaseholdSplits[2];
                        var leaseRecord = await _trenchesRepository.GetORLeaseholdByUPRNAndTitle(long.Parse(leaseUprn), leaseTitle);
                        if (leaseRecord != null)
                        {
                            continue;
                        }
                        var leaseholdForCreate = new ORUPRNsLeaseholdTitles()
                        {
                            ParentUPRN = parentUPRN,
                            UPRN = long.Parse(leaseUprn),
                            FreeholdTitle = freeholdTitle,
                            LeaseholdTitle = leaseTitle,
                            Created = DateTime.UtcNow
                        };
                        await _trenchesRepository.CreateORLeasehold(leaseholdForCreate);
                    }
                   
                    // Step 3: Update Primary Data table
                    var primaryRecords = await _trenchesRepository.GetORPrimaryRecordsByParentUPRN(parentUPRN);
                    foreach (var primaryRecord in primaryRecords)
                    {
                        primaryRecord.Nimbus_UPRN_Processed = 1;
                        primaryRecord.Created = DateTime.UtcNow;
                        primaryRecord.Modified = DateTime.UtcNow;
                    }
                    await _trenchesRepository.MassUpdateORRecords(primaryRecords);
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> HandleOpenReach1402()
        {

            OleDbConnection connection;
            OleDbCommand command;
            OleDbDataReader dr;

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {

                string commandText = "SELECT * FROM [Sheet1$]";
                string inputFullPath = @"D:\1. Roxus\2. Projects\1. Trenches\8. Openreach\Openreach_SM_NimbusRequired.xlsx";
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{inputFullPath}';Extended Properties=\"Excel 12.0;HDR=YES;\"";

                // STEP 1: Read the Excel file
                using (connection = new OleDbConnection(connectionString))
                {
                    command = new OleDbCommand(commandText, connection);
                    connection.Open();
                    dr = command.ExecuteReader();

                    int count = 0;

                    while (dr.Read())
                    {
                        count++;

                        string parentUPRN = dr["Parent_UPRN"].ToString();

                        var oldPrimaryRecords = await _trenchesRepository.GetORPrimaryRecordsByParentUPRN(long.Parse(parentUPRN));

                        // Step 2.1: Check if UPRN is already been requested
                        var nimbusRequest = await _trenchesRepository.GetORNimbusRequest(long.Parse(parentUPRN));
                        string response = string.Empty;
                        if (nimbusRequest != null)
                        {
                            response = nimbusRequest.Response;
                        }

                        // Step 2.2: Check if Freehold table already has record
                        var freeholdRecord = await _trenchesRepository.GetORFreeholdByUPRN(long.Parse(parentUPRN));
                        if (freeholdRecord != null)
                        {
                            foreach (var primaryRecord in oldPrimaryRecords)
                            {
                                primaryRecord.Nimbus_UPRN_Processed = 1;
                                primaryRecord.Phase = 4;
                                primaryRecord.Created = DateTime.UtcNow;
                                primaryRecord.Modified = DateTime.UtcNow;
                            }
                            await _trenchesRepository.MassUpdateORRecords(oldPrimaryRecords);
                            continue;
                        }

                        // Step 2.3: Check if Leasehold table already has record
                        var leaseholdRecord = await _trenchesRepository.GetORLeaseholdByUPRN(long.Parse(parentUPRN));
                        if (leaseholdRecord != null)
                        {
                            foreach (var primaryRecord in oldPrimaryRecords)
                            {
                                primaryRecord.Nimbus_UPRN_Processed = 1;
                                primaryRecord.Phase = 4;
                                primaryRecord.Created = DateTime.UtcNow;
                                primaryRecord.Modified = DateTime.UtcNow;
                            }
                            await _trenchesRepository.MassUpdateORRecords(oldPrimaryRecords);
                            continue;
                        }

                        SiteInventory1Response siteInventoryDetails = null;

                        // Step 2.4: Call Nimbus API to get data
                        if (string.IsNullOrEmpty(response))
                        {
                            var siteInventoryResponse = await _nimbusService.SiteInventory1_OR(long.Parse(parentUPRN));
                            if (siteInventoryResponse.Code != ResultCode.OK)
                            {
                                foreach (var primaryRecord in oldPrimaryRecords)
                                {
                                    primaryRecord.Nimbus_UPRN_Processed = 1;
                                    primaryRecord.Phase = 4;
                                    primaryRecord.Created = DateTime.UtcNow;
                                    primaryRecord.Modified = DateTime.UtcNow;
                                }
                                await _trenchesRepository.MassUpdateORRecords(oldPrimaryRecords);
                                continue;
                            }
                            siteInventoryDetails = siteInventoryResponse.Data;
                        }
                        else
                        {
                            if (response.Contains("No results found"))
                            {
                                foreach (var primaryRecord in oldPrimaryRecords)
                                {
                                    primaryRecord.Nimbus_UPRN_Processed = 1;
                                    primaryRecord.Phase = 4;
                                    primaryRecord.Created = DateTime.UtcNow;
                                    primaryRecord.Modified = DateTime.UtcNow;
                                }
                                await _trenchesRepository.MassUpdateORRecords(oldPrimaryRecords);
                                continue;
                            }

                            siteInventoryDetails = JsonConvert.DeserializeObject<SiteInventory1Response>(response);
                        }

                        var siteResults = siteInventoryDetails.results;

                        var freeholdSets = new HashSet<string>();
                        var leaseholdSets = new HashSet<string>();

                        foreach (var siteResult in siteResults)
                        {
                            string freeholdTitle = siteResult.freeholdTitleNumber;
                            bool corporatelyOwned = siteResult.corporatelyOwned;
                            if (!string.IsNullOrEmpty(freeholdTitle))
                            {
                                string freeholdStr = $"{parentUPRN}|{freeholdTitle}|{corporatelyOwned}";
                                freeholdSets.Add(freeholdStr);
                            }
                            var leaseholds = siteResult.leaseholdTitleNumbers;
                            if (leaseholds != null)
                            {
                                foreach (var leasehold in leaseholds)
                                {
                                    var uprns = leasehold.uprNs;
                                    string leaseholdTitle = leasehold.leasehold;
                                    if (string.IsNullOrEmpty(leaseholdTitle))
                                    {
                                        continue;
                                    }
                                    if (uprns == null || uprns.Count == 0)
                                    {
                                        continue;
                                    }
                                    foreach (var leaseUprn in uprns)
                                    {
                                        if (!string.IsNullOrEmpty(leaseUprn))
                                        {
                                            if (!string.IsNullOrEmpty(freeholdTitle))
                                            {
                                                string freeholdStr = $"{leaseUprn}|{freeholdTitle}|{corporatelyOwned}";
                                                freeholdSets.Add(freeholdStr);
                                                string leaseholdStr = $"{leaseUprn}|{leaseholdTitle}|{freeholdTitle}";
                                                leaseholdSets.Add(leaseholdStr);
                                            }
                                            else
                                            {
                                                string leaseholdStr = $"{leaseUprn}|{leaseholdTitle}|";
                                                leaseholdSets.Add(leaseholdStr);
                                            }

                                        }
                                    }
                                }
                            }
                        }

                        foreach (var freeholdMap in freeholdSets)
                        {
                            var freeholdSplits = freeholdMap.Split('|');
                            string freeUprn = freeholdSplits[0];
                            string freeTitle = freeholdSplits[1];
                            string freeCorporatelyOwned = freeholdSplits[2];
                            var freeRecord = await _trenchesRepository.GetORFreeholdByUPRNAndTitle(long.Parse(freeUprn), freeTitle);
                            if (freeRecord != null)
                            {
                                continue;
                            }
                            var freeholdForCreate = new ORUPRNsFreeholdTitles()
                            {
                                ParentUPRN = long.Parse(parentUPRN),
                                UPRN = long.Parse(freeUprn),
                                FreeholdTitle = freeTitle,
                                Ownership = freeCorporatelyOwned.Equals("True", StringComparison.InvariantCultureIgnoreCase) ? "C" : "P",
                                Created = DateTime.UtcNow
                            };
                            await _trenchesRepository.CreateORFreehold(freeholdForCreate);
                        }

                        foreach (var leaseholdMap in leaseholdSets)
                        {
                            var leaseholdSplits = leaseholdMap.Split('|');
                            string leaseUprn = leaseholdSplits[0];
                            string leaseTitle = leaseholdSplits[1];
                            string freeholdTitle = leaseholdSplits[2];
                            var leaseRecord = await _trenchesRepository.GetORLeaseholdByUPRNAndTitle(long.Parse(leaseUprn), leaseTitle);
                            if (leaseRecord != null)
                            {
                                continue;
                            }
                            var leaseholdForCreate = new ORUPRNsLeaseholdTitles()
                            {
                                ParentUPRN = long.Parse(parentUPRN),
                                UPRN = long.Parse(leaseUprn),
                                FreeholdTitle = freeholdTitle,
                                LeaseholdTitle = leaseTitle,
                                Created = DateTime.UtcNow
                            };
                            await _trenchesRepository.CreateORLeasehold(leaseholdForCreate);
                        }

                        // Step 3: Update Primary Data table
                        var primaryRecords = await _trenchesRepository.GetORPrimaryRecordsByParentUPRN(long.Parse(parentUPRN));
                        foreach (var primaryRecord in primaryRecords)
                        {
                            primaryRecord.Nimbus_UPRN_Processed = 1;
                            primaryRecord.Phase = 4;
                            primaryRecord.Created = DateTime.UtcNow;
                            primaryRecord.Modified = DateTime.UtcNow;
                        }
                        await _trenchesRepository.MassUpdateORRecords(primaryRecords);

                        break;
                    }
                
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> HandleOpenReachAccounts(string apiKey)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = TrenchesReportingConstants.MSG_OR_OWNERSHIP_400
            };

            string emailSubject = "[CompaniesHouse] Cannot find Company Name <<CompanyName>>";

            string emailBody = @"Dear Roxus Support,<br><br>There is no result for Company details below:<br><ul>
                                    <li>Title Number: <<TitleNumber>></li>
                                    <li>Company Name: <<CompanyName>></li>
                                    <li>Company Number: <<CompanyNumber>></li>
                                    <li>Account URL (Zoho CRM): https://crm.zoho.eu/crm/org20069538520/tab/Accounts/<<AccountId>></li>
                                </ul><br>
                                Best Regards,<br> Roxus Automation";

            var emailContent = new EmailContent()
            {
                Email = EmailConstants.TrenchesEmail,
                Body = string.Empty,
                Clients = "help@roxus.io",
                Subject = string.Empty,
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Outlook_Email_SmtpServer
            };
            var appConfiguration = await _roxusLoggingRepository.GetAppConfigurationById(EmailConstants.TrenchesId);
            emailContent.Password = appConfiguration.Password;

            try
            {
                // STEP 1: Get All Openreach Companies
                var ownerships = await _trenchesRepository.GetOROwnerships();
                var accountSet = new HashSet<string>();
                int rowCount = 0;

                // STEP 2: Extract Company Information
                foreach (var ownership in ownerships)
                {
                    rowCount++;
                    /*
                    if (rowCount < 383)
                    {
                        continue;
                    }
                    */
                    Thread.Sleep(1000);
                    var titleNumber = ownership.Title_Number;
                    string companyName1 = ownership.Proprietor_Name_1;

                    if (accountSet.Contains(companyName1))
                    {
                        continue;
                    }
                    accountSet.Add(companyName1);

                    string reduceName1 = StringHelpers.ReduceCompanyName(companyName1);
                    string companyRegistration1 = ownership.Company_Registration_No_1;
                    string companyFullAddress1 = ownership.Proprietor_1_Address_1;

                    string companyName2 = ownership.Proprietor_Name_2;
                    string reduceName2 = StringHelpers.ReduceCompanyName(companyName2);
                    string companyRegistration2 = ownership.Company_Registration_No_2;
                    string companyFullAddress2 = ownership.Proprietor_2_Address_1;

                    AccountForCreation accountForCreation = null;

                    #region Handle Company 1
                    // Step 2.1: Search Company by Company Name
                    string accountId = string.Empty;
                    var searchAccountByNameResult = await _zohoAccountService.SearchAccountByAccountName(apiKey, reduceName1);
                    if (searchAccountByNameResult.Code == ResultCode.OK)
                    {
                        var searchAccountResponse = searchAccountByNameResult.Data;
                        accountId = searchAccountResponse.data[0].id;
                    }
                    if (string.IsNullOrEmpty(accountId) && !string.IsNullOrEmpty(companyRegistration1))
                    {
                        // Step 2.2: Search Company by Company Number
                        var searchAccountByNumberResult = await _zohoAccountService.SearchAccountByCompanyRegistration(apiKey, companyRegistration1);
                        if (searchAccountByNumberResult.Code == ResultCode.OK)
                        {
                            var searchAccountResponse = searchAccountByNumberResult.Data;
                            accountId = searchAccountResponse.data[0].id;
                        }
                    }

                    if (!string.IsNullOrEmpty(accountId))
                    {
                        // Account already exists, END THE FUNCTION
                        continue;
                    }

                    // STEP 3: Create Company in Zoho CRM
                    bool searchCompaniesSuccess = false;
                    if (!string.IsNullOrEmpty(companyRegistration1))
                    {

                        // Step 3.1: Call Companies House API to get company data
                        var companiesResult = await _companiesHouseService.GetCompanies(companyRegistration1);

                        var companiesResponse = companiesResult.Data;
                        var totalResults = companiesResponse.total_results;

                        if (totalResults == 1)
                        {
                            searchCompaniesSuccess = true;
                            // Step 3.2: Extract Companies House data
                            var companyDetails = companiesResponse.items[0];
                            string companyName = companyDetails.title;
                            string companyNumber = companyDetails.company_number;
                            var companyAddress = companyDetails.address;
                            string addressLine1 = companyAddress.address_line_1;
                            string premises = companyAddress.premises;

                            string street = string.Empty;
                            if (!string.IsNullOrEmpty(premises))
                            {
                                street = $"{premises} {addressLine1}";
                            }
                            else
                            {
                                street = addressLine1;
                            }

                            string locality = companyAddress.locality;
                            string postalCode = companyAddress.postal_code;

                            accountForCreation = new AccountForCreation()
                            {
                                Account_Name = reduceName1,
                                Company_Registration = companyNumber,
                                Billing_Street = street,
                                Billing_City = locality,
                                Billing_Code = postalCode
                            };

                            var upsertRequest = new UpsertRequest<AccountForCreation>();
                            upsertRequest.data.Add(accountForCreation);

                            var createAccountResult = await _zohoAccountService.CreateAccount(apiKey, upsertRequest);
                            if (createAccountResult.Code == ResultCode.OK)
                            {
                                var createAccountResponse = createAccountResult.Data;
                                accountId = createAccountResponse.data[0].details.id;
                            }

                        }
                        else
                        {
                            searchCompaniesSuccess = false;
                        }

                    }

                    if (!searchCompaniesSuccess)
                    {

                        // Step 3.3: Create Account based on data in Geo Spatial
                        accountForCreation = new AccountForCreation()
                        {
                            Account_Name = reduceName1,
                            Company_Registration = companyRegistration1,
                            Billing_Street = companyFullAddress1
                        };

                        var upsertRequest = new UpsertRequest<AccountForCreation>();
                        upsertRequest.data.Add(accountForCreation);

                        var createAccountResult = await _zohoAccountService.CreateAccount(apiKey, upsertRequest);
                        if (createAccountResult.Code == ResultCode.OK)
                        {
                            var createAccountResponse = createAccountResult.Data;
                            accountId = createAccountResponse.data[0].details.id;
                        }

                        emailContent.Subject = emailSubject.Replace("<<CompanyName>>", reduceName1, StringComparison.InvariantCultureIgnoreCase);
                        emailContent.Body = emailBody.Replace("<<TitleNumber>>", titleNumber, StringComparison.InvariantCultureIgnoreCase)
                                             .Replace("<<CompanyName>>", reduceName1, StringComparison.InvariantCultureIgnoreCase)
                                             .Replace("<<CompanyNumber>>", string.IsNullOrEmpty(companyRegistration1) ? "{EMPTY}" : companyRegistration1,
                                             StringComparison.InvariantCultureIgnoreCase)
                                             .Replace("<<AccountId>>", accountId, StringComparison.InvariantCultureIgnoreCase);
                        await EmailHelpers.SendEmail(emailContent);

                    }

                    #endregion

                    if (string.IsNullOrEmpty(companyName2))
                    {
                        #region Handle Company 2

                        // Step 2.1: Search Company by Company Name
                        accountId = string.Empty;
                        searchAccountByNameResult = await _zohoAccountService.SearchAccountByAccountName(apiKey, reduceName2);
                        if (searchAccountByNameResult.Code == ResultCode.OK)
                        {
                            var searchAccountResponse = searchAccountByNameResult.Data;
                            accountId = searchAccountResponse.data[0].id;
                        }
                        if (string.IsNullOrEmpty(accountId) && !string.IsNullOrEmpty(companyRegistration1))
                        {
                            // Step 2.2: Search Company by Company Number
                            var searchAccountByNumberResult = await _zohoAccountService.SearchAccountByCompanyRegistration(apiKey, companyRegistration2);
                            if (searchAccountByNumberResult.Code == ResultCode.OK)
                            {
                                var searchAccountResponse = searchAccountByNumberResult.Data;
                                accountId = searchAccountResponse.data[0].id;
                            }
                        }

                        if (!string.IsNullOrEmpty(accountId))
                        {
                            // Account already exists, END THE FUNCTION
                            continue;
                        }

                        // STEP 3: Create Company in Zoho CRM
                        searchCompaniesSuccess = false;
                        if (!string.IsNullOrEmpty(companyRegistration2))
                        {

                            // Step 3.1: Call Companies House API to get company data
                            var companiesResult = await _companiesHouseService.GetCompanies(companyRegistration2);

                            var companiesResponse = companiesResult.Data;
                            var totalResults = companiesResponse.total_results;

                            if (totalResults == 1)
                            {
                                searchCompaniesSuccess = true;
                                // Step 3.2: Extract Companies House data
                                var companyDetails = companiesResponse.items[0];
                                string companyName = companyDetails.title;
                                string companyNumber = companyDetails.company_number;
                                var companyAddress = companyDetails.address;
                                string addressLine1 = companyAddress.address_line_1;
                                string premises = companyAddress.premises;

                                string street = string.Empty;
                                if (!string.IsNullOrEmpty(premises))
                                {
                                    street = $"{premises} {addressLine1}";
                                }
                                else
                                {
                                    street = addressLine1;
                                }

                                string locality = companyAddress.locality;
                                string postalCode = companyAddress.postal_code;

                                accountForCreation = new AccountForCreation()
                                {
                                    Account_Name = reduceName1,
                                    Company_Registration = companyNumber,
                                    Billing_Street = street,
                                    Billing_City = locality,
                                    Billing_Code = postalCode
                                };

                                var upsertRequest = new UpsertRequest<AccountForCreation>();
                                upsertRequest.data.Add(accountForCreation);

                                var createAccountResult = await _zohoAccountService.CreateAccount(apiKey, upsertRequest);
                                if (createAccountResult.Code == ResultCode.OK)
                                {
                                    var createAccountResponse = createAccountResult.Data;
                                    accountId = createAccountResponse.data[0].details.id;
                                }

                            }
                            else
                            {
                                searchCompaniesSuccess = false;
                            }
                        }

                        if (!searchCompaniesSuccess)
                        {

                            // Step 3.3: Create Account based on data in Geo Spatial
                            accountForCreation = new AccountForCreation()
                            {
                                Account_Name = reduceName1,
                                Company_Registration = companyRegistration1,
                                Billing_Street = companyFullAddress1
                            };

                            var upsertRequest = new UpsertRequest<AccountForCreation>();
                            upsertRequest.data.Add(accountForCreation);

                            var createAccountResult = await _zohoAccountService.CreateAccount(apiKey, upsertRequest);
                            if (createAccountResult.Code == ResultCode.OK)
                            {
                                var createAccountResponse = createAccountResult.Data;
                                accountId = createAccountResponse.data[0].details.id;
                            }

                            emailContent.Subject = emailSubject.Replace("<<CompanyName>>", reduceName1, StringComparison.InvariantCultureIgnoreCase);
                            emailContent.Body = emailBody.Replace("<<TitleNumber>>", titleNumber, StringComparison.InvariantCultureIgnoreCase)
                                                 .Replace("<<CompanyName>>", reduceName1, StringComparison.InvariantCultureIgnoreCase)
                                                 .Replace("<<CompanyNumber>>", string.IsNullOrEmpty(companyRegistration1) ? "{EMPTY}" : companyRegistration1,
                                                 StringComparison.InvariantCultureIgnoreCase)
                                                 .Replace("<<AccountId>>", accountId, StringComparison.InvariantCultureIgnoreCase);
                            await EmailHelpers.SendEmail(emailContent);
                        }
                        #endregion
                    }

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = TrenchesReportingConstants.MSG_OR_OWNERSHIP_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            finally
            {
                emailContent.Body = emailBody;
            }
        }

        public async Task<ApiResultDto<string>> HandleOROwnershipLinking(string apiKey)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = TrenchesReportingConstants.MSG_OR_OWNERLINKING_400
            };

            try
            {
                var ownershipLinkings = await _trenchesRepository.GetUnhandledOROwnerLinkings();
                foreach (var ownerLinking in ownershipLinkings)
                {
                    string ownerName = ownerLinking.OwnerName;
                    string ownerType = ownerLinking.OwnerType;
                    string titleNumber = ownerLinking.TitleNumber;
                    string openreachNumber = ownerLinking.OpenreachNumber;
                    string propertyAddress = ownerLinking.PropertyAddress;
                    string companyNumber = ownerLinking.CompanyNumber;

                    // STEP 1: Search Primary Data by OpenreachNumber
                    var primaryData = await _trenchesRepository.GetORPrimaryDataByOpenreachNumber(openreachNumber);
                    string enablementPatch = primaryData.Enablement_Patch;

                    // STEP 2: Handle Company
                    if (ownerType == "C")
                    {
                        // Step 1: Search Company by Company Name
                        string reducedName = StringHelpers.ReduceCompanyName(ownerName);
                        var searchCompanyResult = await _zohoAccountService
                            .SearchAccountByAccountName(apiKey, reducedName);

                        if (searchCompanyResult.Code == ResultCode.OK)
                        {
                            var searchCompanyResponse = searchCompanyResult.Data;
                            ownerLinking.OwnerId = searchCompanyResponse.data[0].id;
                            ownerLinking.ReduceCompanyName = reducedName;
                            ownerLinking.CompanyNumber = searchCompanyResponse.data[0].Company_Registration;
                        }

                        if (!string.IsNullOrEmpty(companyNumber))
                        {
                            // Step 2.2: Search Company by Company Number
                            var searchAccountByNumberResult = await _zohoAccountService.SearchAccountByCompanyRegistration(apiKey, companyNumber);
                            if (searchAccountByNumberResult.Code == ResultCode.OK)
                            {
                                var searchAccountResponse = searchAccountByNumberResult.Data;
                                ownerLinking.OwnerId = searchAccountResponse.data[0].id;
                                ownerLinking.ReduceCompanyName = reducedName;
                                ownerLinking.CompanyNumber = searchAccountResponse.data[0].Company_Registration;
                            }
                        }

                    }

                    // Step 3: Search Openreach by Openreach Number
                    var searchOpenreachResult = await _zohoOpenreachService
                        .SearchOpenreachByOpenreachNumber(apiKey, openreachNumber);
                    if (searchOpenreachResult.Code == ResultCode.OK)
                    {
                        var searchOpenreachResponse = searchOpenreachResult.Data;
                        ownerLinking.OpenreachId = searchOpenreachResponse.data[0].id;
                        ownerLinking.PropertyAddress = searchOpenreachResponse.data[0].Address;
                    }

                    // Step 4: Search Title by Title Number
                    var searchTitleResult = await _zohoTitleService
                        .SearchTitleByTitleNumber(apiKey, titleNumber);
                    if (searchTitleResult.Code == ResultCode.OK)
                    {
                        var searchTitleResponse = searchTitleResult.Data;
                        ownerLinking.TitleId = searchTitleResponse.data[0].id;
                    }
                    else
                    {
                        var openreachTitleForCreation = new OpenreachTitleForCreation()
                        {
                            Name = titleNumber,
                            Project_Name = "Openreach - " + enablementPatch,
                            OR_Type = "MDU",
                            Ownership_Type = "Freeholder - Company UK",
                            Tenure = "Freehold",
                            Layout = TrenchesReportingConstants.OpenreachLayoutId,
                            Reference = propertyAddress
                        };
                        /*
                        if (!string.IsNullOrEmpty(ownerLinking.OpenreachId))
                        {
                            openreachTitleForCreation.Related_Openreach_SM = ownerLinking.OpenreachNumber;
                        }
                        */
                        var createTitleResponse = await _zohoTitleService.UpsertTitleForOpenreach(apiKey, openreachTitleForCreation);
                        var titleId = createTitleResponse.Data.data[0].details.id;
                        ownerLinking.TitleId = titleId;
                    }

                    // STEP 5: Update Owner Linking record
                    ownerLinking.Modified = DateTime.UtcNow;
                    await _trenchesRepository.UpdateOROwnerLinking(ownerLinking);
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = TrenchesReportingConstants.MSG_OR_OWNERLINKING_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> CreateAccessAgreement(
            AccessAgreementRequest accessAgreementRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = TrenchesReportingConstants.MSG_OR_CREATEACCESSAGREEMENT_400
            };

            var outputFiles = new List<string>();

            try
            {
                string apiKey = accessAgreementRequest.ApiKey;
                string openreachId = accessAgreementRequest.OpenreachId;
                string openreachNumber = accessAgreementRequest.OpenreachNumber;

                var getOpenreachByIdResult = await _zohoOpenreachService.GetOpenreachById(apiKey, openreachId);
                var openreachDetails = getOpenreachByIdResult.Data;
                string openreachLocation = openreachDetails.Enablement_Patch_v2;
                string contactType = openreachDetails.Contact_Type;

                bool sendEmailToContact = false;
                if (contactType.Contains("Private", StringComparison.InvariantCultureIgnoreCase))
                {
                    sendEmailToContact = true;
                }

                if (!sendEmailToContact)
                {
                    // STEP 1: Get related data from Owner Linkings table
                    var distinctOwnerIds = new List<string>();
                    // Step 1.1: Get Letter Reference by Openreach Number
                    bool sendLetterBefore = false;
                    var letterReference = await _trenchesRepository
                        .GetLetterReferenceByOpenreachNumber(accessAgreementRequest.OpenreachNumber);

                    if (!string.IsNullOrEmpty(letterReference))
                    {
                        sendLetterBefore = true;
                    }

                    // Step 1.2: Get all Linking Record by Letter Reference
                    if (sendLetterBefore)
                    {
                        var ownerLinkings = await _trenchesRepository.GetOROwnerLinkingsByLetterReference(letterReference);
                        distinctOwnerIds = ownerLinkings.Select(l => l.OwnerId).Distinct().ToList();
                    }

                    // Step 1.3: If Letter not send before, get Owner Linkings by Openreach Number
                    if (!sendLetterBefore)
                    {
                        var allOwnerLinkings = new List<OROwnerLinking>();
                        distinctOwnerIds = await _trenchesRepository.GetDistinctOwnerIdsByOpenreachNumber(openreachNumber);
                        foreach (var ownerId in distinctOwnerIds)
                        {
                            var ownerLinkings = await _trenchesRepository.GetOROwnerLinkingsByOwnerId(ownerId);
                            allOwnerLinkings.AddRange(ownerLinkings);
                        }
                        // Step 1.4: Update all the records with the Letter Reference
                        foreach (var ownerLinking in allOwnerLinkings)
                        {
                            ownerLinking.LetterReference = openreachNumber;
                            ownerLinking.Modified = DateTime.UtcNow;
                            await _trenchesRepository.UpdateOROwnerLinking(ownerLinking);
                        }
                    }

                    // Step 2: Start merging documents
                    // Step 2.1: Copy template file to output folder
                    string noteTitle = $"Access Agreement - {openreachNumber}";
                    string noteContent = string.Empty;

                    foreach (string ownerId in distinctOwnerIds)
                    {
                        var ownerLinkings = await _trenchesRepository.GetOROwnerLinkingsByOwnerId(ownerId);
                        var firstLinking = ownerLinkings.FirstOrDefault();
                        string ownerType = firstLinking.OwnerType;

                        // Step 2.2: Handle Owner Address
                        string ownerAddress = string.Empty;
                        if (ownerType == "C")
                        {
                            var getAccountByIdResult = await _zohoAccountService.GetAccountById(apiKey, firstLinking.OwnerId);
                            if (getAccountByIdResult.Code != ResultCode.OK)
                            {
                                // TODO: Send Email to help@roxus.io, cc the requester to check
                            }
                            var getAccountByIdResponse = getAccountByIdResult.Data;
                            string street = getAccountByIdResponse.Billing_Street;
                            string city = getAccountByIdResponse.Billing_City;
                            string postCode = getAccountByIdResponse.Billing_Code;

                            if (!string.IsNullOrEmpty(street))
                            {
                                ownerAddress = street;
                            }
                            if (!string.IsNullOrEmpty(city))
                            {
                                if (!string.IsNullOrEmpty(ownerAddress))
                                {
                                    ownerAddress += ", " + city;
                                }
                                else
                                {
                                    ownerAddress = city;
                                }
                            }
                            if (!string.IsNullOrEmpty(postCode))
                            {
                                if (!string.IsNullOrEmpty(ownerAddress))
                                {
                                    ownerAddress += ", " + postCode;
                                }
                                else
                                {
                                    ownerAddress = postCode;
                                }
                            }
                        }

                        string accessAgreementFolder = @$"{DocumentPath}\Access Agreement";
                        string templateFolder = @$"{accessAgreementFolder}\{TrenchesReportingConstants.TemplateFolder}";
                        string templateFileName = string.Empty;
                        if (ownerType == "C")
                        {
                            templateFileName = TrenchesReportingConstants.AccessAgreement_Company_Template;
                        }
                        else
                        {
                            templateFileName = TrenchesReportingConstants.AccessAgreement_Private_Template;
                        }
                        string templateFilePath = templateFolder + "\\" + templateFileName;

                        string ownerName = firstLinking.OwnerName;
                        ownerName = ownerName.Replace("-", " ").Replace("/", " ").Replace("\\", " ");

                        string outputFolder = @$"{accessAgreementFolder}\{TrenchesReportingConstants.OutputFolder}";
                        string outputFileName = $"{openreachNumber}_AA_{ownerName}_{DateTime.UtcNow.ToString("yyyyMMddhhmmss")}.docx";
                        string outputFilePath = outputFolder + "\\" + outputFileName;

                        outputFiles.Add(outputFilePath);

                        File.Copy(templateFilePath, outputFilePath);

                        int linkingLength = ownerLinkings.Count();

                        using (var stream = new FileStream(outputFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            //Loads the template document
                            var document = new WordDocument(stream, FormatType.Docx);

                            var textSelections = document.FindAll("<<custom1>>", false, true);
                            if (textSelections != null)
                            {
                                for (int i = 0; i < textSelections.Length; i++)
                                {
                                    // Sets the highlight color for the searched text as Yellow
                                    var textRange = textSelections[i].GetAsOneRange();
                                    textRange.Text = openreachNumber;
                                }
                            }

                            //Handle Custom9
                            var textSelection = document.Find("<<custom9>>", false, true);
                            if (textSelection != null)
                            {
                                var textRange = textSelection.GetAsOneRange();
                                textRange.Text = firstLinking.OwnerName;
                            }

                            //Handle Custom10
                            textSelection = document.Find("<<custom10>>", false, true);
                            if (textSelection != null)
                            {
                                var textRange = textSelection.GetAsOneRange();
                                textRange.Text = firstLinking.CompanyNumber ?? "";
                            }

                            //Handle Custom7
                            textSelection = document.Find("<<custom7>>", false, true);
                            if (textSelection != null)
                            {
                                var textRange = textSelection.GetAsOneRange();
                                textRange.Text = ownerAddress;
                            }

                            //Handle Full Name
                            textSelection = document.Find("<<fullname>>", false, true);
                            if (textSelection != null)
                            {
                                var textRange = textSelection.GetAsOneRange();
                                textRange.Text = firstLinking.OwnerName;
                            }

                            for (int i = 1; i <= 35; i++)
                            {
                                if (i <= ownerLinkings.Count())
                                {
                                    textSelection = document.Find($"<<SMR{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = ownerLinkings[i - 1].OpenreachNumber;
                                    }

                                    textSelection = document.Find($"<<SMAD{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = ownerLinkings[i - 1].PropertyAddress;
                                    }

                                    textSelection = document.Find($"<<TN{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = ownerLinkings[i - 1].TitleNumber;
                                    }

                                }
                                else
                                {
                                    textSelection = document.Find($"<<SMR{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = string.Empty;
                                    }


                                    textSelection = document.Find($"<<SMAD{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = string.Empty;
                                    }

                                    textSelection = document.Find($"<<TN{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = string.Empty;
                                    }
                                }
                            }

                            //Saves and closes the document
                            document.Save(stream, FormatType.Docx);
                            document.Close();
                        }

                        byte[] bytes = File.ReadAllBytes(outputFilePath);
                        string fileContent = Convert.ToBase64String(bytes);

                        string pdfFileName = $"{openreachNumber}_AA_{ownerName}_{DateTime.UtcNow.ToString("yyyyMMdd")}.pdf";
                        string pdfFilePath = outputFolder + "\\" + pdfFileName;

                        //Open the file as Stream
                        FileStream docStream = new FileStream(outputFilePath, FileMode.Open, FileAccess.Read);
                        //Loads file stream into Word document
                        WordDocument wordDocument = new WordDocument(docStream, Syncfusion.DocIO.FormatType.Automatic);
                        //Instantiation of DocIORenderer for Word to PDF conversion
                        DocIORenderer render = new DocIORenderer();
                        //Sets Chart rendering Options.
                        render.Settings.ChartRenderingOptions.ImageFormat = (Syncfusion.OfficeChart.ExportImageFormat)ExportImageFormat.Jpeg;
                        //Converts Word document into PDF document
                        PdfDocument pdfDocument = render.ConvertToPDF(wordDocument);
                        //Releases all resources used by the Word document and DocIO Renderer objects
                        render.Dispose();
                        wordDocument.Dispose();
                        using (var fs = new FileStream(pdfFilePath, FileMode.Create, FileAccess.Write))
                        {
                            pdfDocument.Save(fs);
                        }
                        //Closes the instance of PDF document object
                        pdfDocument.Close();
                        docStream.Close();

                        outputFiles.Add(pdfFilePath);

                        byte[] pdfBytes = File.ReadAllBytes(pdfFilePath);
                        string pdfFileContent = Convert.ToBase64String(pdfBytes);

                        // Step 3: Upload file to Account/Contact
                        if (firstLinking.OwnerType == "C")
                        {
                            // STEP 3.1: Upload Word file
                            var uploadResult = await _zohoAccountService
                                .Account_UploadAttachments(apiKey, ownerId, outputFileName, fileContent);

                            // STEP 3.2: Upload PDF file
                            var uploadPDFResult = await _zohoAccountService
                                .Account_UploadAttachments(apiKey, ownerId, pdfFileName, pdfFileContent);

                            if (string.IsNullOrEmpty(noteContent))
                            {
                                noteContent += "\n";
                            }
                            noteContent += $"- {firstLinking.OwnerName}: https://crm.zoho.eu/crm/org20069538520/tab/Accounts/{firstLinking.OwnerId}";

                            // STEP 3.3: Get Related Contacts from Account
                            var getContactsResponse = await _zohoAccountService.Account_GetRelatedContacts(apiKey, ownerId);
                            if (getContactsResponse.Code == ResultCode.OK)
                            {
                                var relatedContacts = getContactsResponse.Data.data;
                                foreach (var contact in relatedContacts)
                                {

                                    string contactId = contact.id;

                                    // STEP 3.3: Upload Word file
                                    var uploadForContactResult = await _zohoAccountService
                                        .Account_UploadAttachments(apiKey, contactId, outputFileName, fileContent);

                                    // STEP 3.5: Upload PDF file
                                    var uploadPDFForContactResult = await _zohoAccountService
                                        .Account_UploadAttachments(apiKey, contactId, pdfFileName, pdfFileContent);
                                }

                            }

                        }
                        else if (firstLinking.OwnerType == "P")
                        {
                            // STEP 3.3: Upload Word file
                            var uploadResult = await _zohoContactService
                                .Contact_UploadAttachments(apiKey, ownerId, outputFileName, fileContent);

                            // STEP 3.4: Upload PDF file
                            var uploadPDFResult = await _zohoContactService
                                .Contact_UploadAttachments(apiKey, ownerId, pdfFileName, pdfFileContent);

                            if (string.IsNullOrEmpty(noteContent))
                            {
                                noteContent += "\n";
                            }
                            noteContent += $"- {firstLinking.OwnerName}: https://crm.zoho.eu/crm/org20069538520/tab/Contacts/{firstLinking.OwnerId}";
                        }
                    }

                    // Step 4: Add Note to Openreach
                    if (string.IsNullOrEmpty(noteContent))
                    {
                        apiResult.Message = "There is no Owner related to this SM, please contact Roxus team!";
                        return apiResult;
                    }
                    var noteForCreation = new NoteForCreation()
                    {
                        Note_Title = noteTitle,
                        Note_Content = noteContent,
                        Parent_Id = openreachId,
                        se_module = "Openreach"
                    };
                    var upsertRequest = new UpsertRequest<NoteForCreation>();
                    upsertRequest.data.Add(noteForCreation);
                    var createNoteResult = await _zohoNoteService.CreateNote(apiKey, upsertRequest);
                }
                else
                {
                    var accountLetters = new List<OR_AccountLetter>();
                    var contactLetters = new List<OR_ContactLetter>();
                    var allContacts = new List<string>();
                    var allAccounts = new List<string>();

                    // STEP 3: Get all Related Titles of SM
                    var getRelatedTitlesResult = await _zohoOpenreachService
                        .GetOpenreachRelatedTitles(apiKey, openreachId);

                    if (getRelatedTitlesResult.Code != ResultCode.OK)
                    {
                        return apiResult;
                    }

                    var titlesList = getRelatedTitlesResult.Data.data;
                    var contactLinkings = new List<OwnerLinking>();

                    foreach (var relatedTitle in titlesList)
                    {
                        var titleDetails = relatedTitle.Related_Freehold_Titles;

                        if (titleDetails == null)
                        {
                            continue;
                        }

                        string titleId = titleDetails.id;
                        var getTitleByIdResult = await _zohoTitleService
                            .GetTitleById(apiKey, titleId);

                        if (getTitleByIdResult.Code != ResultCode.OK)
                        {
                            continue;
                        }

                        var getTitleDetails = getTitleByIdResult.Data;

                        if (getTitleDetails.Related_Contacts != null)
                        {
                            foreach (var relatedContact in getTitleDetails.Related_Contacts)
                            {
                                if (relatedContact.Contact_Name != null)
                                {
                                    string contactId = relatedContact.Contact_Name.id;
                                    if (!string.IsNullOrEmpty(contactId))
                                    {
                                        allContacts.Add(contactId);
                                    }
                                }
                            }
                        }

                        var linking = new OwnerLinking
                        {
                            OpenreachReference = openreachDetails.Openreach_SM,
                            PropertyAddress = openreachDetails.Address,
                            TitleNumber = getTitleDetails.Name
                        };
                        contactLinkings.Add(linking);
                    }

                    var allContactAddresses = new List<string>();
                    var allContactFullNames = new List<string>();
                    var allContactCustom1 = new List<string>();
                    var allContactIds = new List<string>();

                    foreach (var contactId in allContacts)
                    {
                        string contactUrl = $"{ZohoConstants.TRENCHES_ZCRM_CONTACT_PREFIX_URL}/{contactId}";
                        string contactCustom1 = openreachNumber;
                        var getContactByIdResult = await _zohoContactService.GetContactById(apiKey, contactId);
                        var contactData = getContactByIdResult.Data;
                        // Step 3.2.1: Handle Contact Name
                        string conFirstName = contactData.First_Name;
                        string conLastName = contactData.Last_Name;
                        string conFullName = contactData.Full_Name;
                        string conTitleFullName = contactData.Title_Full_Name;
                        if (!string.IsNullOrEmpty(conTitleFullName))
                        {
                            conFullName = conTitleFullName;
                        }
                        if (conFirstName == "The" &&
                            (conLastName.StartsWith("1") || conLastName.StartsWith("2") || conLastName.StartsWith("6")))
                        {
                            conLastName = "Occupier";
                            conFullName = "The Occupier";
                        }
                        string contactAllNames = conFirstName + "|||" + conLastName + "|||" + conFullName;
                        // Step 3.2.2: Handle Contact Address
                        string conMailingStreet = contactData.Mailing_Street;
                        string conMailingCity = contactData.Mailing_City;
                        string conMailingState = contactData.Mailing_State;
                        string conMailingCode = contactData.Mailing_Zip;
                        string conMailingCountry = contactData.Mailing_Country;
                        int conStreetLength = conMailingStreet.Length;
                        int conCityLength = conMailingCity.Length;
                        int conCodeLength = conMailingCode.Length;
                        string contactAddress = conMailingStreet + "|||" + conMailingCity + "|||" + conMailingState
                            + "|||" + conMailingCode + "|||" + conMailingCountry;
                        if (!allContactAddresses.Contains(contactAddress))
                        {
                            allContactAddresses.Add(contactAddress);
                            allContactFullNames.Add(contactAllNames);
                            allContactCustom1.Add(contactCustom1);
                            allContactIds.Add(contactId);
                        }
                        else
                        {
                            int addressIndex = allContactAddresses.IndexOf(contactAddress);
                            string tempAllName = allContactFullNames[addressIndex];
                            string tempFullName = tempAllName.Split("|||")[2];
                            string newFullName = $"{tempFullName};{conFullName}";
                            allContactFullNames[addressIndex] = allContactFullNames[addressIndex].Replace(tempFullName, newFullName);
                        }
                    }

                    for (int i = 0; i < allContactAddresses.Count; i++)
                    {
                        string contactId = allContactIds[i];

                        var contactLetter = new OR_ContactLetter();
                        contactLetter.OwnerLinkings = contactLinkings;
                        contactLetter.ContactId = allContactIds[i];
                        // Step 4.3.1: Handle Contact Name
                        string allContactNames = allContactFullNames[i];
                        var contactNameSplits = allContactNames.Split("|||");
                        var contactFirstName = contactNameSplits[0];
                        var contactLastName = contactNameSplits[1];
                        var contactAllNames = contactNameSplits[2];
                        var contactSplits = contactAllNames.Split(";");
                        int numberOfPeople = contactSplits.Length;
                        string updatedContactAllNames = contactAllNames.Replace(";", " and ");
                        contactLetter.FirstName = contactFirstName;
                        contactLetter.SurName = contactLastName;
                        contactLetter.FullName = contactSplits[0];
                        contactLetter.Custom9 = updatedContactAllNames;
                        contactLetter.MailingName = $"OR - {openreachLocation} - {openreachNumber}";
                        contactLetter.MailingDescription = $"Mail triggered by Roxus Robot for Openreach Number: {openreachNumber}, send to Contact: {contactLetter.FullName}";

                        // Step 4.3.3: Handle Contact Address
                        string mailingAddress = allContactAddresses[i];
                        var mailingSplits = mailingAddress.Split("|||");
                        string mailingStreet = mailingSplits[0];
                        string mailingCity = mailingSplits[1];
                        string mailingState = mailingSplits[2];
                        string mailingZip = mailingSplits[3];
                        string mailingCountry = mailingSplits[4];

                        contactLetter.Address1 = mailingStreet;
                        contactLetter.Address2 = mailingCity;
                        contactLetter.Address3 = mailingState;
                        contactLetter.Address4 = mailingZip;

                        if (string.IsNullOrEmpty(mailingState))
                        {
                            contactLetter.Custom7 = $"{mailingStreet}, {mailingCity}, {mailingZip}";
                        }
                        else
                        {
                            contactLetter.Custom7 = $"{mailingStreet}, {mailingCity}, {mailingState}, {mailingZip}";
                        }
                        if (!string.IsNullOrEmpty(mailingCountry))
                        {
                            contactLetter.Address5 = mailingCountry;
                            contactLetter.Custom7 += $", {mailingCountry}";
                        }
                        contactLetter.Custom10 = string.Empty;
                        contactLetters.Add(contactLetter);
                    }

                    // Step 2: Start merging documents
                    // Step 2.1: Copy template file to output folder
                    string noteTitle = $"Access Agreement - {openreachNumber}";
                    string noteContent = string.Empty;

                    foreach (var contactLetter in contactLetters)
                    {
                        var ownerLinkings = contactLetter.OwnerLinkings;
                        // Step 2.2: Handle Owner Address
                        string ownerAddress = contactLetter.Custom7;

                        string accessAgreementFolder = @$"{DocumentPath}\Access Agreement";
                        string templateFolder = @$"{accessAgreementFolder}\{TrenchesReportingConstants.TemplateFolder}";

                        string templateFileName = TrenchesReportingConstants.AccessAgreement_Private_Template;
                        string templateFilePath = templateFolder + "\\" + templateFileName;

                        string ownerName = contactLetter.FullName;
                        ownerName = ownerName.Replace("-", " ").Replace("/", " ").Replace("\\", " ");

                        string outputFolder = @$"{accessAgreementFolder}\{TrenchesReportingConstants.OutputFolder}";
                        string outputFileName = $"{openreachNumber}_AA_{ownerName}_{DateTime.UtcNow.ToString("yyyyMMddhhmmss")}.docx";
                        string outputFilePath = outputFolder + "\\" + outputFileName;

                        outputFiles.Add(outputFilePath);

                        File.Copy(templateFilePath, outputFilePath);

                        int linkingLength = ownerLinkings.Count();

                        using (var stream = new FileStream(outputFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            //Loads the template document
                            var document = new WordDocument(stream, FormatType.Docx);

                            var textSelections = document.FindAll("<<custom1>>", false, true);
                            if (textSelections != null)
                            {
                                for (int i = 0; i < textSelections.Length; i++)
                                {
                                    // Sets the highlight color for the searched text as Yellow
                                    var textRange = textSelections[i].GetAsOneRange();
                                    textRange.Text = openreachNumber;
                                }
                            }

                            //Handle Custom9
                            var textSelection = document.Find("<<custom9>>", false, true);
                            if (textSelection != null)
                            {
                                var textRange = textSelection.GetAsOneRange();
                                textRange.Text = contactLetter.FullName;
                            }

                            //Handle Custom7
                            textSelection = document.Find("<<custom7>>", false, true);
                            if (textSelection != null)
                            {
                                var textRange = textSelection.GetAsOneRange();
                                textRange.Text = contactLetter.Custom7;
                            }

                            //Handle Full Name
                            textSelection = document.Find("<<fullname>>", false, true);
                            if (textSelection != null)
                            {
                                var textRange = textSelection.GetAsOneRange();
                                textRange.Text = contactLetter.FullName;
                            }

                            for (int i = 1; i <= 35; i++)
                            {
                                if (i <= ownerLinkings.Count())
                                {
                                    textSelection = document.Find($"<<SMR{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = ownerLinkings[i - 1].OpenreachReference;
                                    }

                                    textSelection = document.Find($"<<SMAD{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = ownerLinkings[i - 1].PropertyAddress;
                                    }

                                    textSelection = document.Find($"<<TN{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = ownerLinkings[i - 1].TitleNumber;
                                    }

                                }
                                else
                                {
                                    textSelection = document.Find($"<<SMR{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = string.Empty;
                                    }


                                    textSelection = document.Find($"<<SMAD{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = string.Empty;
                                    }

                                    textSelection = document.Find($"<<TN{i}>>", false, true);
                                    if (textSelection != null)
                                    {
                                        var textRange = textSelection.GetAsOneRange();
                                        textRange.Text = string.Empty;
                                    }
                                }
                            }

                            //Saves and closes the document
                            document.Save(stream, FormatType.Docx);
                            document.Close();
                        }

                        byte[] bytes = File.ReadAllBytes(outputFilePath);
                        string fileContent = Convert.ToBase64String(bytes);

                        string pdfFileName = $"{openreachNumber}_AA_{ownerName}_{DateTime.UtcNow.ToString("yyyyMMdd")}.pdf";
                        string pdfFilePath = outputFolder + "\\" + pdfFileName;

                        //Open the file as Stream
                        FileStream docStream = new FileStream(outputFilePath, FileMode.Open, FileAccess.Read);
                        //Loads file stream into Word document
                        WordDocument wordDocument = new WordDocument(docStream, Syncfusion.DocIO.FormatType.Automatic);
                        //Instantiation of DocIORenderer for Word to PDF conversion
                        DocIORenderer render = new DocIORenderer();
                        //Sets Chart rendering Options.
                        render.Settings.ChartRenderingOptions.ImageFormat = (Syncfusion.OfficeChart.ExportImageFormat)ExportImageFormat.Jpeg;
                        //Converts Word document into PDF document
                        PdfDocument pdfDocument = render.ConvertToPDF(wordDocument);
                        //Releases all resources used by the Word document and DocIO Renderer objects
                        render.Dispose();
                        wordDocument.Dispose();
                        using (var fs = new FileStream(pdfFilePath, FileMode.Create, FileAccess.Write))
                        {
                            pdfDocument.Save(fs);
                        }
                        //Closes the instance of PDF document object
                        pdfDocument.Close();
                        docStream.Close();

                        outputFiles.Add(pdfFilePath);

                        byte[] pdfBytes = File.ReadAllBytes(pdfFilePath);
                        string pdfFileContent = Convert.ToBase64String(pdfBytes);

                        // Step 3: Upload file to Account/Contact
                        // STEP 3.3: Upload Word file
                        var uploadResult = await _zohoContactService
                            .Contact_UploadAttachments(apiKey, contactLetter.ContactId, outputFileName, fileContent);

                        // STEP 3.4: Upload PDF file
                        var uploadPDFResult = await _zohoContactService
                            .Contact_UploadAttachments(apiKey, contactLetter.ContactId, pdfFileName, pdfFileContent);

                        if (string.IsNullOrEmpty(noteContent))
                        {
                            noteContent += "\n";
                        }
                        noteContent += $"- {contactLetter.FullName}: https://crm.zoho.eu/crm/org20069538520/tab/Contacts/{contactLetter.ContactId}";
                    }

                    // Step 4: Add Note to Openreach
                    if (string.IsNullOrEmpty(noteContent))
                    {
                        apiResult.Message = "There is no Owner related to this SM, please contact Roxus team!";
                        return apiResult;
                    }
                    var noteForCreation = new NoteForCreation()
                    {
                        Note_Title = noteTitle,
                        Note_Content = noteContent,
                        Parent_Id = openreachId,
                        se_module = "Openreach"
                    };
                    var upsertRequest = new UpsertRequest<NoteForCreation>();
                    upsertRequest.data.Add(noteForCreation);
                    var createNoteResult = await _zohoNoteService.CreateNote(apiKey, upsertRequest);
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = TrenchesReportingConstants.MSG_OR_CREATEACCESSAGREEMENT_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            finally
            {

                foreach (var outputFile in outputFiles)
                {
                    if (File.Exists(outputFile))
                    {
                        File.Delete(outputFile);
                    }
                }
            }
        }

        public async Task<ApiResultDto<string>> HandleLetterReference(string apiKey, string openreachId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = TrenchesReportingConstants.MSG_OR_HANDLELETTERREFERENCE_400
            };

            try
            {
                // STEP 1: Get Openreach Details by Id
                var getOpenreachByIdResult = await _zohoOpenreachService.GetOpenreachById(apiKey, openreachId);
                var openreachDetails = getOpenreachByIdResult.Data;
                string openreachNumber = openreachDetails.Openreach_SM;

                // STEP 2: Get related data from Owner Linkings table
                var distinctOwnerIds = new List<string>();
                // Step 2.1: Get Letter Reference by Openreach Number
                bool sendLetterBefore = false;
                var letterReference = await _trenchesRepository
                    .GetLetterReferenceByOpenreachNumber(openreachDetails.Openreach_SM);

                if (!string.IsNullOrEmpty(letterReference))
                {
                    sendLetterBefore = true;
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = TrenchesReportingConstants.MSG_OR_HANDLELETTERREFERENCE_200;
                    apiResult.Data = letterReference;
                }

                // Step 2.2: Get all Linking Record by Letter Reference
                if (sendLetterBefore)
                {
                    var ownerLinkings = await _trenchesRepository.GetOROwnerLinkingsByLetterReference(letterReference);
                    distinctOwnerIds = ownerLinkings.Select(l => l.OwnerId).Distinct().ToList();
                }

                // Step 2.3: If Letter not send before, get Owner Linkings by Openreach Number
                if (!sendLetterBefore)
                {
                    var allOwnerLinkings = new List<OROwnerLinking>();
                    distinctOwnerIds = await _trenchesRepository.GetDistinctOwnerIdsByOpenreachNumber(openreachNumber);
                    foreach (var ownerId in distinctOwnerIds)
                    {
                        var ownerLinkings = await _trenchesRepository.GetOROwnerLinkingsByOwnerId(ownerId);
                        allOwnerLinkings.AddRange(ownerLinkings);
                    }
                    // Step 2.4: Update all the records with the Letter Reference
                    foreach (var ownerLinking in allOwnerLinkings)
                    {
                        ownerLinking.LetterReference = openreachNumber;
                        ownerLinking.Modified = DateTime.UtcNow;
                        await _trenchesRepository.UpdateOROwnerLinking(ownerLinking);
                    }
                }

                // STEP 3: Return Letter Reference
                apiResult.Code = ResultCode.OK;
                apiResult.Message = TrenchesReportingConstants.MSG_OR_HANDLELETTERREFERENCE_200;
                apiResult.Data = openreachNumber;

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<bool>> HandleRegisterPurchase(RegisterPurchaseRequest purchaseRequest)
        {
            var apiResult = new ApiResultDto<bool>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_HRP_400,
                Data = false
            };

            string emailSubject = "[Register Purchase] User {UserEmail} triggerred Register Purchase for Title {TitleNumber}";

            string emailBody = @"Dear Roxus Support,<br><br>User {UserEmail} triggerred <b>Register Purchase</b> for title with details below:<br/>
                                <ul>
                                    <li>Title Number: {TitleNumber}</li>
                                    <li>Title URL: <a href='https://crm.zoho.eu/crm/org20069538520/tab/CustomModule3/{TitleId}'></a></li>
                                </ul>
                                We have found that this Title belongs to company called: <br/>
                                {TitleInformation}
                                <br/>
                                Best Regards,<br> Roxus Automation";

            var emailContent = new EmailContent()
            {
                Email = EmailConstants.TrenchesEmail,
                Body = string.Empty,
                Clients = "help@roxus.io",
                Subject = string.Empty,
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Outlook_Email_SmtpServer
            };

            var appConfiguration = await _roxusLoggingRepository.GetAppConfigurationById(EmailConstants.TrenchesId);
            emailContent.Password = appConfiguration.Password;

            string apiKey = "dHJlbmNoZXNsYXc6em9ob2NybQ==";

            try
            {
                string titleNumber = purchaseRequest.TitleNumber;
                string titleId = purchaseRequest.TitleId;
                string userEmail = purchaseRequest.UserEmail;

                // STEP 1: Get Company Details from table Corporate Ownership
                var corporateOwnership = await _trenchesRepository.GetOwnershipByTitleNumber(titleNumber);

                // STEP 2: If Corporate Ownership is NULL => The Title belongs to Private Owner
                if (corporateOwnership == null)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.CUSTOM_HRP_200;
                    apiResult.Data = true;
                    return apiResult;
                }

                // STEP 3: Extract data from Corporate Ownership table
                string companyName1 = corporateOwnership.Proprietor_Name_1;
                string reduceName1 = StringHelpers.ReduceCompanyName(companyName1);
                string companyRegistration1 = corporateOwnership.Company_Registration_No_1;
                string companyFullAddress1 = corporateOwnership.Proprietor_1_Address_1;

                string companyName2 = corporateOwnership.Proprietor_Name_2;
                string reduceName2 = StringHelpers.ReduceCompanyName(companyName2);
                string companyRegistration2 = corporateOwnership.Company_Registration_No_2;
                string companyFullAddress2 = corporateOwnership.Proprietor_2_Address_1;

                // STEP 3.1: Check if Company Number matches digit
                var regex = new Regex(@"^\d+$");

                if (regex.IsMatch(companyRegistration1))
                {
                    while (companyRegistration1.Length != 8)
                    {
                        companyRegistration1 = "0" + companyRegistration1;
                    }
                }

                // STEP 4: Search Company in Company House
                var searchCompanyResult = await _companiesHouseService.GetCompanies(companyRegistration1);

                #region Handle Company 1
                // Step 4.1: Search Company by Company Name
                string accountId = string.Empty;

                var searchAccountByFullNameResult = await _zohoAccountService
                    .SearchAccountByAccountName(apiKey, companyName1);
                if (searchAccountByFullNameResult.Code == ResultCode.OK)
                {
                    var searchAccountResponse = searchAccountByFullNameResult.Data;
                    accountId = searchAccountResponse.data[0].id;
                }

                if (string.IsNullOrEmpty(accountId))
                {
                    var searchAccountByReduceNameResult = await _zohoAccountService
                        .SearchAccountByAccountName(apiKey, reduceName1);
                    if (searchAccountByReduceNameResult.Code == ResultCode.OK)
                    {
                        var searchAccountResponse = searchAccountByReduceNameResult.Data;
                        accountId = searchAccountResponse.data[0].id;
                    }
                }
                
                if (string.IsNullOrEmpty(accountId) && !string.IsNullOrEmpty(companyRegistration1))
                {
                    // Step 4.2: Search Company by Company Number
                    var searchAccountByNumberResult = await _zohoAccountService.SearchAccountByCompanyRegistration(apiKey, companyRegistration1);
                    if (searchAccountByNumberResult.Code == ResultCode.OK)
                    {
                        var searchAccountResponse = searchAccountByNumberResult.Data;
                        accountId = searchAccountResponse.data[0].id;
                    }
                }

                if (!string.IsNullOrEmpty(accountId))
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.CUSTOM_HRP_200;
                    apiResult.Data = false;
                    return apiResult;
                }

                // STEP 5: Create Company in Zoho CRM
                bool searchCompaniesSuccess = false;
                if (!string.IsNullOrEmpty(companyRegistration1))
                {
                    // Step 5.1: Call Companies House API to get company data
                    var companiesResult = await _companiesHouseService.GetCompanies(companyRegistration1);

                    var companiesResponse = companiesResult.Data;
                    var totalResults = companiesResponse.total_results;

                    if (totalResults == 1)
                    {
                        searchCompaniesSuccess = true;
                        // Step 3.2: Extract Companies House data
                        var companyDetails = companiesResponse.items[0];
                        string companyName = companyDetails.title;
                        string companyNumber = companyDetails.company_number;
                        var companyAddress = companyDetails.address;
                        string addressLine1 = companyAddress.address_line_1;
                        string premises = companyAddress.premises;

                        string street = string.Empty;
                        if (!string.IsNullOrEmpty(premises))
                        {
                            street = $"{premises} {addressLine1}";
                        }
                        else
                        {
                            street = addressLine1;
                        }

                        string locality = companyAddress.locality;
                        string postalCode = companyAddress.postal_code;

                        var accountForCreation = new AccountForCreation()
                        {
                            Account_Name = reduceName1,
                            Company_Registration = companyNumber,
                            Billing_Street = street,
                            Billing_City = locality,
                            Billing_Code = postalCode
                        };

                        var upsertRequest = new UpsertRequest<AccountForCreation>();
                        upsertRequest.data.Add(accountForCreation);

                        var createAccountResult = await _zohoAccountService.CreateAccount(apiKey, upsertRequest);
                        if (createAccountResult.Code == ResultCode.OK)
                        {
                            var createAccountResponse = createAccountResult.Data;
                            accountId = createAccountResponse.data[0].details.id;
                        }

                    }
                    else
                    {
                        searchCompaniesSuccess = false;
                    }

                }

                if (!searchCompaniesSuccess)
                {
                    // Step 3.3: Create Account based on data in Geo Spatial
                    var accountForCreation = new AccountForCreation()
                    {
                        Account_Name = reduceName1,
                        Company_Registration = companyRegistration1,
                        Billing_Street = companyFullAddress1
                    };

                    var upsertRequest = new UpsertRequest<AccountForCreation>();
                    upsertRequest.data.Add(accountForCreation);

                    var createAccountResult = await _zohoAccountService.CreateAccount(apiKey, upsertRequest);
                    if (createAccountResult.Code == ResultCode.OK)
                    {
                        var createAccountResponse = createAccountResult.Data;
                        accountId = createAccountResponse.data[0].details.id;
                    }

                    emailContent.Subject = emailSubject.Replace("<<CompanyName>>", reduceName1, StringComparison.InvariantCultureIgnoreCase);
                    emailContent.Body = emailBody.Replace("<<TitleNumber>>", titleNumber, StringComparison.InvariantCultureIgnoreCase)
                                            .Replace("<<CompanyName>>", reduceName1, StringComparison.InvariantCultureIgnoreCase)
                                            .Replace("<<CompanyNumber>>", string.IsNullOrEmpty(companyRegistration1) ? "{EMPTY}" : companyRegistration1,
                                            StringComparison.InvariantCultureIgnoreCase)
                                            .Replace("<<AccountId>>", accountId, StringComparison.InvariantCultureIgnoreCase);
                    await EmailHelpers.SendEmail(emailContent);

                }

                #endregion
                if (string.IsNullOrEmpty(companyName2))
                {

                    #region Handle Company 2

                    // Step 2.1: Search Company by Company Name
                    accountId = string.Empty;
                    var searchAccount2ByFullNameResult = await _zohoAccountService.SearchAccountByAccountName(apiKey, companyName2);
                    if (searchAccount2ByFullNameResult.Code == ResultCode.OK)
                    {
                        var searchAccountResponse = searchAccount2ByFullNameResult.Data;
                        accountId = searchAccountResponse.data[0].id;
                    }

                    if (string.IsNullOrEmpty(accountId))
                    {
                        var searchAccountByReduceNameResult = await _zohoAccountService.SearchAccountByAccountName(apiKey, reduceName2);
                        if (searchAccountByReduceNameResult.Code == ResultCode.OK)
                        {
                            var searchAccountResponse = searchAccountByReduceNameResult.Data;
                            accountId = searchAccountResponse.data[0].id;
                        }
                    }
                    
                    if (string.IsNullOrEmpty(accountId) && !string.IsNullOrEmpty(companyRegistration2))
                    {
                        // Step 2.2: Search Company by Company Number
                        var searchAccountByNumberResult = await _zohoAccountService.SearchAccountByCompanyRegistration(apiKey, companyRegistration2);
                        if (searchAccountByNumberResult.Code == ResultCode.OK)
                        {
                            var searchAccountResponse = searchAccountByNumberResult.Data;
                            accountId = searchAccountResponse.data[0].id;
                        }
                    }

                    // STEP 3: Create Company in Zoho CRM
                    searchCompaniesSuccess = false;
                    if (!string.IsNullOrEmpty(companyRegistration2))
                    {

                        // Step 3.1: Call Companies House API to get company data
                        var companiesResult = await _companiesHouseService.GetCompanies(companyRegistration2);

                        var companiesResponse = companiesResult.Data;
                        var totalResults = companiesResponse.total_results;

                        if (totalResults == 1)
                        {
                            searchCompaniesSuccess = true;
                            // Step 3.2: Extract Companies House data
                            var companyDetails = companiesResponse.items[0];
                            string companyName = companyDetails.title;
                            string companyNumber = companyDetails.company_number;
                            var companyAddress = companyDetails.address;
                            string addressLine1 = companyAddress.address_line_1;
                            string premises = companyAddress.premises;

                            string street = string.Empty;
                            if (!string.IsNullOrEmpty(premises))
                            {
                                street = $"{premises} {addressLine1}";
                            }
                            else
                            {
                                street = addressLine1;
                            }

                            string locality = companyAddress.locality;
                            string postalCode = companyAddress.postal_code;

                            var accountForCreation = new AccountForCreation()
                            {
                                Account_Name = reduceName1,
                                Company_Registration = companyNumber,
                                Billing_Street = street,
                                Billing_City = locality,
                                Billing_Code = postalCode
                            };

                            var upsertRequest = new UpsertRequest<AccountForCreation>();
                            upsertRequest.data.Add(accountForCreation);

                            var createAccountResult = await _zohoAccountService.CreateAccount(apiKey, upsertRequest);
                            if (createAccountResult.Code == ResultCode.OK)
                            {
                                var createAccountResponse = createAccountResult.Data;
                                accountId = createAccountResponse.data[0].details.id;
                            }

                        }
                        else
                        {
                            searchCompaniesSuccess = false;
                        }
                    }

                    if (!searchCompaniesSuccess)
                    {
                        // Step 3.3: Create Account based on data in Geo Spatial
                        var accountForCreation = new AccountForCreation()
                        {
                            Account_Name = reduceName1,
                            Company_Registration = companyRegistration1,
                            Billing_Street = companyFullAddress1
                        };

                        var upsertRequest = new UpsertRequest<AccountForCreation>();
                        upsertRequest.data.Add(accountForCreation);

                        var createAccountResult = await _zohoAccountService.CreateAccount(apiKey, upsertRequest);
                        if (createAccountResult.Code == ResultCode.OK)
                        {
                            var createAccountResponse = createAccountResult.Data;
                            accountId = createAccountResponse.data[0].details.id;
                        }

                        emailContent.Subject = emailSubject.Replace("<<CompanyName>>", reduceName2, StringComparison.InvariantCultureIgnoreCase);
                        emailContent.Body = emailBody.Replace("<<TitleNumber>>", titleNumber, StringComparison.InvariantCultureIgnoreCase)
                                             .Replace("<<CompanyName>>", reduceName2, StringComparison.InvariantCultureIgnoreCase)
                                             .Replace("<<CompanyNumber>>", string.IsNullOrEmpty(companyRegistration2) ? "{EMPTY}" : companyRegistration2,
                                             StringComparison.InvariantCultureIgnoreCase)
                                             .Replace("<<AccountId>>", accountId, StringComparison.InvariantCultureIgnoreCase);
                        await EmailHelpers.SendEmail(emailContent);
                    }

                    #endregion
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_HRP_200;
                apiResult.Data = false;

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> CheckPrimarySM(string smNumber)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string apiKey = "dHJlbmNoZXNsYXc6em9ob2NybQ==";

                if (string.IsNullOrEmpty(smNumber))
                {
                    return apiResult;
                }

                // Check if SM is for private owner
                var searchOpenreachResponse = await _zohoOpenreachService.SearchOpenreachByOpenreachNumber(apiKey, smNumber);

                var openReachDetails = searchOpenreachResponse.Data.data.FirstOrDefault();
                string contactType = openReachDetails.Contact_Type;

                if (contactType.Contains("Private", StringComparison.InvariantCultureIgnoreCase))
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    return apiResult;
                }

                string letterReference = await _trenchesRepository.GetLetterReferenceByOpenreachNumber(smNumber);
                if (smNumber != letterReference)
                {
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                apiResult.Data = letterReference;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }
    }
}
