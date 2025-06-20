using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class LogToSupplierResponse
    {

        public int? ticket_id { get; set; }
        
        public int? id { get; set; }
        
        public string outcome { get; set; }
        
        public string who { get; set; }
        
        public int? who_type { get; set; }
        
        public int? who_agentid { get; set; }
        
        public DateTime? datetime { get; set; }

        public string note { get; set; }

        public int? replied_to_ticket_id { get; set; }
        
        public int? replied_to_action_id { get; set; }
        
        public int? created_from_ticket_id { get; set; }
        
        public int? created_from_action_id { get; set; }
        
        public int? action_contract_id { get; set; }
        
        public int? action_travel_contract_id { get; set; }
        
        public int? project_id { get; set; }
        
        // public Customfield[] customfields { get; set; }
        
        public DateTime? actionarrivaldate { get; set; }
        
        public DateTime? actioncompletiondate { get; set; }
        
        public int? actionby_agent_id { get; set; }
        
        public string guid { get; set; }
        
        public int? actioncontractid { get; set; }
        
        public bool? actisbillable { get; set; }
        
        public bool? actisreadyforprocessing { get; set; }
        
        public bool? travelisreadyforprocessing { get; set; }
        
        public string note_html { get; set; }
        
        public DateTime? actiondatecreated { get; set; }
        
        public bool? actioninvoicenumber_isproject { get; set; }
        
        public decimal? actiontravelamount { get; set; }
        
        public decimal? actionmileageamount { get; set; }
        
        public int? actionbillingplanid { get; set; }
        
        public int? actioncalendarstatus { get; set; }
        
        public int? actionsmsstatus { get; set; }
        
        public int? asset_id { get; set; }
        
        public int? asset_site { get; set; }
        
        public bool? lwwarrantyreported { get; set; }
        
        public int? from_mailbox_id { get; set; }
        
        public bool? labourwarranty { get; set; }
        
        public bool? actreviewed { get; set; }
        
        public bool? reply_direct { get; set; }
        
        public bool? actioninformownerofaction { get; set; }
        
        public int? agentnotificationneeded { get; set; }
        
        public int? travel_surchargeid { get; set; }
        
        public bool? achargetotalprocessed { get; set; }
        
        public bool? tweetsent { get; set; }
        
        public string tweetfrom { get; set; }
        
        public int? twitterid { get; set; }
        
        public bool? send_to_facebook { get; set; }
        
        public bool? senttofb { get; set; }
        
        public string facebookid { get; set; }
        
        public string facebook_parent_id { get; set; }
        
        public int? pagerdutysendstatus { get; set; }
        
        public int? chatid { get; set; }
        
        public string dynamicsactionid { get; set; }
        
        public int? action_supplier_id { get; set; }
        
        public int? new_agent { get; set; }
        
        public int? new_supplier_id { get; set; }
        
        public string new_supplier_ref { get; set; }
        
        public int? new_supplier_contract_id { get; set; }
        
        public int? emailtemplate_id { get; set; }
        
        public bool? sendemail { get; set; }
        
        public bool? action_showpreview { get; set; }
        
        public object[] attachments { get; set; }
        
        public Outcome_Details outcome_details { get; set; }
        
        public bool? send_survey { get; set; }
        
        public bool? follow { get; set; }
        
        public int? appointment_complete_status { get; set; }
        
        public bool? _canupdate { get; set; }
        
        public bool? dont_do_rules { get; set; }
        
        public string _warning { get; set; }
        
        public decimal? actionoohtime { get; set; }
        
        public decimal? actionholidaytime { get; set; }
        
        public decimal? actiontotaloohtime { get; set; }
        
        public decimal? utcoffset { get; set; }
        
        public int? senttoapisupplierid { get; set; }
        
        public string smsto { get; set; }
        
        public bool? sendsms { get; set; }
        
        public bool? _sendtweet { get; set; }
        
        public bool? _validate_travel { get; set; }
        
        public bool? usecroverride { get; set; }
        
        public bool? azure_action_complete { get; set; }
        
        public int? new_supplier_contact_id { get; set; }
        
        public string pagerdutyid { get; set; }
        
        public string facebook_from_id { get; set; }
        
        public string twitter_from_id { get; set; }
        
        public int? senttojirasupplierid { get; set; }
        
        public string itsm_summary { get; set; }
        
        public bool? send_to_whatsapp { get; set; }
        
        public bool? _ignore_ai { get; set; }
        
        public string instagramid { get; set; }
        
        public string instagramname { get; set; }
        
        public string instagram_parent_id { get; set; }
        
        public bool? send_to_instagram { get; set; }
        
        public bool? senttoinsta { get; set; }
        
        public string instagram_from_id { get; set; }
        
        public int? senttoservicenowsupplierid { get; set; }
        
        public int? timesheet_approval_status { get; set; }
        
        public bool? send_to_googlebusiness { get; set; }
        
        public int? milestone_bill_id { get; set; }
        
        public bool? _agent03_ok { get; set; }
        
        public int? outcome_id { get; set; }
        
        public int? action_systemid { get; set; }
        
        public decimal? timetaken { get; set; }
        
        public decimal? timetakendays { get; set; }
        
        public decimal? timetakenadjusted { get; set; }
        
        public decimal? nonbilltime { get; set; }
        
        public decimal? actionchargehours { get; set; }
        
        public decimal? actionnonchargeamount { get; set; }
        
        public decimal? actionnonchargehours { get; set; }
        
        public decimal? actionchargeamount { get; set; }
        
        public decimal? actionprepayhours { get; set; }
        
        public decimal? actionprepayamount { get; set; }
        
        public decimal? actiontravelchargehours { get; set; }
        
        public int? chargerate { get; set; }
        
        public int? travel_chargerate { get; set; }
        
        public bool? hiddenfromuser { get; set; }
        
        public bool? important { get; set; }
        
        public int? old_status { get; set; }
        
        public int? new_status { get; set; }
        
        public string senttoapiurl { get; set; }
        
        public string colour { get; set; }
        
        public int? attachment_count { get; set; }
        
        public int? unread { get; set; }
        
        public string actionby_application_id { get; set; }
        
        public int? actionby_user_id { get; set; }
    
    }

}
