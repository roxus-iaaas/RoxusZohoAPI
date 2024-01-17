using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets
{
    public class RelatedPurchaseOrdersResponse
    {
        public PurchaseOrderData[] data { get; set; }
        public Info info { get; set; }
    }

    public class Info
    {
        public int? per_page { get; set; }
        public int? count { get; set; }
        public int? page { get; set; }
        public bool? more_records { get; set; }
    }

    public class PurchaseOrderData
    {
        public string currency_symbol { get; set; }
        public object field_states { get; set; }
        public string VAT { get; set; }
        public object Template_Type { get; set; }
        public float? Tax { get; set; }
        public string PO_Date { get; set; }
        public object Additional_Notes { get; set; }
        public string state { get; set; }
        public bool? process_flow { get; set; }
        public float? Exchange_Rate { get; set; }
        public string Currency { get; set; }
        public object Billing_Country { get; set; }
        public string Deliver_To { get; set; }
        public string id { get; set; }
        public bool? approved { get; set; }
        public string Status { get; set; }
        public float? Grand_Total { get; set; }
        public string PO_Number { get; set; }
        public object Billing_Street { get; set; }
        public int? Adjustment { get; set; }
        public bool? editable { get; set; }
        public object Billing_Code { get; set; }
        public Product_Details[] Product_Details { get; set; }
        public Deal Deal { get; set; }
        public string Shipping_City { get; set; }
        public string Shipping_Country { get; set; }
        public string Shipping_Code { get; set; }
        public object Billing_City { get; set; }
        public Created_By Created_By { get; set; }
        public string Shipping_Street { get; set; }
        public object Description { get; set; }
        public int? Discount { get; set; }
        public Vendor_Name Vendor_Name { get; set; }
        public object Shipment_Preference { get; set; }
        public object Related_Project { get; set; }
        public string Shipping_State { get; set; }
        public Review_Process review_process { get; set; }
        public Modified_By Modified_By { get; set; }
        public object review { get; set; }
        public DateTime Modified_Time { get; set; }
        public string Item_Rates_Are { get; set; }
        public object Due_Date { get; set; }
        public object Terms_and_Conditions { get; set; }
        public float? Sub_Total { get; set; }
        public string Subject { get; set; }
        public bool? orchestration { get; set; }
        public object Contact_Name { get; set; }
        public bool? in_merge { get; set; }
        public Related_Customer Related_Customer { get; set; }
        public object Billing_State { get; set; }
        public object[] line_tax { get; set; }
        public object[] Tag { get; set; }
        public string approval_state { get; set; }
    }

    public class Owner
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Deal
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Created_By
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Vendor_Name
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Modified_By
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Related_Customer
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Product_Details
    {
        public Product product { get; set; }
        public float? quantity { get; set; }
        public float? Discount { get; set; }
        public float? total_after_discount { get; set; }
        public float? net_total { get; set; }
        public object book { get; set; }
        public float? Tax { get; set; }
        public float? list_price { get; set; }
        public float? unit_price { get; set; }
        public float? quantity_in_stock { get; set; }
        public float? total { get; set; }
        public string id { get; set; }
        public string product_description { get; set; }
        public object[] line_tax { get; set; }
    }

    public class Product
    {
        public object Product_Code { get; set; }
        public string Currency { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }
}
