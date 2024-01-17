using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AccountForUpdate
    {

        public string Account_Name { get; set; }

        public string Website { get; set; }

        public string Industry { get; set; }

        public string Description { get; set; }

        public string Company_Registration { get; set; }

        public string Billing_Street { get; set; }

        public string Billing_City { get; set; }

        public string Billing_State { get; set; }

        public string Billing_Code { get; set; }

        public string Billing_Country { get; set; }

    }

}
