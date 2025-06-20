using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class GetCannedTextByIdResponse
    {

        public string guid { get; set; }
        
        public int? id { get; set; }

        public string name { get; set; }
        
        public int? group_id { get; set; }
        
        public string text { get; set; }
        
        public string html { get; set; }
        
        public int? restriction_type { get; set; }
        
        public string department_name { get; set; }
        
        public string team_name { get; set; }
        
        public string agent_name { get; set; }
        
        public bool? _canupdate { get; set; }

        public Access_Control[] access_control { get; set; }
        
        public int? access_control_level { get; set; }
        
        public object[] tags { get; set; }
    
    }

    public class Access_Control
    {
        
        public int? id { get; set; }
        
        public string entity { get; set; }
        
        public int? entity_id { get; set; }
        
        public int? type { get; set; }
        
        public int? agent_id { get; set; }
        
        public int? user_id { get; set; }
        
        public int? role_id { get; set; }
        
        public int? team_id { get; set; }
        
        public int? department_id { get; set; }
        
        public int? level { get; set; }
        
        public string entity_text_id { get; set; }
    
    }

}
