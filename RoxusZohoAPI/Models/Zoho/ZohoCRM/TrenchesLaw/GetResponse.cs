using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class GetResponse<T>
    {
        public GetResponse()
        {
            data = new List<T>();
        }
        public List<T> data { get; set; }
    }
}
