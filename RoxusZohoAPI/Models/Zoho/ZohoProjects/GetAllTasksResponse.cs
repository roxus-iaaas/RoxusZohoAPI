using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{

    public class GetAllTasksResponse
    {
        public GetAllTasksResponse()
        {
            tasks = new List<TaskDetail>();
        }
        public List<TaskDetail> tasks { get; set; }
    }

    public class TaskDetail
    {
        public string milestone_id { get; set; }
        public Link link { get; set; }
        public string description { get; set; }
        public string created_by_zpuid { get; set; }
        public string work_form { get; set; }
        public bool is_comment_added { get; set; }
        public string blueprint_id { get; set; }
        public string duration { get; set; }
        public long last_updated_time_long { get; set; }
        public bool is_forum_associated { get; set; }
        public Details details { get; set; }
        public long id { get; set; }
        public string created_by_email { get; set; }
        public string key { get; set; }
        public string created_person { get; set; }
        public long created_time_long { get; set; }
        public string created_time { get; set; }
        public bool is_reminder_set { get; set; }
        public bool is_recurrence_set { get; set; }
        public string created_time_format { get; set; }
        public bool subtasks { get; set; }
        public string work { get; set; }
        public Custom_Fields[] custom_fields { get; set; }
        public string duration_type { get; set; }
        public bool isparent { get; set; }
        public string work_type { get; set; }
        public string blueprint_name { get; set; }
        public bool completed { get; set; }
        public Task_Followers task_followers { get; set; }
        public string priority { get; set; }
        public string created_by { get; set; }
        public string percent_complete { get; set; }
        public Tag[] tags { get; set; }
        public GROUP_NAME GROUP_NAME { get; set; }
        public string last_updated_time { get; set; }
        public string name { get; set; }
        public bool is_docs_assocoated { get; set; }
        public string id_string { get; set; }
        public Log_Hours log_hours { get; set; }
        public Tasklist tasklist { get; set; }
        public string last_updated_time_format { get; set; }
        public string billingtype { get; set; }
        public int order_sequence { get; set; }
        public Status status { get; set; }
        public long start_date_long { get; set; }
        public string end_date_format { get; set; }
        public string start_date_format { get; set; }
        public string end_date { get; set; }
        public long end_date_long { get; set; }
        public string start_date { get; set; }
    }

    public class Timesheet
    {
        public string url { get; set; }
    }

    public class Web
    {
        public string url { get; set; }
    }

    public class Subtask
    {
        public string url { get; set; }
    }

    public class Details
    {
        public Owner[] owners { get; set; }
    }

    public class Task_Followers
    {
        public string FOLUSERS { get; set; }
        public int FOLLOWERSIZE { get; set; }
        public object[] FOLLOWERS { get; set; }
    }

    public class GROUP_NAME
    {
        public ASSOCIATED_TEAMS ASSOCIATED_TEAMS { get; set; }
        public int ASSOCIATED_TEAMS_COUNT { get; set; }
        public bool IS_TEAM_UNASSIGNED { get; set; }
    }

    public class ASSOCIATED_TEAMS
    {
        public string AnyTeam { get; set; }
    }

    public class Log_Hours
    {
        public string non_billable_hours { get; set; }
        public string billable_hours { get; set; }
    }

    public class Tasklist
    {
        public string name { get; set; }
        public string id_string { get; set; }
        public string id { get; set; }
    }

    public class Status
    {
        public string name { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string color_code { get; set; }
    }

    public class Custom_Fields
    {
        public string column_name { get; set; }
        public string label_name { get; set; }
        public string value { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public string id { get; set; }
        public string color_class { get; set; }
    }
}
