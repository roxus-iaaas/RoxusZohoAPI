using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{

    [Table("OR_Tasks")]
    public class ORTask
    {

        [Key]
        public string TaskId { get; set; }

        public string TasklistId { get; set; }

        public string ProjectId { get; set; }

        public string TaskName { get; set; }

        public string TasklistName { get; set; }

        public string ProjectName { get; set; }

        public string CrmSM { get; set; }

        public string LetterReference { get; set; }

        public string TaskStatus { get; set; }

        public DateTime? StatusDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? Letter1Date { get; set; }

        public string Letter1Status { get; set; }

        public DateTime? Letter2Date { get; set; }

        public string Letter2Status { get; set; }

        public DateTime? Letter3Date { get; set; }

        public string Letter3Status { get; set; }

        public string WayleaveStatus { get; set; }

        public DateTime? WayleaveDate { get; set; }

        public string AsbestosResponse { get; set; }

        public DateTime? AsbestosResponseDate { get; set; }

        public string RAStatus { get; set; }

        public DateTime? RADate { get; set; }

        public string ReleasedStatus { get; set; }

        public DateTime? ReleasedDate { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}
