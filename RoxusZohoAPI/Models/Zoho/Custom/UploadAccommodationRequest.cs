using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.Custom
{
    public class UploadAccommodationRequest
    {

        public string DealId { get; set; }

        public string DealName { get; set; }

        public string MergeData { get; set; }

        public string ProjectId { get; set; }

        public string ZohoProjectsApiKey { get; set; }

        public string ZohoWriterApiKey { get; set; }

    }
}
