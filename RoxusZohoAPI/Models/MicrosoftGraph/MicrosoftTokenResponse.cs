using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.MicrosoftGraph
{

    public class MicrosoftTokenResponse
    {

        public string token_type { get; set; }

        public string scope { get; set; }

        public decimal? expires_in { get; set; }
        public decimal? ext_expires_in { get; set; }

        public string access_token { get; set; }

        public string refresh_token { get; set; }

    }

}
