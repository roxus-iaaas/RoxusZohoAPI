using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class UprnResponse
    {
        public string Classification_Code { get; set; }
        public Owner Owner { get; set; }
        public string currency_symbol { get; set; }
        public string Latitude { get; set; }
        public string Name { get; set; }
        public DateTime Last_Activity_Time { get; set; }
        public string Welsh_Locality { get; set; }
        public string Collection_ID { get; set; }
        public string Post_Code { get; set; }
        public string Currency { get; set; }
        public string Classification { get; set; }
        public string MDU_ID { get; set; }
        public string Spectrum_Pole_ID { get; set; }
        public string id { get; set; }
        public string Welsh_Side_Street { get; set; }
        public string Pole_ID { get; set; }
        public string Pole_Location { get; set; }
        public string Address_Line_4 { get; set; }
        public string SAN_ID { get; set; }
        public string Loc_Status { get; set; }
        public Approval approval { get; set; }
        public string Address_Line_1 { get; set; }
        public string Sub_Building_Name { get; set; }
        public string SF_Number { get; set; }
        public string Priority { get; set; }
        public string Address_Line_2 { get; set; }
        public string Spectrum_Code { get; set; }
        public string City { get; set; }
        public Subform[] subform { get; set; }
        public string Active_Cab_ID { get; set; }
        public string Longitude { get; set; }
        public string Building_Number_or_Name { get; set; }
        public string Wayleave_Code { get; set; }
        public object Owner_Account { get; set; }
        public string Country { get; set; }
        public string Drop_Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Project_ID { get; set; }
        public Review_Process review_process { get; set; }
        public UPRN_Titles[] UPRN_Titles { get; set; }
        public object Related_Parent_UPRN { get; set; }
        public string County { get; set; }
        public string UPRN_Type { get; set; }
        public string xFDP_ID { get; set; }
        public object Organisation_Tenant { get; set; }
        public string Welsh_Town { get; set; }
        public string OLT_ID { get; set; }
        public object Owner_Contact { get; set; }
        public string Building_Number { get; set; }
        public string Lateral_Ref { get; set; }
        public string approval_state { get; set; }
    }

    public class Owner
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

    public class Review_Process
    {
        public bool approve { get; set; }
        public bool reject { get; set; }
        public bool resubmit { get; set; }
    }

    public class Subform
    {
        public object Title_Number_1 { get; set; }
        public bool in_merge { get; set; }
        public DateTime Created_Time { get; set; }
        public Parent_Id Parent_Id { get; set; }
        public object Maintenance_1 { get; set; }
        public string id { get; set; }
        public USRN USRN { get; set; }
        public object Tenure_1 { get; set; }
    }

    public class Parent_Id
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class USRN
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class UPRN_Titles
    {
        public object Title_Type { get; set; }
        public bool in_merge { get; set; }
        public DateTime Created_Time { get; set; }
        public Title_Parent Parent_Id { get; set; }
        public Title Title { get; set; }
        public string id { get; set; }
        public object Title_Tenure { get; set; }
    }

    public class Title_Parent
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Title
    {
        public string name { get; set; }
        public string id { get; set; }
    }
}
