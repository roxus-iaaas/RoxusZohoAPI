using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{
    public class TaskModel
    {
        public ZohoTask[] tasks { get; set; }

        public class ZohoTask
        {
            public string milestone_id { get; set; }
            public Link link { get; set; }
            public string created_by_zpuid { get; set; }
            public string work_form { get; set; }
            public bool? is_comment_added { get; set; }
            public string blueprint_id { get; set; }
            public string duration { get; set; }
            public bool is_forum_associated { get; set; }
            public Details details { get; set; }
            public long id { get; set; }
            public string created_by_email { get; set; }
            public string key { get; set; }
            public string created_person { get; set; }
            public long? created_time_long { get; set; }
            public long? start_date_long { get; set; }
            public long? end_date_long { get; set; }
            public long? last_updated_time_long { get; set; }
            public string created_time { get; set; }
            public bool? is_reminder_set { get; set; }
            public bool? is_recurrence_set { get; set; }
            public string created_time_format { get; set; }
            public bool? subtasks { get; set; }
            public string work { get; set; }
            public Custom_Fields[] custom_fields { get; set; }
            public string duration_type { get; set; }
            public string work_type { get; set; }
            public string blueprint_name { get; set; }
            public bool completed { get; set; }
            public string priority { get; set; }
            public string created_by { get; set; }
            public string percent_complete { get; set; }
            public Transition[] transition { get; set; }
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
        }

        public class Link
        {
            public Timesheet timesheet { get; set; }
            public Web web { get; set; }
            public Self self { get; set; }
            public Subtask subtask { get; set; }
        }

        public class Timesheet
        {
            public string url { get; set; }
        }

        public class Web
        {
            public string url { get; set; }
        }

        public class Self
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

        public class Owner
        {
            public string name { get; set; }
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

        public class Transition
        {
            public string TRANSITION_NAME { get; set; }
            public string BLUEPRINT_ID { get; set; }
            public string IS_APPLIED_TO_SUBTASK { get; set; }
            public string IS_APPLIED_TO_ALL { get; set; }
            public string IS_GLOBAL_TRANSITION { get; set; }
            public string TRANSITION_POSITION { get; set; }
            public string TRANSITION_ID { get; set; }
            public string FROM_PORT { get; set; }
            public string TO_PORT { get; set; }
        }
    }
}
