using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public interface IZohoContactService
    {
        Task<ApiResultDto<ContactResponse>> GetContactById(string apiKey, string contactId);

        Task<ApiResultDto<ContactResponse>> SearchContactByFirstNameAndLastName(string apiKey, string firstName, string lastName);

        Task<ApiResultDto<ContactResponse>> SearchContactByEmail(string apiKey, string email);

        Task<ApiResultDto<UpdateResponse>> UpdateContact(string apiKey, string contactId, ContactForUpdate contactForUpdate);

        Task<ApiResultDto<UploadResponse>> Contact_UploadAttachments(string apiKey, string contactId, string fileName, string fileContent);
    }
}
