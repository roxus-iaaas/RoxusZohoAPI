using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoBooks
{

    public class GetItemsResponse
    {

        public int code { get; set; }

        public string message { get; set; }

        public Item[] items { get; set; }

        public Page_Context page_context { get; set; }

    }

    public class Item
    {

        public string group_id { get; set; }

        public string group_name { get; set; }

        public string item_id { get; set; }

        public string name { get; set; }

        public string item_name { get; set; }

        public string unit { get; set; }

        public string status { get; set; }

        public string source { get; set; }

        public bool? is_combo_product { get; set; }

        public bool? is_linked_with_zohocrm { get; set; }

        public string zcrm_product_id { get; set; }

        public string description { get; set; }

        public string brand { get; set; }

        public string manufacturer { get; set; }

        public double? rate { get; set; }

        public string tax_id { get; set; }

        public string tax_name { get; set; }

        public decimal? tax_percentage { get; set; }

        public string purchase_account_id { get; set; }

        public string purchase_account_name { get; set; }

        public string account_id { get; set; }

        public string account_name { get; set; }

        public string purchase_description { get; set; }

        public double? purchase_rate { get; set; }

        public string item_type { get; set; }

        public string product_type { get; set; }

        public double? stock_on_hand { get; set; }

        public bool? has_attachment { get; set; }

        public bool? is_returnable { get; set; }

        public decimal? available_stock { get; set; }

        public decimal? actual_available_stock { get; set; }

        public string attribute_id1 { get; set; }

        public string attribute_id2 { get; set; }

        public string attribute_id3 { get; set; }

        public string attribute_name1 { get; set; }

        public string attribute_name2 { get; set; }

        public string attribute_name3 { get; set; }

        public string attribute_type1 { get; set; }

        public string attribute_type2 { get; set; }

        public string attribute_type3 { get; set; }

        public string attribute_option_id1 { get; set; }

        public string attribute_option_id2 { get; set; }

        public string attribute_option_id3 { get; set; }

        public string attribute_option_name1 { get; set; }

        public string attribute_option_name2 { get; set; }

        public string attribute_option_name3 { get; set; }

        public string attribute_option_data1 { get; set; }

        public string attribute_option_data2 { get; set; }

        public string attribute_option_data3 { get; set; }

        public string sku { get; set; }

        public string upc { get; set; }

        public string ean { get; set; }

        public string isbn { get; set; }

        public string part_number { get; set; }

        public object reorder_level { get; set; }

        public string image_name { get; set; }

        public string image_type { get; set; }

        public string image_document_id { get; set; }

        public DateTime? created_time { get; set; }

        public DateTime? last_modified_time { get; set; }

        public bool? show_in_storefront { get; set; }

        public string cf_margin { get; set; }

        public decimal? cf_margin_unformatted { get; set; }

        public string length { get; set; }

        public string width { get; set; }

        public string height { get; set; }

        public string weight { get; set; }


        public string weight_unit { get; set; }

        public string dimension_unit { get; set; }

    }

}
