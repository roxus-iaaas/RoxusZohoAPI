using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoBooks
{
    public class UpdatePoResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public PurchaseorderDetail purchaseorder { get; set; }
    }
}
