﻿using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class GetPersonByEmailResponse
    {

        public List<EmailPersonDetails> EmailPersons { get; set; }
    }

    public class EmailPersonDetails
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
        public Relationship[] relationships { get; set; }
        public Custom[] custom { get; set; }
        public Case_History[] case_history { get; set; }
    }

    public class Relationship
    {
        public int? relationship_id { get; set; }
        public int? relationship_type { get; set; }
        public string relationship_type_name { get; set; }
        public int? relationship_type_is_business { get; set; }
        public int? person_id { get; set; }
    }

    public class Case_History
    {
        public int? case_id { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string address_city { get; set; }
        public string address_county { get; set; }
        public string address_country { get; set; }
        public string address_postcode { get; set; }
        public string role_name { get; set; }
        public string case_type_name { get; set; }
    }

}
