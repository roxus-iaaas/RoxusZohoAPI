using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class OpenreachForUpdate
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OR_Status { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Task_Status { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OR_Status_Date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status_Date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Due_Date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Complete_Date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OR_Salesforce { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Hold_Date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Hold_Reason { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? Data_Phase { get; set; }

    }
}
