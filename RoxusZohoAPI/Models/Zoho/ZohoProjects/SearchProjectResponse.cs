using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{

    public class SearchProjectResponse
    {
        public SearchProjectResponse()
        {
            projects = new List<Project>();
        }
        public List<Project> projects { get; set; }
        public int projects_count { get; set; }
    }

    public class Project
    {
        public Owner owner { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string billing_status { get; set; }
        public long start_date_long { get; set; }
        public string id { get; set; }
        public string status { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public long end_date_long { get; set; }
    }

    public class Owner
    {
        public string name { get; set; }
        public string id { get; set; }
    }

}
