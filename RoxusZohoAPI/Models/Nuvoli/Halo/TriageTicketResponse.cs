using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class TriageTicketResponse
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
        
        public Customfield[] customfields { get; set; }
        
        public DateTime? actionarrivaldate { get; set; }
        
        public DateTime? actioncompletiondate { get; set; }
        
        public int? actionby_agent_id { get; set; }
        
        public string guid { get; set; }
        
        public int? actioncontractid { get; set; }

        public bool? actisbillable { get; set; }
        
        public bool? actisreadyforprocessing { get; set; }
        
        public bool? travelisreadyforprocessing { get; set; }
        
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
        
        public string new_team { get; set; }
        
        public int? new_agent { get; set; }
        
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
        
        public string colour { get; set; }
        
        public int? attachment_count { get; set; }
        
        public int? unread { get; set; }
        
        public string actionby_application_id { get; set; }
        
        public int? actionby_user_id { get; set; }
    
    }

    public class Outcome_Details
    {
        
        public int? id { get; set; }
        
        public string guid { get; set; }
        
        public string outcome { get; set; }
        
        public string buttonname { get; set; }
        
        public string labellong { get; set; }
        
        public string colour { get; set; }
        
        public int? sequence { get; set; }
        
        public bool? hidden { get; set; }
        
        public int? systemid { get; set; }
        
        public bool? dontshowscreen { get; set; }
        
        public int? respondtype { get; set; }
        
        public int? update_parent_attachment_type { get; set; }
        
        public int? sendemail { get; set; }
        
        public int? sendsms { get; set; }
        
        public bool? actionvisibility { get; set; }
        
        public int? child_template_id { get; set; }
        
        public int? createchildticketortemplate { get; set; }
        
        public int? child_ticket_type_id { get; set; }
        
        public string tfstype { get; set; }
        
        public string icon { get; set; }
        
        public bool? translate_before_sending { get; set; }
        
        public int? colour_type { get; set; }
        
        public int? newstatus { get; set; }
        
        public bool? slaholdischecked { get; set; }
        
        public int? emailtemplate_id { get; set; }
        
        public bool? slareleaseischecked { get; set; }
        
        public int? chargerate { get; set; }
        
        public int? approval_process_id { get; set; }
        
        public bool? hidefromuser { get; set; }
        
        public bool? hidesendemail { get; set; }
        
        public bool? hidesendsms { get; set; }
        
        public bool? hidecloserequest { get; set; }
        
        public int? defaultagent { get; set; }
        
        public int? defaultpriority { get; set; }
        
        public string defaultteam { get; set; }
        
        public bool? includecc { get; set; }
        
        public bool? includeto { get; set; }
        
        public int? requesttype { get; set; }
        
        public int? worddocid { get; set; }
        
        public int? workflow_id { get; set; }
        
        public int? workflow_step_id { get; set; }
        
        public bool? showhidecheckbox { get; set; }
        
        public bool? showslacheckbox { get; set; }
        
        public bool? showimportantcheckbox { get; set; }
        
        public bool? isimportant { get; set; }
        
        public bool? showfollowcheckbox { get; set; }
        
        public bool? follow { get; set; }
        
        public int? defaultappointmentstatus { get; set; }
        
        public int? reportid { get; set; }
        
        public bool? report_attach_pdf { get; set; }
        
        public bool? report_attach_xls { get; set; }
        
        public bool? report_attach_csv { get; set; }
        
        public bool? report_attach_json { get; set; }
        
        public int? todo_template_id { get; set; }
        
        public int? child_emailtemplate_id { get; set; }
        
        public int? default_linkedticket_status { get; set; }
        
        public string newaction_subject { get; set; }
        
        public string newaction_subject_close { get; set; }
        
        public Field[] fields { get; set; }
        
        public bool? _ticketinfoloaded { get; set; }
        
        public int? script_id { get; set; }
        
        public bool? usetimer { get; set; }
        
        public bool? update_dynamic_email_list { get; set; }
        
        public bool? skip_pending_closure { get; set; }
        
        public bool? newsummaryfromsource { get; set; }
        
        public bool? newdetailsfromsource { get; set; }
        
        public bool? show_email_fields { get; set; }
        
        public int? logtimeunits { get; set; }
        
        public int? callscreencallscript { get; set; }
        
        public string newaction_smsto { get; set; }
        
        public int? pdftemplate_id { get; set; }
        
        public bool? excludeFromDynamicLists { get; set; }
        
        public bool? setdetailsfromnewticket { get; set; }
        
        public bool? hideactionfromcandi { get; set; }
        
        public bool? bccfollowers { get; set; }
        
        public int? followersccbcc { get; set; }
        
        public object[] access_control { get; set; }
        
        public int? access_control_level { get; set; }
        
        public bool? showevenifnochild { get; set; }
        
        public int? replytype { get; set; }
        
        public int? parentdefaultuser { get; set; }
        
        public string assetsearchtext { get; set; }
        
        public bool? action_resets_response { get; set; }
        
        public bool? showattachmentstouser { get; set; }
        
        public string customurl { get; set; }
        
        public int? useremailintellisense { get; set; }
        
        public int? supplieremailintellisense { get; set; }
        
        public int? azure_action { get; set; }
        
        public int? azure_connection_id { get; set; }
        
        public int? defaultsupplier_id { get; set; }
        
        public int? default_asset_type { get; set; }
        
        public bool? showautorelease { get; set; }
        
        public int? default_asset_status { get; set; }
        
        public int? travelrate { get; set; }
        
        public int? tagreleasetype { get; set; }
        
        public int? ai_operation { get; set; }
        
        public string ai_model { get; set; }
        
        public int? default_opp_closure_cat { get; set; }
        
        public bool? default_send_to_pagerduty { get; set; }
        
        public int? minattachments { get; set; }
        
        public int? defaultappointmenttype { get; set; }
        
        public bool? showsendsurvey { get; set; }
        
        public int? sendsurvey { get; set; }
        
        public bool? showsendinstagram { get; set; }
        
        public bool? showslacheckbox_mandatory { get; set; }
        
        public bool? ai_improve_agent_note_default_on { get; set; }
        
        public int? defaultcommunicationmethod { get; set; }
        
        public int? related_note_visibility { get; set; }
        
        public int? override_from_mailbox_id { get; set; }
        
        public int? create_issue_type { get; set; }
        
        public string submitlabeloverride { get; set; }
        
        public bool? default_send_jira_note { get; set; }
        
        public int? config_type { get; set; }
        
        public int? show_on_parent { get; set; }
        
        public int? show_on_child { get; set; }
        
        public int? newactiondisplay { get; set; }
        
        public int? newactiondisplaysize { get; set; }
        
        public bool? hide_header_button { get; set; }
        
        public bool? new_ticket_attachments { get; set; }
        
        public bool? run_ai_insights { get; set; }
        
        public int? email_editing { get; set; }
        
        public int? default_distribution_list { get; set; }
    
    }

    public class Field
    {
        
        public bool? copytochildonupdate { get; set; }
        
        public int? id { get; set; }
        
        public int? rtid { get; set; }
        
        public int? fieldid { get; set; }
        
        public int? seq { get; set; }
        
        public int? tableid { get; set; }
        
        public int? groupid { get; set; }
        
        public int? endusernew { get; set; }
        
        public int? enduserdetail { get; set; }
        
        public int? enduserdetailrejected { get; set; }
        
        public int? technew { get; set; }
        
        public int? techdetail { get; set; }
        
        public Fieldinfo fieldinfo { get; set; }
        
        public Value_Restrictions[] value_restrictions { get; set; }
        
        public int? technewlocation { get; set; }
        
        public int? restrictupdate { get; set; }
        
        public int? techtab_id { get; set; }
        
        public string techtab_name { get; set; }
        
        public string override_fieldname { get; set; }
        
        public bool? boldlabelandvalue { get; set; }
        
        public bool? enduserdetailhideifempty { get; set; }
        
        public bool? techdetailhideifempty { get; set; }
        
        public bool? copytochild { get; set; }
        
        public bool? copytorelated { get; set; }
        
        public int? restrictread { get; set; }
        
        public int? techaction { get; set; }
        
        public int? enduseraction { get; set; }
        
        public bool? endusercheckboxmandatory { get; set; }
        
        public int? display_type { get; set; }
    
    }

    public class Fieldinfo
    {
        
        public int? id { get; set; }
        
        public string guid { get; set; }
        
        public string name { get; set; }
        
        public string label { get; set; }
        
        public string labellong { get; set; }
        
        public string summary { get; set; }
        
        public string hint { get; set; }
        
        public int? type { get; set; }
        
        public int? custom { get; set; }
        
        public int? usage { get; set; }
        
        public int? tab_id { get; set; }
        
        public string tab_name { get; set; }
        
        public int? table_id { get; set; }
        
        public bool? _readonly { get; set; }
        
        public bool? addunknown { get; set; }
        
        public string calcfield { get; set; }
        
        public int? inputtype { get; set; }
        
        public bool? copytochild { get; set; }
        
        public bool? searchable { get; set; }
        
        public bool? user_searchable { get; set; }
        
        public bool? calendar_searchable { get; set; }
        
        public bool? ordervaluesalphanumerically { get; set; }
        
        public int? ordervalueby { get; set; }
        
        public string variable_name { get; set; }
        
        public string faults_field_name { get; set; }
        
        public string actions_field_name { get; set; }
        
        public bool? database_lookup_auto { get; set; }
        
        public string third_party_name { get; set; }
        
        public string sqllookup { get; set; }
        
        public bool? copytochildonupdate { get; set; }
        
        public bool? showintable { get; set; }
        
        public string importalias { get; set; }
        
        public string hyperlink { get; set; }
        
        public string groupname { get; set; }
        
        public bool? showhintondetails { get; set; }
        
        public bool? showondetailsscreen { get; set; }
        
        public int? selection_field_id { get; set; }
        
        public int? customextratableid { get; set; }
        
        public bool? copytorelated { get; set; }
        
        public bool? deleteafterclosure { get; set; }
        
        public int? deleteafterclosuredays { get; set; }
        
        public int? defaultdate { get; set; }
        
        public string calculation { get; set; }
        
        public int? rounding { get; set; }
        
        public string regex { get; set; }
        
        public bool? excludefromallfields { get; set; }
        
        public bool? is_horizontal { get; set; }
        
        public bool? isencrypted { get; set; }
        
        public int? sql_connection_type { get; set; }
        
        public int? sql_certificate_id { get; set; }
        
        public int? hint_type { get; set; }
        
        public bool? new_storage_method { get; set; }
        
        public int? lookup_method { get; set; }
    
    }

    public class Value_Restrictions
    {
        
        public int? id { get; set; }
        
        public int? tickettype_id { get; set; }
        
        public int? field_id { get; set; }
        
        public int? value_id { get; set; }
        
        public string value_name { get; set; }
    
    }

    public class Customfield
    {
        
        public int? id { get; set; }
        
        public string name { get; set; }
        
        public string label { get; set; }
        
        public string summary { get; set; }
        
        public int? type { get; set; }
        
        public object value { get; set; }
        
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
        
        public bool? showintable { get; set; }
        
        public bool? new_storage_method { get; set; }
        
        public int? load_type { get; set; }
        
        public int? extratype { get; set; }
        
        public bool? onlyshowforagents { get; set; }
        
        public object[] validation_data { get; set; }
    
    }

}
