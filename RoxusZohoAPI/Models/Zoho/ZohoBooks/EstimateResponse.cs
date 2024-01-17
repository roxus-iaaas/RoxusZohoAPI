using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoBooks
{
    public class EstimateResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public EstimateDetails estimate { get; set; }
    }

    public class EstimateDetails
    {
        public string estimate_id { get; set; }
        public string estimate_number { get; set; }
        public string zcrm_potential_id { get; set; }
        public string zcrm_potential_name { get; set; }
        public string date { get; set; }
        public string created_date { get; set; }
        public string reference_number { get; set; }
        public string status { get; set; }
        public string color_code { get; set; }
        public string current_sub_status_id { get; set; }
        public string current_sub_status { get; set; }
        public object[] sub_statuses { get; set; }
        public string customer_id { get; set; }
        public object[] documents { get; set; }
        public string customer_name { get; set; }
        public bool is_signed_and_accepted { get; set; }
        public bool is_transaction_created { get; set; }
        public bool is_converted_to_open { get; set; }
        public string vat_treatment { get; set; }
        public string contact_category { get; set; }
        public string tax_treatment { get; set; }
        public string[] contact_persons { get; set; }
        public string currency_id { get; set; }
        public string currency_code { get; set; }
        public string currency_symbol { get; set; }
        public float exchange_rate { get; set; }
        public string expiry_date { get; set; }
        public float discount_amount { get; set; }
        public float discount { get; set; }
        public float discount_applied_on_amount { get; set; }
        public bool is_discount_before_tax { get; set; }
        public string discount_type { get; set; }
        public bool is_viewed_by_client { get; set; }
        public string client_viewed_time { get; set; }
        public bool is_inclusive_tax { get; set; }
        public string tax_rounding { get; set; }
        public bool is_digitally_signed { get; set; }
        public bool is_edited_after_sign { get; set; }
        public string estimate_url { get; set; }
        public bool is_reverse_charge_applied { get; set; }
        public Line_Items[] line_items { get; set; }
        public string submitter_id { get; set; }
        public string submitted_date { get; set; }
        public string submitted_by { get; set; }
        public string approver_id { get; set; }
        public string shipping_charge_tax_id { get; set; }
        public string shipping_charge_tax_name { get; set; }
        public string shipping_charge_tax_type { get; set; }
        public string shipping_charge_tax_percentage { get; set; }
        public string shipping_charge_tax { get; set; }
        public string bcy_shipping_charge_tax { get; set; }
        public float shipping_charge_exclusive_of_tax { get; set; }
        public float shipping_charge_inclusive_of_tax { get; set; }
        public string shipping_charge_tax_formatted { get; set; }
        public string shipping_charge_exclusive_of_tax_formatted { get; set; }
        public string shipping_charge_inclusive_of_tax_formatted { get; set; }
        public float shipping_charge { get; set; }
        public float bcy_shipping_charge { get; set; }
        public float adjustment { get; set; }
        public float bcy_adjustment { get; set; }
        public string adjustment_description { get; set; }
        public float roundoff_value { get; set; }
        public string transaction_rounding_type { get; set; }
        public float sub_total { get; set; }
        public float bcy_sub_total { get; set; }
        public float sub_total_inclusive_of_tax { get; set; }
        public float sub_total_exclusive_of_discount { get; set; }
        public float discount_total { get; set; }
        public float bcy_discount_total { get; set; }
        public float discount_percent { get; set; }
        public float total { get; set; }
        public float bcy_total { get; set; }
        public float tax_total { get; set; }
        public float bcy_tax_total { get; set; }
        public float reverse_charge_tax_total { get; set; }
        public int price_precision { get; set; }
        public Tax[] taxes { get; set; }
        public object[] invoice_ids { get; set; }
        public Billing_Address billing_address { get; set; }
        public Shipping_Address shipping_address { get; set; }
        public Customer_Default_Billing_Address customer_default_billing_address { get; set; }
        public string notes { get; set; }
        public string terms { get; set; }
        public Custom_Fields[] custom_fields { get; set; }
        public Custom_Field_Hash custom_field_hash { get; set; }
        public string template_id { get; set; }
        public string template_name { get; set; }
        public string template_type { get; set; }
        public bool is_signature_enabled_in_template { get; set; }
        public string page_width { get; set; }
        public string page_height { get; set; }
        public string orientation { get; set; }
        public DateTime created_time { get; set; }
        public DateTime last_modified_time { get; set; }
        public string created_by_id { get; set; }
        public string last_modified_by_id { get; set; }
        public Contact_Persons_Details[] contact_persons_details { get; set; }
        public string salesperson_id { get; set; }
        public string salesperson_name { get; set; }
        public string attachment_name { get; set; }
        public bool can_send_in_mail { get; set; }
        public bool can_send_estimate_sms { get; set; }
        public bool allow_partial_payments { get; set; }
        public Payment_Options payment_options { get; set; }
        public object[] retainerinvoices { get; set; }
        public bool accept_retainer { get; set; }
        public string retainer_percentage { get; set; }
        public string subject_content { get; set; }
        public Approvers_List[] approvers_list { get; set; }
    }

    public class Shipping_Address
    {
        public string address { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string fax { get; set; }
        public string phone { get; set; }
        public string attention { get; set; }
    }

    public class Customer_Default_Billing_Address
    {
        public string zip { get; set; }
        public string country { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string street2 { get; set; }
        public string state { get; set; }
        public string fax { get; set; }
        public string state_code { get; set; }
    }

    public class Payment_Options
    {
        public object[] payment_gateways { get; set; }
    }

    public class Package_Details
    {
        public string length { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string weight_unit { get; set; }
        public string dimension_unit { get; set; }
    }

    public class Item_Custom_Fields
    {
        public string customfield_id { get; set; }
        public bool show_in_store { get; set; }
        public bool show_in_portal { get; set; }
        public bool is_active { get; set; }
        public int index { get; set; }
        public string label { get; set; }
        public bool show_on_pdf { get; set; }
        public bool edit_on_portal { get; set; }
        public bool edit_on_store { get; set; }
        public bool show_in_all_pdf { get; set; }
        public string value_formatted { get; set; }
        public string search_entity { get; set; }
        public string data_type { get; set; }
        public string placeholder { get; set; }
        public float value { get; set; }
        public bool is_dependent_field { get; set; }
    }

    public class Tax
    {
        public string tax_name { get; set; }
        public float tax_amount { get; set; }
    }

    public class Contact_Persons_Details
    {
        public string contact_person_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public bool is_primary_contact { get; set; }
        public string photo_url { get; set; }
    }
}
