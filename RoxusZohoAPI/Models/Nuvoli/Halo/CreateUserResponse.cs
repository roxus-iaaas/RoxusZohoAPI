using System;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{
    public class CreateUserResponse
    {
        
        public bool? is_comms_user { get; set; }
        
        public bool? ischangeapprover2 { get; set; }
        
        public int? id { get; set; }
        
        public string name { get; set; }
        
        public decimal? site_id { get; set; }
        
        public int? site_id_int { get; set; }
        
        public string site_name { get; set; }
        
        public string client_name { get; set; }
        
        public string firstname { get; set; }
        
        public string surname { get; set; }
        
        public string initials { get; set; }
        
        public string emailaddress { get; set; }
        
        public string sitephonenumber { get; set; }
        
        public int? telpref { get; set; }
        
        public bool? inactive { get; set; }
        
        public string colour { get; set; }
        
        public bool? isimportantcontact { get; set; }
        
        public bool? neversendemails { get; set; }
        
        public int? priority_id { get; set; }
        
        public int? linked_agent_id { get; set; }
        
        public Customfield1[] customfields { get; set; }
        
        public object[] custombuttons { get; set; }
        
        public bool? isserviceaccount { get; set; }
        
        public bool? ignoreautomatedbilling { get; set; }
        
        public bool? isimportantcontact2 { get; set; }
        
        public int? connectwiseid { get; set; }
        
        public int? autotaskid { get; set; }
        
        public int? messagegroup_id { get; set; }
        
        public string sitetimezone { get; set; }
        
        public int? client_account_manager_id { get; set; }
        
        public DateTime? datecreated { get; set; }
        
        public bool? dontackemails { get; set; }
        
        public int? web_access_level { get; set; }
        
        public int? showmeonly { get; set; }
        
        public int? showrecentonly { get; set; }
        
        public bool? inform { get; set; }
        
        public bool? informaction { get; set; }
        
        public bool? informclearance { get; set; }
        
        public bool? showslatimes { get; set; }
        
        public bool? canadd { get; set; }
        
        public bool? allowviewsitedocs { get; set; }
        
        public bool? ischangeapprover { get; set; }
        
        public bool? cancreateuser { get; set; }
        
        public bool? isheadofdept { get; set; }
        
        public bool? iscontractcontact { get; set; }
        
        public bool? informnewarea { get; set; }
        
        public bool? informactionarea { get; set; }
        
        public bool? informclearancearea { get; set; }
        
        public bool? viewallcleared { get; set; }
        
        public int? cataloglevel { get; set; }
        
        public bool? canaccesscatalog { get; set; }
        
        public bool? canviewcontracts { get; set; }
        
        public bool? ispoapprover { get; set; }
        
        public int? encmethod { get; set; }
        
        public int? adconnection { get; set; }
        
        public int? useadlogin { get; set; }
        
        public Site site { get; set; }
        
        public bool? ismaincontact { get; set; }
        
        public bool? _isnew { get; set; }
        
        public bool? delegation_activated { get; set; }
        
        public bool? delegation_timebased { get; set; }
        
        public DateTime? delegation_start { get; set; }
        
        public DateTime? delegation_end { get; set; }
        
        public int? delegation_user_id { get; set; }
        
        public string googleworkplace_id { get; set; }
        
        public bool? isnhserveremaildefault { get; set; }
        
        public string servicenow_id { get; set; }
        
        public string servicenow_username { get; set; }
        
        public string sgatewayid { get; set; }
        
        public bool? canaccessinvoices { get; set; }
        
        public string samaccountname { get; set; }
        
        public string oktaid { get; set; }
        
        public DateTime? ulastupdate { get; set; }
        
        public bool? locked { get; set; }
        
        public bool? allowviewclientdocs { get; set; }
        
        public string azure_tenant_id { get; set; }
        
        public DateTime? azure_last_login_date { get; set; }
        
        public int? linked_user_id { get; set; }
        
        public string hubspot_id { get; set; }
        
        public string hubspot_url { get; set; }
        
        public bool? hubspot_dont_sync { get; set; }
        
        public bool? hubspot_archived { get; set; }
        
        public int? passportal_id { get; set; }
        
        public int? opportunity_id { get; set; }
        
        public bool? unsubscribed { get; set; }
        
        public bool? canviewopps { get; set; }
        
        public string azure_tenant_domain { get; set; }
        
        public object[] external_links { get; set; }
        
        public bool? salesforce_dontsync { get; set; }
        
        public int? facebook_id { get; set; }
        
        public string facebook_username { get; set; }
        
        public int? twitter_id { get; set; }
        
        public string twitter_username { get; set; }
        
        public int? device_access_level { get; set; }
        
        public bool? dontackemails2 { get; set; }
        
        public int? instagram_id { get; set; }
        
        public string instagram_username { get; set; }
        
        public bool? service_account_overridden { get; set; }
        
        public bool? informifack { get; set; }
        
        public bool? informnewareaifack { get; set; }
        
        public bool? canuploaddocuments { get; set; }
        
        public string use { get; set; }
        
        public int? key { get; set; }
        
        public int? table { get; set; }
        
        public int? client_id { get; set; }
        
        public bool? is_prospect { get; set; }
        
        public string whatsapp_number { get; set; }
    
    }

    public class Site
    {
        
        public int? id { get; set; }
        
        public string name { get; set; }
        
        public decimal? client_id { get; set; }
        
        public string client_name { get; set; }
        
        public string clientsite_name { get; set; }
        
        public bool? inactive { get; set; }
        
        public int? sla_id { get; set; }
        
        public string colour { get; set; }
        
        public string timezone { get; set; }
        
        public bool? invoice_address_isdelivery { get; set; }
        
        public bool? isstocklocation { get; set; }
        
        public int? messagegroup_id { get; set; }
        
        public DateTime? datecreated { get; set; }
        
        public string emaildomain { get; set; }
        
        public bool? isinvoicesite { get; set; }
        
        public int? refnumber { get; set; }
        
        public bool? defaultdelivery { get; set; }
        
        public string todomain { get; set; }
        
        public int? stopped { get; set; }
        
        public int? sitedateformat { get; set; }
        
        public string emailsubjectprefix { get; set; }
        
        public DateTime? contractlastchecked { get; set; }
        
        public string maincontact_name { get; set; }
        
        public string wildcardref { get; set; }
        
        public int? language_id { get; set; }
        
        public bool? slocked { get; set; }
        
        public Client client { get; set; }
        
        public object[] popup_notes { get; set; }
        
        public UserField[] fields { get; set; }
        
        public int? ninjarmmid { get; set; }
        
        public string servicenowid { get; set; }
        
        public bool? isnhserveremaildefault { get; set; }
        
        public string datto_id { get; set; }
        
        public int? datto_alternate_id { get; set; }
        
        public string datto_url { get; set; }
        
        public int? connectwiseid { get; set; }
        
        public string azuretenantid { get; set; }
        
        public int? autotaskid { get; set; }
        
        public string pagerdutywildcard { get; set; }
        
        public int? ateraid { get; set; }
        
        public DateTime? slastupdate { get; set; }
        
        public int? site_service_tax_code { get; set; }
        
        public int? site_prepay_tax_code { get; set; }
        
        public int? site_contract_tax_code { get; set; }
        
        public int? site_purchase_tax_code { get; set; }
        
        public int? syncroid { get; set; }
        
        public string auvik_id { get; set; }
        
        public bool? all_faqlists_allowed { get; set; }
        
        public string hubspot_id { get; set; }
        
        public int? passportal_id { get; set; }
        
        public int? liongardid { get; set; }
        
        public string country_code { get; set; }
        
        public int? region_code { get; set; }
        
        public string use { get; set; }
        
        public object[] customfields { get; set; }
        
        public string itglue_id { get; set; }
        
        public object[] custombuttons { get; set; }
        
        public int? maincontact_id { get; set; }
        
        public int? site_item_tax_code { get; set; }
    
    }

    public class Client
    {
        
        public int? id { get; set; }
        
        public string name { get; set; }
        
        public int? toplevel_id { get; set; }
        
        public string toplevel_name { get; set; }
        
        public bool? inactive { get; set; }
        
        public string colour { get; set; }
        
        public int? confirmemail { get; set; }
        
        public int? actionemail { get; set; }
        
        public int? clearemail { get; set; }
        
        public int? messagegroup_id { get; set; }
        
        public int? mailbox_override { get; set; }
        
        public int? default_mailbox_id { get; set; }
        
        public int? item_tax_code { get; set; }
        
        public int? service_tax_code { get; set; }
        
        public int? prepay_tax_code { get; set; }
        
        public int? contract_tax_code { get; set; }
        
        public UserCustomField[] customfields { get; set; }
        
        public object[] custombuttons { get; set; }
        
        public int? pritech { get; set; }
        
        public int? sectech { get; set; }
        
        public int? accountmanagertech { get; set; }
        
        public DateTime? datecreated { get; set; }
        
        public int? createdfrom_id { get; set; }
        
        public bool? prinotify { get; set; }
        
        public bool? secnotify { get; set; }
        
        public bool? priassign { get; set; }
        
        public bool? secassign { get; set; }
        
        public bool? invoiceyes { get; set; }
        
        public bool? floverride { get; set; }
        
        public bool? fluserdef1hide { get; set; }
        
        public bool? fluserdef2hide { get; set; }
        
        public bool? fluserdef3hide { get; set; }
        
        public bool? fluserdef4hide { get; set; }
        
        public bool? fluserdef5hide { get; set; }
        
        public bool? fluserdef1mand { get; set; }
        
        public bool? fluserdef2mand { get; set; }
        
        public bool? fluserdef3mand { get; set; }
        
        public bool? fluserdef4mand { get; set; }
        
        public bool? fluserdef5mand { get; set; }
        
        public bool? includeactions { get; set; }
        
        public bool? needsinvoice { get; set; }
        
        public bool? dontinvoice { get; set; }
        
        public bool? showslaonweb { get; set; }
        
        public int? imageindex { get; set; }
        
        public int? fcemail { get; set; }
        
        public bool? emailinvoice { get; set; }
        
        public bool? dont_auto_send_invoices { get; set; }
        
        public bool? monthlyreportinclude { get; set; }
        
        public bool? monthlyreportemaildirect { get; set; }
        
        public bool? monthlyreportemailmanager { get; set; }
        
        public bool? monthlyreportshowonweb { get; set; }
        
        public int? unmatchedcombinations { get; set; }
        
        public bool? billforrecurringprepayamount { get; set; }
        
        public int? surchargeid { get; set; }
        
        public int? billingtemplate_id { get; set; }
        
        public int? automatic_callscript_id { get; set; }
        
        public int? isopportunity { get; set; }
        
        public int? main_site_id { get; set; }
        
        public string main_site_name { get; set; }
        
        public bool? all_organisations_allowed { get; set; }
        
        public object[] popup_notes { get; set; }
        
        public bool? allowall_tickettypes { get; set; }
        
        public Allowed_Tickettypes[] allowed_tickettypes { get; set; }
        
        public bool? allowall_category1 { get; set; }
        
        public bool? allowall_category2 { get; set; }
        
        public bool? allowall_category3 { get; set; }
        
        public bool? allowall_category4 { get; set; }
        
        public bool? alocked { get; set; }
        
        public bool? allowallchargerates { get; set; }
        
        public bool? override_portalcolour { get; set; }
        
        public string portalcolour { get; set; }
        
        public int? ninjarmmid { get; set; }
        
        public int? purchase_tax_code { get; set; }
        
        public bool? prepayrecurringminimumdeductiononlyactive { get; set; }
        
        public int? qbodefaulttax { get; set; }
        
        public int? device42id { get; set; }
        
        public string servicenowid { get; set; }
        
        public bool? isnhserveremaildefault { get; set; }
        
        public string datto_id { get; set; }
        
        public int? datto_alternate_id { get; set; }
        
        public string datto_url { get; set; }
        
        public int? qbodefaulttaxcode { get; set; }
        
        public string qbodefaulttaxcodename { get; set; }
        
        public int? connectwiseid { get; set; }
        
        public int? autotaskid { get; set; }
        
        public int? ateraid { get; set; }
        
        public int? kashflowid { get; set; }
        
        public string website { get; set; }
        
        public DateTime? alastupdate { get; set; }
        
        public string snelstart_id { get; set; }
        
        public int? syncroid { get; set; }
        
        public string hubspot_id { get; set; }
        
        public string hubspot_url { get; set; }
        
        public bool? hubspot_dont_sync { get; set; }
        
        public bool? hubspot_archived { get; set; }
        
        public string domain { get; set; }
        
        public int? passportal_id { get; set; }
        
        public bool? prepayasamount { get; set; }
        
        public string hubspot_lifecycle { get; set; }
        
        public int? prepayrecurringexpirymonths { get; set; }
        
        public object[] external_links { get; set; }
        
        public int? liongardid { get; set; }
        
        public bool? default_team_to_salesrep_override { get; set; }
        
        public string portalchatprofile { get; set; }
        
        public string trading_name { get; set; }
        
        public bool? salesforce_dontsync { get; set; }
        
        public string stripe_payment_method_id { get; set; }
        
        public string servicenow_url { get; set; }
        
        public string servicenow_locale { get; set; }
        
        public string servicenow_username { get; set; }
        
        public string servicenow_assignment_group { get; set; }
        
        public string servicenow_assignment_group_name { get; set; }
        
        public string servicenow_defaultuser_id { get; set; }
        
        public string servicenow_defaultuser_name { get; set; }
        
        public int? sage_business_cloud_details_id { get; set; }
        
        public int? exact_division { get; set; }
        
        public int? ncentral_details_id { get; set; }
        
        public string jira_url { get; set; }
        
        public string jira_username { get; set; }
        
        public string jira_servicedesk_id { get; set; }
        
        public string jira_servicedesk_name { get; set; }
        
        public string jira_user_id { get; set; }
        
        public string jira_user_name { get; set; }
        
        public int? sync_servicenow_attachments { get; set; }
        
        public int? override_layout_id { get; set; }
        
        public string servicenow_ticket_sync { get; set; }
        
        public int? invoice_mailbox_override { get; set; }
        
        public int? quote_mailbox_override { get; set; }
        
        public string use { get; set; }
        
        public int? key { get; set; }
        
        public int? table { get; set; }
        
        public string xero_tenant_id { get; set; }
        
        public bool? excludefrominvoicesync { get; set; }
        
        public int? client_to_invoice { get; set; }
        
        public string client_to_invoice_name { get; set; }
        
        public string itglue_id { get; set; }
        
        public string sentinel_subscription_id { get; set; }
        
        public string sentinel_workspace_name { get; set; }
        
        public string sentinel_resource_group_name { get; set; }
        
        public int? default_currency_code { get; set; }
        
        public int? client_to_invoice_recurring { get; set; }
        
        public string client_to_invoice_recurring_name { get; set; }
        
        public string qbo_company_id { get; set; }
        
        public string dbc_company_id { get; set; }
        
        public int? stopped { get; set; }
        
        public int? customertype { get; set; }
        
        public object[] customer_relationship { get; set; }
        
        public string customer_relationship_list { get; set; }
        
        public bool? servicenow_validated { get; set; }
        public bool? jira_validated { get; set; }
        public bool? ticket_invoices_for_each_site { get; set; }
        public bool? is_vip { get; set; }
        public bool? taxable { get; set; }
        public int? percentage_to_survey { get; set; }
        public int? overridepdftemplatequote { get; set; }
        public bool? is_account { get; set; }
    }

    public class UserCustomField
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string label { get; set; }
        public string summary { get; set; }
        public int? type { get; set; }
        public int? value { get; set; }
        public string display { get; set; }
        public int? characterlimit { get; set; }
        public int? characterlimittype { get; set; }
        public int? inputtype { get; set; }
        public bool? copytochild { get; set; }
        public bool? copytoparent { get; set; }
        public bool? searchable { get; set; }
        public bool? ordervalues { get; set; }
        public int? ordervaluesby { get; set; }
        public bool? database_lookup_auto { get; set; }
        public string third_party_name { get; set; }
        public bool? copytochildonupdate { get; set; }
        public bool? copytoparentonupdate { get; set; }
        public string hyperlink { get; set; }
        public int? usage { get; set; }
        public bool? showondetailsscreen { get; set; }
        public string third_party_value { get; set; }
        public int? custom_extra_table_id { get; set; }
        public bool? copytorelated { get; set; }
        public string calculation { get; set; }
        public int? rounding { get; set; }
        public string regex { get; set; }
        public bool? is_horizontal { get; set; }
        public bool? isencrypted { get; set; }
        public string guid { get; set; }
        public int? selection_field_id { get; set; }
        public object[] validation_data { get; set; }
        public bool? showintable { get; set; }
        public bool? new_storage_method { get; set; }
        public int? load_type { get; set; }
    }

    public class Allowed_Tickettypes
    {
        public int? id { get; set; }
        public string guid { get; set; }
        public string name { get; set; }
        public string use { get; set; }
        public int? sequence { get; set; }
        public int? default_sla { get; set; }
        public int? group_id { get; set; }
        public string group_name { get; set; }
        public string jira_issue_type { get; set; }
        public bool? cancreate { get; set; }
        public bool? agentscanselect { get; set; }
        public int? itilrequesttype { get; set; }
        public bool? allowattachments { get; set; }
        public bool? copyattachmentstochild { get; set; }
        public bool? copyattachmentstorelated { get; set; }
        public bool? is_sprint { get; set; }
        public bool? enduserscanselect { get; set; }
        public bool? anonymouscanselect { get; set; }
        public int? project_type { get; set; }
        public object[] kanbanstatuschoice { get; set; }
        public string kanbanstatuschoice_list { get; set; }
        public int? default_agent { get; set; }
        public string default_team { get; set; }
        public int? default_priority { get; set; }
        public bool? visible { get; set; }
    }

    public class UserField
    {

        public int? id { get; set; }
        
        public string name { get; set; }
        
        public string validate { get; set; }
        
        public string value { get; set; }
        
        public string display { get; set; }
        
        public bool? mandatory { get; set; }
        
        public bool? showonactivity { get; set; }
        
        public int? lookup { get; set; }
        
        public int? systemuse { get; set; }
        
        public int? parenttype_id { get; set; }
        
        public string url { get; set; }
        
        public int? access_level { get; set; }
    
    }

    public class Customfield1
    {
        
        public int? id { get; set; }
        
        public string name { get; set; }
        
        public string label { get; set; }
        
        public string summary { get; set; }
        
        public int? type { get; set; }
        
        public bool? value { get; set; }
        
        public string display { get; set; }
        
        public int? characterlimit { get; set; }
        
        public int? characterlimittype { get; set; }
        
        public int? inputtype { get; set; }
        
        public bool? copytochild { get; set; }
        
        public bool? copytoparent { get; set; }
        
        public bool? searchable { get; set; }
        
        public bool? ordervalues { get; set; }
        
        public int? ordervaluesby { get; set; }
        
        public bool? database_lookup_auto { get; set; }
        
        public string third_party_name { get; set; }
        
        public bool? copytochildonupdate { get; set; }
        
        public bool? copytoparentonupdate { get; set; }
        
        public string hyperlink { get; set; }
        
        public int? usage { get; set; }
        
        public int? linked_table_id { get; set; }
        
        public bool? showondetailsscreen { get; set; }
        
        public int? custom_extra_table_id { get; set; }
        
        public bool? copytorelated { get; set; }
        
        public string calculation { get; set; }
        
        public int? rounding { get; set; }
        
        public string regex { get; set; }
        
        public bool? is_horizontal { get; set; }
        
        public bool? isencrypted { get; set; }
        
        public string guid { get; set; }
        
        public int? selection_field_id { get; set; }
        
        public object[] validation_data { get; set; }
        
        public bool? showintable { get; set; }
        
        public bool? new_storage_method { get; set; }

    }

}
