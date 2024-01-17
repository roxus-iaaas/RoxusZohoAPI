using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    class UsrnForCreation
    {
        public object WKT { get; set; }
        public object[] Subform_3 { get; set; }
        public string Name { get; set; }
        public object Last_Activity_Time { get; set; }
        public object Title_Number { get; set; }
        public string Status { get; set; }
        public string Street_Name { get; set; }
        public object Locality { get; set; }
        public object Address_Line_3 { get; set; }
        public object Unsubscribed_Time { get; set; }
        public string Maintenance { get; set; }
        public UPRN_Associated[] UPRN_Associated { get; set; }
        public string Country { get; set; }
    }
}
