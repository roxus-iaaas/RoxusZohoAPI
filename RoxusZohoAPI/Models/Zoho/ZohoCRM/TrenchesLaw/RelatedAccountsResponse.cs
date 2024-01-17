using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw
{

    public class RelatedAccountsResponse
    {

        public AccountData[] data { get; set; }

        public Info info { get; set; }

    }

    public class Info
    {
        public int? per_page { get; set; }
        public int? count { get; set; }
        public int? page { get; set; }
        public bool? more_records { get; set; }
    }

    public class AccountData
    {
        public Owner Owner { get; set; }


        public string Email { get; set; }

        public string Name { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public Modified_By Modified_By { get; set; }

        public int? Exchange_Rate { get; set; }

        public string Currency { get; set; }

        public string id { get; set; }

        public Approval approval { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public Related_Openreach_SM Related_Openreach_SM { get; set; }

        public Created_By Created_By { get; set; }

        public Related_Account_Owner Related_Account_Owner { get; set; }

    }

    public class Related_Openreach_SM
    {

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Related_Account_Owner
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
