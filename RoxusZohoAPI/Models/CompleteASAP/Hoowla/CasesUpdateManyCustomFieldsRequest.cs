using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesUpdateManyCustomFieldsRequest
    {

        public CasesUpdateManyCustomFieldsRequest()
        {
            
            casedetail = new List<CaseDetail>();

        }

        public List<CaseDetail> casedetail { get; set; }

    }

    public class CaseDetail
    {

        public string slug { get; set; }
        
        public string value { get; set; }
    
    }


}
