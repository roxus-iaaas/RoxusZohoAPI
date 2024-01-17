using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class TitleResponse
    {
        public string WKT { get; set; }
        public string Title_Task { get; set; }
        public string Manual_Task { get; set; }
        public string Project_Name { get; set; }
        public object Related_Freehold_Title { get; set; }
        public string PIA_Task { get; set; }
        public string Name { get; set; }
        public DateTime Last_Activity_Time { get; set; }
        public string Tenure { get; set; }
        public string Wayleave_Template { get; set; }
        public string Task_URL { get; set; }
        public string id { get; set; }
        public string Reference { get; set; }
        public string Collection_Id { get; set; }
        public Related_Contacts[] Related_Contacts { get; set; }
        public string Type { get; set; }
        public object Linked_Titles { get; set; }
        public object Dependent_Titles { get; set; }
        public Related_UPRN[] Related_UPRN { get; set; }
        public Title_RelatedUsrn USRN { get; set; }
        public string PRRD_Task { get; set; }
    }

    public class Title_RelatedUsrn
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Related_Contacts
    {
        public bool in_merge { get; set; }
        public DateTime Created_Time { get; set; }
        public Account_Name Account_Name { get; set; }
        public string id { get; set; }
        public Contact_Name Contact_Name { get; set; }
    }

    public class Contact_Name
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Related_UPRN
    {
        public UPRN UPRN { get; set; }
        public bool in_merge { get; set; }
        public DateTime Created_Time { get; set; }
        public string id { get; set; }
    }

    public class UPRN
    {
        public string name { get; set; }
        public string id { get; set; }
    }

}
