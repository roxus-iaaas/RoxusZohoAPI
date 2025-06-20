using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.PureFinance.Pipedrive
{

    public class CreateNoteRequest
    {
        
        public string content { get; set; }
        
        public string deal_id { get; set; }
        
        public string user_id { get; set; }
        
        public string pinned_to_deal_flag { get; set; }
    
    }

}
