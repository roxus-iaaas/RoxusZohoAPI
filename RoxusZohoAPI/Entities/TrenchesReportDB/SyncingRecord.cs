using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{
    [Table("SyncingRecords")]
    public class SyncingRecord
    {
        [Key]
        public Guid Id { get; set; }
        public string ModuleName { get; set; }
        public string ModuleId { get; set; }
        public string TaskUrl { get; set; }
        public SyncingAction Action { get; set; }
        public SyncingStatus Status { get; set; }
        public DateTime? ZohoDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public enum SyncingAction
    {
        Delete = 0,
        Create = 1,
        Update = 2
    }

    public enum SyncingStatus
    {
        Pending = 0,
        Completed = 1
    }
}
