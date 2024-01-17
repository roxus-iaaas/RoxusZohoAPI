using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.Custom
{

    public class UploadApprovedEstimateToDeal
    {

        public string EstimateId { get; set; }

        public string DealId { get; set; }

        public string ZohoCrmApiKey { get; set; }

        public string ZohoBooksApiKey { get; set; }

    }

}
