using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoBooks
{


    public class GetCompositeItemByIdResponse
    {
        public int? code { get; set; }
        public string message { get; set; }
        public Composite_Item composite_item { get; set; }
    }

    public class Composite_Item
    {
        public string composite_item_id { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public string category_id { get; set; }
        public string page_layout_id { get; set; }
        public string offline_created_date_with_time { get; set; }
        public bool? show_in_storefront { get; set; }
        public string brand { get; set; }
        public string manufacturer { get; set; }
        public string url { get; set; }
        public string product_description { get; set; }
        public object[] product_tags { get; set; }
        public string product_short_description { get; set; }
        public string category_name { get; set; }
        public object[] documents { get; set; }
        public string description { get; set; }
        public string tax_id { get; set; }
        public string tax_name { get; set; }
        public int? tax_percentage { get; set; }
        public string tax_type { get; set; }
        public string product_type { get; set; }
        public string purchase_account_id { get; set; }
        public string purchase_account_name { get; set; }
        public string account_id { get; set; }
        public string account_name { get; set; }
        public string inventory_account_id { get; set; }
        public string inventory_account_name { get; set; }
        public object[] tags { get; set; }
        public string status { get; set; }
        public string source { get; set; }
        public bool? is_combo_product { get; set; }
        public bool? is_boxing_exist { get; set; }
        public string item_type { get; set; }
        public double? rate { get; set; }
        public double? label_rate { get; set; }
        public double? pricebook_rate { get; set; }
        public double? purchase_rate { get; set; }
        public double? reorder_level { get; set; }
        public bool? is_returnable { get; set; }
        public double? initial_stock { get; set; }
        public double? initial_stock_rate { get; set; }
        public double? total_initial_stock { get; set; }
        public string vendor_id { get; set; }
        public string vendor_name { get; set; }
        public double? stock_on_hand { get; set; }
        public string asset_value { get; set; }
        public double? available_stock { get; set; }
        public double? actual_available_stock { get; set; }
        public double? committed_stock { get; set; }
        public double? actual_committed_stock { get; set; }
        public double? available_for_sale_stock { get; set; }
        public double? actual_available_for_sale_stock { get; set; }
        public string sku { get; set; }
        public string upc { get; set; }
        public string ean { get; set; }
        public string isbn { get; set; }
        public string part_number { get; set; }
        public string image_document_id { get; set; }
        public string image_name { get; set; }
        public string purchase_description { get; set; }
        public Custom_Fields[] custom_fields { get; set; }
        public Custom_Field_Hash custom_field_hash { get; set; }
        public string seo_description { get; set; }
        public string seo_keyword { get; set; }
        public string seo_title { get; set; }
        public object[] specifications { get; set; }
        public string specificationset_id { get; set; }
        public string specificationset_name { get; set; }
        public DateTime? last_modified_time { get; set; }
        public Package_Details package_details { get; set; }
        public Mapped_Items[] mapped_items { get; set; }
        public object[] sales_channels { get; set; }
        public Composite_Component_Items[] composite_component_items { get; set; }
        public object[] composite_service_items { get; set; }
        public Warehouse[] warehouses { get; set; }
        public Preferred_Vendors[] preferred_vendors { get; set; }
    }

    public class Mapped_Items
    {
        public string mapped_item_id { get; set; }
        public string item_id { get; set; }
        public int item_order { get; set; }
        public string name { get; set; }
        public double? rate { get; set; }
        public double? purchase_rate { get; set; }
        public bool? is_combo_product { get; set; }
        public string sku { get; set; }
        public int? status { get; set; }
        public string image_name { get; set; }
        public string image_document_id { get; set; }
        public string purchase_description { get; set; }
        public string image_type { get; set; }
        public double? pricebook_rate { get; set; }
        public object[] price_brackets { get; set; }
        public string product_type { get; set; }
        public string unit { get; set; }
        public string description { get; set; }
        public double? quantity { get; set; }
        public double? stock_on_hand { get; set; }
        public double? available_stock { get; set; }
        public double? actual_available_stock { get; set; }
    }

    public class Composite_Component_Items
    {
        public string mapped_item_id { get; set; }
        public string item_id { get; set; }
        public int item_order { get; set; }
        public string name { get; set; }
        public float rate { get; set; }
        public float purchase_rate { get; set; }
        public string sku { get; set; }
        public int status { get; set; }
        public string image_name { get; set; }
        public string image_document_id { get; set; }
        public string purchase_description { get; set; }
        public string image_type { get; set; }
        public string unit { get; set; }
        public string product_type { get; set; }
        public bool is_combo_product { get; set; }
        public string description { get; set; }
        public float quantity { get; set; }
        public float stock_on_hand { get; set; }
        public float available_stock { get; set; }
        public float actual_available_stock { get; set; }
    }

    public class Warehouse
    {
        public string warehouse_id { get; set; }
        public string warehouse_name { get; set; }
        public bool? is_primary { get; set; }
        public string status { get; set; }
        public double? warehouse_stock_on_hand { get; set; }
        public int? initial_stock { get; set; }
        public double? initial_stock_rate { get; set; }
        public double? warehouse_available_stock { get; set; }
        public double? warehouse_actual_available_stock { get; set; }
        public double? warehouse_committed_stock { get; set; }
        public double? warehouse_actual_committed_stock { get; set; }
        public double? warehouse_available_for_sale_stock { get; set; }
        public double? warehouse_actual_available_for_sale_stock { get; set; }
    }

    public class Preferred_Vendors
    {
        public string vendor_id { get; set; }
        public string vendor_name { get; set; }
        public bool? is_primary { get; set; }
        public double? item_stock { get; set; }
        public double? item_price { get; set; }
        public DateTime last_modified_time { get; set; }
    }

}
