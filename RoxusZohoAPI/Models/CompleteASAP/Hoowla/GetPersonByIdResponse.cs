namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class GetPersonByIdResponse
    {

        public int? person_id { get; set; }
        
        public string person_title { get; set; }
        
        public string person_fname { get; set; }
        
        public string person_lname { get; set; }
        
        public object person_dob { get; set; }
        
        public string person_email { get; set; }
        
        public string person_website { get; set; }
        
        public string person_source { get; set; }
        
        public string person_ni { get; set; }
        
        public bool? person_is_company { get; set; }
        
        public string person_company { get; set; }

        public object person_locked_at { get; set; }
        
        public object person_locked_by { get; set; }
        
        public string person_created { get; set; }
        
        public string person_edited { get; set; }
        
        public int? person_type { get; set; }
        
        public string companytype_name { get; set; }
        
        public object gender_id { get; set; }
        
        public object gender_description { get; set; }
        
        public object maritalstatus_id { get; set; }
        
        public object maritalstatus_description { get; set; }
        
        public string address_line1 { get; set; }
        
        public string address_line2 { get; set; }
        
        public string address_city { get; set; }
        
        public string address_county { get; set; }
        
        public string address_postcode { get; set; }
        
        public Phone_Numbers[] phone_numbers { get; set; }
        
        public object[] relationships { get; set; }
        
        public Custom[] custom { get; set; }
    
    }



    public class Phone_Numbers
    {
        
        public string phone_number { get; set; }
        
        public bool? phone_primary { get; set; }
        
        public string phone_type_description { get; set; }
    
    }

    public class Custom
    {
        
        public int? people_detail_type { get; set; }
        
        public string people_detail_type_text { get; set; }
        
        public string people_detail_title { get; set; }
        
        public string people_detail_slug { get; set; }
        
        public object people_detail_value { get; set; }
        
        public int? people_detail_permissions { get; set; }
        
        public string people_detail_created { get; set; }
        
        public string people_detail_edited { get; set; }
    
    }

}
