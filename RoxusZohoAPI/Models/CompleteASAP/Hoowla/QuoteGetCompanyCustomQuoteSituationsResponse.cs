using System;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class QuoteGetCompanyCustomQuoteSituationsResponse
    {
        public int? id { get; set; }
        public string name { get; set; }
        public bool? purchase { get; set; }
        public bool? sale { get; set; }
        public bool? remortgage { get; set; }
        public bool? transfer { get; set; }
    }
}