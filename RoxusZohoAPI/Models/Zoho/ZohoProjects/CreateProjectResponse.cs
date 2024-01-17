using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{
    public class CreateProjectResponse
    {
        public CreateProjectResponse()
        {
            projects = new List<ProjectDetail>();
        }

        public List<ProjectDetail> projects { get; set; }
    }
}
