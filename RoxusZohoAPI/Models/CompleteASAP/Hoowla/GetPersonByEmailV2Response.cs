using System.Collections.Generic;
using System.Text.Json;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class GetPersonByEmailV2Response
    {

        public List<EmailPersonDetailsV2> EmailPersons { get; set; }
    }

    public class EmailPersonDetailsV2
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
        public object person_edited { get; set; }
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
        public RelationshipV2[] relationships { get; set; }
        public Custom[] custom { get; set; }
        public object[] case_history { get; set; }
        public int? case_number { get; set; }
    }

    public class RelationshipV2
    {
        public int? relationship_id { get; set; }
        public int? relationship_type { get; set; }
        public string relationship_type_name { get; set; }
        public int? relationship_type_is_business { get; set; }
        public int? person_id { get; set; }
    }
}
