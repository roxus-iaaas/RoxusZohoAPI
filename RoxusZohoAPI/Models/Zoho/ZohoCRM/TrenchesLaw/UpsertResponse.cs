using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class UpsertResponse<T>
    {
        public List<ResponseData> data { get; set; }

        public class ResponseData
        {
            public string code { get; set; }
            public T details { get; set; }
            public string message { get; set; }
            public string status { get; set; }
        }
    }
}
