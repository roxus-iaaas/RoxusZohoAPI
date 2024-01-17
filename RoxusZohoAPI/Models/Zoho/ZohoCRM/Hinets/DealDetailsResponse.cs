using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets
{

    public class DealDetailsResponse
    {
        public DealData[] data { get; set; }
    }

    public class DealData
    {
        public Owner Owner { get; set; }
        public object Description { get; set; }
        public string currency_symbol { get; set; }
        public Review_Process review_process { get; set; }
        public string onedriveextension__OneDrive_Folder_ID { get; set; }
        public string Closing_Date { get; set; }
        public string Target_Completion_Date { get; set; }
        public DateTime Last_Activity_Time { get; set; }
        public Modified_By Modified_By { get; set; }
        public object Lead_Conversion_Time { get; set; }
        public string state { get; set; }
        public bool process_flow { get; set; }
        public string Deal_Name { get; set; }
        public int? Exchange_Rate { get; set; }
        public int? Expected_Revenue { get; set; }
        public string Currency { get; set; }
        public int? Overall_Sales_Duration { get; set; }
        public string Stage { get; set; }
        public Account_Name Account_Name { get; set; }
        public string id { get; set; }
        public string Site_Postcode { get; set; }
        public DateTime? Modified_Time { get; set; }
        public string A_E_Phone { get; set; }
        public DateTime? Created_Time { get; set; }
        public int? Amount { get; set; }
        public string Primary_Contact_Name { get; set; }
        public object Site_What3Words { get; set; }
        public bool? editable { get; set; }
        public bool? orchestration { get; set; }
        public Contact_Name Contact_Name { get; set; }
        public int Sales_Cycle_Duration { get; set; }
        public string Site_Address { get; set; }
        public object Type { get; set; }
        public bool in_merge { get; set; }
        public object A_E_Address { get; set; }
        public object Lead_Source { get; set; }
        public Created_By Created_By { get; set; }
        public object[] Tag { get; set; }
        public object Nearest_A_E { get; set; }
        public object[] Accommodation { get; set; }
        public string approval_state { get; set; }
        public object Install_Days { get; set; }
    }

    public class Review_Process
    {
        public bool approve { get; set; }
        public bool reject { get; set; }
        public bool resubmit { get; set; }
    }

    public class Account_Name
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Approval
    {
        public bool _delegate { get; set; }
        public bool approve { get; set; }
        public bool reject { get; set; }
        public bool resubmit { get; set; }
    }

    public class Contact_Name
    {
        public string name { get; set; }
        public string id { get; set; }
    }

}
