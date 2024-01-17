using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.Zoho.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Hinets
{
    public interface IHinetsWriterService
    {

        Task<ApiResultDto<string>> MergeAndDownloadDocument(UploadAccommodationRequest accommodationRequest);

    }
}
