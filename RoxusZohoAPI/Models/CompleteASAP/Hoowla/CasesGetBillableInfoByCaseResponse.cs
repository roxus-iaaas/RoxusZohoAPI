using System;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class CasesGetBillableInfoByCaseResponse
    {
        public int? Case { get; set; }
        public int? category { get; set; }
        public string billable_name { get; set; }
        public string title { get; set; }
        public int? hours { get; set; }
        public int? minutes { get; set; }
        public int? amount { get; set; }
        public double? rate { get; set; }
        public double? value { get; set; }
        public double? vat_value { get; set; }
        public DateTime? date { get; set; }
        public string fullname { get; set; }
        public string type { get; set; }
        public string invoice { get; set; }
    }
}