using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class UploadResponse
    {
        public UploadData[] data { get; set; }
    }

    public class UploadData
    {
        public string code { get; set; }
        public Details details { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }

    public class Details
    {
        public DateTime Modified_Time { get; set; }
        public Modified_By Modified_By { get; set; }
        public DateTime Created_Time { get; set; }
        public string id { get; set; }
        public Created_By Created_By { get; set; }
    }
}
