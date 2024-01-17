using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.RoxusDB
{
    public class TrenchesTask
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string TaskListId { get; set; }
        public string TaskListName { get; set; }
        public string Owner { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? DueTime { get; set; }
        public DateTime? ZohoCreatedTime { get; set; }
        public DateTime? ZohoModifiedTime { get; set; }
        public string Status { get; set; }
        public string Letter1Date { get; set; }
        public string Letter1Status { get; set; }
        public string Letter2Date { get; set; }
        public string Letter2Status { get; set; }
        public string Letter3Date { get; set; }
        public string Letter3Status { get; set; }
        public string Letter4Date { get; set; }
        public string Letter4Status { get; set; }
        public string CRMTitle { get; set; }
        public string WayleaveTemplate { get; set; }
        public string TitleNumber { get; set; }
        public string TitleType { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
    }
}
