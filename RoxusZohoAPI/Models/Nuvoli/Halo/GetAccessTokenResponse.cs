using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class GetAccessTokenResponse
    {

        public string token_type { get; set; }
        
        public string access_token { get; set; }
        
        public int? expires_in { get; set; }
    
    }


}
