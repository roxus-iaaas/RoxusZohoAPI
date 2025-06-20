namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesViewACaseResponse
    {

        public int? case_id { get; set; }
        
        public int? case_creator { get; set; }
        
        public int? case_owner { get; set; }
        
        public int? case_owner_company_id { get; set; }
        
        public object case_created_from { get; set; }

        public int? case_type { get; set; }
        
        public string case_type_name { get; set; }
        
        public string case_name { get; set; }
        
        public string case_start { get; set; }
        
        public string case_created_at { get; set; }
        
        public int? case_risk { get; set; }
        
        public string case_status { get; set; }
        
        public string address_line1 { get; set; }
        
        public string address_line2 { get; set; }
        
        public string address_city { get; set; }
        
        public string address_county { get; set; }
        
        public string address_country { get; set; }
        
        public string address_postcode { get; set; }
        
        public object address_longitude { get; set; }
        
        public object address_latitude { get; set; }
        
        public Contributor[] contributors { get; set; }
        
        public Title_Tenure[] title_tenure { get; set; }
        
        public Workflow[] workflows { get; set; }
    
    }

    public class Contributor
    {
        
        public string type { get; set; }
        
        public string name { get; set; }
        
        public string email { get; set; }
        
        public bool? is_primary_client { get; set; }
        
        public bool? is_secondary_client { get; set; }
        
        public string role_name { get; set; }
        
        public string branch_name { get; set; }
        
        public string company_name { get; set; }
        
        public int? user_id { get; set; }
        
        public int? person_id { get; set; }
        
        public int? entity_id { get; set; }
        
        public string entity_name { get; set; }
    
    }


    public class Title_Tenure
    {
        
        public string title_number { get; set; }
        
        public string tenure { get; set; }
    
    }

    public class Workflow
    {
        
        public int? workflow_id { get; set; }
        
        public string workflow_name { get; set; }
    
    }

}
