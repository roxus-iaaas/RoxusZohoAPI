using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets
{

    public class RelatedProjectsResponse
    {
        public RelatedProject[] data { get; set; }
        public Info info { get; set; }
    }

    public class RelatedProject
    {
        public string name { get; set; }
        public string description { get; set; }
        public string id { get; set; }
    }

}
