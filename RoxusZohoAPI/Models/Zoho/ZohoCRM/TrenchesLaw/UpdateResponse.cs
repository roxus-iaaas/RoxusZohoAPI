using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{


    public class UpdateResponse
    {
        public UpdateData[] data { get; set; }
    }

    public class UpdateData
    {
        public string code { get; set; }
        public UpdateDetail details { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }

    public class UpdateDetail
    {
        public DateTime Modified_Time { get; set; }
        public Modified_By Modified_By { get; set; }
        public DateTime Created_Time { get; set; }
        public string id { get; set; }
        public Created_By Created_By { get; set; }
    }
}
