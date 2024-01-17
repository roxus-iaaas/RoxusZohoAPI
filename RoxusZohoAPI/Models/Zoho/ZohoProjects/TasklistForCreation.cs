using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{
    public class TasklistForCreation
    {
        public TasklistForCreation()
        {
            flag = "external";
        }

        public string name { get; set; }
        public string flag { get; set; }
    }
}
