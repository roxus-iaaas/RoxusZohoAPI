using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.PureFinance.Pipedrive
{

    public class CreatePersonResponse
    {

        public bool success { get; set; }
        
        public Data data { get; set; }
        
        public Additional_Data additional_data { get; set; }
    
    }

    public class CreatePersonData
    {

        public int id { get; set; }
        
    }

}
