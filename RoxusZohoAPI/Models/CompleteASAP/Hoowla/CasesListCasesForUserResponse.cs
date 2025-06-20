using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesListCasesForUserResponse
    {

        public CasesListCasesForUserResponse()
        {

            Cases = new List<HoowlaListCase>();
        
        }

        public List<HoowlaListCase> Cases { get; set; }

    }

    public class HoowlaListCase
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

        public string address_longitude { get; set; }

        public string address_latitude { get; set; }

        public HoowlaContributor[] contributors { get; set; }

        public HoowlaTitleTenure[] title_tenure { get; set; }

        public HoowlaWorkflow[] workflows { get; set; }

    }

    public class HoowlaContributor
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


    public class HoowlaTitleTenure
    {
        
        public string title_number { get; set; }
        
        public string tenure { get; set; }
    
    }

    public class HoowlaWorkflow
    {
        
        public int? workflow_id { get; set; }
        
        public string workflow_name { get; set; }
    
    }

}
