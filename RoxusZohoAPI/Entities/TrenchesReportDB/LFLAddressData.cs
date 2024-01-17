using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{
    [Table("LFL_AddressDatas")]
    public class LFLAddressData
    {

        [Key]
        public long UPRN { get; set; }

        public string Classification { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        public string Type { get; set; }

        public string PNCab { get; set; }

        public string DPCab { get; set; }

    }
}
