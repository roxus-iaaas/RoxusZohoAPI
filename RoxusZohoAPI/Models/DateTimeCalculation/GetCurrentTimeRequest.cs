using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.DateTimeCalculation
{
    public class GetCurrentTimeRequest
    {

        public string TimeZone { get; set; }

        public string Format { get; set; }

        public int? AddDay { get; set; }

    }
}
