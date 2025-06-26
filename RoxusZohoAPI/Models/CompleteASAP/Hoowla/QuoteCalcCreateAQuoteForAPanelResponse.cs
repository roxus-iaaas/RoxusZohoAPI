using System;
using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class QuoteCalcCreateAQuoteForAPanelResponse
    {
        public List<Quote> quotes { get; set; }
        public Panel panel { get; set; }
    }

    public class Quote
    {
        public string company_name { get; set; }
        public string company_website { get; set; }
        public string form_disclaimer { get; set; }
        public string form_introduction { get; set; }
        public string form_name { get; set; }
        public List<string> quotes { get; set; }
        public double? total_ex { get; set; }
        public double? total_fee { get; set; }
        public double? total_fee_ex { get; set; }
        public double? total_inc { get; set; }
    }

    public class Panel
    {
        public string panel_naame { get; set; }
        public string company_name { get; set; }
        public string company_website { get; set; }
        public string company_logo { get; set; }
    }
}