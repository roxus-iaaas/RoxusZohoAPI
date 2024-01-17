using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Hinets
{
    public interface IHinetsProjectsService
    {
        #region Projects

        Task<ApiResultDto<GetFolderResponse>> GetProjectFolders(string apiKey, string projectId);

        Task<ApiResultDto<string>> UploadFileToProject(string apiKey, string filePath,
            string fileName, string projectId, string folderId = null);

        Task<ApiResultDto<GetAllDocumentsResponse>> GetAllDocuments(string apiKey, string projectId, string folderId);

        Task<ApiResultDto<HinetsProjectDetailsResponse>> GetHinetsProjectDetails(string apiKey, string projectId);

        Task<ApiResultDto<string>> DeleteDocument(string apiKey, string projectId, string documentId);

        #endregion
    }
}
