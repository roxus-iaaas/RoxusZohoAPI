using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{
    public class CorporateOwnershipView : ViewCorporateOwnership
    {

        public string Country_Incorporated_1 { get; set; }

        public string Country_Incorporated_2 { get; set; }

        public string Country_Incorporated_3 { get; set; }

        public string Country_Incorporated_4 { get; set; }

    }
}
