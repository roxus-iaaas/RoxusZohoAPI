using Newtonsoft.Json;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesCreateANewCaseRequest
    {
        public int case_type_id { get; set; }

        public string case_start { get; set; }

        public int person_id { get; set; }

        public int? quote_id { get; set; }

        public int workflow_id { get; set; }

        public string address_line1 { get; set; }

        public string address_line2 { get; set; }

        public string address_city { get; set; }

        public string address_county { get; set; }

        public string address_postcode { get; set; }

        public int address_title_tenure { get; set; }

        public string address_title_number { get; set; }

        public string case_name { get; set; }

        public int? fee_earner_id { get; set; }

        public int? case_worker_id { get; set; }

        public int? supervisor_id { get; set; }

    }

}
