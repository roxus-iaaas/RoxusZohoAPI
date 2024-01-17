using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.Custom
{

    public class SendLetterRequest
    {

        public string crmKey { get; set; }

        public string projectKey { get; set; }

        public string projectId { get; set; }

        public string tasklistId { get; set; }

        public string taskId { get; set; }

    }

}
