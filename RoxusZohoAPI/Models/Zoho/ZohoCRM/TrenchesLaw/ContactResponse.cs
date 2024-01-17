using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class ContactResponse
    {
        public Owner Owner { get; set; }
        public string Email { get; set; }
        public string Mailing_State { get; set; }
        public string Other_State { get; set; }
        public string Other_Country { get; set; }
        public DateTime? Last_Activity_Time { get; set; }
        public object Department { get; set; }
        public string state { get; set; }
        public object Unsubscribed_Mode { get; set; }
        public object Assistant { get; set; }
        public string Mailing_Country { get; set; }
        public object Data_Processing_Basis_Details { get; set; }
        public string id { get; set; }
        public string Data_Source { get; set; }
        public object Reporting_To { get; set; }
        public object First_Visited_URL { get; set; }
        public object Days_Visited { get; set; }
        public object Other_City { get; set; }
        public object Home_Phone { get; set; }
        public object Last_Visited_Time { get; set; }
        public object Description { get; set; }
        public string Mailing_Zip { get; set; }
        public object Number_Of_Chats { get; set; }
        public object Twitter { get; set; }
        public object Other_Zip { get; set; }
        public string Mailing_Street { get; set; }
        public object Average_Time_Spent_Minutes { get; set; }
        public object Salutation { get; set; }
        public string First_Name { get; set; }
        public string Full_Name { get; set; }
        public string Title_Full_Name { get; set; }
        public object Asst_Phone { get; set; }
        public object Record_Image { get; set; }
        public object Phone { get; set; }
        public Account_Name Account_Name { get; set; }
        public bool Email_Opt_Out { get; set; }
        public string Hubspot_Id { get; set; }
        public string Mailing_City { get; set; }
        public object Unsubscribed_Time { get; set; }
        public object Title { get; set; }
        public object Other_Street { get; set; }
        public object Mobile { get; set; }
        public object First_Visited_Time { get; set; }
        public string Last_Name { get; set; }
        public object Referrer { get; set; }
        public object Lead_Source { get; set; }
        public object[] Tag { get; set; }
    }

    public class Account_Name
    {
        public string name { get; set; }
        public string id { get; set; }
    }

}
