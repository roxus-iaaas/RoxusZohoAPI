using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesListCaseTypesResponse
    {
    
        public List<HoowlaCaseTypeDetails> CaseTypes { get; set; }
    
    }

    public class HoowlaCaseTypeDetails
    {

        public int? case_type_id { get; set; }
        
        public string case_type_name { get; set; }
    
    }


}
