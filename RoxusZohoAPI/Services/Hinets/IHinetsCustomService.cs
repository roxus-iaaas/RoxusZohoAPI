using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Hinets
{
    public interface IHinetsCustomService
    {
        Task<ApiResultDto<string>> UploadDrawingsToProject(UploadDocumentsToProject uploadDocuments);

        Task<ApiResultDto<string>> UploadEstimateToProject(UploadDocumentsToProject uploadDocuments);

        Task<ApiResultDto<string>> UploadPoToProject(UploadDocumentsToProject uploadDocuments);

        Task<ApiResultDto<string>> UpdateTaskDates(UpdateTaskDates updateProjectDates);

        Task<ApiResultDto<string>> ReUploadPoDocuments(UploadDocumentsToProject uploadDocuments);

        Task<ApiResultDto<string>> DownloadAccommodationDocumentsAndUploadToProject(UploadAccommodationRequest accommodationRequest);

        Task<ApiResultDto<string>> CreateItemsExcelFileAndSendToAndy(GetItemsRequest getItemsRequest);

        Task<ApiResultDto<string>> UpdateItemPrice(string updateContent);

        Task<ApiResultDto<string>> UploadApprovedEstimateToDeal(UploadApprovedEstimateToDeal uploadDocuments);

        public Task<ApiResultDto<string>> AddProjectEventForTask(AddProjectEventForTask addProjectEventForTask);

    }
}
