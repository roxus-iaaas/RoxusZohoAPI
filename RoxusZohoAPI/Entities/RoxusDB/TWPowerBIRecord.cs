using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.RoxusDB
{
    [Table("TW_PowerBIRecords")]
    public class TWPowerBIRecord
    {
        [Key]
        public Guid Id { get; set; }
        public string ClientName { get; set; }
        public string ReportName { get; set; }
        public string RobotName { get; set; }
        public string InputParameters { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public byte IsSuccess { get; set; }
    }
}
