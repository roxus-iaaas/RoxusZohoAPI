using System.Collections.Generic;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class ListAssetsResponse
    {

        public ListAssetsResponse()
        {
            
            assets = new List<Asset>();

        }

        public int? page_no { get; set; }
        
        public int? page_size { get; set; }
        
        public int? record_count { get; set; }
        
        public List<Asset> assets { get; set; }
    
    }

    public class Asset
    {

        public int? id { get; set; }
        
        public string changeguid { get; set; }
        
        public string inventory_number { get; set; }
        
        public string key_field { get; set; }
        
        public string key_field2 { get; set; }
        
        public string key_field3 { get; set; }
        
        public int? client_id { get; set; }
        
        public string client_name { get; set; }
        
        public int? site_id { get; set; }
        
        public string site_name { get; set; }
        
        public int? business_owner_id { get; set; }
        
        public string business_owner_name { get; set; }
        
        public int? business_owner_cab_id { get; set; }
        
        public string username { get; set; }
        
        public int? technical_owner_id { get; set; }
        
        public string technical_owner_name { get; set; }
        
        public int? technical_owner_cab_id { get; set; }
        
        public int? assettype_id { get; set; }
        
        public string assettype_name { get; set; }
        
        public string colour { get; set; }
        
        public bool? inactive { get; set; }
        
        public int? supplier_id { get; set; }
        
        public int? supplier_contract_id { get; set; }
        
        public int? supplier_sla_id { get; set; }
        
        public int? supplier_priority_id { get; set; }
        
        public int? itemstock_id { get; set; }
        
        public int? item_id { get; set; }
        
        public bool? non_consignable { get; set; }
        
        public int? reserved_salesorder_id { get; set; }
        
        public int? reserved_salesorder_line_id { get; set; }
        
        public int? device42_id { get; set; }
        
        public int? criticality { get; set; }
        
        public string use { get; set; }
        
        public int? device_number { get; set; }
        
        public int? status_id { get; set; }
        
        public int? third_party_id { get; set; }
        
        public int? automate_id { get; set; }
        
        public int? ninjarmm_id { get; set; }
        
        public int? syncroid { get; set; }
        
        public string itglue_url { get; set; }
        
        public int? defaultsequence { get; set; }
        
        public int? datto_alternate_id { get; set; }
        
        public int? passportal_id { get; set; }
        
        public string auvik_device_id { get; set; }
        
        public string datto_id { get; set; }
        
        public string addigy_id { get; set; }
        
        public string stockbin_name { get; set; }
        
        public int? issue_consignment_line_id { get; set; }
        
        public string item_name { get; set; }
        
        public string datto_url { get; set; }
        
        public int? ncentral_details_id { get; set; }
        
        public int? sla_id { get; set; }
        
        public int? priority_id { get; set; }
        
        public bool? is_template { get; set; }
    
    }

}
