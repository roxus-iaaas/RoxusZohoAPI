using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesListCustomFieldsResponse
    {

        public List<HoowlaCustomFieldDetails> CustomFields { get; set; }
    
    }
    
    public class HoowlaCustomFieldDetails
    {

        public int? casedetail_id { get; set; }
        
        public int? casedetail_user { get; set; }
        
        public int? casedetail_casedetail_type { get; set; }
        
        public string casedetail_title { get; set; }
        
        public string casedetail_slug { get; set; }
        
        public object casedetail_value { get; set; }
        
        public int? casedetail_permissions { get; set; }
        
        public object casedetail_person { get; set; }
        
        public string casedetail_created { get; set; }
        
        public string casedetail_edited { get; set; }
        
        public int? casedetail_edited_by { get; set; }
        
        public int? defaultdetail_id { get; set; }
    
    }

}
