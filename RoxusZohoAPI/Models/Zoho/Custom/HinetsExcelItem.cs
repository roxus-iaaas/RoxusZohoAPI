using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.Custom
{
    public class HinetsExcelItem
    {

        public string ItemName { get; set; }

        public string ItemId { get; set; }

        public string SKU { get; set; }

        public float? PurchaseRate { get; set; }

        public float? Rate { get; set; }

        public float? Margin { get; set; }

        public float? Markup { get; set; }

        public float? ItemType { get; set; }

        public bool? IsComboProduct { get; set; }

        public string Description { get; set; }

    }
}
