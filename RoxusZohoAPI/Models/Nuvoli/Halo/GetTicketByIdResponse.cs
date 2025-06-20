using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class GetTicketByIdResponse
    {

        public GetTicketByIdResponse()
        {
            customfields = new List<CustomField>();
        }

        public int? id { get; set; }

        public string dateorccurred { get; set; }

        public string summary { get; set; }

        public string details { get; set; }

        public int? status_id { get; set; }

        public int? tickettype_id { get; set; }

        public int? sla_id { get; set; }

        public string sla_name { get; set; }

        public int? priority_id { get; set; }

        public int? client_id { get; set; }

        public string client_name { get; set; }

        public int? site_id { get; set; }

        public string site_name { get; set; }

        public int? user_id { get; set; }

        public string user_name { get; set; }

        public string team { get; set; }

        public int? agent_id { get; set; }

        public string details_html { get; set; }

        public List<CustomField> customfields { get; set; }

    }

    public class CustomField
    {

        public int? id { get; set; }

        public string name { get; set; }

        public string label { get; set; }
        
        public string summary { get; set; }
        
        public int? type { get; set; }

        public string value { get; set; }

        public string display { get; set; }
        
        public int? characterlimit { get; set; }
        
        public int? characterlimittype { get; set; }

        public int? inputtype { get; set; }
        
        public bool? copytochild { get; set; }
        
        public bool? copytoparent { get; set; }
        
        public bool? searchable { get; set; }
        
        public bool? ordervalues { get; set; }
        
        public int? ordervaluesby { get; set; }
        
        public bool? database_lookup_auto { get; set; }
        
        public int? extratype { get; set; }

        public bool? copytochildonupdate { get; set; }
        
        public bool? copytoparentonupdate { get; set; }
        
        public int? usage { get; set; }
        
        public bool? showondetailsscreen { get; set; }
        
        public string third_party_value { get; set; }
        
        public int? custom_extra_table_id { get; set; }
        
        public bool? copytorelated { get; set; }
        
        public string calculation { get; set; }
        
        public int? rounding { get; set; }
        
        public string regex { get; set; }
        
        public bool? is_horizontal { get; set; }
        
        public bool? isencrypted { get; set; }
        
        public string guid { get; set; }
        
        public int? selection_field_id { get; set; }
        
        public object[] validation_data { get; set; }
        
        public bool? showintable { get; set; }
        
        public int? load_type { get; set; }
        
        public bool? onlyshowforagents { get; set; }
    
    }

}
