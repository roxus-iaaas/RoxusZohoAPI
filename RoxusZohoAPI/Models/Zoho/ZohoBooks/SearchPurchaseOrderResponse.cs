using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoBooks
{

    public class SearchPurchaseOrderResponse
    {

        public SearchPurchaseOrderResponse()
        {
            purchaseorders = new List<PurchaseOrderDetail>();
        }

        public int? code { get; set; }

        public string message { get; set; }

        public List<PurchaseOrderDetail> purchaseorders { get; set; }

        public Page_Context page_context { get; set; }

    }

    public class PurchaseOrderDetail
    {

        public string purchaseorder_id { get; set; }

        public string vendor_id { get; set; }

        public string vendor_name { get; set; }

        public string company_name { get; set; }

        public string order_status { get; set; }

        public string billed_status { get; set; }

        public string received_status { get; set; }

        public string status { get; set; }

        public string color_code { get; set; }

        public string current_sub_status_id { get; set; }

        public string current_sub_status { get; set; }

        public string purchaseorder_number { get; set; }

        public string reference_number { get; set; }

        public string date { get; set; }

        public string delivery_date { get; set; }

        public string delivery_days { get; set; }

        public string due_by_days { get; set; }

        public string due_in_days { get; set; }

        public string currency_id { get; set; }

        public string currency_code { get; set; }

        public string price_precision { get; set; }

        public float? total { get; set; }

        public bool? has_attachment { get; set; }

        public DateTime? created_time { get; set; }

        public DateTime? last_modified_time { get; set; }

        public bool? is_drop_shipment { get; set; }

        public float? total_ordered_quantity { get; set; }

        public float? quantity_yet_to_receive { get; set; }

        public float? quantity_marked_as_received { get; set; }

        public bool is_po_marked_as_received { get; set; }

        public bool? is_backorder { get; set; }

        public string cf_related_customer { get; set; }

        public long? cf_related_customer_unformatted { get; set; }

        public string cf_site_contact { get; set; }

        public string cf_site_contact_unformatted { get; set; }

        public object[] receives { get; set; }

        public string client_viewed_time { get; set; }

        public bool? is_viewed_by_client { get; set; }

        public string cf_delivery_office { get; set; }

        public string cf_delivery_office_unformatted { get; set; }

        public string cf_delivery_office_contact { get; set; }

        public string cf_delivery_office_contact_unformatted { get; set; }

    }

}
