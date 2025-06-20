using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.PureFinance.Airtable
{

    public class AirtablePayload
    {
        
        [JsonProperty("base")]
        public Base _base { get; set; }
        
        public Webhook webhook { get; set; }
        
        public DateTime? timestamp { get; set; }

    }

    public class Base
    {
        
        public string id { get; set; }

    }

    public class Webhook
    {

        public string id { get; set; }
    
    }

}
