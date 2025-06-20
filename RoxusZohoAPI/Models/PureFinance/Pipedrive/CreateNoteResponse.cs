using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.PureFinance.Pipedrive
{

    public class CreateNoteResponse
    {
        
        public bool? success { get; set; }
        
        public NoteData data { get; set; }
    
    }

    public class NoteData
    {

        public int? id { get; set; }
        
        public int? user_id { get; set; }
        
        public int? deal_id { get; set; }
        
        public int? person_id { get; set; }
        
        public object org_id { get; set; }
        
        public object lead_id { get; set; }
        
        public string content { get; set; }
        
        public string add_time { get; set; }
        
        public string update_time { get; set; }
        
        public bool? active_flag { get; set; }
        
        public bool? pinned_to_deal_flag { get; set; }
        
        public bool? pinned_to_person_flag { get; set; }
        
        public bool? pinned_to_organization_flag { get; set; }
        
        public bool? pinned_to_lead_flag { get; set; }
        
        public object last_update_user_id { get; set; }
        
        public object organization { get; set; }
        
        public Person person { get; set; }
        
        public Deal deal { get; set; }
        
        public object lead { get; set; }
        
        public User user { get; set; }
    
    }

    public class Person
    {
        
        public string name { get; set; }
    
    }

    public class Deal
    {
        
        public string title { get; set; }
    
    }

    public class User
    {
        
        public string email { get; set; }
        
        public string name { get; set; }
        
        public object icon_url { get; set; }
        
        public bool? is_you { get; set; }
    
    }

}
