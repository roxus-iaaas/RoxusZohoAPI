using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw
{

    public class SearchTitleResponse
    {

        public string WKT { get; set; }

        public Owner Owner { get; set; }

        public string Title_Task { get; set; }

        public string Manual_Task { get; set; }

        public string Project_Name { get; set; }

        public string PIA_Task { get; set; }
        public string Name { get; set; }

        public string Ownership_Type { get; set; }

        public string Tenure { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Wayleave_Template { get; set; }

        public string Task_URL { get; set; }

        public string id { get; set; }

        public Approval approval { get; set; }

        public DateTime? Modified_Time { get; set; }

        public string Automated { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Reference { get; set; }

        public string Hybrid { get; set; }

        public string OR_Type { get; set; }

        public string Collection_Id { get; set; }

        public string Type { get; set; }

        public Layout Layout { get; set; }

        public Created_By Created_By { get; set; }

        public string E_Sending { get; set; }

        public string PRRD_Task { get; set; }

    }

    public class Review_Process
    {

        public bool approve { get; set; }

        public bool reject { get; set; }

        public bool resubmit { get; set; }

    }

    public class Modified_By
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

    public class Approval
    {

        public bool _delegate { get; set; }

        public bool approve { get; set; }

        public bool reject { get; set; }

        public bool resubmit { get; set; }

    }

    public class Layout
    {

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Created_By
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

}
