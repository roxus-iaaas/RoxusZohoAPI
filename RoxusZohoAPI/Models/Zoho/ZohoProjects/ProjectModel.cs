using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{
    public class ProjectModel
    {
        public ZohoProject[] projects { get; set; }

        public class ZohoProject
        {
            public string is_strict { get; set; }
            public Bug_Count bug_count { get; set; }
            public string owner_id { get; set; }
            public string bug_client_permission { get; set; }
            public string taskbug_prefix { get; set; }
            public long updated_date_long { get; set; }
            public bool show_project_overview { get; set; }
            public int billing_type_value { get; set; }
            public Task_Count task_count { get; set; }
            public string updated_date_format { get; set; }
            public int budget_type_value { get; set; }
            public string bug_defaultview { get; set; }
            public long id { get; set; }
            public bool is_chat_enabled { get; set; }
            public bool is_sprints_project { get; set; }
            public string owner_name { get; set; }
            public long created_date_long { get; set; }
            public string group_name { get; set; }
            public string created_date_format { get; set; }
            public long profile_id { get; set; }
            public string name { get; set; }
            public string bug_prefix { get; set; }
            public string status { get; set; }
            public string project_percent { get; set; }
            public string role { get; set; }
            public bool IS_BUG_ENABLED { get; set; }
            public string budget_value { get; set; }
            public Link link { get; set; }
            public string custom_status_id { get; set; }
            public string description { get; set; }
            public Milestone_Count milestone_count { get; set; }
            public string custom_status_name { get; set; }
            public string owner_zpuid { get; set; }
            public string is_client_assign_bug { get; set; }
            public string billing_type { get; set; }
            public string billing_status { get; set; }
            public string currency { get; set; }
            public string primary_client_id { get; set; }
            public string key { get; set; }
            public string budget_type { get; set; }
            public string custom_status_color { get; set; }
            public string currency_symbol { get; set; }
            public long group_id { get; set; }
            public string[] enabled_tabs { get; set; }
            public string is_public { get; set; }
            public string id_string { get; set; }
            public string created_date { get; set; }
            public string updated_date { get; set; }
            public Cascade_Setting cascade_setting { get; set; }
            public Layout_Details layout_details { get; set; }
        }

        public class Bug_Count
        {
            public int closed { get; set; }
            public int open { get; set; }
        }

        public class Task_Count
        {
            public int closed { get; set; }
            public int open { get; set; }
        }

        public class Link
        {
            public Activity activity { get; set; }
            public Document document { get; set; }
            public Forum forum { get; set; }
            public Timesheet timesheet { get; set; }
            public Task task { get; set; }
            public Folder folder { get; set; }
            public Milestone milestone { get; set; }
            public Bug bug { get; set; }
            public Self self { get; set; }
            public Tasklist tasklist { get; set; }
            public Event _event { get; set; }
            public User user { get; set; }
            public Status status { get; set; }
        }

        public class Activity
        {
            public string url { get; set; }
        }

        public class Document
        {
            public string url { get; set; }
        }

        public class Forum
        {
            public string url { get; set; }
        }

        public class Timesheet
        {
            public string url { get; set; }
        }

        public class Task
        {
            public string url { get; set; }
        }

        public class Folder
        {
            public string url { get; set; }
        }

        public class Milestone
        {
            public string url { get; set; }
        }

        public class Bug
        {
            public string url { get; set; }
        }

        public class Self
        {
            public string url { get; set; }
        }

        public class Tasklist
        {
            public string url { get; set; }
        }

        public class Event
        {
            public string url { get; set; }
        }

        public class User
        {
            public string url { get; set; }
        }

        public class Status
        {
            public string url { get; set; }
        }

        public class Milestone_Count
        {
            public string closed { get; set; }
            public string open { get; set; }
        }

        public class Cascade_Setting
        {
            public bool date { get; set; }
            public bool logHours { get; set; }
            public bool plan { get; set; }
            public bool percentage { get; set; }
            public bool workHours { get; set; }
        }

        public class Layout_Details
        {
            public Task1 task { get; set; }
            public Bug1 bug { get; set; }
            public Project1 project { get; set; }
        }

        public class Task1
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class Bug1
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class Project1
        {
            public string name { get; set; }
            public string id { get; set; }
        }
    }
}
