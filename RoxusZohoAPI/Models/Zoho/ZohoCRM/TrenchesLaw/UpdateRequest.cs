using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class UpdateRequest
    {
        public UpdateRequest()
        {
            data = new List<object>();
        }

        public List<object> data { get; set; }
    }
}
