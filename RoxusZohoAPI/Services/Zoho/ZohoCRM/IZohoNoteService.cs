using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.ZohoCRM;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho.ZohoCRM
{
    public interface IZohoNoteService
    {

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateNote(string apiKey, UpsertRequest<NoteForCreation> noteRequest);

    }
}
