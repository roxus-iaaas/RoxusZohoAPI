using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{
    [Table("Task")]
    public class ZohoTask
    {
        [Key]
        public string TaskId { get; set; }

        public string ProjectId { get; set; }

        public string TaskName { get; set; }

        public string TaskStatus { get; set; }

        public DateTime? StatusDate { get; set; }

        public DateTime? ReleasedDate { get; set; }

        public DateTime? Letter1Date { get; set; }

        public string Letter1Status { get; set; }

        public DateTime? Letter2Date { get; set; }

        public string Letter2Status { get; set; }

        public DateTime? Letter3Date { get; set; }

        public string Letter3Status { get; set; }

        public DateTime? Letter4Date { get; set; }

        public string Letter4Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
