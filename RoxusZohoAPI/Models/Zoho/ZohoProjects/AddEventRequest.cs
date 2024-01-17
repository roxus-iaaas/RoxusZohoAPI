using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AddEventRequest
    {

        public string title { get; set; }

        public string date { get; set; }

        public string hour { get; set; }

        public string minutes { get; set; }

        public string ampm { get; set; }

        public string duration_hour { get; set; }

        public string duration_mins { get; set; }

        public string participants { get; set; }

        public string repeat { get; set; }

        public string nooftimes_repeat { get; set; }

        public string location { get; set; }

    }
}
