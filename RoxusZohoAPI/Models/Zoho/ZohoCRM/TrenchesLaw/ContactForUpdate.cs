using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ContactForUpdate
    {

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Account_Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public string Mailing_Street { get; set; }

        public string Mailing_City { get; set; }

        public string Mailing_State { get; set; }

        public string Mailing_Zip { get; set; }

        public string Mailing_Country { get; set; }

    }
}
