using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoDesk
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CreateTicketRequest
    {

        public string subject { get; set; }

        public string departmentId { get; set; }

        public string contactId { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string description { get; set; }

        public string status { get; set; }

        public string priority { get; set; }

        public string channel { get; set; }

        public string classification { get; set; }

    }
}
