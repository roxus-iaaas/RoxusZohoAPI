using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class OpenreachTitleForCreation
    {

        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Project_Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Reference { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OR_Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Related_Openreach_SM { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Ownership_Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Tenure { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Layout { get; set; }

    }
}
