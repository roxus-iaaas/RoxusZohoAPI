using OfficeOpenXml;
using RoxusWebAPI.Services.Zoho.ZohoProjects;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.Custom;
using RoxusZohoAPI.Models.Zoho.ZohoBooks;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Hinets
{
    public class HinetsCustomService : IHinetsCustomService
    {
        private readonly IHinetsCrmService _hinetsCrmService;
        private readonly IHinetsProjectsService _hinetsProjectsService;
        private readonly IHinetsBooksService _hinetsBooksService;
        private readonly IHinetsWriterService _hinetsWriterService;
        private readonly IZohoTaskService _zohoTaskService;
        private readonly IZohoProjectService _zohoProjectService;
        private const string MasterPoTemplateId = "125423000002268392";
        private const string MasterEstimateTemplateId = "125423000001101083";

        private readonly string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";
        private readonly string ItemMasterFile = "Hi-nets - ZohoBooks - All Items.xlsx";
        private readonly string TemplateFolder = "Template";
        private readonly string ProcessingFolder = "Processing";

        public HinetsCustomService(IHinetsCrmService hinetsCrmService, IHinetsProjectsService hinetsProjectsService,
            IHinetsBooksService hinetsBooksService, IHinetsWriterService hinetsWriterService, 
            IZohoTaskService zohoTaskService, IZohoProjectService zohoProjectService)
        {
            _hinetsCrmService = hinetsCrmService;
            _hinetsProjectsService = hinetsProjectsService;
            _hinetsBooksService = hinetsBooksService;
            _hinetsWriterService = hinetsWriterService;
            _zohoTaskService = zohoTaskService;
            _zohoProjectService = zohoProjectService;
        }

        public async Task<ApiResultDto<string>> UploadDrawingsToProject(UploadDocumentsToProject uploadDocuments)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string crmApiKey = uploadDocuments.ZohoCrmApiKey;
                string booksApiKey = uploadDocuments.ZohoBooksApiKey;
                string projectsApiKey = uploadDocuments.ZohoProjectsApiKey;
                string dealId = uploadDocuments.DealId;
                string projectId = uploadDocuments.ProjectId;
                // STEP 1: Get Drawing Folder Id
                string procurementId = string.Empty;
                string drawingId = string.Empty;
                var getFoldersResult = new ApiResultDto<GetFolderResponse>() 
                {
                    Code = ResultCode.BadRequest,
                    Message = ZohoConstants.MSG_400
                };
                int retry = 0;
                while ((getFoldersResult.Code != ResultCode.OK || getFoldersResult.Data == null || getFoldersResult.Data.folders.Length == 0) 
                    && retry < 30)
                {
                    getFoldersResult = await _hinetsProjectsService.GetProjectFolders(projectsApiKey, projectId);
                    retry++;
                    Thread.Sleep(1000);
                }
                var projectFolders = getFoldersResult.Data.folders;
                foreach (var folder in projectFolders)
                {
                    if (folder.res_name == "Procurement")
                    {
                        procurementId = folder.res_id;
                    }
                    else if (folder.res_name == "Drawings")
                    {
                        drawingId = folder.res_id;
                    }
                }

                // STEP 2: Get all Drawing documents and delete them in the project
                var getAllDocumentsResult = await _hinetsProjectsService.GetAllDocuments(projectsApiKey, projectId, drawingId);
                var allDocuments = getAllDocumentsResult.Data.dataobj;
                foreach (var document in allDocuments)
                {
                    string fileName = document.res_name;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string documentId = document.res_id;
                        var deleteDocumentResult = await _hinetsProjectsService.DeleteDocument(projectsApiKey, projectId, documentId);
                    }
                }

                // STEP 2: Upload Deal Attachments to the drawing folders
                var getAttachmentsResult = await _hinetsCrmService.GetDealAttachments(crmApiKey, dealId);
                if (getAttachmentsResult.Code == ResultCode.OK)
                {
                    var dealAttachments = getAttachmentsResult.Data.data;
                    foreach (var dealAttachment in dealAttachments)
                    {
                        string attachmentId = dealAttachment.id;
                        string attachmentName = dealAttachment.File_Name;
                        if (attachmentName.StartsWith("QTE-", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                        var downloadAttachmentResult = await _hinetsCrmService
                            .DownloadAttachmentsFromDeal(crmApiKey, dealId, attachmentName, attachmentId);
                        string attachmentFilePath = downloadAttachmentResult.Data;
                        // STEP 2.1: Upload attachment to folder Drawing
                        var uploadDocumentToProject = await _hinetsProjectsService
                            .UploadFileToProject(projectsApiKey, attachmentFilePath, attachmentName, projectId, drawingId);
                        // STEP 2.2: Delete attachment
                        if (File.Exists(attachmentFilePath))
                        {
                            File.Delete(attachmentFilePath);
                        }
                    }
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_UAP_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = ex.Message + "\n" + ex.StackTrace;
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> UploadEstimateToProject(UploadDocumentsToProject uploadDocuments)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string crmApiKey = uploadDocuments.ZohoCrmApiKey;
                string booksApiKey = uploadDocuments.ZohoBooksApiKey;
                string projectsApiKey = uploadDocuments.ZohoProjectsApiKey;
                string dealId = uploadDocuments.DealId;
                string projectId = uploadDocuments.ProjectId;
                // STEP 1: Get Procurement Folder Id
                string procurementId = string.Empty;
                string drawingId = string.Empty;
                var getFoldersResult = new ApiResultDto<GetFolderResponse>();
                int retry = 0;
                while ((getFoldersResult.Code != ResultCode.OK || getFoldersResult.Data == null) && retry < 10)
                {
                    getFoldersResult = await _hinetsProjectsService.GetProjectFolders(projectsApiKey, projectId);
                    retry++;
                    Thread.Sleep(1000);
                }
                var projectFolders = getFoldersResult.Data.folders;
                foreach (var folder in projectFolders)
                {
                    if (folder.res_name == "Procurement")
                    {
                        procurementId = folder.res_id;
                    }
                    else if (folder.res_name == "Drawings")
                    {
                        drawingId = folder.res_id;
                    }
                }
                // STEP 3: Search and Handle Estimate by Deal Id
                var searchEstimatesResult = await _hinetsBooksService.SearchEstimateByDealId(booksApiKey, dealId);
                if (searchEstimatesResult.Code == 0)
                {
                    var searchEstimates = searchEstimatesResult.Data.estimates;
                    // STEP 3.0: Handle case when estimate is accepted
                    foreach (var estimate in searchEstimates)
                    {
                        var estimateStatus = estimate.status;
                        if (estimateStatus == "accepted" || estimateStatus == "invoiced")
                        {
                            string estimateId = estimate.estimate_id;
                            // STEP 3.1: Get Estimate By Id
                            var getEstimateResponse = await _hinetsBooksService.GetEstimateById(booksApiKey, estimateId);
                            var estimateData = getEstimateResponse.Data.estimate;
                            string estimateNumber = estimateData.estimate_number;
                            string estimateFileName = $"{estimateNumber}.pdf";
                            string templateId = estimateData.estimate_id;
                            bool correctEstimateTemplate = false;
                            if (templateId == MasterEstimateTemplateId)
                            {
                                correctEstimateTemplate = true;
                            }
                            if (!correctEstimateTemplate)
                            {
                                // STEP 3.2: Update Estimate with the download template id
                                var updateRequest = new
                                {
                                    template_id = MasterEstimateTemplateId
                                };
                                var updateEstimateResponse = await _hinetsBooksService.UpdateEstimate(booksApiKey, estimateId, updateRequest);
                            }
                            // STEP 3.3: Download Estimate PDF
                            var downloadEstimateResponse = await _hinetsBooksService.DownloadEstimatePDF(booksApiKey, estimateId, estimateNumber);
                            string estimateFilePath = downloadEstimateResponse.Data;
                            // STEP 3.4: Update Estimate with the old template id
                            if (!correctEstimateTemplate)
                            {
                                var updateRequest = new
                                {
                                    template_id = templateId
                                };
                                var updateEstimateResponse = await _hinetsBooksService.UpdateEstimate(booksApiKey, estimateId, updateRequest);
                            }
                            // STEP 3.5: Upload File to Project
                            var uploadDocumentToProject = await _hinetsProjectsService.UploadFileToProject(projectsApiKey, estimateFilePath, estimateFileName, projectId, procurementId);
                            // STEP 3.6: Delete Estimate File Path
                            if (File.Exists(estimateFilePath))
                            {
                                File.Delete(estimateFilePath);
                            }
                        }
                    }
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_UEP_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = ex.Message + "\n" + ex.StackTrace;
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> UploadPoToProject(UploadDocumentsToProject uploadDocuments)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string crmApiKey = uploadDocuments.ZohoCrmApiKey;
                string booksApiKey = uploadDocuments.ZohoBooksApiKey;
                string projectsApiKey = uploadDocuments.ZohoProjectsApiKey;
                string dealId = uploadDocuments.DealId;
                string projectId = uploadDocuments.ProjectId;
                // STEP 1: Get Procurement Folder Id
                string procurementId = string.Empty;
                string drawingId = string.Empty;
                var getFoldersResult = new ApiResultDto<GetFolderResponse>();
                int retry = 0;
                while ((getFoldersResult.Code != ResultCode.OK || getFoldersResult.Data == null) && retry < 10)
                {
                    getFoldersResult = await _hinetsProjectsService.GetProjectFolders(projectsApiKey, projectId);
                    retry++;
                    Thread.Sleep(1000);
                }
                var projectFolders = getFoldersResult.Data.folders;
                foreach (var folder in projectFolders)
                {
                    if (folder.res_name == "Procurement")
                    {
                        procurementId = folder.res_id;
                    }
                    else if (folder.res_name == "Drawings")
                    {
                        drawingId = folder.res_id;
                    }
                }
                // STEP 4: Get all Related PO

                // var getRelatedPOResult = await _hinetsCrmService.GetRelatedPurchaseOrders(crmApiKey, dealId);
                var searchEstimateByDealIdResponse = await _hinetsBooksService.SearchEstimateByDealId(booksApiKey, dealId);

                if (searchEstimateByDealIdResponse.Code == ResultCode.OK && searchEstimateByDealIdResponse.Data.estimates.Length > 0)
                {
                    var searchEstimates = searchEstimateByDealIdResponse.Data.estimates;
                    foreach (var estimate in searchEstimates)
                    {
                        var estimateNumber = estimate.estimate_number;
                        var searchPOByEstimateResult = await _hinetsBooksService.SearchPurchaseOrdersByReference(booksApiKey, estimateNumber);
                        if (searchPOByEstimateResult.Code == ResultCode.OK && searchPOByEstimateResult.Data.purchaseorders.Count > 0)
                        {
                            var purchaseOrders = searchPOByEstimateResult.Data.purchaseorders;
                            foreach (var relatedPo in purchaseOrders)
                            {
                                string poId = relatedPo.purchaseorder_id;
                                // STEP 4.1: Get Purchase Order By Id
                                var getPoByIdResult = await _hinetsBooksService.GetPoById(booksApiKey, poId);
                                if (getPoByIdResult.Code != ResultCode.OK)
                                {
                                    continue;
                                }
                                var purchaseOrder = getPoByIdResult.Data.purchaseorder;
                                string poNumber = purchaseOrder.purchaseorder_number;
                                string vendorName = purchaseOrder.vendor_name;
                                string poFileName = $"{poNumber} - {vendorName}.pdf";
                                string poTemplateId = purchaseOrder.template_id;
                                string poStatus = purchaseOrder.status;

                                if (!poStatus.Equals("open", StringComparison.InvariantCultureIgnoreCase) || poTemplateId == "125423000000561658")
                                {
                                    continue;
                                }

                                bool correctPoTemplate = false;
                                if (poTemplateId == MasterPoTemplateId)
                                {
                                    correctPoTemplate = true;
                                }
                                if (!correctPoTemplate)
                                {
                                    // STEP 4.2: Update Estimate with the download template id
                                    var updateRequest = new
                                    {
                                        template_id = MasterEstimateTemplateId
                                    };
                                    var updatePoResponse = await _hinetsBooksService.UpdatePurchaseOrder(booksApiKey, poId, updateRequest);
                                }
                                // STEP 4.3: Download Purchase Order PDF
                                var downloadPoResponse = await _hinetsBooksService.DownloadPoPDF(booksApiKey, poId, poNumber);
                                string poFilePath = downloadPoResponse.Data;
                                // STEP 4.4: Update Estimate with the old template id
                                if (!correctPoTemplate)
                                {
                                    var updateRequest = new
                                    {
                                        template_id = poTemplateId
                                    };
                                    var updateEstimateResponse = await _hinetsBooksService.UpdatePurchaseOrder(booksApiKey, poId, updateRequest);
                                }
                                // STEP 4.5: Upload File to Project
                                var uploadDocumentToProject = await _hinetsProjectsService.UploadFileToProject(projectsApiKey, poFilePath, poFileName, projectId, procurementId);
                                // STEP 4.6: Delete PO File Path
                                if (File.Exists(poFilePath))
                                {
                                    File.Delete(poFilePath);
                                }
                            }
                        }
                    }
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_UPP_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = ex.Message + "\n" + ex.StackTrace;
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> UpdateTaskDates(UpdateTaskDates updateProjectDates)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string projectsApiKey = updateProjectDates.ZohoProjectsApiKey;
                string projectId = updateProjectDates.ProjectId;
                // STEP 1: Get All Tasks in the project
                var getTasksResult = await _zohoTaskService.GetAllTasksInProject(projectsApiKey, projectId);
                if (getTasksResult.Code != ResultCode.OK)
                {
                    apiResult.Data = ZohoConstants.CUSTOM_UTD_T01;
                    return apiResult;
                }
                var projectTasks = getTasksResult.Data.tasks;
                // STEP 2: Get Start Date, End Date of the Installation Task
                string installStartStr = string.Empty;
                string installEndStr = string.Empty;
                foreach (var task in projectTasks)
                {
                    string taskName = task.name;
                    
                    if (taskName == "Installation")
                    {
                        installStartStr = task.start_date;
                        installEndStr = task.end_date;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(installStartStr) || string.IsNullOrEmpty(installEndStr))
                {
                    apiResult.Data = ZohoConstants.CUSTOM_UTD_T02;
                    return apiResult;
                }

                // STEP 3: Convert Start Date, End Date to date time
                var installStartDate = DateTimeHelpers.ConvertProjectDateToDateTime(installStartStr);
                var installEndDate = DateTimeHelpers.ConvertProjectDateToDateTime(installEndStr);

                foreach (var task in projectTasks)
                {
                    string taskName = task.name;
                    string taskId = task.id_string;
                    if (taskName == "Confirm installation crew")
                    {
                        var updateTaskRequest = new TaskForUpdation() {
                            start_date = installStartDate.AddDays(-14).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-7).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Book Accommodation")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-14).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-7).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Send RAMS to client")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-28).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Book Site Visit")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-42).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Check drawings")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-28).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Location for materials and equipment storage")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-28).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Location for spoil storage")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-28).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Site movement plan for equipment, materials, spoil")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-28).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Procedures for materials and equipment receiving")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-28).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Site operational requirements")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-28).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Access restrictions")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-28).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Conduct risk assessment")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-28).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Confirm Net Drawings")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-35).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-28).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "[PO-S2] Concrete Purchase Order")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-21).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-14).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "[PO-S2] Mesh  Purchase Order")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-21).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-14).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "[PO-S2] Plant Hire Purchase Order")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-21).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-14).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "[PO-S2] Nuts & Bolts Purchase Order")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-21).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-14).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "[PO-S2] Crane Purchase Order")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-14).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(+7).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "[PO-S2] Transport Purchase Order")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-14).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(+7).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "[PO-S2] Tower Furniture Purchase Order")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-30).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(+7).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Confirm concrete delivery")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installStartDate.AddDays(-7).ToString("MM-dd-yyyy"),
                            end_date = installStartDate.AddDays(-5).ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Off hire plant machinery")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installEndDate.ToString("MM-dd-yyyy")
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Issue completion documents")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installEndDate.AddDays(7).ToString("MM-dd-yyyy"),
                            end_date = installEndDate.AddDays(14).ToString("MM-dd-yyyy"),
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "As built information")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installEndDate.AddDays(7).ToString("MM-dd-yyyy"),
                            end_date = installEndDate.AddDays(14).ToString("MM-dd-yyyy"),
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                    else if (taskName == "Issue maintenance contract")
                    {
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            start_date = installEndDate.AddDays(7).ToString("MM-dd-yyyy"),
                            end_date = installEndDate.AddDays(14).ToString("MM-dd-yyyy"),
                        };
                        var updateTaskResult = await _zohoTaskService
                                .UpdateTaskForHinets(projectsApiKey, projectId, taskId, updateTaskRequest);
                    }
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_UTD_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = ex.Message + "\n" + ex.StackTrace;
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> ReUploadPoDocuments(UploadDocumentsToProject uploadDocuments)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string projectId = uploadDocuments.ProjectId;
                string crmKey = uploadDocuments.ZohoCrmApiKey;
                string projectKey = uploadDocuments.ZohoProjectsApiKey;
                string bookKey = uploadDocuments.ZohoBooksApiKey;

                // STEP 1: Get Deal Id from Project Id
                var getProjectDetailResult = await _hinetsProjectsService.GetHinetsProjectDetails(projectKey, projectId);
                if (getProjectDetailResult.Code != ResultCode.OK)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_RPD_400;
                    return apiResult;
                }
                var projectDetails = getProjectDetailResult.Data.projects.First();
                var customFields = projectDetails.custom_fields;
                string dealId = string.Empty;
                foreach (var customField in customFields)
                {
                    string opportunityId = customField.OpportunityId;
                    if (!string.IsNullOrEmpty(opportunityId))
                    {
                        dealId = opportunityId;
                        break;
                    }
                }
                uploadDocuments.DealId = dealId;

                // STEP 2: Get Folder Id for Procurement
                var getFolderResult = await _hinetsProjectsService.GetProjectFolders(projectKey, projectId);
                if (getFolderResult.Code != ResultCode.OK)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_RPD_400;
                    return apiResult;
                }
                var projectFolders = getFolderResult.Data.folders;
                string folderId = string.Empty;
                foreach (var folder in projectFolders)
                {
                    string folderName = folder.res_name;
                    if (folderName.Equals("Procurement", StringComparison.InvariantCultureIgnoreCase))
                    {
                        folderId = folder.res_id;
                        break;
                    }
                }

                // STEP 3: Get all PO documents and delete them in the project
                var getAllDocumentsResult = await _hinetsProjectsService.GetAllDocuments(projectKey, projectId, folderId);
                var allDocuments = getAllDocumentsResult.Data.dataobj;
                foreach (var document in allDocuments)
                {
                    string fileName = document.res_name;
                    if (!string.IsNullOrEmpty(fileName) && fileName.StartsWith("PO-", StringComparison.InvariantCultureIgnoreCase)) 
                    {
                        string documentId = document.res_id;
                        var deleteDocumentResult = await _hinetsProjectsService.DeleteDocument(projectKey, projectId, documentId);
                    }
                }

                // STEP 4: Upload PO documents to the project
                var uploadPOResult = await UploadPoToProject(uploadDocuments);
                if (uploadPOResult.Code != ResultCode.OK)
                {
                    apiResult.Message = ZohoConstants.CUSTOM_RPD_400;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_RPD_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = ex.Message + "\n" + ex.StackTrace;
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> DownloadAccommodationDocumentsAndUploadToProject(UploadAccommodationRequest accommodationRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_UAD_400
            };
            string accommodationFilePath = string.Empty;
            try
            {
                string projectKey = accommodationRequest.ZohoProjectsApiKey;
                string projectId = accommodationRequest.ProjectId;
                string fileName = $"Accommodation - {accommodationRequest.DealName}.pdf";

                // STEP 1: Download Accommodation File
                var downloadFileResult = await _hinetsWriterService.MergeAndDownloadDocument(accommodationRequest);
                if (downloadFileResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }
                accommodationFilePath = downloadFileResult.Data;

                // STEP 2: Get Folder Id for Procurement
                var getFolderResult = await _hinetsProjectsService.GetProjectFolders(projectKey, projectId);
                if (getFolderResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }
                var projectFolders = getFolderResult.Data.folders;
                string folderId = string.Empty;
                foreach (var folder in projectFolders)
                {
                    string folderName = folder.res_name;
                    if (folderName.Equals("Accommodation", StringComparison.InvariantCultureIgnoreCase))
                    {
                        folderId = folder.res_id;
                        break;
                    }
                }

                // STEP 3: Get all PO documents and delete them in the project
                var getAllDocumentsResult = await _hinetsProjectsService.GetAllDocuments(projectKey, projectId, folderId);
                var allDocuments = getAllDocumentsResult.Data.dataobj;
                foreach (var document in allDocuments)
                {
                    string documentId = document.res_id;
                    var deleteDocumentResult = await _hinetsProjectsService.DeleteDocument(projectKey, projectId, documentId);
                }

                // STEP 4: Upload Accommodation documents to project
                var uploadPOResult = await _hinetsProjectsService
                    .UploadFileToProject(projectKey, accommodationFilePath, fileName, projectId, folderId);
                if (uploadPOResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_UAD_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = ex.Message + "\n" + ex.StackTrace;
                return apiResult;
            }
            finally
            {
                if (File.Exists(accommodationFilePath))
                {
                    File.Delete(accommodationFilePath);
                }
            }
        }

        public async Task<ApiResultDto<string>> CreateItemsExcelFileAndSendToAndy(GetItemsRequest getItemsRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_CIEE_400
            };

            string processingFilePath = string.Empty;

            try
            {
                string apiKey = getItemsRequest.ZohoBooksApiKey;
                string userEmail = getItemsRequest.UserEmail;
                // STEP 1: Get All Active Items
                var itemList = new List<Item>();
                var getAllItemsResult = await _hinetsBooksService.GetActiveItems(apiKey, 1);
                if (getAllItemsResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }
                var getAllItems = getAllItemsResult.Data;
                itemList.AddRange(getAllItems.items);

                int nextPage = 2;
                while(getAllItems.page_context.has_more_page)
                {
                    getAllItemsResult = await _hinetsBooksService.GetActiveItems(apiKey, nextPage);
                    getAllItems = getAllItemsResult.Data;
                    itemList.AddRange(getAllItems.items);
                    nextPage++;
                }

                // STEP 2: Get Excel Template file
                string templateFilePath = $"{DocumentPath}\\{TemplateFolder}\\{ItemMasterFile}";
                string currentTime = DateTime.UtcNow.ToString("ddMMMyyyy HHmmss");
                string processingFileName = $"Hinets All Items - {currentTime}.xlsx";
                processingFilePath = $"{DocumentPath}\\{ProcessingFolder}\\{processingFileName}";
                File.Copy(templateFilePath, processingFilePath);

                // STEP 3: Write to Excel File
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var pck = new ExcelPackage(new FileInfo(processingFilePath)))
                {
                    var worksheet = pck.Workbook.Worksheets.FirstOrDefault();
                    var rowIndex = 2;

                    foreach (var item in itemList)
                    {
                        worksheet.InsertRow(rowIndex, 1);
                        worksheet.Cells[rowIndex, 1].Value = item.name;
                        worksheet.Cells[rowIndex, 2].Value = item.item_id;
                        worksheet.Cells[rowIndex, 3].Value = item.sku;
                        if (item.purchase_rate.HasValue)
                        {
                            worksheet.Cells[rowIndex, 4].Value = Math.Round(item.purchase_rate.Value, 2);
                        }
                        if (item.rate.HasValue)
                        {
                            worksheet.Cells[rowIndex, 5].Value = Math.Round(item.rate.Value, 2);
                        }
                        
                        worksheet.Cells[rowIndex, 6].Value = Math.Round(item.rate.Value - item.purchase_rate.Value, 2);
                        if (item.purchase_rate == 0)
                        {
                            worksheet.Cells[rowIndex, 7].Value = 0;
                        }
                        else
                        {
                            worksheet.Cells[rowIndex, 7].Value = Math.Round(item.rate.Value / item.purchase_rate.Value, 2);
                        }
                        worksheet.Cells[rowIndex, 10].Value = item.item_type;
                        worksheet.Cells[rowIndex, 11].Value = "false";
                        worksheet.Cells[rowIndex, 12].Value = item.description;
                        rowIndex++;
                    }
                    pck.Save();
                }

                // STEP 4: Send email to Andy
                var emailContent = new EmailContent() 
                {
                    Subject = "Hinets All Active Items List",
                    Body  = @"Hello Hi-nets,<br><br>Attachment is the latest Hi-nets Item List.<br>
                            Please update the 2 columns H (Updated Markup) and I (Updated Cost) and send email back to: help@roxus.io.<br><br>
                            Thanks & Regards,<br>Roxus Automation",
                    Clients = userEmail,
                    Email = EmailConstants.HelloEmail,
                    Password = EmailConstants.HelloPassword,
                    FromName = "Roxus Automation",
                    SmtpPort = EmailConstants.SmtpPort,
                    SmtpServer = EmailConstants.Outlook_Email_SmtpServer
                };
                var attachments = new List<Attachment>
                {
                    new Attachment(processingFilePath)
                };
                await EmailHelpers.SendEmailWithAttachment(emailContent, attachments);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_CIEE_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            finally
            {
                if (File.Exists(processingFilePath))
                {
                    File.Delete(processingFilePath);
                }
            }
        }

        public async Task<ApiResultDto<string>> UpdateItemPrice(string updatedContents)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.CUSTOM_UIP_400
            };


            try
            {
                string bookApiKey = "aGktbmV0czp6b2hvYm9va3M=";
                // STEP 1: Check if there are data in updated contents
                if (string.IsNullOrEmpty(updatedContents))
                {
                    apiResult.Message = ZohoConstants.CUSTOM_UIP_C01;
                    return apiResult;
                }

                // STEP 2: Get List of Items which have updated price
                var updatedItemSet = new HashSet<string>();
                var itemList = updatedContents.Split(CommonConstants.RowDelimiter);
                string updatedItems = string.Empty;
                foreach (string itemDetails in itemList)
                {
                    var itemSplit = itemDetails.Split(CommonConstants.ColumnDelimiter);
                    string itemId = itemSplit[0];
                    string itemName = itemSplit[7];
                    double purchaseRate = double.Parse(itemSplit[1]);
                    double rate = double.Parse(itemSplit[2]);
                    double marginAmount = double.Parse(itemSplit[3]);
                    double markup = double.Parse(itemSplit[4]);

                    string updateMarkupStr = itemSplit[5];
                    double updateMarkup = 0;
                    if (!string.IsNullOrEmpty(updateMarkupStr))
                    {
                        updateMarkupStr = updateMarkupStr.Replace("$", "")
                            .Replace("£", "").Replace("€", "");
                        updateMarkup = double.Parse(updateMarkupStr);
                    }

                    string updateCostStr = itemSplit[6];
                    double updateCost = 0;
                    if (!string.IsNullOrEmpty(updateCostStr))
                    {
                        updateCostStr = updateCostStr.Replace("$", "")
                            .Replace("£", "").Replace("€", "");
                        updateCost = double.Parse(updateCostStr);
                    }

                    // STEP 3: Update Item Price

                    var itemForUpdate = new ItemForUpdate();

                    if (updateMarkup > 0 && updateCost == 0)
                    {
                        itemForUpdate.purchase_rate = Math.Round(purchaseRate, 2);
                        itemForUpdate.rate = Math.Round(purchaseRate * updateMarkup, 2);
                    }
                    else if (updateMarkup == 0 && updateCost > 0)
                    {
                        itemForUpdate.purchase_rate = Math.Round(updateCost, 2);
                        itemForUpdate.rate = Math.Round(updateCost * markup, 2);
                    }
                    else if (updateMarkup > 0 && updateCost > 0)
                    {
                        itemForUpdate.purchase_rate = Math.Round(updateCost, 2);
                        itemForUpdate.rate = Math.Round(updateCost * updateMarkup, 2);
                    }

                    var updateItemResult = await _hinetsBooksService.UpdateItem(bookApiKey, itemId, itemForUpdate);
                    updatedItemSet.Add(itemId);
                    updatedItems += $"<li>Id: {itemId} - Name: {itemName} - Purchase Rate: {purchaseRate} &rarr; {itemForUpdate.purchase_rate} - Rate: {rate} &rarr; {itemForUpdate.rate}</li>";
                }

                // STEP 4: Update Composite Item Price
                int currentPage = 1;
                bool morePage = false;
                do
                {
                    var getCompositeItemsResult = await _hinetsBooksService.GetCompositeItems(bookApiKey, currentPage++);
                    var getCompositeItemsResponse = getCompositeItemsResult.Data;
                    var compositeItems = getCompositeItemsResponse.items;

                    foreach (var item in compositeItems)
                    {
                        string itemId = item.item_id;
                        string itemName = item.item_name;
                        double oldPurchaseRate = item.purchase_rate.Value;
                        double oldRate = item.rate.Value;

                        var getCompositeItemResult = await _hinetsBooksService.GetCompositeItemById(bookApiKey, itemId);
                        var getCompositeItemResponse = getCompositeItemResult.Data;
                        var compositeItemDetails = getCompositeItemResponse.composite_item;
                        var mappedItems = compositeItemDetails.mapped_items;

                        bool updated = false;
                        foreach (var mappedItem in mappedItems)
                        {
                            string mappedId = mappedItem.item_id;
                            if (updatedItemSet.Contains(mappedId))
                            {
                                updated = true;
                                break;
                            }
                        }

                        if (!updated)
                        {
                            continue;
                        }

                        double newPurchaseRate = 0;
                        double newRate = 0;
                        foreach (var mappedItem in mappedItems)
                        {

                            double quantity = mappedItem.quantity.Value;
                            double purchaseRate = mappedItem.purchase_rate.Value;
                            double rate = mappedItem.rate.Value;
                            newPurchaseRate += purchaseRate * quantity;
                            newRate += quantity * rate;
                        }

                        newPurchaseRate = Math.Round(newPurchaseRate, 2);
                        newRate = Math.Round(newRate, 2);

                        // Update Item Rate and Purchase Rate
                        var itemForUpdate = new ItemForUpdate()
                        {
                            purchase_rate = newPurchaseRate,
                            rate = newRate
                        };
                        var updateItemResult = await _hinetsBooksService.UpdateItem(bookApiKey, itemId, itemForUpdate);
                        updatedItemSet.Add(itemId);
                        updatedItems += $"<li>Id: {itemId} - Name: {itemName} - Purchase Rate: {oldPurchaseRate} &rarr; {newPurchaseRate} - Rate: {oldRate} &rarr; {newRate}</li>";

                        Thread.Sleep(1000);
                    }

                    var pageContext = getCompositeItemsResponse.page_context;
                    morePage = pageContext.has_more_page;
                } while (morePage);


                // STEP 6: Send Email to Andy
                var emailContent = new EmailContent()
                {
                    Subject = "Margin Update Result",
                    Body = @"Hello Hi-nets,<br><br>Below is the list of updated items:<br><ol>{UpdatedContents}</ol><br><br>Thanks & Regards,<br>Roxus Automation",
                    Clients = "james.nicolson@roxus.io;andy@hi-nets.co.uk",
                    Email = EmailConstants.HelloEmail,
                    Password = EmailConstants.HelloPassword,
                    FromName = "Roxus Automation",
                    SmtpPort = EmailConstants.SmtpPort,
                    SmtpServer = EmailConstants.Outlook_Email_SmtpServer
                };

                emailContent.Body = emailContent.Body.Replace("{UpdatedContents}", updatedItems);
                await EmailHelpers.SendEmail(emailContent);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_UIP_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> UploadApprovedEstimateToDeal(UploadApprovedEstimateToDeal uploadDocuments)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };
            try
            {
                string crmApiKey = uploadDocuments.ZohoCrmApiKey;
                string booksApiKey = uploadDocuments.ZohoBooksApiKey;
                string dealId = uploadDocuments.DealId;
                string estimateId = uploadDocuments.EstimateId;

                // STEP 1: Get Estimate By Id
                var getEstimateResponse = await _hinetsBooksService.GetEstimateById(booksApiKey, estimateId);
                var estimateData = getEstimateResponse.Data.estimate;
                string estimateNumber = estimateData.estimate_number;
                string estimateFileName = $"{estimateNumber}.pdf";
                string templateId = estimateData.estimate_id;
                bool correctEstimateTemplate = false;
                /*
                if (templateId == MasterEstimateTemplateId)
                {
                    correctEstimateTemplate = true;
                }
                if (!correctEstimateTemplate)
                {
                    // STEP 2: Update Estimate with the download template id
                    var updateRequest = new
                    {
                        template_id = MasterEstimateTemplateId
                    };
                    var updateEstimateResponse = await _hinetsBooksService.UpdateEstimate(booksApiKey, estimateId, updateRequest);
                }
                */
                // STEP 3: Download Estimate PDF
                var downloadEstimateResponse = await _hinetsBooksService.DownloadEstimatePDF(booksApiKey, estimateId, estimateNumber);
                string estimateFilePath = downloadEstimateResponse.Data;
                // STEP 4: Update Estimate with the old template id
                /*
                if (!correctEstimateTemplate)
                {
                    var updateRequest = new
                    {
                        template_id = templateId
                    };
                    var updateEstimateResponse = await _hinetsBooksService.UpdateEstimate(booksApiKey, estimateId, updateRequest);
                }
                */
                // STEP 5: Upload File to Deal
                var uploadDocumentToProject = await _hinetsCrmService.UploadAttachmentsToDeal(crmApiKey, dealId, estimateFileName, estimateFilePath);
                // STEP 6: Delete Estimate File Path
                if (File.Exists(estimateFilePath))
                {
                    File.Delete(estimateFilePath);
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZohoConstants.CUSTOM_UED_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = ex.Message + "\n" + ex.StackTrace;
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> AddProjectEventForTask(AddProjectEventForTask addProjectEventForTask)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZohoConstants.MSG_400
            };

            try
            {

                string projectApiKey = "aGktbmV0czp6b2hvcHJvamVjdHM=";
                string projectId = addProjectEventForTask.projectId;
                string taskId = addProjectEventForTask.taskId;

                // STEP 1: Get Task Details and extract data
                var getTaskByIdResponse = await _zohoTaskService.GetTaskDetails(projectApiKey, projectId, taskId);

                var taskDetails = getTaskByIdResponse.Data;
                string taskName = taskDetails.name;
                string taskStartDate = taskDetails.start_date;
                string taskEndDate = taskDetails.end_date;

                var taskOwners = taskDetails.details.owners;

                string participants = string.Empty;
                foreach (var owner in taskOwners)
                {
                    string ownerId = owner.id;
                    if (!string.IsNullOrEmpty(participants))
                    {
                        participants += ",";
                    }
                    participants += ownerId;
                }

                if (string.IsNullOrEmpty(taskStartDate))
                {
                    // Task Start Date must not be EMPTY
                    return apiResult;
                }
                // Calculate Duration Hours
                string durationHours = string.Empty;
                if (string.IsNullOrEmpty(taskEndDate))
                {
                    var addEventRequest = new AddEventRequest()
                    {
                        title = taskName,
                        date = taskStartDate,
                        hour = "00",
                        minutes = "00",
                        ampm = "am",
                        duration_hour = "23",
                        duration_mins = "59",
                        participants = participants,
                        repeat = "only once"
                    };
                    var addEventResponse = await _zohoProjectService.AddEvent(projectApiKey, projectId, addEventRequest);
                }
                else
                {

                    var startDateSplits = taskStartDate.Split("-");
                    string startMonth = startDateSplits[0];
                    string startDay = startDateSplits[1];
                    string startYear = startDateSplits[2];
                    var startDateTime = new DateTime(int.Parse(startYear),
                        int.Parse(startMonth), int.Parse(startDay), 0, 0, 1);

                    var endDateSplits = taskEndDate.Split("-");
                    string endMonth = endDateSplits[0];
                    string endDay = endDateSplits[1];
                    string endYear = endDateSplits[2];
                    var endDateTime = new DateTime(int.Parse(endYear),
                        int.Parse(endMonth), int.Parse(endDay), 23, 59, 59);

                    while(startDateTime < endDateTime)
                    {
                        string startDate = startDateTime.ToString("MM-dd-yyyy");
                        var addEventRequest = new AddEventRequest()
                        {
                            title = taskName,
                            date = startDate,
                            hour = "00",
                            minutes = "00",
                            ampm = "am",
                            duration_hour = "23",
                            duration_mins = "59",
                            participants = participants,
                            repeat = "only once"
                        };
                        var addEventResponse = await _zohoProjectService.AddEvent(projectApiKey, projectId, addEventRequest);
                        Thread.Sleep(500);
                        startDateTime = startDateTime.AddDays(1);
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