using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{
    public class HinetsProjectDetailsResponse
    {
        public HinetsProject[] projects { get; set; }
    }

    public class HinetsProject
    {
        public string is_strict { get; set; }
        public Bug_Count bug_count { get; set; }
        public string owner_id { get; set; }
        public string bug_client_permission { get; set; }
        public string taskbug_prefix { get; set; }
        public long? start_date_long { get; set; }
        public long? updated_date_long { get; set; }
        public bool? show_project_overview { get; set; }
        public int? billing_type_value { get; set; }
        public Task_Count task_count { get; set; }
        public string updated_date_format { get; set; }
        public string workspace_id { get; set; }
        public int? budget_type_value { get; set; }
        public string bug_defaultview { get; set; }
        public long? id { get; set; }
        public bool? is_chat_enabled { get; set; }
        public bool? is_sprints_project { get; set; }
        public string owner_name { get; set; }
        public long? created_date_long { get; set; }
        public HinetsCustomFields[] custom_fields { get; set; }
        public string created_by { get; set; }
        public string created_date_format { get; set; }
        public long? profile_id { get; set; }
        public string name { get; set; }
        public string updated_by { get; set; }
        public string updated_by_id { get; set; }
        public string created_by_id { get; set; }
        public long? updated_by_zpuid { get; set; }
        public string bug_prefix { get; set; }
        public string status { get; set; }
        public string end_date { get; set; }
        public string project_percent { get; set; }
        public string role { get; set; }
        public bool? IS_BUG_ENABLED { get; set; }
        public string budget_value { get; set; }
        public Link link { get; set; }
        public long? created_by_zpuid { get; set; }
        public string custom_status_id { get; set; }
        public string description { get; set; }
        public Milestone_Count milestone_count { get; set; }
        public long? end_date_long { get; set; }
        public string custom_status_name { get; set; }
        public string owner_zpuid { get; set; }
        public string is_client_assign_bug { get; set; }
        public string billing_type { get; set; }
        public string billing_status { get; set; }
        public string currency { get; set; }
        public string primary_client_id { get; set; }
        public string key { get; set; }
        public string budget_type { get; set; }
        public string start_date { get; set; }
        public string custom_status_color { get; set; }
        public string currency_symbol { get; set; }
        public string[] enabled_tabs { get; set; }
        public string is_public { get; set; }
        public string id_string { get; set; }
        public string created_date { get; set; }
        public string updated_date { get; set; }
        public Other_Service other_service { get; set; }
        public Cascade_Setting cascade_setting { get; set; }
        public Layout_Details layout_details { get; set; }
    }

    public class Other_Service
    {
        public string finance_project_id { get; set; }
    }

    public class HinetsCustomFields
    {
        [JsonProperty("Site What3Words")]
        public string SiteWhat3Words { get; set; }
        [JsonProperty("Site Address")]
        public string SiteAddress { get; set; }
        [JsonProperty("Site Postcode")]
        public string SitePostcode { get; set; }
        [JsonProperty("A & E Name")]
        public string AEName { get; set; }
        [JsonProperty("A & E Address")]
        public string AEAddress { get; set; }
        [JsonProperty("A & E Phone")]
        public string AEPhone { get; set; }
        [JsonProperty("Opportunity Name")]
        public string OpportunityName { get; set; }
        [JsonProperty("Primary Contact Name")]
        public string PrimaryContactName { get; set; }
        [JsonProperty("Install Days")]
        public string InstallDays { get; set; }
        [JsonProperty("Target Completion Date")]
        public string TargetCompletionDate { get; set; }
        [JsonProperty("Opportunity Id")]
        public string OpportunityId { get; set; }
    }
}
