using Newtonsoft.Json;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class CasesUpdateACaseRequest
    {
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string address_city { get; set; }
        public string address_county { get; set; }
        public string address_postcode { get; set; }
        public string case_name { get; set; }
    }
}
