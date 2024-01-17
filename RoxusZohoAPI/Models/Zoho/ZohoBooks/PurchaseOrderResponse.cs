using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoBooks
{
    public class PurchaseOrderResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public PurchaseorderDetail purchaseorder { get; set; }
    }

    public class PurchaseorderDetail
    {
        public string purchaseorder_id { get; set; }
        public object[] documents { get; set; }
        public string vat_treatment { get; set; }
        public string tax_treatment { get; set; }
        public string contact_category { get; set; }
        public string purchaseorder_number { get; set; }
        public string date { get; set; }
        public string client_viewed_time { get; set; }
        public bool? is_viewed_by_client { get; set; }
        public string expected_delivery_date { get; set; }
        public string reference_number { get; set; }
        public string status { get; set; }
        public string order_status { get; set; }
        public string signed_document_id { get; set; }
        public bool? is_digitally_signed { get; set; }
        public bool? is_edited_after_sign { get; set; }
        public bool? is_signature_enabled_in_template { get; set; }
        public string received_status { get; set; }
        public string billed_status { get; set; }
        public string color_code { get; set; }
        public string current_sub_status_id { get; set; }
        public string current_sub_status { get; set; }
        public object[] sub_statuses { get; set; }
        public string vendor_id { get; set; }
        public string vendor_name { get; set; }
        public string[] contact_persons { get; set; }
        public string currency_id { get; set; }
        public string currency_code { get; set; }
        public string currency_symbol { get; set; }
        public float? exchange_rate { get; set; }
        public string delivery_date { get; set; }
        public bool? is_emailed { get; set; }
        public bool? is_drop_shipment { get; set; }
        public bool? is_inclusive_tax { get; set; }
        public string tax_rounding { get; set; }
        public bool? is_reverse_charge_applied { get; set; }
        public bool? is_adv_tracking_in_receive { get; set; }
        public bool? is_backorder { get; set; }
        public float? total_quantity { get; set; }
        public Line_Items[] line_items { get; set; }
        public bool? has_qty_cancelled { get; set; }
        public float? adjustment { get; set; }
        public string adjustment_description { get; set; }
        public float? discount_amount { get; set; }
        public float? discount { get; set; }
        public float? discount_applied_on_amount { get; set; }
        public bool? is_discount_before_tax { get; set; }
        public string discount_account_id { get; set; }
        public string discount_type { get; set; }
        public float? sub_total { get; set; }
        public float? sub_total_inclusive_of_tax { get; set; }
        public float? tax_total { get; set; }
        public float? total { get; set; }
        public object[] taxes { get; set; }
        public float? price_precision { get; set; }
        public string submitted_date { get; set; }
        public string submitted_by { get; set; }
        public string submitter_id { get; set; }
        public string approver_id { get; set; }
        public Approvers_List[] approvers_list { get; set; }
        public string billing_address_id { get; set; }
        public Billing_Address billing_address { get; set; }
        public string notes { get; set; }
        public string terms { get; set; }
        public float? payment_terms { get; set; }
        public string payment_terms_label { get; set; }
        public string ship_via { get; set; }
        public string ship_via_id { get; set; }
        public string attention { get; set; }
        public string delivery_org_address_id { get; set; }
        public string delivery_customer_id { get; set; }
        public string delivery_customer_address_id { get; set; }
        public Delivery_Address delivery_address { get; set; }
        public Custom_Fields[] custom_fields { get; set; }
        public Custom_Field_Hash custom_field_hash { get; set; }
        public string attachment_name { get; set; }
        public bool can_send_in_mail { get; set; }
        public string template_id { get; set; }
        public string template_name { get; set; }
        public string page_width { get; set; }
        public string page_height { get; set; }
        public string orientation { get; set; }
        public string template_type { get; set; }
        public DateTime? created_time { get; set; }
        public string created_by_id { get; set; }
        public DateTime? last_modified_time { get; set; }
        public bool? can_mark_as_bill { get; set; }
        public bool? can_mark_as_unbill { get; set; }
        public object[] purchasereceives { get; set; }
        public object[] salesorders { get; set; }
        public object[] bills { get; set; }
    }

    public class Billing_Address
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

    public class Delivery_Address
    {
        public string zip { get; set; }
        public string country { get; set; }
        public string address { get; set; }
        public string organization_address_id { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string address1 { get; set; }
        public string state { get; set; }
    }

    public class Custom_Field_Hash
    {
        public string cf_site_contact { get; set; }
        public string cf_site_contact_unformatted { get; set; }
    }

    public class Line_Items
    {
        public string item_id { get; set; }
        public string line_item_id { get; set; }
        public string image_document_id { get; set; }
        public string sku { get; set; }
        public string warehouse_id { get; set; }
        public string warehouse_name { get; set; }
        public string account_id { get; set; }
        public string account_name { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? item_order { get; set; }
        public float bcy_rate { get; set; }
        public string pricebook_id { get; set; }
        public string header_id { get; set; }
        public string header_name { get; set; }
        public float? rate { get; set; }
        public float? quantity { get; set; }
        public float? discount { get; set; }
        public object[] discounts { get; set; }
        public float? quantity_received { get; set; }
        public float? quantity_manually_received { get; set; }
        public float? quantity_cancelled { get; set; }
        public float? quantity_billed { get; set; }
        public string unit { get; set; }
        public float? item_total { get; set; }
        public string tax_id { get; set; }
        public string tax_name { get; set; }
        public string tax_type { get; set; }
        public float? tax_percentage { get; set; }
        public string product_type { get; set; }
        public string item_type { get; set; }
        public object[] tags { get; set; }
        public object[] item_custom_fields { get; set; }
        public string project_id { get; set; }
        public string image_name { get; set; }
        public string image_type { get; set; }
        public object[] purchase_request_items { get; set; }
        public bool is_combo_product { get; set; }
    }

    public class Approvers_List
    {
        public int? order { get; set; }
        public string approver_user_id { get; set; }
        public string approver_name { get; set; }
        public string email { get; set; }
        public bool? has_approved { get; set; }
        public string approval_status { get; set; }
        public bool? is_next_approver { get; set; }
        public string submitted_date { get; set; }
        public string approved_date { get; set; }
        public string photo_url { get; set; }
        public string user_status { get; set; }
        public bool? is_final_approver { get; set; }
        public string[] available_apps { get; set; }
        public string zuid { get; set; }
    }

    public class Custom_Fields
    {
        public string customfield_id { get; set; }
        public bool? show_in_store { get; set; }
        public bool? show_in_portal { get; set; }
        public bool? is_active { get; set; }
        public float? index { get; set; }
        public string label { get; set; }
        public bool? show_on_pdf { get; set; }
        public bool? edit_on_portal { get; set; }
        public bool? edit_on_store { get; set; }
        public bool? show_in_all_pdf { get; set; }
        public string selected_option_id { get; set; }
        public string value_formatted { get; set; }
        public string search_entity { get; set; }
        public string data_type { get; set; }
        public string placeholder { get; set; }
        public string value { get; set; }
        public bool? is_dependent_field { get; set; }
    }
}
