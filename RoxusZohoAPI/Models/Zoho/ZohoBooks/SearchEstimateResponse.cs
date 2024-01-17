using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoBooks
{
    public class SearchEstimateResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public SearchEstimate[] estimates { get; set; }
        public Page_Context page_context { get; set; }
    }

    public class Page_Context
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public bool has_more_page { get; set; }
        public string report_name { get; set; }
        public string applied_filter { get; set; }
        public string sort_column { get; set; }
        public string sort_order { get; set; }
        public Search_Criteria[] search_criteria { get; set; }
    }

    public class Search_Criteria
    {
        public string column_name { get; set; }
        public string search_text { get; set; }
        public string search_text_formatted { get; set; }
        public string comparator { get; set; }
    }

    public class SearchEstimate
    {
        public string estimate_id { get; set; }
        public string zcrm_potential_id { get; set; }
        public string zcrm_potential_name { get; set; }
        public string customer_name { get; set; }
        public string customer_id { get; set; }
        public string company_name { get; set; }
        public string status { get; set; }
        public bool is_digitally_signed { get; set; }
        public bool is_edited_after_sign { get; set; }
        public string color_code { get; set; }
        public string current_sub_status_id { get; set; }
        public string current_sub_status { get; set; }
        public string estimate_number { get; set; }
        public string reference_number { get; set; }
        public string date { get; set; }
        public string currency_id { get; set; }
        public string currency_code { get; set; }
        public bool is_signed_and_accepted { get; set; }
        public float total { get; set; }
        public DateTime created_time { get; set; }
        public DateTime last_modified_time { get; set; }
        public string accepted_date { get; set; }
        public string declined_date { get; set; }
        public string expiry_date { get; set; }
        public bool has_attachment { get; set; }
        public bool is_viewed_by_client { get; set; }
        public string client_viewed_time { get; set; }
        public bool is_emailed { get; set; }
        public string template_type { get; set; }
        public string template_id { get; set; }
        public bool is_signature_enabled_in_template { get; set; }
        public string salesperson_id { get; set; }
        public string salesperson_name { get; set; }
        public string cf_margin { get; set; }
        public float cf_margin_unformatted { get; set; }
        public string cf_invoice_terms { get; set; }
        public string cf_invoice_terms_unformatted { get; set; }
    }
}
