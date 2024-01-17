using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw
{

    public class Account_RelatedContacts
    {
        public RelatedContactData[] data { get; set; }
        public Info info { get; set; }
    }

    public class RelatedContactData
    {

        public Owner Owner { get; set; }

        public string Email { get; set; }

        public object Other_Phone { get; set; }

        public object Mailing_State { get; set; }

        public object Other_State { get; set; }

        public object Other_Country { get; set; }

        public object Last_Activity_Time { get; set; }

        public string Currency { get; set; }

        public object Mailing_Country { get; set; }

        public object Data_Processing_Basis_Details { get; set; }

        public string id { get; set; }

        public string Data_Source { get; set; }

        public Approval approval { get; set; }

        public string Enrich_Status__s { get; set; }

        public object First_Visited_URL { get; set; }

        public object Days_Visited { get; set; }

        public string Other_City { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Home_Phone { get; set; }

        public object Last_Visited_Time { get; set; }

        public Created_By Created_By { get; set; }

        public string Secondary_Email { get; set; }

        public string Description { get; set; }

        public string Mailing_Zip { get; set; }

        public object Number_Of_Chats { get; set; }

        public string Website { get; set; }

        public string Other_Zip { get; set; }

        public string Mailing_Street { get; set; }

        public object Average_Time_Spent_Minutes { get; set; }

        public string Salutation { get; set; }

        public string First_Name { get; set; }

        public string Full_Name { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Phone { get; set; }

        public Account_Name Account_Name { get; set; }

        public string Title_Full_Name { get; set; }

        public bool? Email_Opt_Out { get; set; }

        public DateTime? Modified_Time { get; set; }

        public string Mailing_City { get; set; }

        public string Other_Street { get; set; }

        public string Mobile { get; set; }

        public object First_Visited_Time { get; set; }

        public string Last_Name { get; set; }

        public bool? in_merge { get; set; }
    }

    public class Account_Name
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
