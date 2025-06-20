using System;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class GetTicketActionsResponse
    {

        public int? ticket_id { get; set; }

        public int? record_count { get; set; }

        public Action[] actions { get; set; }

    }

    public class Action
    {

        public int? ticket_id { get; set; }

        public int? id { get; set; }

        public string outcome { get; set; }

        public string who { get; set; }

        public int? who_type { get; set; }

        public string who_imgpath { get; set; }

        public int? who_agentid { get; set; }

        public DateTime? datetime { get; set; }

        public DateTime? actiondatecreated { get; set; }

        public string note { get; set; }

        public int? replied_to_ticket_id { get; set; }

        public int? replied_to_action_id { get; set; }

        public int? created_from_ticket_id { get; set; }

        public int? created_from_action_id { get; set; }

        public int? action_contract_id { get; set; }

        public int? project_id { get; set; }

        public bool? personal_unread { get; set; }

        public DateTime? actionarrivaldate { get; set; }

        public DateTime? actioncompletiondate { get; set; }

        public object[] translations { get; set; }

        public string on_behalf_of_name { get; set; }

        public int? actionby_agent_id { get; set; }

        public string guid { get; set; }

        public int? actioncontractid { get; set; }

        public bool? actisbillable { get; set; }

        public bool? actisreadyforprocessing { get; set; }

        public int? outcome_id { get; set; }

        public int? action_systemid { get; set; }

        public decimal? timetakendays { get; set; }

        public decimal? timetakenadjusted { get; set; }

        public decimal? actionchargehours { get; set; }

        public decimal? actionnonchargeamount { get; set; }

        public decimal? actionnonchargehours { get; set; }

        public decimal? actionchargeamount { get; set; }

        public decimal? actionprepayhours { get; set; }

        public decimal? actionprepayamount { get; set; }

        public int? chargerate { get; set; }

        public int? travel_chargerate { get; set; }

        public bool? hiddenfromuser { get; set; }

        public bool? important { get; set; }

        public int? old_status { get; set; }

        public int? new_status { get; set; }

        public string new_status_name { get; set; }

        public string colour { get; set; }

        public int? attachment_count { get; set; }

        public int? unread { get; set; }

        public string actionby_application_id { get; set; }

        public int? actionby_user_id { get; set; }

        public bool? hide_user_visibility_toggle { get; set; }

        public int? action_travel_contract_id { get; set; }

        public bool? travelisreadyforprocessing { get; set; }

        public decimal? timetaken { get; set; }

        public decimal? nonbilltime { get; set; }

        public decimal? actiontravelchargehours { get; set; }

        public int? merged_from_ticketid { get; set; }

        public string email_message_id { get; set; }

        public DateTime? dateemailed { get; set; }

        public string emailfrom { get; set; }

        public string emailtonew { get; set; }

        public string emailto { get; set; }

        public string emailccnew { get; set; }

        public string emaildirection { get; set; }

        public string emailcc { get; set; }

        public string emailsubjectnew { get; set; }

        public int? warning_type { get; set; }

        public string senttoapiurl { get; set; }

    }

}
