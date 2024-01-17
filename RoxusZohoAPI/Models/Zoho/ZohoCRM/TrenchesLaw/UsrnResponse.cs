using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class UsrnResponse
    {
        public object WKT { get; set; }
        public object[] Subform_3 { get; set; }
        public string Name { get; set; }
        public object Last_Activity_Time { get; set; }
        public object Title_Number { get; set; }
        public string Currency { get; set; }
        public string id { get; set; }
        public string Status { get; set; }
        public string Street_Name { get; set; }
        public string Locality { get; set; }
        public string Address_Line_3 { get; set; }
        public object Unsubscribed_Time { get; set; }
        public string Maintenance { get; set; }
        public UPRN_Associated[] UPRN_Associated { get; set; }
        public string Country { get; set; }
    }

    public class UPRN_Associated
    {
        public Usrn_RelatedUprn UPRN { get; set; }
        public object Type { get; set; }
        public DateTime Created_Time { get; set; }
        public string id { get; set; }
        public object UPRN_Building { get; set; }
        public object Wayleave_Status_1 { get; set; }
    }

    public class Usrn_RelatedUprn
    {
        public string name { get; set; }
        public string id { get; set; }
    }
}
