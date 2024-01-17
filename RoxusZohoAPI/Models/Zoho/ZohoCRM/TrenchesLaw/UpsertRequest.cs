using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class UpsertRequest<T>
    {
        public UpsertRequest()
        {
            data = new List<T>();
            duplicate_check_fields = new List<string>();
        }
        public List<T> data { get; set; }
        public List<string> duplicate_check_fields { get; set; }
    }
}
