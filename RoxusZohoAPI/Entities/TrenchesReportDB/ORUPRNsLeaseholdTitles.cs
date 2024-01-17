using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{
    [Table("OR_UPRNsLeaseholdTitles")]
    public class ORUPRNsLeaseholdTitles
    {

        public long ParentUPRN { get; set; }

        public long UPRN { get; set; }

        public string FreeholdTitle { get; set; }

        public string LeaseholdTitle { get; set; }

        public DateTime? Created { get; set; }

    }
}
