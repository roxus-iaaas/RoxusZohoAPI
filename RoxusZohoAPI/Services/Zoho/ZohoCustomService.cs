using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoxusWebAPI.Services.Zoho.ZohoProjects;
using RoxusZohoAPI.Entities.TrenchesReportDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.Custom;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using RoxusZohoAPI.Repositories;
using RoxusZohoAPI.Services.Zoho.ZohoCRM;
using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;

namespace RoxusZohoAPI.Services.Zoho
{
    public class ZohoCustomService : IZohoCustomService
    {
        private readonly IZohoContactService _zohoContactService;
        private readonly IZohoAccountService _zohoAccountService;
        private readonly IZohoUsrnService _zohoUsrnService;
        private readonly IZohoUprnService _zohoUprnService;
        private readonly IZohoTitleService _zohoTitleService;
        private readonly IZohoProjectService _zohoProjectService;
        private readonly IZohoTasklistService _zohoTasklistService;
        private readonly IZohoTaskService _zohoTaskService;
        private readonly IZohoOpenreachService _zohoOpenreachService;
        private readonly IZohoNoteService _zohoNoteService;
        private readonly IRoxusLoggingRepository _roxusLoggingRepository;
        private readonly ITrenchesReportingRepository _trenchesRepository;

        private readonly HttpClient _httpClient = new HttpClient(
         new HttpClientHandler()
         {
             AutomaticDecompression = DecompressionMethods.GZip
         });

        public ZohoCustomService(IZohoUsrnService zohoUsrnService, IZohoUprnService zohoUprnService, IZohoTitleService zohoTitleService,
            IZohoProjectService zohoProjectService, IZohoTaskService zohoTaskService, IZohoTasklistService zohoTasklistService,
            IZohoAccountService zohoAccountService, IZohoContactService zohoContactService, IRoxusLoggingRepository roxusLoggingRepository,
            ITrenchesReportingRepository trenchesReportingRepository, IZohoOpenreachService openreachService, IZohoNoteService zohoNoteService)
        {
            _zohoUsrnService = zohoUsrnService;
            _zohoUprnService = zohoUprnService;
            _zohoTitleService = zohoTitleService;
            _zohoProjectService = zohoProjectService;
            _zohoTasklistService = zohoTasklistService;
            _zohoTaskService = zohoTaskService;
            _httpClient.Timeout = new TimeSpan(0, 0, 1800);
            _httpClient.DefaultRequestHeaders.Clear();
            _zohoAccountService = zohoAccountService;
            _zohoContactService = zohoContactService;
            _roxusLoggingRepository = roxusLoggingRepository;
            _trenchesRepository = trenchesReportingRepository;
            _zohoOpenreachService = openreachService;
            _zohoNoteService = zohoNoteService;
        }

        public async Task<ApiResultDto<string>> RemoveRedundantOccupier(string apiKey, string titleId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                // STEP 1: Get Title By Id
                var getTitleByIdResult = await _zohoTitleService.GetTitleById(apiKey, titleId);

                if (getTitleByIdResult.Code != 0)
                {
                    return apiResult;
                }

                // STEP 2: Extract Title Data
                var titleData = getTitleByIdResult.Data;
                string titleName = titleData.Name;
                var relatedContacts = titleData.Related_Contacts;

                var contactIds = new List<string>();
                var accountIds = new List<string>();
                var occupierIds = new List<string>();
                bool removeOccupier = false;
                foreach (var relatedContact in relatedContacts)
                {
                    if (relatedContact.Contact_Name != null)
                    {
                        string contactId = relatedContact.Contact_Name.id;
                        if (!string.IsNullOrEmpty(contactId))
                        {
                            contactIds.Add(contactId);
                        }
                    }
                    if (relatedContact.Account_Name != null)
                    {
                        string accountId = relatedContact.Account_Name.id;
                        if (!string.IsNullOrEmpty(accountId))
                        {
                            accountIds.Add(accountId);
                        }
                    }
                }
                // Loop through all Associate Contacts to check if there are any The Occupiers
                foreach (var contactId in contactIds)
                {
                    // Get Contact By Id
                    var getContactByIdResult = await _zohoContactService.GetContactById(apiKey, contactId);
                    if (getContactByIdResult.Code == ResultCode.OK)
                    {
                        var contactData = getContactByIdResult.Data;
                        string titleFullName = contactData.Title_Full_Name;
                        string firstName = contactData.First_Name;
                        string lastName = contactData.Last_Name;
                        if (titleFullName == "The Occupier" || (firstName == "The" && lastName == "Occupier"))
                        {
                            occupierIds.Add(contactId);
                        }
                        else
                        {
                            removeOccupier = true;
                        }
                    }
                }
                if (occupierIds.Count == 0)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZohoConstants.CUSTOM_RRO_O1;
                    return apiResult;
                }

                if (removeOccupier)
                {
                    foreach (var occupierId in occupierIds)
                    {
                        contactIds.Remove(occupierId);
                    }
                    string updatedContactIds = string.Empty;
                    foreach (var contactId in contactIds)
                    {
                        if (string.IsNullOrWhiteSpace(updatedContactIds))
                        {
                            updatedContactIds = contactId;
                        }
                        else
                        {
                            updatedContactIds += "," + contactId;
                        }
                    }
                    string updatedAccountIds = string.Empty;
                    foreach (var accountId in accountIds)
                    {
                        if (string.IsNullOrWhiteSpace(updatedAccountIds))
                        {
                            updatedAccountIds = accountId;
                        }
                        else
                        {
                            updatedAccountIds += "," + accountId;
                        }
                    }

                    var linkingOwnerResult = await _zohoTitleService.Title_LinkingWithOwners(apiKey, titleId,
                        updatedContactIds, updatedAccountIds);
                    if (linkingOwnerResult.Code != ResultCode.OK)
                    {
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.CUSTOM_RRO_O3;
                        return apiResult;
                    }
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_RRO_O2;
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> CreateUnregisteredTitle(string apiKey, string usrnId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            try
            {
                // Step 1: Get USRN By Id
                var usrnApiResult = await _zohoUsrnService.GetUsrnById(apiKey, usrnId);

                if (usrnApiResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                var usrn = usrnApiResult.Data;

                // Step 2: Prepare Data for Title
                // Title Name
                string titleName = $"USRN_{usrn.Name}";
                // Type
                string type = "Unregistered Road";
                // Tenure
                string tenure = "Unknown";
                // Reference
                string reference = "PRIVATE ROAD: ";
                int refCount = 0;

                if (!string.IsNullOrEmpty(usrn.Street_Name))
                {
                    if (refCount > 0)
                    {
                        reference += $", {usrn.Street_Name}";
                    }
                    else
                    {
                        reference += usrn.Street_Name;
                    }
                    refCount++;
                }
                if (!string.IsNullOrEmpty(usrn.Locality))
                {
                    if (refCount > 0)
                    {
                        reference += $", {usrn.Locality}";
                    }
                    else
                    {
                        reference += usrn.Locality;
                    }
                    refCount++;
                }
                if (!string.IsNullOrEmpty(usrn.Address_Line_3))
                {
                    if (refCount > 0)
                    {
                        reference += $", {usrn.Address_Line_3}";
                    }
                    else
                    {
                        reference += usrn.Address_Line_3;
                    }
                    refCount++;
                }

                // Project Name
                string projectName = string.Empty;
                // Associate Uprn
                string associateUprn = string.Empty;

                foreach (var uprnAssociated in usrn.UPRN_Associated)
                {
                    if (uprnAssociated.UPRN != null)
                    {
                        string uprnId = uprnAssociated.UPRN.id;
                        var uprnApiResult = await _zohoUprnService.GetUprnById(apiKey, uprnId);
                        if (uprnApiResult.Code != ResultCode.OK)
                        {
                            return apiResult;
                        }

                        if (string.IsNullOrWhiteSpace(associateUprn))
                        {
                            associateUprn = uprnId;
                        }
                        else
                        {
                            associateUprn += $",{uprnId}";
                        }

                        var uprn = uprnApiResult.Data;

                        if (!string.IsNullOrWhiteSpace(uprn.Project_ID) && string.IsNullOrWhiteSpace(projectName))
                        {
                            projectName = uprn.Project_ID;
                        }
                    }
                }

                // Step 3: Upsert Title
                var titleForCreation = new TitleForCreation()
                {
                    Project_Name = projectName,
                    Name = titleName,
                    Tenure = tenure,
                    Reference = reference,
                    Type = type,
                    Title_Task = "TRUE"
                };

                var titleUpsertResult = await _zohoTitleService.UpsertTitle(apiKey, titleForCreation);
                if (titleUpsertResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                string titleId = titleUpsertResult.Data.data.First().details.id;

                // Step 4: Linking between Title, USRN, UPRN
                // Title - Linking with USRN
                var title_linkingUsrnResult = await _zohoTitleService.Title_LinkingWithUSRN(apiKey, titleId, usrn.id);
                if (title_linkingUsrnResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                // Title - Linking with UPRNs
                if (!string.IsNullOrWhiteSpace(associateUprn))
                {
                    var title_linkingUprnResult = await _zohoTitleService.Title_LinkingWithUPRNs(apiKey, titleId, associateUprn);
                    if (title_linkingUprnResult.Code != ResultCode.OK)
                    {
                        return apiResult;
                    }
                }

                // USRN - Linking with Title
                var usrn_linkingTitleResult = await _zohoUsrnService.USRN_LinkingWithTitle(apiKey, usrn.id, titleId);
                if (usrn_linkingTitleResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                // UPRN - Linking with Title
                foreach (var uprnAssociated in usrn.UPRN_Associated)
                {
                    string uprnId = uprnAssociated.UPRN.id;
                    string associateTitleIds = string.Empty;
                    var getUprnResult = await _zohoUprnService.GetUprnById(apiKey, uprnId);

                    var uprnData = getUprnResult.Data;
                    foreach (var uprnTitle in uprnData.UPRN_Titles)
                    {
                        if (uprnTitle.Title != null)
                        {
                            if (string.IsNullOrWhiteSpace(associateTitleIds))
                            {
                                associateTitleIds = uprnTitle.Title.id;
                            }
                            else
                            {
                                associateTitleIds += $",{uprnTitle.Title.id}";
                            }
                        }

                    }
                    if (!associateTitleIds.Contains(titleId))
                    {
                        if (string.IsNullOrWhiteSpace(associateTitleIds))
                        {
                            associateTitleIds = titleId;
                        }
                        else
                        {
                            associateTitleIds += $",{titleId}";
                        }
                    }
                    var uprnLinkingTitlesResult = await _zohoUprnService.UPRN_LinkingWithTitles(apiKey, uprnId, associateTitleIds);
                    if (uprnLinkingTitlesResult.Code != ResultCode.OK)
                    {
                        return apiResult;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = "Create Unregistered Title from USRN successfully.";

                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
            }
        }

        public async Task<ApiResultDto<string>> CreateUnregisteredTask(string crmKey, string projectKey, string titleId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            try
            {
                // STEP 1: Get Title Information
                var getTitleResult = await _zohoTitleService.GetTitleById(crmKey, titleId);
                if (getTitleResult.Code != ResultCode.OK)
                {
                    apiResult.Data = ZohoConstants.CUSTOM_CUT_ERROR_TT01;
                    return apiResult;
                }

                var title = getTitleResult.Data;

                // STEP 2: Prepare Variable to create Project, Tasklist and Task
                // Project Name
                string projectName = title.Project_Name;
                // Wayleave Template
                string titleWayleave = title.Wayleave_Template;
                // Title Type
                string titleType = title.Type;
                if (titleType != "Unregistered Road")
                {
                    apiResult.Data = ZohoConstants.CUSTOM_CUT_ERROR_TT02;
                    return apiResult;
                }
                // Title Task
                string titleTask = title.Title_Task;
                switch (titleTask)
                {
                    case "TRUE":
                        titleTask = "True";
                        break;
                    case "FALSE":
                        titleTask = "False";
                        break;
                    case "TRUE_Company":
                        titleTask = "True_Company";
                        break;
                    case "TRUE_Private":
                        titleTask = "True_Private";
                        break;
                    default:
                        break;
                }
                // Task Name
                string titleNumber = title.Name;
                if (!titleNumber.Contains("_"))
                {
                    apiResult.Data = ZohoConstants.CUSTOM_CUT_ERROR_TT03;
                    return apiResult;
                }
                string taskName = $"[UR] {titleNumber.Split('_')[1]}";
                // Title CRM
                string titleCrm = $"{ZohoConstants.TRENCHES_ZCRM_TITLE_PREFIX_URL}/{titleId}";

                // STEP 3: Check if Project Exist
                string projectId = string.Empty;
                string searchTerm = string.Empty;
                if (projectName.Contains("-"))
                {
                    searchTerm = projectName.Split("-")[1].Trim();
                }
                else
                {
                    searchTerm = projectName;
                }

                var getAllProjectsResult = await _zohoProjectService.SearchProjectInPortal(projectKey, searchTerm);
                var allProjects = getAllProjectsResult.Data;
                var searchProject = allProjects.projects.Where(p => p.name.ToLower() == projectName.ToLower()).FirstOrDefault();
                if (searchProject == null)
                {
                    // Project does not exist, NEED TO CREATE
                    var projectForCreation = new ProjectForCreation()
                    {
                        name = projectName,
                        description = ZohoConstants.ZPRJ_PROJECT_DESCRIPTION
                    };
                    var createProjectResult = await _zohoProjectService.CreateProject(projectKey, projectForCreation);
                    if (createProjectResult.Code != ResultCode.Created)
                    {
                        apiResult.Data = ZohoConstants.CUSTOM_CUT_ERROR_P01;
                        return apiResult;
                    }
                    var project = createProjectResult.Data.projects.First();
                    projectId = project.id.ToString();
                }
                else
                {
                    projectId = searchProject.id.ToString();
                }

                // STEP 4: Check if Tasklist Exist
                string tasklistId = string.Empty;
                var getAllTasklistsInProject = await _zohoTasklistService.GetAllTasklistsInProject(projectKey, projectId);
                var allTasklists = getAllTasklistsInProject.Data;

                var searchTasklists = allTasklists.tasklists
                    .Where(tl => tl.name.ToLower() == ZohoConstants.ZPRJ_TASKLIST_UNREGISTERED_ROAD.ToLower())
                    .FirstOrDefault();
                if (searchTasklists == null)
                {
                    // Tasklist does not exist, NEED TO CREATE
                    var tasklistForCreation = new TasklistForCreation()
                    {
                        name = ZohoConstants.ZPRJ_TASKLIST_UNREGISTERED_ROAD,
                        flag = "external"
                    };
                    var createTasklistResult = await _zohoTasklistService.CreateTasklist(projectKey, projectId, tasklistForCreation);
                    if (createTasklistResult.Code != ResultCode.Created)
                    {
                        apiResult.Data = ZohoConstants.CUSTOM_CUT_ERROR_TL01;
                        return apiResult;
                    }
                    var tasklist = createTasklistResult.Data.tasklists.First();
                    tasklistId = tasklist.id.ToString();
                }
                else
                {
                    tasklistId = searchTasklists.id.ToString();
                }

                // STEP 5: Check if Task Exist
                string taskId = string.Empty;
                var getAllTasksInProject = await _zohoTaskService.GetAllTasksInProject(projectKey, projectId);
                var allTasks = getAllTasksInProject.Data;
                bool needToCreateTask = false;

                if (allTasks != null)
                {
                    var searchTask = allTasks.tasks
                    .Where(t => t.name.ToLower() == taskName.ToLower())
                    .FirstOrDefault();
                    if (searchTask == null)
                    {
                        needToCreateTask = true;

                    }
                    else
                    {
                        taskId = searchTask.id.ToString();
                    }
                }
                else
                {
                    needToCreateTask = true;
                }

                if (needToCreateTask)
                {
                    // Task does not exist, NEED TO CREATE
                    var taskForCreation = new TaskForCreation()
                    {
                        name = taskName,
                        tasklist_id = tasklistId,
                        UDF_CHAR1 = titleCrm,
                        UDF_CHAR2 = titleWayleave,
                        UDF_CHAR82 = titleTask,
                        UDF_CHAR83 = "Unregistered Road"
                    };
                    var createTaskResult = await _zohoTaskService.CreateTask(projectKey, projectId, taskForCreation);
                    if (createTaskResult.Code != ResultCode.Created)
                    {
                        apiResult.Data = ZohoConstants.CUSTOM_CUT_ERROR_TA01;
                        return apiResult;
                    }
                    var task = createTaskResult.Data.tasks.First();
                    taskId = task.id.ToString();
                }

                // Step 6: Update Task Url in Title in Zoho CRM
                string taskUrl = $"{ZohoConstants.ZPRJ_TASK_PREFIX_URL}/{projectId}/{tasklistId}/{taskId}";
                var titleUpdateRequest = new UpdateRequest();
                titleUpdateRequest.data.Add(new
                {
                    Task_URL = taskUrl
                });
                var updateTitleResult = await _zohoTitleService.UpdateTitle(crmKey, titleId, titleUpdateRequest);
                if (updateTitleResult.Code != ResultCode.OK)
                {
                    apiResult.Data = ZohoConstants.CUSTOM_CUT_ERROR_TT04;
                    return apiResult;
                }

                // Step 7: Upsert Task data to Trenches_Reporting DB


                // RETURN SUCCESS RESULT
                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.MSG_200;
                apiResult.Data = "Unregistered Task created successfully";

                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> ProcessPostcardMailing(string crmKey,
            string projectKey, string taskUrl, string credential)
        {
            var apiResult = new ApiResultDto<string>
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_VSP_ERROR,
            };

            var emailBody = $"Dear Sir/Madam,<br/><br/> Error processing mailing for: {taskUrl}<br/> $ErrorDetails$<br/><br/> Best Regards,<br/> Roxus Automation";
            var errorDetails = string.Empty;
            var postcardMailingRequest = new PostcardMailingRequest();
            var emailContent = new EmailContent()
            {
                Email = EmailConstants.TrenchesEmail,
                Body = string.Empty,
                Clients = "help@roxus.io",
                Subject = $"Postcard Error Alert for Task {taskUrl}",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Outlook_Email_SmtpServer
            };

            var appConfiguration = await _roxusLoggingRepository.GetAppConfigurationById(EmailConstants.TrenchesId);
            emailContent.Password = appConfiguration.Password;
            try
            {
                // Step 1: Split Task URL to get Project Id and Title Id
                string[] urlSplits = taskUrl.Split("/");
                int urlLength = urlSplits.Length;
                string taskId = urlSplits[urlLength - 1];
                string projectId = urlSplits[urlLength - 3];
                postcardMailingRequest.ProjectId = projectId;
                postcardMailingRequest.TaskId = taskId;

                // Step 2: Get Title Id from Task Details
                var getTaskDetailsResult = await _zohoTaskService.GetTaskDetails(projectKey, projectId, taskId);

                if (getTaskDetailsResult.Code != ResultCode.OK)
                {
                    // SEND ERROR EMAIL TO help@roxus.io
                    errorDetails += "<br/>- Error Code: TA01, Error Message: Cannot get Task Details.";
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }

                var taskDetails = getTaskDetailsResult.Data;
                var customFields = taskDetails.custom_fields;
                string titleUrl = string.Empty;
                foreach (var customField in customFields)
                {
                    if (customField.column_name == "UDF_CHAR1")
                    {
                        titleUrl = customField.value;
                        break;
                    }
                }
                string[] titleUrlSplits = titleUrl.Split('/');
                string titleId = titleUrlSplits[titleUrlSplits.Length - 1];

                if (string.IsNullOrWhiteSpace(titleId))
                {
                    // SEND ERROR EMAIL TO help@roxus.io
                    errorDetails += $"<br/>- Error Code: TTO1, Error Message: Invalid Title URL in Task.";
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }

                // Step 3: Get Related UPRNs from Title Details
                var getTitleByIdResult = await _zohoTitleService.GetTitleById(crmKey, titleId);
                if (getTitleByIdResult.Code != ResultCode.OK)
                {
                    // SEND ERROR EMAIL TO help@roxus.io
                    errorDetails += $"<br/>- Error Code: TTO2, Error Message: Cannot get Title by Id: {titleId}";
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                postcardMailingRequest.TitleId = getTitleByIdResult.Data.id;
                postcardMailingRequest.TitleNumber = getTitleByIdResult.Data.Name;

                var relatedUprns = getTitleByIdResult.Data.Related_UPRN;
                if (relatedUprns.Length == 0)
                {
                    // SEND ERROR EMAIL TO help@roxus.io
                    errorDetails += $"<br/>- Error Code: TTO3, Error Message: Title doesn't have any Related UPRNs. Title URL: {titleUrl}";
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }

                // Handle Project Name
                string projectName = getTitleByIdResult.Data.Project_Name;
                if (string.IsNullOrWhiteSpace(projectName))
                {
                    // SEND ERROR EMAIL TO help@roxus.io
                    errorDetails += $"<br/>- Error Code: TTO4, Error Message: Project Name in Title is empty. Title URL: {titleUrl}";
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                postcardMailingRequest.ProjectName = projectName;

                ISet<string> uprnLists = new HashSet<string>();

                foreach (var relatedUprn in relatedUprns)
                {
                    string uprnId = relatedUprn.UPRN.id;
                    if (!string.IsNullOrWhiteSpace(uprnId))
                    {
                        uprnLists.Add(uprnId);
                    }
                }

                foreach (var uprnId in uprnLists)
                {
                    // Start extracting UPRN Address and check if they are valid
                    var getUprnByIdResult = await _zohoUprnService.GetUprnById(crmKey, uprnId);
                    if (getUprnByIdResult.Code != ResultCode.OK)
                    {
                        // SEND ERROR EMAIL TO help@roxus.io
                        errorDetails += $"<br/>- Error Code: UP01, Error Message: Cannot get UPRN Details.";
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }
                    var getUprnById = getUprnByIdResult.Data;
                    var postcardDetail = new PostcardDetail();

                    // Handle Name
                    postcardDetail.FirstName = "The";
                    postcardDetail.SurName = "Occupier";
                    postcardDetail.FullName = "The Occupier";

                    // Handle Address
                    string subBuildingName = getUprnById.Sub_Building_Name;
                    string buildingName = getUprnById.Building_Number_or_Name;
                    string buildingNumber = getUprnById.Building_Number;
                    string street = getUprnById.Address_Line_1;
                    string city = getUprnById.City;
                    string state = getUprnById.County;
                    string postcode = getUprnById.Post_Code;
                    // Handle Address 1
                    string address1 = string.Empty;
                    int countAddress1 = 0;
                    if (!string.IsNullOrWhiteSpace(buildingNumber))
                    {
                        address1 = $"{buildingNumber} {street}";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(subBuildingName))
                        {
                            address1 = subBuildingName;
                            countAddress1++;
                        }
                        if (!string.IsNullOrWhiteSpace(buildingName))
                        {
                            if (countAddress1 == 0)
                            {
                                address1 = buildingName;
                            }
                            else
                            {
                                address1 += $", {buildingName}";
                            }
                            countAddress1++;
                        }
                        if (!string.IsNullOrWhiteSpace(street))
                        {
                            if (countAddress1 == 0)
                            {
                                address1 = street;
                            }
                            else
                            {
                                address1 += $", {street}";
                            }
                            countAddress1++;
                        }
                    }
                    if (address1.Length > 50)
                    {
                        // SEND ERROR EMAIL TO help@roxus.io
                        errorDetails += $"<br/>- Error Code: UP02, Error Message: UPRN Address is more than 50 characters. UPRN URL: {ZohoConstants.ZCRM_UPRN_PREFIX_URL}/{getUprnById.id}.";
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }
                    postcardDetail.Address1 = address1;
                    // Handle Address 2
                    postcardDetail.Address2 = city;
                    // Handle Address 3
                    postcardDetail.Address3 = state;
                    // Handle Address 4
                    postcardDetail.Address4 = postcode;
                    // Handle Custom Fields
                    postcardDetail.Custom1 = getUprnById.Name;
                    postcardMailingRequest.Postcards.Add(postcardDetail);
                }

                // Send Request to Docmail API
                string requestBody = JsonConvert.SerializeObject(postcardMailingRequest);
                string endpoint = "https://roxusdocmailapi.azurewebsites.net/api/docmail/custom/send-postcard";
                // string endpoint = "https://localhost:44321/api/docmail/custom/send-postcard";
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credential}");

                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = JObject.Parse(responseData);

                        // Update Task
                        var taskForUpdation = new TaskForUpdation()
                        {
                            UDF_DATE1 = DateTime.UtcNow.ToString(CommonConstants.TaskDateFormat),
                            UDF_CHAR4 = $"Send Postcard successfully for all UPRNs",
                        };
                        var updateTaskResult = await _zohoTaskService.UpdateTaskForTrenches(projectKey, projectId, taskId, taskForUpdation);

                        if (updateTaskResult.Code != ResultCode.OK)
                        {
                            // SEND ERROR EMAIL TO help@roxus.io
                            errorDetails += $"<br/>- Error Code: TA02, Error Message: Cannot Update Task.";
                            emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                            await EmailHelpers.SendEmail(emailContent);
                            return apiResult;
                        }

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZohoConstants.MSG_200;
                        var processPostcardResult = JsonConvert.DeserializeObject<ApiResultDto<string>>(responseData);
                        apiResult.Data = processPostcardResult.Data;
                    }
                    return apiResult;
                }
            }
            catch (Exception)
            {
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
            }
        }

        public async Task<ApiResultDto<string>> ProcessSendingLetter(SendLetterRequest letterRequest)
        {
            var apiResult = new ApiResultDto<string>
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_PSL_ERROR,
            };
            string taskUrl = ZohoConstants.ZPRJ_TASK_URL
                .Replace("{ProjectId}", letterRequest.projectId)
                .Replace("{TasklistId}", letterRequest.tasklistId)
                .Replace("{TaskId}", letterRequest.taskId);
            var emailBody = $"Dear Roxus Support,<br><br> Error processing mailing for: <a href='{taskUrl}' target='_blank'>{taskUrl}</a><br> $ErrorDetails$<br><br> Best Regards,<br> Roxus Automation";
            var errorDetails = string.Empty;
            // var sendLetterRequest = new PostcardMailingRequest();

            var emailContent = new EmailContent()
            {
                Email = EmailConstants.TrenchesEmail,
                Body = string.Empty,
                Clients = "help@roxus.io",
                Subject = $"[Ogi] $ErrorCode$ Docmail Error Alert for Title: $TitleNumber$",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Outlook_Email_SmtpServer
            };
            var appConfiguration = await _roxusLoggingRepository.GetAppConfigurationById(EmailConstants.TrenchesId);
            emailContent.Password = appConfiguration.Password;

            ProcessLetterRequest processLetterRequest = null;
            try
            {
                // STEP 0: Prepare Constants and Variables
                string baseTemplate = "{TemplatePrefix}_{Type}_{LetterNumber}";
                string projectId = letterRequest.projectId;
                string taskId = letterRequest.taskId;
                // STEP 1: Get Task Details and extract data
                var getTaskDetailsResult = await _zohoTaskService.GetTaskDetails(letterRequest.projectKey, projectId, taskId);
                if (getTaskDetailsResult.Code != ResultCode.OK)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-T01]")
                        .Replace("$TitleNumber$", taskUrl);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_T01;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                var taskDetails = getTaskDetailsResult.Data;
                string taskName = taskDetails.name;
                var customFields = taskDetails.custom_fields;
                string titleUrl = string.Empty;
                string taskStatus = taskDetails.status.name;
                int letterNumber = 0;
                switch (taskStatus)
                {
                    case "Letter 1":
                        letterNumber = 1;
                        break;
                    case "Letter 2":
                        letterNumber = 2;
                        break;
                    case "Letter 3":
                        letterNumber = 3;
                        break;
                    case "Letter  4":
                        letterNumber = 4;
                        break;
                    default:
                        break;
                }
                if (letterNumber == 0)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-T04]")
                        .Replace("$TitleNumber$", taskUrl);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_T04;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                string letter1Date = string.Empty;
                string letter2Date = string.Empty;
                string letter3Date = string.Empty;
                string letter4Date = string.Empty;
                foreach (var customField in customFields)
                {
                    switch (customField.label_name)
                    {
                        case "CRM Title":
                            titleUrl = customField.value;
                            break;
                        case "Letter 1 Date":
                            letter1Date = customField.value;
                            break;
                        case "Letter 2 Date":
                            letter2Date = customField.value;
                            break;
                        case "Letter 3 Date":
                            letter3Date = customField.value;
                            break;
                        case "Letter 4 Date":
                            letter4Date = customField.value;
                            break;
                    }
                }
                if (string.IsNullOrEmpty(letter1Date) && letterNumber >= 2)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-LT01]")
                        .Replace("$TitleNumber$", taskUrl);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_LT01;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                if (string.IsNullOrEmpty(letter2Date) && letterNumber >= 3)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-LT02]")
                        .Replace("$TitleNumber$", taskUrl);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_LT02;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                if (string.IsNullOrEmpty(letter3Date) && letterNumber == 4)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-LT03]")
                        .Replace("$TitleNumber$", taskUrl);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_LT03;
                    emailBody = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                string[] titleSplits = titleUrl.Split("/");
                int titleLength = titleSplits.Length;
                string titleId = titleSplits[titleLength - 1];

                // STEP 2: Get Title and extract data
                // Step 2.1: Remove Redundant Occupier(s) 
                var removeOccupiersResult = await RemoveRedundantOccupier(letterRequest.crmKey, titleId);
                if (removeOccupiersResult.Code != ResultCode.OK)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[RRO-400]")
                        .Replace("$TitleNumber$", taskUrl);
                    errorDetails = ZohoConstants.CUSTOM_RR0_400;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                // Step 2.2: Get Title by Id
                var getTitleByIdResult = await _zohoTitleService.GetTitleById(letterRequest.crmKey, titleId);
                if (getTitleByIdResult.Code != ResultCode.OK)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL_TT01]")
                        .Replace("$TitleNumber$", taskUrl);
                    errorDetails = ZohoConstants.CUSTOM_PSL_TT01;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                var titleData = getTitleByIdResult.Data;
                // Step 2.3: Extract Title Data
                string titleNumber = titleData.Name;
                // Step 2.3.1: Get Related UPRNs in Title
                string titleType = titleData.Type;
                var relatedUPRNs = titleData.Related_UPRN;
                int uprnCount = relatedUPRNs.Count();
                string relatedUPRNStr = string.Empty;
                foreach (var relatedUPRN in relatedUPRNs)
                {
                    if (relatedUPRN.UPRN != null)
                    {
                        var uprn = relatedUPRN.UPRN;
                        if (!string.IsNullOrEmpty(uprn.name))
                        {
                            if (string.IsNullOrEmpty(relatedUPRNStr))
                            {
                                relatedUPRNStr = uprn.name;
                            }
                            else
                            {
                                relatedUPRNStr += $",{uprn.name}";
                            }
                        }
                    }
                }
                
                // Step 2.3.2: Generate Custom1
                string projectName = titleData.Project_Name;
                string titleWayleave = titleData.Wayleave_Template;
                string custom1 = $"{projectName}/{titleNumber}";
                // Step 2.3.3: Generate Custom2
                string titleReference = titleData.Reference;
                string custom2 = titleReference;
                string custom8 = titleReference;
                string mailingSubject = titleReference;
                // Step 2.3.4: Handle Task Name
                string bracketPattern = @"\[(.*?)\]";
                var taskMatches = Regex.Matches(taskName, bracketPattern);
                string newTitleType = string.Empty;
                string taskPrefix1 = string.Empty;
                string taskPrefix2 = string.Empty;
                if (taskMatches.Count >= 2)
                {
                    taskPrefix1 = taskMatches[0].Value.Replace("[", "").Replace("]", "");
                    taskPrefix2 = taskMatches[1].Value.Replace("[", "").Replace("]", "");
                    newTitleType = taskPrefix2;
                }
                else
                {
                    newTitleType = titleType;
                }
                if (!(taskPrefix1.Contains("Automation", StringComparison.InvariantCultureIgnoreCase) 
                    || taskPrefix1.Contains("PIA", StringComparison.InvariantCultureIgnoreCase)))
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-T02]")
                        .Replace("$TitleNumber$", titleNumber);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_T02;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                if (taskPrefix1.Contains("PIA", StringComparison.InvariantCultureIgnoreCase) && letterNumber == 4)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-T03]")
                        .Replace("$TitleNumber$", titleNumber);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_T03;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                // Step 2.3.5: Handle Letter Templates
                string templatePrefix = string.Empty;
                string clientCode = string.Empty;
                if (projectName.Contains("WightFibre", StringComparison.OrdinalIgnoreCase))
                {
                    templatePrefix = "TL_WF";
                    clientCode = "WF";
                }
                else if (projectName.Contains("OGI", StringComparison.OrdinalIgnoreCase))
                {
                    templatePrefix = "TL_OGI";
                    clientCode = "OGI";
                }
                else
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-C01]")
                        .Replace("$TitleNumber$", titleNumber);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_C01.Replace("{clientName}", projectId);
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                baseTemplate = baseTemplate.Replace("{TemplatePrefix}", templatePrefix)
                    .Replace("{Type}", newTitleType).Replace("{LetterNumber}", letterNumber.ToString());
                string wayleaveTemplate = $"{templatePrefix}_Wayleave";
                string accountWayleaveTemplate = $"{templatePrefix}_Wayleave_Company";
                if (titleType == "Wildanet")
                {
                    baseTemplate = $"TL_Wildanet_{letterNumber}";
                }
                // Step 2.3.6: Handle Fallback if there is missing information in Title
                if (string.IsNullOrEmpty(titleType))
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-TT02]")
                        .Replace("$TitleNumber$", titleNumber);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_TT02.Replace("{TitleUrl}", titleUrl);
                }
                if (string.IsNullOrEmpty(titleReference))
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-TT03]")
                        .Replace("$TitleNumber$", titleNumber);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_TT03.Replace("{TitleUrl}", titleUrl);
                }
                if (string.IsNullOrEmpty(projectName))
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-TT04]")
                        .Replace("$TitleNumber$", titleNumber);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_TT04.Replace("{TitleUrl}", titleUrl);
                }
                if (string.IsNullOrEmpty(wayleaveTemplate))
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-TT05]")
                        .Replace("$TitleNumber$", titleNumber);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_TT05.Replace("{TitleUrl}", titleUrl);
                }
                var allContacts = new List<string>();
                var allAccounts = new List<string>();
                if (titleData.Related_Contacts != null)
                {
                    foreach (var relatedContact in titleData.Related_Contacts)
                    {
                        if (relatedContact.Contact_Name != null)
                        {
                            string contactId = relatedContact.Contact_Name.id;
                            if (!string.IsNullOrEmpty(contactId))
                            {
                                allContacts.Add(contactId);
                            }
                        }
                        if (relatedContact.Account_Name != null)
                        {
                            string accountId = relatedContact.Account_Name.id;
                            if (!string.IsNullOrEmpty(accountId))
                            {
                                allAccounts.Add(accountId);
                            }
                        }
                    }
                }
                if (allContacts.Count == 0 && allAccounts.Count == 0)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-TT06]")
                        .Replace("$TitleNumber$", titleNumber);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_TT06.Replace("{TitleUrl}", titleUrl);
                }
                if (!string.IsNullOrEmpty(errorDetails))
                {
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                // STEP 3: Verify Account and Contact Data
                // Step 3.1: Verify Account
                bool accountError = false;
                var allAccountData = new List<AccountResponse>();
                foreach (var accountId in allAccounts)
                {
                    string accountUrl = $"{ZohoConstants.TRENCHES_ZCRM_ACCOUNT_PREFIX_URL}/{accountId}";
                    var getAccountByIdResult = await _zohoAccountService.GetAccountById(letterRequest.crmKey, accountId);
                    var accountData = getAccountByIdResult.Data;
                    allAccountData.Add(accountData);
                    string accountName = accountData.Account_Name;
                    int accNameLength = accountName.Length;
                    if (accNameLength > 50)
                    {
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-AC03]")
                            .Replace("$TitleNumber$", titleNumber);
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_AC03.Replace("{AccountUrl}", accountUrl);
                        accountError = true;
                    }
                    string accBillingStreet = accountData.Billing_Street;
                    string accBillingCity = accountData.Billing_City;
                    string accBillingState = accountData.Billing_State;
                    string accBillingCode = accountData.Billing_Code;
                    if (string.IsNullOrEmpty(accBillingStreet) || string.IsNullOrEmpty(accBillingCity) || string.IsNullOrEmpty(accBillingCode))
                    {
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-AC01]")
                            .Replace("$TitleNumber$", titleNumber);
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_AC01.Replace("{AccountUrl}", accountUrl);
                        accountError = true;
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }
                    int accStreetLength = accBillingStreet.Length;
                    int accCityLength = accBillingCity.Length;
                    int accCodeLength = accBillingCode.Length;
                    if (accStreetLength > 50 || accCityLength > 50 || accCodeLength > 50)
                    {
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-AC02]")
                            .Replace("$TitleNumber$", titleNumber);
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_AC02.Replace("{AccountUrl}", accountUrl);
                        accountError = true;
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }
                }
                // Step 3.2: Verify Contact
                bool contactError = false;
                var allContactAddresses = new List<string>();
                var allContactFullNames = new List<string>();
                var allContactCustom1 = new List<string>();
                var allContactIds = new List<string>();

                foreach (var contactId in allContacts)
                {
                    string contactUrl = $"{ZohoConstants.TRENCHES_ZCRM_CONTACT_PREFIX_URL}/{contactId}";
                    string last6Digit = contactId.Substring(contactId.Length - 6);
                    string contactCustom1 = $"{custom1}/{last6Digit}";
                    var getContactByIdResult = await _zohoContactService.GetContactById(letterRequest.crmKey, contactId);
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

                    if (string.IsNullOrWhiteSpace(conMailingStreet) || string.IsNullOrWhiteSpace(conMailingCity) || string.IsNullOrWhiteSpace(conMailingCode))
                    {
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-CO01]")
                            .Replace("$TitleNumber$", titleNumber);
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_CO01.Replace("{ContactUrl}", contactUrl);
                        contactError = true;
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }
                    int conStreetLength = conMailingStreet.Length;
                    int conCityLength = conMailingCity.Length;
                    int conCodeLength = conMailingCode.Length;
                    if (conStreetLength > 50 || conCityLength > 50 || conCodeLength > 50)
                    {
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-CO02]")
                            .Replace("$TitleNumber$", titleNumber);
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_CO02.Replace("{ContactUrl}", contactUrl);
                        contactError = true;
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }
                    string contactAddress = conMailingStreet + "|||" + conMailingCity + "|||" + conMailingState + "|||" + conMailingCode + 
                        "|||" + conMailingCountry;
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
                // Step 3.3: End process if Account or Contact have errors
                if (accountError || contactError)
                {
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                // STEP 4: Process Letter
                // Step 4.1: Initialize Process Letter
                processLetterRequest = new ProcessLetterRequest();
                processLetterRequest.TaskUrl = taskUrl;
                processLetterRequest.TitleId = titleId;
                processLetterRequest.TitleNumber = titleNumber;
                processLetterRequest.RelatedUPRN = relatedUPRNStr;
                processLetterRequest.UprnCount = uprnCount;
                processLetterRequest.ProjectId = projectId;
                processLetterRequest.TaskId = taskId;
                processLetterRequest.CustomerId = DocmailConstants.TrenchesCustomerId;
                processLetterRequest.Environment = "PRODUCTION";
                processLetterRequest.MailingContent = titleWayleave;
                processLetterRequest.MailingStatus = DocmailConstants.DefaultMailingStatus;
                if (baseTemplate.Contains("Wildanet"))
                {
                    processLetterRequest.MailingType = $"{DocmailConstants.TrenchesCustomerCode}_{titleType}_{letterNumber}";
                }
                else
                {
                    processLetterRequest.MailingType = $"{DocmailConstants.TrenchesCustomerCode}_{clientCode}_{titleType}_{letterNumber}";
                }
                // Step 4.2: Process Letter for Accounts
                foreach (var accountData in allAccountData)
                {
                    var accountLetter = new AccountLetter();
                    string accountName = accountData.Account_Name;
                    string companyNumber = accountData.Company_Registration;
                    // Step 4.2.1: Handle Custom 9
                    if (!string.IsNullOrEmpty(companyNumber))
                    {
                        accountLetter.Custom9 = $"{accountName} ({companyNumber})"; 
                    }
                    else
                    {
                        accountLetter.Custom9 = $"{accountName}";
                    }
                    // Step 4.2.2: Handle Template
                    string accountId = accountData.id;
                    string last6Digit = accountId.Substring(accountId.Length - 6);
                    accountLetter.AccountId = accountId;
                    accountLetter.Custom1 = $"{custom1}/{last6Digit}";
                    accountLetter.Custom2 = custom2;
                    accountLetter.MailingName = accountLetter.Custom1;
                    accountLetter.MailingDescription = $"Mail triggered by Roxus Robot for Title Number: {titleNumber}, send to Account: {accountName}";
                    accountLetter.OuterEnvelope = DocmailConstants.Trenches_OuterEnvelopeColour;
                    accountLetter.BaseTemplate = baseTemplate;
                    if (baseTemplate.Contains("Wildanet", StringComparison.InvariantCultureIgnoreCase))
                    {
                        accountLetter.WayleaveTemplate = DocmailConstants.Trenches_Wildanet_Wayleave_Template;
                        accountLetter.MapImage = string.Empty;
                    }
                    else if (baseTemplate.Contains("POLE", StringComparison.InvariantCultureIgnoreCase)
                        || baseTemplate.Contains("PIT", StringComparison.InvariantCultureIgnoreCase)
                        || baseTemplate.Contains("BP", StringComparison.InvariantCultureIgnoreCase))
                    {
                        accountLetter.WayleaveTemplate = string.Empty;
                        accountLetter.MapImage = titleWayleave;
                    }
                    else
                    {
                        accountLetter.WayleaveTemplate = accountWayleaveTemplate;
                        accountLetter.MapImage = titleWayleave;
                    }
                    accountLetter.ReplyEnvelope = DocmailConstants.Trenches_BusinessReplyEnvelope_2nd;
                    // Step 4.2.3: Handle Account Address
                    string accBillingStreet = accountData.Billing_Street;
                    string accBillingCity = accountData.Billing_City;
                    string accBillingState = accountData.Billing_State;
                    string accBillingCode = accountData.Billing_Code;
                    string accBillingCountry = accountData.Billing_Country;

                    if (string.IsNullOrEmpty(accBillingState))
                    {
                        accountLetter.Custom7 = $"{accBillingStreet}, {accBillingCity}, {accBillingCode}";
                    }
                    else
                    {
                        accountLetter.Custom7 = $"{accBillingStreet}, {accBillingCity}, {accBillingState}, {accBillingCode}";
                    }

                    if (!string.IsNullOrEmpty(accBillingCountry))
                    {
                        accountLetter.Address5 = accBillingCountry;
                        accountLetter.Custom7 = $", {accBillingCountry}";
                    }

                    accountLetter.Address1 = accountData.Billing_Street;
                    accountLetter.Address2 = accountData.Billing_City;
                    accountLetter.Address3 = accountData.Billing_State;
                    accountLetter.Address4 = accountData.Billing_Code;
                    accountLetter.FirstName = "The";
                    accountLetter.SurName = "Directors";
                    accountLetter.FullName = "Directors";
                    accountLetter.CompanyName = accountName;
                    // Step 4.2.4: Handle Account Custom fields
                    var custom3Date = DateTimeHelpers.AddDaysForLetter(16);
                    accountLetter.Custom3 = custom3Date.ToString(DocmailConstants.LongDateFormat);
                    if (letterNumber == 2)
                    {
                        accountLetter.Custom4 = DateTimeHelpers.ConvertLetterDateTime(letter1Date)
                            .ToString(DocmailConstants.LongDateFormat);
                    }
                    else if (letterNumber == 3)
                    {
                        accountLetter.Custom4 = DateTimeHelpers.ConvertLetterDateTime(letter1Date)
                            .ToString(DocmailConstants.LongDateFormat);
                        accountLetter.Custom5 = DateTimeHelpers.ConvertLetterDateTime(letter2Date)
                            .ToString(DocmailConstants.LongDateFormat);
                    }
                    else if (letterNumber == 4)
                    {
                        accountLetter.Custom4 = DateTimeHelpers.ConvertLetterDateTime(letter1Date)
                            .ToString(DocmailConstants.LongDateFormat);
                        accountLetter.Custom5 = DateTimeHelpers.ConvertLetterDateTime(letter2Date)
                            .ToString(DocmailConstants.LongDateFormat);
                        accountLetter.Custom6 = DateTimeHelpers.ConvertLetterDateTime(letter3Date)
                            .ToString(DocmailConstants.LongDateFormat);
                    }
                    accountLetter.Custom8 = custom8;
                    accountLetter.Custom10 = string.Empty;
                    processLetterRequest.AccountData.Add(accountLetter);
                }
                // Step 4.3: Process Letter for Contacts 
                for (int i = 0; i < allContactAddresses.Count; i++)
                {
                    var contactLetter = new ContactLetter();
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
                    contactLetter.MailingName = allContactCustom1[i];
                    contactLetter.MailingDescription = $"Mail triggered by Roxus Robot for Title Number: {titleNumber}, send to Contact: {contactLetter.FullName}";
                    // Step 4.3.2: Handle Template
                    contactLetter.OuterEnvelope = DocmailConstants.Trenches_OuterEnvelopeColour;
                    contactLetter.BaseTemplate = baseTemplate;
                    if (baseTemplate.Contains("POLE", StringComparison.InvariantCultureIgnoreCase) 
                        || baseTemplate.Contains("PIT", StringComparison.InvariantCultureIgnoreCase)
                        || baseTemplate.Contains("BP", StringComparison.InvariantCultureIgnoreCase))
                    {
                        contactLetter.WayleaveTemplate = string.Empty;
                        contactLetter.MapImage = titleWayleave;
                    }
                    else if (baseTemplate.Contains("Wildanet"))
                    {
                        contactLetter.WayleaveTemplate = DocmailConstants.Trenches_Wildanet_Wayleave_Template;
                        contactLetter.MapImage = string.Empty;
                    }
                    else
                    {
                        contactLetter.WayleaveTemplate = $"{wayleaveTemplate}_{numberOfPeople}";
                        contactLetter.MapImage = titleWayleave;
                    }
                    contactLetter.ReplyEnvelope = DocmailConstants.Trenches_BusinessReplyEnvelope_2nd;
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

                    // Step 4.3.4: Handle Contact Custom Fields
                    contactLetter.Custom1 = allContactCustom1[i];
                    contactLetter.Custom2 = custom2;
                    contactLetter.Custom8 = custom8;
                    var custom3Date = DateTimeHelpers.AddDaysForLetter(16);
                    contactLetter.Custom3 = custom3Date.ToString(DocmailConstants.LongDateFormat);
                    if (letterNumber == 2)
                    {
                        contactLetter.Custom4 = DateTimeHelpers.ConvertLetterDateTime(letter1Date)
                            .ToString(DocmailConstants.LongDateFormat);
                    }
                    else if (letterNumber == 3)
                    {
                        contactLetter.Custom4 = DateTimeHelpers.ConvertLetterDateTime(letter1Date)
                            .ToString(DocmailConstants.LongDateFormat);
                        contactLetter.Custom5 = DateTimeHelpers.ConvertLetterDateTime(letter2Date)
                            .ToString(DocmailConstants.LongDateFormat);
                    }
                    else if (letterNumber == 4)
                    {
                        contactLetter.Custom4 = DateTimeHelpers.ConvertLetterDateTime(letter1Date)
                            .ToString(DocmailConstants.LongDateFormat);
                        contactLetter.Custom5 = DateTimeHelpers.ConvertLetterDateTime(letter2Date)
                            .ToString(DocmailConstants.LongDateFormat);
                        contactLetter.Custom6 = DateTimeHelpers.ConvertLetterDateTime(letter3Date)
                            .ToString(DocmailConstants.LongDateFormat);
                    }
                    contactLetter.Custom10 = string.Empty;
                    processLetterRequest.ContactData.Add(contactLetter);
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.MSG_200;
                // STEP 5: Process send letter
                string requestBody = JsonConvert.SerializeObject(processLetterRequest);
                string uploadEndpoint = RoxusEndpointConstants.Docmail_SendLetter_PRODUCTION;
                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    uploadEndpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {RoxusEndpointConstants.Docmail_BasicAuth_PRODUCTION}");
                using (var response = await _httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();
                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_ERROR;
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }
                }
                _httpClient.DefaultRequestHeaders.Clear();
                // STEP 6: Update Task Letter Status and Message
                var taskForUpdation = new TaskForUpdation();
                taskForUpdation.end_date = DateTimeHelpers.AddDaysForLetter(16).ToString("MM-dd-yyyy");
                var letterSendDate = DateTimeHelpers.AddDaysForLetter(1).ToString("MM-dd-yyyy");
                switch (letterNumber)
                {
                    case 1:
                        taskForUpdation.UDF_DATE1 = letterSendDate;
                        taskForUpdation.UDF_CHAR4 = $"Letter {letterNumber} sent SUCCESSFULLY";
                        break;
                    case 2:
                        taskForUpdation.UDF_DATE2 = letterSendDate;
                        taskForUpdation.UDF_CHAR5 = $"Letter {letterNumber} sent SUCCESSFULLY";
                        break;
                    case 3:
                        taskForUpdation.UDF_DATE3 = letterSendDate;
                        taskForUpdation.UDF_CHAR6 = $"Letter {letterNumber} sent SUCCESSFULLY";
                        break;
                    case 4:
                        taskForUpdation.UDF_DATE4 = letterSendDate;
                        taskForUpdation.UDF_CHAR7 = $"Letter {letterNumber} sent SUCCESSFULLY";
                        break;
                    default:
                        break;
                }
                Thread.Sleep(5000);
                var updateTaskResult = await _zohoTaskService.UpdateTaskForTrenches(letterRequest.projectKey, projectId, taskId, taskForUpdation);
                if (updateTaskResult.Code != ResultCode.OK)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-T05]")
                        .Replace("$TitleNumber$", titleNumber);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_T05;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                // STEP 7: Update Trenches Task
                var insertTaskResult = await _zohoProjectService.InsertTaskToTable(letterRequest.projectKey, taskId, projectId);
                if (insertTaskResult.Code != ResultCode.OK)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[PSL-T06]")
                        .Replace("$TitleNumber$", titleNumber);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_T06;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                // STEP 8: Return success message
                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_PSL_SUCCESS;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message}\n{ex.StackTrace}";
                emailContent.Body = $"{errorDetails}<br>{ex.Message}<br>{ex.StackTrace}";
                emailContent.Subject = $"Web Hook FAILED: {taskUrl}";
                await EmailHelpers.SendEmail(emailContent);
                return apiResult;
            }
            finally
            {
                _httpClient.DefaultRequestHeaders.Clear();
            }
        }

        public async Task<ApiResultDto<string>> OR_ProcessSendingLetter(SendLetterRequest letterRequest)
        {
            var apiResult = new ApiResultDto<string>
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_PSL_ERROR,
            };

            string taskUrl = ZohoConstants.ZPRJ_TASK_URL
                .Replace("{ProjectId}", letterRequest.projectId)
                .Replace("{TasklistId}", letterRequest.tasklistId)
                .Replace("{TaskId}", letterRequest.taskId);
            var emailBody = $"Dear Roxus Support,<br><br> Error processing mailing for: <a href='{taskUrl}' target='_blank'>{taskUrl}</a><br> $ErrorDetails$<br><br> Best Regards,<br> Roxus Automation";
            var errorDetails = string.Empty;
            // var sendLetterRequest = new PostcardMailingRequest();

            var emailContent = new EmailContent()
            {
                Email = EmailConstants.TrenchesEmail,
                Body = string.Empty,
                Clients = "help@roxus.io",
                Subject = $"[Openreach] $ErrorCode$ Docmail Error Alert for Openreach: $Openreach$",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Outlook_Email_SmtpServer
            };
            var appConfiguration = await _roxusLoggingRepository.GetAppConfigurationById(EmailConstants.TrenchesId);
            emailContent.Password = appConfiguration.Password;

            OR_ProcessLetterRequest orLetterRequest = null;

            try
            {
                // STEP 0: Prepare Constants and Variables
                string projectId = letterRequest.projectId;
                string tasklistId = letterRequest.tasklistId;
                string taskId = letterRequest.taskId;
                var distinctORNumbers = new HashSet<string>();
                string prefixTaskName = string.Empty;

                orLetterRequest = new OR_ProcessLetterRequest();
                // STEP 1: Get Task Details and extract data
                var getTaskDetailsResult = await _zohoTaskService.GetTaskDetails(letterRequest.projectKey, projectId, taskId);
                Thread.Sleep(5000);
                if (getTaskDetailsResult.Code != ResultCode.OK)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-T01]")
                        .Replace("$Openreach$", taskUrl);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_T01;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                var taskDetails = getTaskDetailsResult.Data;
                string taskName = taskDetails.name;

                if (taskName.StartsWith("ORA", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefixTaskName = "ORA";
                }
                else if (taskName.StartsWith("ORD", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefixTaskName = "ORD";
                }
                else if (taskName.StartsWith("ORH", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefixTaskName = "ORH";
                }
                else
                {
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_T02.Replace("{TaskUrl}", taskUrl);
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }

                var customFields = taskDetails.custom_fields;
                string openreachUrl = string.Empty;

                string letter1DateStr = string.Empty;
                string letter2DateStr = string.Empty;
                string letter3DateStr = string.Empty;
                string letter4Date = string.Empty;
                string taskLetterReference = string.Empty;

                foreach (var customField in customFields)
                {
                    string value = customField.value;
                    string labelName = customField.label_name;

                    switch (labelName)
                    {
                        case "CRM SM":
                            openreachUrl = value;
                            break;
                        case "Letter 1 Date":
                            letter1DateStr = customField.value;
                            break;
                        case "Letter 2 Date":
                            letter2DateStr = customField.value;
                            break;
                        case "Letter 3 Date":
                            letter3DateStr = customField.value;
                            break;
                    }

                }
                var openreachSplits = openreachUrl.Split("/");
                string openreachId = openreachSplits[openreachSplits.Length - 1];

                string titleUrl = string.Empty;
                string taskStatus = taskDetails.status.name;
                int letterNumber = 0;

                switch (taskStatus)
                {
                    case "Letter 1":
                        letterNumber = 1;
                        break;
                    case "Letter 2":
                        letterNumber = 2;
                        break;
                    case "Letter 3":
                        letterNumber = 3;
                        break;
                    default:
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-T01]")
                            .Replace("$Openreach$", taskUrl);
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_T03.Replace("{TaskUrl}", taskUrl);
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                }

                if (string.IsNullOrEmpty(letter1DateStr) && letterNumber >= 2)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-LT01]")
                        .Replace("$Openreach$", taskUrl);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_LT01.Replace("{TaskUrl}", taskUrl);
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                else if (!string.IsNullOrEmpty(letter1DateStr) && letterNumber >= 2)
                {
                    // Check if Letter 1 Date is 16 days compare to the current date
                    string newLetter1DateStr = letter1DateStr;
                    var letter1Splits = letter1DateStr.Split(" ");
                    newLetter1DateStr = letter1Splits[0];
                    var newLetter1Date = DateTime.ParseExact(newLetter1DateStr, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    var nextLetterDate = newLetter1Date.AddDays(16).Date;
                    var todayDate = DateTime.UtcNow.Date;
                    if (nextLetterDate > todayDate)
                    {
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-LD01]")
                            .Replace("$Openreach$", taskUrl);
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_LD01
                            .Replace("{TaskUrl}", taskUrl)
                            .Replace("{CurrentDate}", todayDate.ToString("dd MMM yyyy"))
                            .Replace("{NextLetterDate}", nextLetterDate.ToString("dd MMM yyyy"));
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }
                }

                if (string.IsNullOrEmpty(letter2DateStr) && letterNumber >= 3)
                {
                    emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-LT02]")
                            .Replace("$Openreach$", taskUrl);
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_LT02.Replace("{TaskUrl}", taskUrl);
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                else if (!string.IsNullOrEmpty(letter2DateStr) && letterNumber >= 3)
                {
                    // Check if Letter 1 Date is 16 days compare to the current date
                    string newLetter2DateStr = letter2DateStr;
                    var letter2Splits = letter2DateStr.Split(" ");
                    newLetter2DateStr = letter2Splits[0];
                    var newLetter2Date = DateTime.ParseExact(newLetter2DateStr, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    var nextLetterDate = newLetter2Date.AddDays(16).Date;
                    var todayDate = DateTime.UtcNow.Date;
                    if (nextLetterDate > todayDate)
                    {
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-LD02]")
                            .Replace("$Openreach$", taskUrl);
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_LD02
                            .Replace("{TaskUrl}", taskUrl)
                            .Replace("{CurrentDate}", todayDate.ToString("dd MMM yyyy"))
                            .Replace("{NextLetterDate}", nextLetterDate.ToString("dd MMM yyyy")); ;
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }
                }

                // STEP 2: Get Openreach Details
                var getOpenreachResult = await _zohoOpenreachService.GetOpenreachById(letterRequest.crmKey, openreachId);
                if (getOpenreachResult.Code != ResultCode.OK)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_POL_T02;
                    return apiResult;
                }
                var openreachDetails = getOpenreachResult.Data;
                string openreachNumber = openreachDetails.Openreach_SM;
                string openreachLocation = openreachDetails.Enablement_Patch_v2;
                string contactType = openreachDetails.Contact_Type;

                bool sendEmailToContact = false;
                if (contactType.Contains("Private", StringComparison.InvariantCultureIgnoreCase))
                {
                    sendEmailToContact = true;
                }

                string letterTemplate = string.Empty;
                switch (openreachLocation)
                {
                    case "London":
                        letterTemplate = $"TL_OR_L{letterNumber}_L";
                        break;
                    case "North":
                        letterTemplate = $"TL_OR_L{letterNumber}_N";
                        break;
                    case "Midlands":
                        letterTemplate = $"TL_OR_L{letterNumber}_M";
                        break;
                    case "South & East":
                        letterTemplate = $"TL_OR_L{letterNumber}_S";
                        break;
                    case "Wales & West":
                        letterTemplate = $"TL_OR_L{letterNumber}_W";
                        break;
                    default:
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-O03]")
                           .Replace("$Openreach$", openreachNumber);
                        errorDetails = ZohoConstants.CUSTOM_POL_O03;
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        break;
                }

                bool accountError = false;
                bool contactError = false;
                var accountLetters = new List<OR_AccountLetter>();
                var contactLetters = new List<OR_ContactLetter>();
                var allContacts = new List<string>();
                var allAccounts = new List<string>();

                if (!sendEmailToContact)
                {
                    // STEP 2: Get Related Owner Linking
                    var distinctOwnerIds = new List<string>();
                    // Step 2.1: Get Letter Reference by Openreach Number
                    bool sendLetterBefore = false;
                    var letterReference = await _trenchesRepository
                        .GetLetterReferenceByOpenreachNumber(openreachNumber);

                    if (!string.IsNullOrEmpty(letterReference) && openreachNumber != letterReference)
                    {
                        return apiResult;
                    }

                    if (!string.IsNullOrEmpty(letterReference))
                    {
                        sendLetterBefore = true;
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

                    // STEP 3: Handle Account and Contact

                    if (distinctOwnerIds.Count() == 0)
                    {
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-O02]")
                           .Replace("$Openreach$", openreachNumber);
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_O02;
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }

                    foreach (string ownerId in distinctOwnerIds)
                    {
                        var ownerLinkings = await _trenchesRepository.GetOROwnerLinkingsByOwnerId(ownerId);
                        var firstLinking = ownerLinkings.FirstOrDefault();
                        string ownerType = firstLinking.OwnerType;

                        // Step 2.2: Handle Owner Address
                        string ownerAddress = string.Empty;
                        if (ownerType == "C")
                        {
                            var accountLetter = new OR_AccountLetter();

                            var getAccountByIdResult = await _zohoAccountService.GetAccountById(letterRequest.crmKey, firstLinking.OwnerId);
                            if (getAccountByIdResult.Code != ResultCode.OK)
                            {
                                // TODO: Send Email to help@roxus.io, cc the requester to check
                            }
                            var getAccountByIdResponse = getAccountByIdResult.Data;
                            string accountId = getAccountByIdResponse.id;
                            string accountUrl = ZohoConstants.TRENCHES_ZCRM_ACCOUNT_PREFIX_URL + "/" + accountId;

                            string accountName = getAccountByIdResponse.Account_Name;
                            string accountNumber = getAccountByIdResponse.Company_Registration;
                            string street = getAccountByIdResponse.Billing_Street;
                            string city = getAccountByIdResponse.Billing_City;
                            string state = getAccountByIdResponse.Billing_State;
                            string postCode = getAccountByIdResponse.Billing_Code;
                            string country = getAccountByIdResponse.Billing_Country;

                            if (string.IsNullOrEmpty(street) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(postCode))
                            {
                                accountError = true;
                                emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-AC01]")
                                    .Replace("$Openreach$", openreachNumber);
                                errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_AC01.Replace("{AccountUrl}", accountUrl);
                            }
                            if (street.Length > 50 || city.Length > 50)
                            {
                                accountError = true;
                                emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-AC02]")
                                    .Replace("$Openreach$", openreachNumber);
                                errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_AC02.Replace("{AccountUrl}", accountUrl);
                            }
                            if (accountName.Length > 50)
                            {
                                accountError = true;
                                emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-AC03]")
                                    .Replace("$Openreach$", openreachNumber);
                                errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_AC03.Replace("{AccountUrl}", accountUrl);
                            }

                            if (string.IsNullOrEmpty(state))
                            {
                                accountLetter.Custom7 = $"{street}, {city}, {postCode}";
                            }
                            else
                            {
                                accountLetter.Custom7 = $"{street}, {city}, {state}, {postCode}";
                            }

                            if (!string.IsNullOrEmpty(country))
                            {
                                accountLetter.Address5 = country;
                                accountLetter.Custom7 += $", {country}";
                            }

                            accountLetter.AccountId = accountId;
                            accountLetter.MailingName = $"OR - {openreachLocation} - {openreachNumber}";
                            accountLetter.MailingDescription = $"Mail triggered by Roxus Robot for {openreachNumber}, send to: {accountName}";
                            accountLetter.FirstName = "The";
                            accountLetter.SurName = "Directors";
                            accountLetter.FullName = "The Directors";
                            accountLetter.Address1 = street;
                            accountLetter.Address2 = city;
                            accountLetter.Address3 = state;
                            accountLetter.Address4 = postCode;
                            accountLetter.CompanyName = accountName;
                            accountLetter.Custom1 = openreachNumber;
                            accountLetter.Custom2 = firstLinking.PropertyAddress;
                            accountLetter.Custom9 = accountName;
                            accountLetter.Custom10 = accountNumber;

                            if (!string.IsNullOrEmpty(accountNumber) && !accountNumber.EndsWith("R", StringComparison.InvariantCultureIgnoreCase))
                            {
                                accountLetter.AccessAgreementTemplate = DocmailConstants.Trenches_Openreach_CompanyAccessAgreementTemplate;
                            }
                            else
                            {
                                accountLetter.AccessAgreementTemplate = DocmailConstants.Trenches_Openreach_NoRegAccessAgreementTemplate;
                            }

                            accountLetter.LetterTemplate = letterTemplate;

                            accountLetter.OuterEnvelope = DocmailConstants.Trenches_Openreach_OuterEnvelope;
                            accountLetter.ReplyEnvelope = DocmailConstants.Trenches_Openreach_ReturnEnvelope;

                            // Step 4.2.4: Handle Account Custom fields
                            var custom3Date = DateTimeHelpers.AddDaysForLetter(16);
                            accountLetter.Custom3 = custom3Date.ToString(DocmailConstants.LongDateFormat);
                            if (letterNumber == 2)
                            {
                                accountLetter.Custom4 = DateTimeHelpers.ConvertLetterDateTime(letter1DateStr)
                                    .ToString(DocmailConstants.LongDateFormat);
                            }
                            else if (letterNumber == 3)
                            {
                                accountLetter.Custom4 = DateTimeHelpers.ConvertLetterDateTime(letter1DateStr)
                                    .ToString(DocmailConstants.LongDateFormat);
                                accountLetter.Custom5 = DateTimeHelpers.ConvertLetterDateTime(letter2DateStr)
                                    .ToString(DocmailConstants.LongDateFormat);
                            }

                            var accountLinkings = new List<OwnerLinking>();
                            foreach (var ownerLinking in ownerLinkings)
                            {
                                var linking = new OwnerLinking
                                {
                                    OpenreachReference = ownerLinking.OpenreachNumber,
                                    PropertyAddress = ownerLinking.PropertyAddress,
                                    TitleNumber = ownerLinking.TitleNumber
                                };
                                distinctORNumbers.Add(ownerLinking.OpenreachNumber);
                                accountLinkings.Add(linking);
                            }
                            accountLetter.OwnerLinkings = accountLinkings;
                            accountLetters.Add(accountLetter);
                        }
                    }
                }
                else
                {

                    // STEP 3: Get all Related Titles of SM
                    var getRelatedTitlesResult = await _zohoOpenreachService
                        .GetOpenreachRelatedTitles(letterRequest.crmKey, openreachId);

                    if (getRelatedTitlesResult.Code != ResultCode.OK)
                    {
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-RT01]")
                            .Replace("$Openreach$", openreachNumber);
                        errorDetails += $"<br/>{ZohoConstants.CUSTOM_POL_RT01.Replace("{OpenreachUrl}", openreachUrl)}";
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", ZohoConstants.CUSTOM_POL_RT01);
                        await EmailHelpers.SendEmail(emailContent);
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
                            .GetTitleById(letterRequest.crmKey, titleId);

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

                    if (allContacts.Count == 0)
                    {
                        emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-TT06]")
                            .Replace("$Openreach$", openreachNumber);
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_TT06.Replace("{TitleUrl}", titleUrl);
                    }

                    if (!string.IsNullOrEmpty(errorDetails))
                    {
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }

                    var allContactAddresses = new List<string>();
                    var allContactFullNames = new List<string>();
                    var allContactCustom1 = new List<string>();
                    var allContactIds = new List<string>();

                    foreach (var contactId in allContacts)
                    {
                        string contactUrl = $"{ZohoConstants.TRENCHES_ZCRM_CONTACT_PREFIX_URL}/{contactId}";
                        string contactCustom1 = openreachNumber;
                        var getContactByIdResult = await _zohoContactService.GetContactById(letterRequest.crmKey, contactId);
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
                        if (string.IsNullOrWhiteSpace(conMailingStreet) || string.IsNullOrWhiteSpace(conMailingCity)
                            || string.IsNullOrWhiteSpace(conMailingCode))
                        {
                            emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-CO01]")
                                .Replace("$Openreach$", openreachNumber);
                            errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_CO01.Replace("{ContactUrl}", contactUrl);
                            contactError = true;
                            emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                            await EmailHelpers.SendEmail(emailContent);
                            return apiResult;
                        }
                        int conStreetLength = conMailingStreet.Length;
                        int conCityLength = conMailingCity.Length;
                        int conCodeLength = conMailingCode.Length;
                        if (conStreetLength > 50 || conCityLength > 50 || conCodeLength > 50)
                        {
                            emailContent.Subject = emailContent.Subject.Replace("$ErrorCode$", "[POL-CO02]")
                                .Replace("$Openreach$", openreachNumber);
                            errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_CO02.Replace("{ContactUrl}", contactUrl);
                            contactError = true;
                            emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                            await EmailHelpers.SendEmail(emailContent);
                            return apiResult;
                        }
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
                        if (contactFirstName.Contains("The", StringComparison.InvariantCultureIgnoreCase) && 
                            contactLastName.Contains("Occupier", StringComparison.InvariantCultureIgnoreCase))
                        {
                            contactLetter.Custom9 = "The Freeholder/s";
                        }

                        contactLetter.MailingName = $"OR - {openreachLocation} - {openreachNumber}";
                        contactLetter.MailingDescription = $"Mail triggered by Roxus Robot for Openreach Number: {openreachNumber}, send to Contact: {contactLetter.FullName}";
                        // Step 4.3.2: Handle Template
                        contactLetter.OuterEnvelope = DocmailConstants.Trenches_Openreach_OuterEnvelope;
                        contactLetter.LetterTemplate = letterTemplate;
                        contactLetter.AccessAgreementTemplate = DocmailConstants.Trenches_Openreach_PrivateAccessAgreementTemplate;
                        contactLetter.ReplyEnvelope = DocmailConstants.Trenches_Openreach_ReturnEnvelope;
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

                        // Step 4.3.4: Handle Contact Custom Fields
                        contactLetter.Custom1 = allContactCustom1[i];
                        contactLetter.Custom2 = openreachDetails.Address;
                        var custom3Date = DateTimeHelpers.AddDaysForLetter(16);
                        contactLetter.Custom3 = custom3Date.ToString(DocmailConstants.LongDateFormat);
                        if (letterNumber == 2)
                        {
                            contactLetter.Custom4 = DateTimeHelpers.ConvertLetterDateTime(letter1DateStr)
                                .ToString(DocmailConstants.LongDateFormat);
                        }
                        else if (letterNumber == 3)
                        {
                            contactLetter.Custom4 = DateTimeHelpers.ConvertLetterDateTime(letter1DateStr)
                                .ToString(DocmailConstants.LongDateFormat);
                            contactLetter.Custom5 = DateTimeHelpers.ConvertLetterDateTime(letter2DateStr)
                                .ToString(DocmailConstants.LongDateFormat);
                        }
                        contactLetter.Custom10 = string.Empty;
                        contactLetters.Add(contactLetter);
                    }

                    if (accountError || contactError)
                    {
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }

                }

                // STEP 4: Process Letter
                // Step 4.1: Initialize Process Letter
                orLetterRequest.TaskUrl = taskUrl;
                orLetterRequest.ProjectId = projectId;
                orLetterRequest.TasklistId = tasklistId;
                orLetterRequest.TaskId = taskId;
                orLetterRequest.LetterNumber = letterNumber;
                orLetterRequest.CustomerId = DocmailConstants.TrenchesCustomerId;
                orLetterRequest.Environment = "PRODUCTION";
                orLetterRequest.MailingStatus = DocmailConstants.DefaultMailingStatus;
                orLetterRequest.OpenreachId = openreachId;
                orLetterRequest.OpenreachNumber = openreachNumber;
                orLetterRequest.AccountData = accountLetters;
                orLetterRequest.ContactData = contactLetters;

                // STEP 5: Process send Openreach letter
                string requestBody = JsonConvert.SerializeObject(orLetterRequest);
                string uploadEndpoint = RoxusEndpointConstants.Docmail_SendOpenreachLetter_PRODUCTION;
                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    uploadEndpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {RoxusEndpointConstants.Docmail_BasicAuth_PRODUCTION}");
                using (var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();
                    // Convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        errorDetails += "<br>" + ZohoConstants.CUSTOM_PSL_ERROR;
                        emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                        await EmailHelpers.SendEmail(emailContent);
                        return apiResult;
                    }
                }
                httpClient.DefaultRequestHeaders.Clear();

                // STEP 6: Update Task Letter Status and Message
                var taskForUpdation = new TaskForUpdation();
                taskForUpdation.end_date = DateTimeHelpers.AddDaysForLetter(16).ToString("MM-dd-yyyy");
                var letterSendDate = DateTimeHelpers.AddDaysForLetter(1).ToString("MM-dd-yyyy");
                switch (letterNumber)
                {
                    case 1:
                        taskForUpdation.UDF_DATE1 = letterSendDate;
                        taskForUpdation.UDF_CHAR4 = $"Letter {letterNumber} sent SUCCESSFULLY";
                        taskForUpdation.UDF_CHAR66 = openreachNumber;
                        taskForUpdation.custom_status = ZohoConstants.ZPRJ_STATUS_LETTER1;
                        break;
                    case 2:
                        taskForUpdation.UDF_DATE2 = letterSendDate;
                        taskForUpdation.UDF_CHAR5 = $"Letter {letterNumber} sent SUCCESSFULLY";
                        taskForUpdation.UDF_CHAR66 = openreachNumber;
                        taskForUpdation.custom_status = ZohoConstants.ZPRJ_STATUS_LETTER2;
                        break;
                    case 3:
                        taskForUpdation.UDF_DATE3 = letterSendDate;
                        taskForUpdation.UDF_CHAR6 = $"Letter {letterNumber} sent SUCCESSFULLY";
                        taskForUpdation.UDF_CHAR66 = openreachNumber;
                        taskForUpdation.custom_status = ZohoConstants.ZPRJ_STATUS_LETTER3;
                        break;
                    default:
                        break;
                }

                Thread.Sleep(5000);

                var updateTaskResult = await _zohoTaskService.UpdateTaskForTrenches(letterRequest.projectKey, projectId, taskId, taskForUpdation);
                if (updateTaskResult.Code != ResultCode.OK)
                {
                    errorDetails += "<br>" + ZohoConstants.CUSTOM_POL_T04;
                    emailContent.Body = emailBody.Replace("$ErrorDetails$", errorDetails);
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }

                // STEP 7: Update Related SM to same status
                foreach (string orNumber in distinctORNumbers)
                {
                    if (orNumber == openreachNumber)
                    {
                        continue;
                    }
                    // Step 7.1: Search Task using task name
                    string relatedTaskName = prefixTaskName + "-" + orNumber;
                    var searchTaskResult = await _zohoTaskService.SearchTasksInProjects(letterRequest.projectKey, projectId, relatedTaskName);
                    if (searchTaskResult.Code != ResultCode.OK || searchTaskResult.Data.tasks_count == 0)
                    {
                        continue;
                    }

                    var searchTaskDetails = searchTaskResult.Data.tasks[0];
                    string searchTaskId = searchTaskDetails.id.ToString();

                    var trenchesTaskForUpdation = new TaskForUpdation()
                    {
                        end_date = DateTimeHelpers.AddDaysForLetter(16).ToString("MM-dd-yyyy"),

                    };
                    switch (letterNumber)
                    {
                        case 1:
                            trenchesTaskForUpdation.UDF_DATE1 = letterSendDate;
                            trenchesTaskForUpdation.UDF_CHAR4 = $"Letter {letterNumber} sent SUCCESSFULLY";
                            trenchesTaskForUpdation.UDF_CHAR66 = openreachNumber;
                            trenchesTaskForUpdation.custom_status = ZohoConstants.ZPRJ_STATUS_LETTER1;
                            break;
                        case 2:
                            trenchesTaskForUpdation.UDF_DATE2 = letterSendDate;
                            trenchesTaskForUpdation.UDF_CHAR5 = $"Letter {letterNumber} sent SUCCESSFULLY";
                            trenchesTaskForUpdation.UDF_CHAR66 = openreachNumber;
                            trenchesTaskForUpdation.custom_status = ZohoConstants.ZPRJ_STATUS_LETTER2;
                            break;
                        case 3:
                            trenchesTaskForUpdation.UDF_DATE3 = letterSendDate;
                            trenchesTaskForUpdation.UDF_CHAR6 = $"Letter {letterNumber} sent SUCCESSFULLY";
                            trenchesTaskForUpdation.UDF_CHAR66 = openreachNumber;
                            trenchesTaskForUpdation.custom_status = ZohoConstants.ZPRJ_STATUS_LETTER3;
                            break;
                        default:
                            break;
                    }

                    Thread.Sleep(5000);

                    var updateRelatedTaskResult = await _zohoTaskService
                        .UpdateTaskForTrenches(letterRequest.projectKey, projectId, searchTaskId, trenchesTaskForUpdation);
                }

                // STEP 8: Upsert Openreach Task to table
                bool existed = false;
                var orTask = await _trenchesRepository.GetORTaskByTaskId(taskId);
                if (orTask == null)
                {
                    orTask = new ORTask
                    {
                        TaskId = taskId,
                        ProjectId = projectId,
                        TasklistId = tasklistId,
                        Created = DateTime.UtcNow,
                    };
                }
                else
                {
                    existed = true;
                    orTask.Modified = DateTime.UtcNow;
                }

                orTask.DueDate = DateTimeHelpers.AddDaysForLetter(16);
                orTask.LetterReference = openreachNumber;
                switch (letterNumber)
                {
                    case 1:
                        orTask.Letter1Date = DateTimeHelpers.AddDaysForLetter(1);
                        orTask.Letter1Status = $"Letter {letterNumber} sent SUCCESSFULLY";
                        break;
                    case 2:
                        orTask.Letter2Date = DateTimeHelpers.AddDaysForLetter(1);
                        orTask.Letter2Status = $"Letter {letterNumber} sent SUCCESSFULLY";
                        break;
                    case 3:
                        orTask.Letter3Date = DateTimeHelpers.AddDaysForLetter(1);
                        orTask.Letter3Status = $"Letter {letterNumber} sent SUCCESSFULLY";
                        break;
                    default:
                        break;
                }
                if (existed)
                {
                    await _trenchesRepository.CreateORTask(orTask);
                }
                else
                {
                    await _trenchesRepository.UpdateORTask(orTask);
                }

                // await OR_SyncFromTaskToCRM(projectId, taskId);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_POL_SUCCESS;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message}\n{ex.StackTrace}";
                emailContent.Body = $"{errorDetails}<br>{ex.Message}<br>{ex.StackTrace}";
                emailContent.Subject = $"Web Hook FAILED: {taskUrl}";
                // await EmailHelpers.SendEmail(emailContent);
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> OR_SyncFromTaskToCRM(string projectId, string taskId)
        {
            var apiResult = new ApiResultDto<string>() { 
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_STO_400
            };

            try
            {
                string crmApiKey = "dHJlbmNoZXNsYXc6em9ob2NybQ==";
                string projectApiKey = "dHJlbmNoZXNsYXc6b3BlbnJlYWNoem9ob3Byb2plY3Rz";

                // STEP 1: Get Task Details
                Thread.Sleep(5000);
                var getTaskDetailsResponse = await _zohoTaskService.GetTaskDetails(projectApiKey, projectId, taskId);

                if (getTaskDetailsResponse.Code != ResultCode.OK)
                {
                    getTaskDetailsResponse.Message = ZohoConstants.CUSTOM_STO_T01;
                    return apiResult;
                }

                // STEP 2: Extract Task Details
                var taskDetails = getTaskDetailsResponse.Data;
                string taskName = taskDetails.name;
                string taskStatus = taskDetails.status.name;
                string taskStatusId = taskDetails.status.id;
                var taskCustomFields = taskDetails.custom_fields;
                var lastUpdatedTime = taskDetails.last_updated_time;
                var taskDueDate = taskDetails.end_date;
                string statusDate = string.Empty;
                string endDate = string.Empty;

                if (!string.IsNullOrEmpty(lastUpdatedTime))
                {
                    var timeSplits = lastUpdatedTime.Split("-");
                    statusDate = $"{timeSplits[2]}-{timeSplits[0]}-{timeSplits[1]}";
                }

                if (!string.IsNullOrEmpty(taskDueDate))
                {
                    var timeSplits = taskDueDate.Split("-");
                    endDate = $"{timeSplits[2]}-{timeSplits[0]}-{timeSplits[1]}";
                }

                if (!taskName.StartsWith("OR"))
                {
                    apiResult.Message = ZohoConstants.CUSTOM_STO_T02;
                    return apiResult;
                }

                var taskNameSplits = taskName.Split("-", 2);
                string openreachNumber = taskNameSplits[1];

                string letterReference = string.Empty;
                string holdReason = string.Empty;
                foreach (var customField in taskCustomFields)
                {
                    string columnName = customField.column_name;
                    string value = customField.value;
                    if (columnName == "UDF_CHAR66")
                    {
                        letterReference = value;
                    }
                    else if (columnName == "UDF_MULTI2")
                    {
                        holdReason = value;
                    }
                }

                // STEP 3: Get all the Letter References if letter Reference if not empty
                if (string.IsNullOrEmpty(letterReference))
                {
                    letterReference = openreachNumber;
                }

                var ownerLinkings = await _trenchesRepository.GetOROwnerLinkingsByLetterReference(letterReference);
                var orSet = new HashSet<string>();

                orSet.Add(openreachNumber);

                foreach (var linking in ownerLinkings)
                {
                    orSet.Add(linking.OpenreachNumber);
                }

                foreach (var orNumber in orSet)
                {
                    Thread.Sleep(2000);
                    var searchORResponse = await _zohoOpenreachService.SearchOpenreachByOpenreachNumber(crmApiKey, orNumber);

                    if (searchORResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = ZohoConstants.CUSTOM_STO_T02;
                        return apiResult;
                    }

                    var orDetails = searchORResponse.Data.data.FirstOrDefault();
                    string enablementPatch = orDetails.Enablement_Patch_v2;
                    string orProjectId = string.Empty;

                    switch (enablementPatch)
                    {

                        case "London":
                            orProjectId = "85664000003027061";
                            break;

                        case "Midlands":
                            orProjectId = "85664000003019985";
                            break;

                        case "North":
                            orProjectId = "85664000003021915";
                            break;

                        case "South & East":
                            orProjectId = "85664000003021967";
                            break;

                        case "Wales & West":
                            orProjectId = "85664000003018869";
                            break;

                        default:
                            break;
                    }

                    string orId = orDetails.id;
                    var orForUpdate = new OpenreachForUpdate();

                    if (orForUpdate.Task_Status != ZohoConstants.ZPRJ_STATUS_COMPLETE)
                    {
                        orForUpdate.Task_Status = taskStatus;
                    }

                   
                    if (!string.IsNullOrEmpty(holdReason))
                    {
                        orForUpdate.Hold_Reason = holdReason;
                        if (!string.IsNullOrEmpty(statusDate))
                        {
                            orForUpdate.Hold_Date = statusDate;
                        }
                        else
                        {
                            orForUpdate.Hold_Date = DateTime.UtcNow.ToString("yyyy-MM-dd");
                        }
                    }

                    if (!string.IsNullOrEmpty(statusDate))
                    {
                        orForUpdate.Status_Date = statusDate;
                    }
                    else
                    {
                        orForUpdate.Status_Date = DateTime.UtcNow.ToString("yyyy-MM-dd");
                    }

                    if (!string.IsNullOrEmpty(endDate))
                    {
                        orForUpdate.Due_Date = endDate;
                    }

                    if (taskStatus == "Complete")
                    {
                        if (!string.IsNullOrEmpty(statusDate))
                        {
                            orForUpdate.Complete_Date = statusDate;
                        }
                        else
                        {
                            orForUpdate.Complete_Date = DateTime.UtcNow.ToString("yyyy-MM-dd");
                        }
                    }
                    var updateOpenreachResponse = await _zohoOpenreachService.UpdateOpenreach(crmApiKey, orId, orForUpdate);
                    if (updateOpenreachResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = ZohoConstants.CUSTOM_STO_O02;
                        return apiResult;
                    }

                    var searchTaskResult = await _zohoTaskService.SearchTasksInProjects(projectApiKey, orProjectId, orNumber);

                    if (searchTaskResult.Code == ResultCode.OK && searchTaskResult.Data.tasks_count > 0)
                    {
                        var searchTaskDetails = searchTaskResult.Data.tasks[0];
                        string searchTaskId = searchTaskDetails.id.ToString();

                        var taskForUpdation = new TaskForUpdation
                        {
                            custom_status = taskStatusId
                        };

                        var updateRelatedTaskResult = await _zohoTaskService
                            .UpdateTaskForTrenches(projectApiKey, orProjectId, searchTaskId, taskForUpdation);
                    }

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = "Update Related Openreach status successfully";
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                throw;
            }
        }

        public async Task<ApiResultDto<string>> OR_MassSyncFromTaskToCRM()
        {
            OleDbConnection connection;
            OleDbCommand command;
            OleDbDataReader dr;
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            int count = 2;

            try
            {
                string commandText = "SELECT * FROM [All Projects$]";
                string inputFullPath = @"D:\1. Roxus\2. Projects\1. Trenches\8. Openreach\Update Due Date\task_export_20230111.xlsx";
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{inputFullPath}';Extended Properties=\"Excel 12.0;HDR=YES;\"";

                using (connection = new OleDbConnection(connectionString))
                {
                    command = new OleDbCommand(commandText, connection);
                    connection.Open();
                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        if (count < 4)
                        {
                            count++;
                            continue;
                        }

                        string projectId = dr["ProjectId"].ToString();
                        string taskId = dr["TaskId"].ToString();

                        var syncTaskResult = await OR_SyncFromTaskToCRM(projectId, taskId);
                        count++;
                        // break;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = "Sync Openreach from CRM to DB SUCCESSFULLY";
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> OR_SyncFromCRMToDB(string openreachId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_SCD_400
            };

            string emailSubject = "[Openreach] {OpenreachNumber} Related Title / Related Account Updated";
            string emailBody = "Dear Roxus Support,<br><br>Please check the changes below for {OpenreachNumber}" +
                "<br> Old Related Titles: {OldRelatedTitles} - New Related Titles: {NewRelatedTitles}" +
                "<br> Old Related Account: {OldRelatedAccounts} - New Related Accounts: {NewRelatedAccounts}" +
                "<br><br> Best Regards,<br> Roxus Automation";

            var emailContent = new EmailContent()
            {
                Email = EmailConstants.TrenchesEmail,
                Body = string.Empty,
                Clients = "help@roxus.io",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Outlook_Email_SmtpServer
            };
            var appConfiguration = await _roxusLoggingRepository.GetAppConfigurationById(EmailConstants.TrenchesId);
            emailContent.Password = appConfiguration.Password;

            try
            {
                Thread.Sleep(5000);
                string crmApiKey = "dHJlbmNoZXNsYXc6em9ob2NybQ==";
                string projectApiKey = "dHJlbmNoZXNsYXc6em9ob3Byb2plY3Rz";

                // STEP 1: Get Openreach Details
                var getOpenreachResponse = await _zohoOpenreachService.GetOpenreachById(crmApiKey, openreachId);
                if (getOpenreachResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_SCD_O01;
                    return apiResult;
                }

                // STEP 2: Update Zoho Projects with basic fields: Contact Type, Wayleave Received, RA Received,
                // Asbestos Form Response, Asbestos Register Received, Survey Status
                var openreachDetails = getOpenreachResponse.Data;
                string openreachNumber = openreachDetails.Openreach_SM;
                string contactType = openreachDetails.Contact_Type;
                string wayleaveReceived = openreachDetails.Wayleave_Received;
                string wayleaveReceivedDate = openreachDetails.Wayleave_Date;
                string routeApprovalStatus = openreachDetails.Route_Approval;
                string routeApprovalDate = openreachDetails.RA_Received_Date;
                string asbestosResponseDetail = openreachDetails.Asbestos_Response_Detail;
                var asbestosRegisterStatus = openreachDetails.Asbestos_Register;
                string asbestosRegisterDate = openreachDetails.Asbestos_Received_Date;
                string surveyStatus = openreachDetails.Survey;
                string automatedUrl = openreachDetails.Automated;
                string hybridUrl = openreachDetails.Hybrid;
                string digitalUrl = openreachDetails.E_Sending;
                long? dataPhase = openreachDetails.Data_Phase;

                // STEP 2.1: Get Task URL
                var taskForUpdate = new TaskForUpdation();

                // STEP 2.2: Update DB
                var primaryData = await _trenchesRepository.GetORPrimaryDataByOpenreachNumber(openreachNumber);

                if (!string.IsNullOrEmpty(contactType))
                {
                    taskForUpdate.UDF_CHAR11 = contactType;
                    primaryData.Contact_Type = contactType;
                }
                if (!string.IsNullOrEmpty(wayleaveReceived))
                {
                    if (wayleaveReceived == "Received")
                    {
                        taskForUpdate.UDF_BOOLEAN4 = "true";
                    }
                    else
                    {
                        taskForUpdate.UDF_BOOLEAN4 = "false";
                    }
                    primaryData.Wayleave_Received = wayleaveReceived;
                }
                if (!string.IsNullOrEmpty(wayleaveReceivedDate))
                {
                    taskForUpdate.UDF_CHAR50 = wayleaveReceivedDate;
                    primaryData.Wayleave_Date = DateTime.ParseExact(wayleaveReceivedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(routeApprovalStatus))
                {
                    switch (routeApprovalStatus)
                    {
                        case "Not Required":
                            taskForUpdate.UDF_CHAR67 = "Not Required";
                            break;
                        case "Received":
                            taskForUpdate.UDF_CHAR67 = "Yes";
                            break;
                        case "Sent to OR":
                            taskForUpdate.UDF_CHAR67 = "Yes";
                            break;
                        default:
                            taskForUpdate.UDF_CHAR67 = "No";
                            break;
                    }
                    primaryData.Route_Approval = routeApprovalStatus;
                }
                if (!string.IsNullOrEmpty(routeApprovalDate))
                {
                    taskForUpdate.UDF_CHAR68 = routeApprovalDate;
                    primaryData.RA_Date = DateTime.ParseExact(routeApprovalDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(asbestosResponseDetail))
                {
                    if (asbestosResponseDetail.StartsWith("Constructed post-2000"))
                    {
                        taskForUpdate.UDF_CHAR58 = "Constructed post-2000 and no asbestos";
                    }
                    else if (asbestosResponseDetail.StartsWith("Refurbished post-2000"))
                    {
                        taskForUpdate.UDF_CHAR58 = "Refurbished post-2000 asbestos removed";
                    }
                    else if (asbestosResponseDetail.StartsWith("Asbestos Management Survey report"))
                    {
                        taskForUpdate.UDF_CHAR58 = "Asbestos Management Survey provided";
                    }
                    primaryData.Asbestos_Response_Detail = asbestosResponseDetail;
                }
                if (!string.IsNullOrEmpty(asbestosRegisterDate))
                {
                    taskForUpdate.UDF_CHAR64 = asbestosRegisterDate;
                    primaryData.Asbestos_Reveiced_Date = DateTime.ParseExact(asbestosRegisterDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                if (dataPhase.HasValue)
                {
                    taskForUpdate.UDF_LONG10 = dataPhase;
                }
                if (asbestosRegisterStatus != null && asbestosRegisterStatus.Length > 0)
                {
                    foreach (var asbestosStatus in asbestosRegisterStatus)
                    {
                        if (asbestosStatus == "Received" || asbestosStatus == "Sent to OR")
                        {
                            taskForUpdate.UDF_CHAR69 = "Yes";
                            break;
                        }
                        if (asbestosStatus == "Not Required")
                        {
                            taskForUpdate.UDF_CHAR69 = "Not Required";
                            break;
                        }
                    }

                    primaryData.Asbestos_Register = string.Join(",", asbestosRegisterStatus); 
                }
                await _trenchesRepository.UpdatePrimaryData(primaryData);

                // STEP 2.3: Update Task 
                if (!string.IsNullOrEmpty(digitalUrl))
                {
                    var urlSplits = digitalUrl.Split("/");
                    int length = urlSplits.Length;
                    string taskId = urlSplits[length - 1];
                    string projectId = urlSplits[length - 3];
                    var updateTaksResponse = await _zohoTaskService.UpdateTaskForTrenches(projectApiKey, projectId, taskId, taskForUpdate);
                }

                if (!string.IsNullOrEmpty(hybridUrl))
                {
                    var urlSplits = hybridUrl.Split("/");
                    int length = urlSplits.Length;
                    string taskId = urlSplits[length - 1];
                    string projectId = urlSplits[length - 3];
                    var updateTaksResponse = await _zohoTaskService.UpdateTaskForTrenches(projectApiKey, projectId, taskId, taskForUpdate);
                }

                if (!string.IsNullOrEmpty(automatedUrl))
                {
                    var urlSplits = automatedUrl.Split("/");
                    int length = urlSplits.Length;
                    string taskId = urlSplits[length - 1];
                    string projectId = urlSplits[length - 3];
                    var updateTaksResponse = await _zohoTaskService.UpdateTaskForTrenches(projectApiKey, projectId, taskId, taskForUpdate);
                }

                // STEP 3.1: Get Openreach Related Titles
                var crmTitleSet = new HashSet<string>();
                var getRelatedTitlesResponse = await _zohoOpenreachService.GetOpenreachRelatedTitles(crmApiKey, openreachId);
                if (getRelatedTitlesResponse.Code == ResultCode.OK)
                {
                    var relatedTitles = getRelatedTitlesResponse.Data;
                    foreach (var title in relatedTitles.data)
                    {
                        if (title.Related_Freehold_Titles != null)
                        {
                            crmTitleSet.Add(title.Related_Freehold_Titles.name);
                        }
                    }
                }
                crmTitleSet = crmTitleSet.OrderBy(t => t).ToHashSet();
                string newTitles = string.Join(",", crmTitleSet);

                // STEP 3.2: Get Openreach Related Accounts
                var crmAccountSet = new HashSet<string>();
                var getRelatedAccountsResponse = await _zohoOpenreachService.GetOpenreachRelatedAccount(crmApiKey, openreachId);
                if (getRelatedAccountsResponse.Code == ResultCode.OK)
                {
                    var relatedAccounts = getRelatedAccountsResponse.Data;
                    foreach (var account in relatedAccounts.data)
                    {
                        if (account.Related_Account_Owner != null)
                        {
                            crmAccountSet.Add(account.Related_Account_Owner.id);
                        }
                    }
                }
                crmAccountSet = crmAccountSet.OrderBy(a => a).ToHashSet();
                string newAccounts = string.Join(",", crmAccountSet);

                // STEP 3.3: Get Owner Linkings for this SM
                var dbTitlesSet = new HashSet<string>();
                var dbAccountsSet = new HashSet<string>();
                var ownerLinkings = await _trenchesRepository.GetOROwnerLinkingsByOpenreachNumber(openreachNumber);

                foreach (var linking in ownerLinkings)
                {
                    string titleNumber = linking.TitleNumber;
                    dbTitlesSet.Add(titleNumber);

                    string ownerType = linking.OwnerType;
                    if (ownerType == "C")
                    {
                        string accountId = linking.OwnerId;
                        dbAccountsSet.Add(accountId);
                    }
                }
                dbTitlesSet = dbTitlesSet.OrderBy(t => t).ToHashSet();
                string oldTitles = string.Join(",", dbTitlesSet);
                dbAccountsSet = dbAccountsSet.OrderBy(a => a).ToHashSet();
                string oldAccounts = string.Join(",", dbAccountsSet);

                if (oldAccounts != newAccounts)
                {
                    emailSubject = emailSubject.Replace("{OpenreachNumber}", openreachNumber);
                    emailBody = emailBody.Replace("{OldRelatedTitles}", oldTitles)
                                               .Replace("{NewRelatedTitles}", newTitles)
                                               .Replace("{OldRelatedAccounts}", oldAccounts)
                                               .Replace("{NewRelatedAccounts}", newAccounts);

                    emailContent.Subject = emailSubject;
                    emailContent.Body = emailBody;

                    // await EmailHelpers.SendEmail(emailContent);
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_SCD_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> OR_MassSyncFromCRMToDB()
        {
            OleDbConnection connection;
            OleDbCommand command;
            OleDbDataReader dr;
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            string EmailSubject = "[Openreach] {OpenreachNumber} Related Title / Related Account Updated";
            string EmailBody = "Dear Roxus Support,<br><br>Please check the changes below for {OpenreachNumber}" +
                "<br> Old Related Titles: {OldRelatedTitles} - New Related Titles: {NewRelatedTitles}" +
                "<br> Old Related Account: {OldRelatedAccounts} - New Related Accounts: {NewRelatedAccounts}" +
                "<br><br> Best Regards,<br> Roxus Automation";

            var emailContent = new EmailContent()
            {
                Email = EmailConstants.TrenchesEmail,
                Clients = "help@roxus.io",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Outlook_Email_SmtpServer
            };
            var appConfiguration = await _roxusLoggingRepository.GetAppConfigurationById(EmailConstants.TrenchesId);
            emailContent.Password = appConfiguration.Password;

            try
            {
                string commandText = "SELECT * FROM [Openreach$]";
                string inputFullPath = @"D:\1. Roxus\2. Projects\1. Trenches\8. Openreach\Openreach - Copy.xlsx";
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{inputFullPath}';Extended Properties=\"Excel 12.0;HDR=YES;\"";

                string crmApiKey = "dHJlbmNoZXNsYXc6em9ob2NybQ==";
                string projectApiKey = "dHJlbmNoZXNsYXc6em9ob3Byb2plY3Rz";

                var allOwnerLinkings = await _trenchesRepository.GetAllOROwnerLinkings();

                using (connection = new OleDbConnection(connectionString))
                {
                    command = new OleDbCommand(commandText, connection);
                    connection.Open();
                    dr = command.ExecuteReader();

                    int count = 2;

                    while (dr.Read())
                    {
                        string openreachId = dr["Id"].ToString();

                        // STEP 1: Get Openreach Details
                        var getOpenreachResponse = await _zohoOpenreachService.GetOpenreachById(crmApiKey, openreachId);
                        if (getOpenreachResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = ZohoConstants.CUSTOM_SCD_O01;
                            return apiResult;
                        }

                        // STEP 2: Update Zoho Projects with basic fields: Contact Type, Wayleave Received, RA Received,
                        // Asbestos Form Response, Asbestos Register Received, Survey Status
                        var openreachDetails = getOpenreachResponse.Data;

                        string openreachNumber = openreachDetails.Openreach_SM;
                        string contactType = openreachDetails.Contact_Type;
                        string wayleaveReceived = openreachDetails.Wayleave_Received;
                        string wayleaveReceivedDate = openreachDetails.Wayleave_Date;
                        string routeApprovalStatus = openreachDetails.Route_Approval;
                        string routeApprovalDate = openreachDetails.RA_Received_Date;
                        string asbestosResponseDetail = openreachDetails.Asbestos_Response_Detail;
                        var asbestosRegisterStatus = openreachDetails.Asbestos_Register;
                        string asbestosRegisterDate = openreachDetails.Asbestos_Received_Date;
                        string surveyStatus = openreachDetails.Survey;
                        string automatedUrl = openreachDetails.Automated;
                        string hybridUrl = openreachDetails.Hybrid;
                        string digitalUrl = openreachDetails.E_Sending;

                        // STEP 2.1: Get Task URL
                        var taskForUpdate = new TaskForUpdation();

                        // STEP 2.2: Update DB
                        var primaryData = await _trenchesRepository.GetORPrimaryDataByOpenreachNumber(openreachNumber);

                        if (!string.IsNullOrEmpty(contactType))
                        {
                            taskForUpdate.UDF_CHAR11 = contactType;
                            primaryData.Contact_Type = contactType;
                        }
                        if (!string.IsNullOrEmpty(wayleaveReceived))
                        {
                            if (wayleaveReceived == "Received")
                            {
                                taskForUpdate.UDF_BOOLEAN4 = "true";
                            }
                            else
                            {
                                taskForUpdate.UDF_BOOLEAN4 = "false";
                            }
                            primaryData.Wayleave_Received = wayleaveReceived;
                        }
                        if (!string.IsNullOrEmpty(wayleaveReceivedDate))
                        {
                            taskForUpdate.UDF_CHAR50 = wayleaveReceivedDate;
                            primaryData.Wayleave_Date = DateTime.ParseExact(wayleaveReceivedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        if (!string.IsNullOrEmpty(routeApprovalStatus))
                        {
                            switch (routeApprovalStatus)
                            {
                                case "Not Required":
                                    taskForUpdate.UDF_CHAR67 = "Not Required";
                                    break;
                                case "Received":
                                    taskForUpdate.UDF_CHAR67 = "Yes";
                                    break;
                                case "Sent to OR":
                                    taskForUpdate.UDF_CHAR67 = "Yes";
                                    break;
                                default:
                                    taskForUpdate.UDF_CHAR67 = "No";
                                    break;
                            }
                            primaryData.Route_Approval = routeApprovalStatus;
                        }
                        if (!string.IsNullOrEmpty(routeApprovalDate))
                        {
                            taskForUpdate.UDF_CHAR68 = routeApprovalDate;
                            primaryData.RA_Date = DateTime.ParseExact(routeApprovalDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        if (!string.IsNullOrEmpty(asbestosResponseDetail))
                        {
                            if (asbestosResponseDetail.StartsWith("Constructed post-2000"))
                            {
                                taskForUpdate.UDF_CHAR58 = "Constructed post-2000 and no asbestos";
                            }
                            else if (asbestosResponseDetail.StartsWith("Refurbished post-2000"))
                            {
                                taskForUpdate.UDF_CHAR58 = "Refurbished post-2000 asbestos removed";
                            }
                            else if (asbestosResponseDetail.StartsWith("Asbestos Management Survey report"))
                            {
                                taskForUpdate.UDF_CHAR58 = "Asbestos Management Survey provided";
                            }
                            primaryData.Asbestos_Response_Detail = asbestosResponseDetail;
                        }
                        if (!string.IsNullOrEmpty(asbestosRegisterDate))
                        {
                            taskForUpdate.UDF_CHAR64 = asbestosRegisterDate;
                            primaryData.Asbestos_Reveiced_Date = DateTime.ParseExact(asbestosRegisterDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        if (asbestosRegisterStatus != null && asbestosRegisterStatus.Length > 0)
                        {
                            foreach (var asbestosStatus in asbestosRegisterStatus)
                            {
                                if (asbestosStatus == "Received" || asbestosStatus == "Sent to OR")
                                {
                                    taskForUpdate.UDF_CHAR69 = "Yes";
                                    break;
                                }
                                if (asbestosStatus == "Not Required")
                                {
                                    taskForUpdate.UDF_CHAR69 = "Not Required";
                                    break;
                                }
                            }

                            primaryData.Asbestos_Register = string.Join(",", asbestosRegisterStatus);
                        }
                        await _trenchesRepository.UpdatePrimaryData(primaryData);

                        // STEP 2.3: Update Task 
                        if (!string.IsNullOrEmpty(digitalUrl))
                        {
                            var urlSplits = digitalUrl.Split("/");
                            int length = urlSplits.Length;
                            string taskId = urlSplits[length - 1];
                            string projectId = urlSplits[length - 3];
                            var updateTaksResponse = await _zohoTaskService.UpdateTaskForTrenches(projectApiKey, projectId, taskId, taskForUpdate);
                        }

                        if (!string.IsNullOrEmpty(hybridUrl))
                        {
                            var urlSplits = hybridUrl.Split("/");
                            int length = urlSplits.Length;
                            string taskId = urlSplits[length - 1];
                            string projectId = urlSplits[length - 3];
                            var updateTaksResponse = await _zohoTaskService.UpdateTaskForTrenches(projectApiKey, projectId, taskId, taskForUpdate);
                        }

                        if (!string.IsNullOrEmpty(automatedUrl))
                        {
                            var urlSplits = automatedUrl.Split("/");
                            int length = urlSplits.Length;
                            string taskId = urlSplits[length - 1];
                            string projectId = urlSplits[length - 3];
                            var updateTaksResponse = await _zohoTaskService.UpdateTaskForTrenches(projectApiKey, projectId, taskId, taskForUpdate);
                        }

                        // STEP 3.1: Get Openreach Related Titles
                        var crmTitleSet = new HashSet<string>();
                        var getRelatedTitlesResponse = await _zohoOpenreachService.GetOpenreachRelatedTitles(crmApiKey, openreachId);
                        if (getRelatedTitlesResponse.Code == ResultCode.OK)
                        {
                            var relatedTitles = getRelatedTitlesResponse.Data;
                            foreach (var title in relatedTitles.data)
                            {
                                if (title.Related_Freehold_Titles != null)
                                {
                                    crmTitleSet.Add(title.Related_Freehold_Titles.name);
                                }
                            }
                        }
                        crmTitleSet = crmTitleSet.OrderBy(t => t).ToHashSet();
                        string newTitles = string.Join(",", crmTitleSet);

                        // STEP 3.2: Get Openreach Related Accounts
                        var crmAccountSet = new HashSet<string>();
                        var getRelatedAccountsResponse = await _zohoOpenreachService.GetOpenreachRelatedAccount(crmApiKey, openreachId);
                        if (getRelatedAccountsResponse.Code == ResultCode.OK)
                        {
                            var relatedAccounts = getRelatedAccountsResponse.Data;
                            foreach (var account in relatedAccounts.data)
                            {
                                if (account.Related_Account_Owner != null)
                                {
                                    crmAccountSet.Add(account.Related_Account_Owner.id);
                                }
                            }
                        }
                        crmAccountSet = crmAccountSet.OrderBy(a => a).ToHashSet();
                        string newAccounts = string.Join(",", crmAccountSet);

                        // STEP 3.3: Get Owner Linkings for this SM
                        var dbTitlesSet = new HashSet<string>();
                        var dbAccountsSet = new HashSet<string>();
                        var ownerLinkings = allOwnerLinkings.Where(l => l.OpenreachNumber == openreachNumber);

                        foreach (var linking in ownerLinkings)
                        {
                            string titleNumber = linking.TitleNumber;
                            dbTitlesSet.Add(titleNumber);

                            string ownerType = linking.OwnerType;
                            if (ownerType == "C")
                            {
                                string accountId = linking.OwnerId;
                                dbAccountsSet.Add(accountId);
                            }
                        }
                        dbTitlesSet = dbTitlesSet.OrderBy(t => t).ToHashSet();
                        string oldTitles = string.Join(",", dbTitlesSet);
                        dbAccountsSet = dbAccountsSet.OrderBy(a => a).ToHashSet();
                        string oldAccounts = string.Join(",", dbAccountsSet);

                        if (oldAccounts != newAccounts)
                        {
                            string emailSubject = EmailSubject.Replace("{OpenreachNumber}", openreachNumber);
                            string emailBody = EmailBody.Replace("{OpenreachNumber}", openreachNumber)
                                                 .Replace("{OldRelatedTitles}", oldTitles)
                                                 .Replace("{NewRelatedTitles}", newTitles)
                                                 .Replace("{OldRelatedAccounts}", oldAccounts)
                                                 .Replace("{NewRelatedAccounts}", newAccounts);

                            emailContent.Subject = emailSubject;
                            emailContent.Body = emailBody;
                            await EmailHelpers.SendEmail(emailContent);
                        }
                        count++;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = "Sync Openreach from CRM to DB SUCCESSFULLY";
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> OR_MassUpdateTasksToRemoved()
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = ZohoConstants.MSG_400
            };

            OleDbConnection connection;
            OleDbCommand command;
            OleDbDataReader dr;

            try
            {

                string commandText = "SELECT * FROM [Tasks$]";
                string inputFullPath = @"D:\1. Roxus\2. Projects\1. Trenches\8. Openreach\Openreach Phase 3 - Urgent Hold 16.11.22 - TASK STATUSES_-_Removed.xlsx";
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{inputFullPath}';Extended Properties=\"Excel 12.0;HDR=YES;\"";

                string crmApiKey = "dHJlbmNoZXNsYXc6em9ob2NybQ==";
                string projectApiKey = "dHJlbmNoZXNsYXc6em9ob3Byb2plY3Rz";

                using (connection = new OleDbConnection(connectionString))
                {
                    command = new OleDbCommand(commandText, connection);
                    connection.Open();
                    dr = command.ExecuteReader();

                    int count = 2;

                    while (dr.Read())
                    {
                        if (count < 150)
                        {
                            count++;
                            continue;
                        }

                        Thread.Sleep(5000);

                        string taskId = dr["Task ID"].ToString();
                        string projectId = dr["Project ID"].ToString();

                        var taskForUpdate = new TaskForUpdation();
                        taskForUpdate.custom_status = ZohoConstants.ZPRJ_STATUS_REMOVED;
                        taskForUpdate.UDF_MULTI2 = "Openreach - Removed";

                        var updateTaskResponse = await _zohoTaskService.UpdateTaskForTrenches(projectApiKey, projectId, taskId, taskForUpdate);

                        count++;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = "Mass Update Tasks to Removed successfully";

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> HandleTransactionQueue(int queueId)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_HTQ_400
            };

            try
            {

                var transactionQueue = await _roxusLoggingRepository.GetTransactionQueueById(queueId);
                if (transactionQueue == null)
                {
                    return apiResult;
                }

                // if (transactionQueue.Status != 3)
                // {
                //    return apiResult;
                // }

                int transactionType = transactionQueue.TransactionType;
                string projectId = transactionQueue.ProjectId;
                string tasklistId = transactionQueue.TasklistId;
                string taskId = transactionQueue.TaskId;

                var letterRequest = new SendLetterRequest()
                {
                    projectId = projectId,
                    tasklistId = tasklistId,
                    taskId = taskId,
                    crmKey = ZohoConstants.Trenches_CRM_Key,
                    projectKey = ZohoConstants.Trenches_Project_Key
                };

                if (transactionType == 1)
                {
                    
                    var sendORLetterResult = await OR_ProcessSendingLetter(letterRequest);
                    if (sendORLetterResult.Code == ResultCode.OK)
                    {
                        transactionQueue.Status = 1;
                    }
                    else
                    {
                        transactionQueue.Status = 2;
                    }
                }
                else if (transactionType == 2)
                {
                    var sendLetterResult = await ProcessSendingLetter(letterRequest);
                    if (sendLetterResult.Code == ResultCode.OK)
                    {
                        transactionQueue.Status = 1;
                        
                    }
                    else
                    {
                        transactionQueue.Status = 2;
                    }
                }

                transactionQueue.Modified = DateTime.UtcNow;
                await _roxusLoggingRepository.UpdateApiTransactionQueue(transactionQueue);

                apiResult.Message = ZohoConstants.CUSTOM_HTQ_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<string> OR_SyncFromDBToProject()
        {
            OleDbConnection connection;
            OleDbCommand command;
            OleDbDataReader dr;

            try
            {
                string commandText = "SELECT * FROM [Sheet1$]";
                string inputFullPath = @"D:\1. Roxus\2. Projects\1. Trenches\8. Openreach\Openreach_Tasks.xlsx";
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{inputFullPath}';Extended Properties=\"Excel 12.0;HDR=YES;\"";

                string projectApiKey = "dHJlbmNoZXNsYXc6em9ob3Byb2plY3Rz";

                using (connection = new OleDbConnection(connectionString))
                {
                    command = new OleDbCommand(commandText, connection);
                    connection.Open();
                    dr = command.ExecuteReader();

                    int count = 2;
                    var allLinkings = await _trenchesRepository.GetAllOROwnerLinkings();

                    while (dr.Read())
                    {
                        if (count < 3)
                        {
                            count++;
                            continue;
                        }

                        // STEP 1: Get Task Details from Excel
                        string projectId = dr["Project ID"].ToString();
                        string taskId = dr["Task ID"].ToString();
                        string taskName = dr["Task Name"].ToString();
                        string letterReference = dr["Letter Reference"].ToString();

                        if (!string.IsNullOrEmpty(letterReference))
                        {
                            continue;
                        }

                        // STEP 2: Get Letter Reference from DB
                        var taskNameSplits = taskName.Split(new[] { '-' }, 2);
                        string openreachName = taskNameSplits[1];
                        var orLinkings = allLinkings.Where(l => l.OpenreachNumber == openreachName);
                        var orLinking = orLinkings.FirstOrDefault();
                        if (orLinking == null)
                        {
                            continue;
                        }

                        // STEP 3: Update Task Reference 
                        var taskForUpdation = new TaskForUpdation()
                        {
                            UDF_CHAR66 = orLinking.LetterReference
                        };
                        var updateTaskResponse = await _zohoTaskService.UpdateTaskForTrenches(projectApiKey, projectId, taskId, taskForUpdation);
                    }
                }

                return "Sync Letter Reference from DB to Task SUCCESSFULLY";
            }
            catch (Exception ex)
            {
                return $"{ex.Message} - {ex.StackTrace}";
            }
        }

        public async Task<ApiResultDto<string>> InsertTransactionQueue(ApiTransactionQueue transactionQueue)
        {
            await _roxusLoggingRepository.InsertApiTransactionQueue(transactionQueue);
            return new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = ZohoConstants.MSG_200
            };
        }

        public async Task<ApiResultDto<string>> HandleReturnedSM()
        {
            var apiResult = new ApiResultDto<string>() 
            { 
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string crmApiKey = "dHJlbmNoZXNsYXc6em9ob2NybQ==";
                string projectApiKey = "dHJlbmNoZXNsYXc6em9ob3Byb2plY3Rz";

                string removedSMs = File.ReadAllText(@"D:\1. Roxus\2. Projects\1. Trenches\8. Openreach\Update to Remove\Removed SM.txt");

                var removedSMList = removedSMs.Split("\r\n");

                int currentRow = 0;
                foreach (var removedSM in removedSMList)
                {
                    currentRow++;
                    if (currentRow <= 1)
                    {
                        continue;
                    }
                    Thread.Sleep(5000);

                    // STEP 1: Search SM in Zoho CRM
                    var searchSMInCRM = await _zohoOpenreachService.SearchOpenreachByOpenreachNumber(crmApiKey, removedSM);
                    if (searchSMInCRM.Code == ResultCode.OK && searchSMInCRM.Data.data.Length > 0)
                    {
                        var openreachDetails = searchSMInCRM.Data.data[0];
                        string openreachId = openreachDetails.id;
                        var openreachForUpdate = new OpenreachForUpdate()
                        {
                            Data_Phase = 3
                        };
                        var updateOpenreach = await _zohoOpenreachService.UpdateOpenreach(crmApiKey, openreachId, openreachForUpdate);
                    }

                    var openreachSM = await _trenchesRepository.GetORPrimaryDataByOpenreachNumber(removedSM);
                    if (openreachSM != null)
                    {
                        string projectId = string.Empty;
                        string location = openreachSM.Enablement_Patch_v2;
                        switch (location)
                        {
                            case "London":
                                projectId = "85664000003027061";
                                break;
                            case "Wales & West":
                                projectId = "85664000003018869";
                                break;
                            case "North":
                                projectId = "85664000003021915";
                                break;
                            case "Midlands":
                                projectId = "85664000003019985";
                                break;
                            case "South & East":
                                projectId = "85664000003021967";
                                break;
                        }

                        var searchTaskInProject = await _zohoTaskService.SearchTasksInProjects(projectApiKey, projectId, removedSM);
                        if (searchTaskInProject.Code == ResultCode.OK && searchTaskInProject.Data.tasks.Length > 0)
                        {
                            var taskDetails = searchTaskInProject.Data.tasks[0];
                            string taskId = taskDetails.id.ToString();
                            var taskForUpdate = new TaskForUpdation()
                            {
                                custom_status = ZohoConstants.ZPRJ_STATUS_RXSREMOVED,
                                UDF_LONG10 = 3
                            };
                            var updateTaskResponse = await _zohoTaskService.UpdateTaskForTrenches(projectApiKey, projectId, taskId, taskForUpdate);
                        }
                    }

                    // break;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.MSG_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> GetLetterDocumentForOpenreach(string openreachId)
        {

            var apiResult = new ApiResultDto<string>() { 
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_ABN_400
            };

            try
            {

                string zohoApiKey = "dHJlbmNoZXNsYXc6em9ob2NybQ==";

                // STEP 1: Get Openreach Details
                var getOpenreachByIdResult = await _zohoOpenreachService.GetOpenreachById(zohoApiKey, openreachId);

                if (getOpenreachByIdResult.Code != ResultCode.OK)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_ABN_E01;
                    return apiResult;
                }

                // STEP 2: Search Docmail Record(s) using Openreach Id
                var apiResponse = getOpenreachByIdResult.Data;
                string openreachNumber = apiResponse.Openreach_SM;

                var ownerLinkings = await _trenchesRepository.GetOROwnerLinkingsByOpenreachNumber(openreachNumber);
                string letterReference = string.Empty;

                foreach (var linking in ownerLinkings)
                {
                    if (!string.IsNullOrEmpty(linking.LetterReference))
                    {
                        letterReference = linking.LetterReference;
                        break;
                    }
                }

                IEnumerable<DocmailRecord> docmailRecords = new List<DocmailRecord>();

                docmailRecords = await _roxusLoggingRepository
                .GetDocmailRecordByMailingReference(!string.IsNullOrEmpty(letterReference) ? letterReference : openreachNumber);

                if (docmailRecords.Count() == 0)
                {
                    apiResult.Message = "There is no letter document for this record";
                    return apiResult;
                }

                var allLinkings = await _trenchesRepository.GetOROwnerLinkingsByLetterReference(!string.IsNullOrEmpty(letterReference) ? letterReference : openreachId);

                var distinctOpenreachIds = allLinkings.Select(l => l.OpenreachId).Distinct().ToList();

                foreach (var linking in allLinkings)
                {
                    string openId = linking.OpenreachId;
                    foreach (var record in docmailRecords)
                    {

                        // Step 1.1: Handle Note Title and Note Content
                        string noteTitle = record.MailingReference;
                        string noteContent = record.BlobUrl;

                        // Step 1.2: Add Note to Openreach
                        var noteForCreation = new NoteForCreation()
                        {
                            Note_Title = noteTitle,
                            Note_Content = noteContent,
                            Parent_Id = openId,
                            se_module = "Openreach"
                        };
                        var upsertRequest = new UpsertRequest<NoteForCreation>();
                        upsertRequest.data.Add(noteForCreation);
                        var createNoteResult = await _zohoNoteService.CreateNote(zohoApiKey, upsertRequest);

                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_ABN_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> MassUpdateTasks()
        {

            OleDbConnection connection;
            OleDbCommand command;
            OleDbDataReader dr;

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            try
            {

                string commandText = "SELECT * FROM [Sheet1$]";
                string inputFullPath = @"D:\1. Roxus\2. Projects\1. Trenches\8. Openreach\update tasks list.xlsx";
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{inputFullPath}';Extended Properties=\"Excel 12.0;HDR=YES;\"";

                string projectApiKey = "dHJlbmNoZXNsYXc6em9ob3Byb2plY3Rz";

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
                        // if (count <= 2)
                        // {
                        //    continue;
                        // }

                        var taskForUpdation = new TaskForUpdation()
                        {
                            UDF_DATE3 = "2023-02-13",
                            UDF_CHAR6 = "Letter 3 sent SUCCESSFULLY",
                            end_date = "2023-03-01"
                        };

                        string projectId = dr["ProjectId"].ToString();
                        string taskId = dr["TaskId"].ToString();

                        var updateTaskResponse = await _zohoTaskService.UpdateTaskForTrenches(projectApiKey, projectId, taskId, taskForUpdation);

                        break;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.MSG_200;
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
