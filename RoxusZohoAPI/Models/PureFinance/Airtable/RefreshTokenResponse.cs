using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.PureFinance.Airtable
{

    public class RefreshTokenResponse
    {

        public string access_token { get; set; }
        
        public string refresh_token { get; set; }
        
        public string token_type { get; set; }
        
        public string scope { get; set; }
        
        public int? expires_in { get; set; }
        
        public int? refresh_expires_in { get; set; }
    
    }

}
