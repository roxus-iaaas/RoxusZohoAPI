using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public interface IZohoTitleService
    {

        Task<ApiResultDto<TitleResponse>> GetTitleById(string apiKey, string titleId);

        Task<ApiResultDto<SearchResponse<SearchTitleResponse>>> SearchTitleByTitleNumber(string apiKey, string titleNumber);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpsertTitle(string apiKey, TitleForCreation titleForCreation);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpsertTitleForOpenreach(string apiKey, OpenreachTitleForCreation titleForCreation);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateTitle(string apiKey, string titleId, UpdateRequest updateRequest);

        Task<ApiResultDto<UploadResponse>> Title_UploadAttachments(string apiKey, string titleId, string fileName, string fileContent);

        Task<ApiResultDto<UpsertResponse<LinkingResponse>>> Title_LinkingWithUPRNs(string apiKey, string titleId, string uprnIds);

        Task<ApiResultDto<UpsertResponse<LinkingResponse>>> Title_LinkingWithUSRN(string apiKey, string titleId, string usrnId);

        Task<ApiResultDto<UpsertResponse<LinkingResponse>>> Title_LinkingWithOwners(string apiKey, string titleId, string contactIds, string accountIds);

    }
}
