using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{
    [Table("LFL_UPRNsLeaseholdTitles")]
    public class LFLUPRNsLeaseholdTitles
    {

        public long UPRN { get; set; }

        public string LeaseholdTitle { get; set; }

        public DateTime? Created { get; set; }

    }
}
