using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{
    public class GetAllTasklistsResponse
    {
        public GetAllTasklistsResponse()
        {
            tasklists = new List<TasklistDetail>();
        }
        public List<TasklistDetail> tasklists { get; set; }
    }

    public class TasklistDetail
    {
        public long created_time_long { get; set; }
        public string created_time { get; set; }
        public string flag { get; set; }
        public string created_time_format { get; set; }
        public Link link { get; set; }
        public bool completed { get; set; }
        public bool rolled { get; set; }
        public Task_Count task_count { get; set; }
        public int sequence { get; set; }
        public Milestone milestone { get; set; }
        public string last_updated_time { get; set; }
        public long last_updated_time_long { get; set; }
        public string name { get; set; }
        public string id_string { get; set; }
        public long id { get; set; }
        public string last_updated_time_format { get; set; }
    }

    public class Link
    {
        public Task task { get; set; }
        public Self self { get; set; }
    }

    public class Task
    {
        public string url { get; set; }
    }

    public class Self
    {
        public string url { get; set; }
    }

    public class Task_Count
    {
        public int open { get; set; }
        public int closed { get; set; }
    }

    public class Milestone
    {
        public string name { get; set; }
        public long id { get; set; }
    }

}
