using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.RoxusDB
{

    [Table("IntegrationLogs")]
    public class IntegrationLog
    {

        [Key]
        public int Id { get; set; }

        public string InputSummary { get; set; }

        public string Input1 { get; set; }

        public string Input2 { get; set; }

        public string Input3 { get; set; }

        public string Input4 { get; set; }

        public string Input5 { get; set; }

        public string Input6 { get; set; }

        public string Input7 { get; set; }

        public string Input8 { get; set; }

        public string Input9 { get; set; }

        public string Input10 { get; set; }

        public string OutputSummary { get; set; }

        public int? WebhookCursor { get; set; }

        public Guid CustomerName { get; set; }

        public string PlatformName { get; set; }

        public string RequestType { get; set; }

        public string Result { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTime? ModifiedTime { get; set; }

    }
}
