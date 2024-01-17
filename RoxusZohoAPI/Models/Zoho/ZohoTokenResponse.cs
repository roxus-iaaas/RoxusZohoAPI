using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho
{
    public class ZohoTokenResponse
    {
        public string access_token { get; set; }
        public string api_domain { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}
