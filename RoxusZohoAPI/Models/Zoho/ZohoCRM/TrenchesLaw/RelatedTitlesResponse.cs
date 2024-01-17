using System;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM.TrenchesLaw
{

    public class RelatedTitlesResponse
    {

        public TitleData[] data { get; set; }

        public Info info { get; set; }

    }

    public class TitleData
    {

        public Owner Owner { get; set; }

        public string Name { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public Modified_By Modified_By { get; set; }

        public int? Exchange_Rate { get; set; }

        public string Currency { get; set; }

        public Related_Freehold_Titles Related_Freehold_Titles { get; set; }

        public string id { get; set; }

        public Approval approval { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public Related_Openreach_SM Related_Openreach_SM { get; set; }

        public Created_By Created_By { get; set; }

    }

    public class Related_Freehold_Titles
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
