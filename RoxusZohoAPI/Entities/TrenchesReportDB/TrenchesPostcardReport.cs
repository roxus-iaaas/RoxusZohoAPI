using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{
    [Table("TrenchesPostcardReport")]
    public class TrenchesPostcardReport
    {
        [Key]
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string UsrnId { get; set; }
        public string UsrnName { get; set; }
        public string TasklistId { get; set; }
        public string TasklistName { get; set; }
        public string TaskStatus { get; set; }
        public string TitleId { get; set; }
        public string TitleName { get; set; }
        public string TitleOwnership { get; set; }
        public string TitleType { get; set; }
        public string TitleRelatedUPRN { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
