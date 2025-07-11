﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{
    [Table("OR_UPRNsFreeholdTitles")]
    public class ORUPRNsFreeholdTitles
    {

        public long ParentUPRN { get; set; }

        public long UPRN { get; set; }

        public string FreeholdTitle { get; set; }

        public string Ownership { get; set; }

        public DateTime? Created { get; set; }

    }
}
