using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class GetAccessTokenRequest
    {

        public string grant_type { get; set; }

        public string client_id { get; set; }

        public string client_secret { get; set; }

        public string scope { get; set; }

    }

}
