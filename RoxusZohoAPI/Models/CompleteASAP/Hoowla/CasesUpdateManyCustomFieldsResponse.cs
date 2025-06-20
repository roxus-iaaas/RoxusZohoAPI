using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesUpdateManyCustomFieldsResponse
    {

        public List<string> updated { get; set; }
        
        public List<string> no_change { get; set; }
        
        public object[] error_or_missing { get; set; }
        
        public int? http_code { get; set; }
    
    }

}
