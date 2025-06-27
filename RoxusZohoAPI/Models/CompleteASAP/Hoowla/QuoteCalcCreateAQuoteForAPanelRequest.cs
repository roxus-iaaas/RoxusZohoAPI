using System;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class QuoteCalcCreateAQuoteForAPanelRequest
    {
        public string form_id { get; set; }
        public int form_type { get; set; }
        public string form_buying_line1 { get; set; }
        public string form_buying_line2 { get; set; }
        public string form_buying_city { get; set; }
        public string form_buying_postcode { get; set; }
        public string form_buying_value { get; set; }
        public int? form_buying_holding { get; set; }
        public object form_buying_mortgaged { get; set; }
        public object form_buying_unregistered { get; set; }
        public object form_buying_shared_ownership { get; set; }
        public object form_buying_new_build { get; set; }
        public object form_buying_help_to_buy_equity_loan { get; set; }
        public object form_buying_help_to_buy_isa { get; set; }
        public object form_buying_buy_to_let { get; set; }
        public object form_buying_first_time_buyer { get; set; }
        public object form_buying_right_to_buy { get; set; }
        public object form_buying_islamic_mortgage { get; set; }
        public object form_buying_self_build { get; set; }
        public string form_selling_line1 { get; set; }
        public string form_selling_line2 { get; set; }
        public string form_selling_city { get; set; }
        public string form_selling_postcode { get; set; }
        public string form_selling_value { get; set; }
        public int? form_selling_holding { get; set; }
        public object form_selling_mortgaged { get; set; }
        public object form_selling_unregistered { get; set; }
        public object form_selling_shared_ownership { get; set; }
        public object form_selling_help_to_buy_equity_loan { get; set; }
        public string form_remortgage_line1 { get; set; }
        public string form_remortgage_line2 { get; set; }
        public string form_remortgage_city { get; set; }
        public string form_remortgage_postcode { get; set; }
        public string form_remortgage_value { get; set; }
        public string form_remortgage_newvalue { get; set; }
        public int? form_remortgage_holding { get; set; }
        public object form_remortgage_unregistered { get; set; }
        public object form_remortgage_shared_ownership { get; set; }
        public object form_remortgage_help_to_buy_equity_loan { get; set; }
        public string form_transfer_line1 { get; set; }
        public string form_transfer_line2 { get; set; }
        public string form_transfer_city { get; set; }
        public string form_transfer_postcode { get; set; }
        public string form_transfer_value { get; set; }
        public int? form_transfer_holding { get; set; }
        public object form_transfer_mortgaged { get; set; }
        public object form_transfer_consideration { get; set; }
        public object form_transfer_to_add { get; set; }
        public object form_transfer_to_remove { get; set; }
        public string form_title { get; set; }
        public string form_fname { get; set; }
        public string form_lname { get; set; }
        public string form_email { get; set; }
        public string form_phone { get; set; }
        public string form_client_address_line1 { get; set; }
        public string form_client_address_line2 { get; set; }
        public string form_client_address_city { get; set; }
        public string form_client_address_county { get; set; }
        public string form_client_address_postcode { get; set; }
        public string form_attached_note { get; set; }
        public int? person_send { get; set; }
    }
}