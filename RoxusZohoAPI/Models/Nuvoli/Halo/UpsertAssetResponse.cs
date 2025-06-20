using System;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class UpsertAssetResponse
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
        public int? technical_owner_id { get; set; }
        public string technical_owner_name { get; set; }
        public int? technical_owner_cab_id { get; set; }
        public int? assettype_id { get; set; }
        public string assettype_name { get; set; }
        public string colour { get; set; }
        public bool? inactive { get; set; }
        public string contract_ref { get; set; }
        public int? supplier_id { get; set; }
        public string supplier_name { get; set; }
        public int? supplier_contract_id { get; set; }
        public string supplier_contract_ref { get; set; }
        public int? supplier_sla_id { get; set; }
        public int? supplier_priority_id { get; set; }
        public Field[] fields { get; set; }
        public Customfield[] customfields { get; set; }
        public object[] custombuttons { get; set; }
        public int? itemstock_id { get; set; }
        public int? item_id { get; set; }
        public bool? non_consignable { get; set; }
        public int? reserved_salesorder_id { get; set; }
        public int? reserved_salesorder_line_id { get; set; }
        public int? device42_id { get; set; }
        public int? criticality { get; set; }
        public string import_guid { get; set; }
        public int? contract_id { get; set; }
        public int? goodsin_po_id { get; set; }
        public int? commissioned { get; set; }
        public User[] users { get; set; }
        public string intune_id { get; set; }
        public int? prtg_id { get; set; }
        public int? prtg_details_id { get; set; }
        public string status_name { get; set; }
        public string ateraid { get; set; }
        public string lansweeper_id { get; set; }
        public string lansweeper_url { get; set; }
        public DateTime? dlastupdate { get; set; }
        public decimal? item_cost { get; set; }
        public string itglue_id { get; set; }
        public bool? bookmarked { get; set; }
        public string auvik_network_id { get; set; }
        public string qualys_id { get; set; }
        public Assettype_Config assettype_config { get; set; }
        public string azureTenantId { get; set; }
        public int? access_control_level { get; set; }
        public DateTime? last_modified { get; set; }
        public bool? is_stock_site { get; set; }
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
        public string datto_url { get; set; }
        public int? ncentral_details_id { get; set; }
        public int? sla_id { get; set; }
        public int? priority_id { get; set; }
        public bool? is_template { get; set; }
    }

    public class Assettype_Config
    {
        public string guid { get; set; }
        public int? id { get; set; }
        public string name { get; set; }
        public int? assetgroup_id { get; set; }
        public int? keyfield_id { get; set; }
        public int? keyfield2_id { get; set; }
        public int? keyfield3_id { get; set; }
        public int? manufacturer { get; set; }
        public int? supplier1 { get; set; }
        public bool? useteamviewer { get; set; }
        public decimal? weeklycost { get; set; }
        public decimal? monthlycost { get; set; }
        public decimal? quarterlycost { get; set; }
        public decimal? sixmonthlycost { get; set; }
        public decimal? yearlycost { get; set; }
        public decimal? twoyearlycost { get; set; }
        public decimal? threeyearlycost { get; set; }
        public decimal? fouryearlycost { get; set; }
        public decimal? fiveyearlycost { get; set; }
        public bool? show_to_users { get; set; }
        public string item_code { get; set; }
        public int? defaultsequence { get; set; }
        public string tagprefix { get; set; }
        public int? column_profile_id { get; set; }
        public bool? enableresourcebooking { get; set; }
        public int? resourcebooking_workdays_id { get; set; }
        public bool? resourcebooking_allow_asset_selection { get; set; }
        public int? resourcebooking_asset_restriction_type { get; set; }
        public decimal? resourcebooking_min_hours_advance { get; set; }
        public decimal? resourcebooking_max_days_advance { get; set; }
        public object[] bookingtypes { get; set; }
        public int? linkedcontracttype { get; set; }
        public int? fiid { get; set; }
        public object[] xtype_roles { get; set; }
        public int? asset_details_tab_display { get; set; }
        public bool? allowall_status { get; set; }
        public int? notes_visibility { get; set; }
        public int? visibility_level { get; set; }
        public int? resourcebooking_site_selection_type { get; set; }
        public int? access_control_level { get; set; }
        public int? licence_visibility { get; set; }
        public bool? allow_all_view { get; set; }
        public int? criticality_visibility { get; set; }
        public bool? is_service { get; set; }
        public bool? is_businessapp { get; set; }
    }

}
