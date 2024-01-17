using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{

    public class SearchTasksResponse
    {
        public TaskDetail[] tasks { get; set; }

        public int tasks_count { get; set; }

    } 

}
