using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UpdateTicketRequest
    {

        public UpdateTicketRequest()
        {

            customfields = new List<UpdateCustomField>();

        }

        public string id { get; set; }
        
        // public object files { get; set; }
        
        public bool? apply_rules { get; set; }

        public string status_id { get; set; }

        public List<UpdateCustomField> customfields { get; set; }
        
        // public int? utcoffset { get; set; }
        
        // public bool? _refreshresponse { get; set; }
    
    }

    public class UpdateCustomField
    {
        
        public string id { get; set; }
        
        public string value { get; set; }
    
    }


}
