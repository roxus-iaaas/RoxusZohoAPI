using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class AccountResponse
    {
        public string Company_Registration { get; set; }
        public object Ownership { get; set; }
        public Owner Owner { get; set; }
        public string Description { get; set; }
        public string Account_Type { get; set; }
        public string SIC_Code { get; set; }
        public string Shipping_State { get; set; }
        public string Website { get; set; }
        public int? Employees { get; set; }
        public DateTime? Last_Activity_Time { get; set; }
        public string Industry { get; set; }
        public Modified_By Modified_By { get; set; }
        public string Account_Site { get; set; }
        public string Phone { get; set; }
        public string Currency { get; set; }
        public string Billing_Country { get; set; }
        public string Account_Name { get; set; }
        public string id { get; set; }
        public string Account_Number { get; set; }
        public DateTime? Modified_Time { get; set; }
        public string Billing_Street { get; set; }
        public DateTime? Created_Time { get; set; }
        public string Billing_Code { get; set; }
        public string Shipping_City { get; set; }
        public string Shipping_Country { get; set; }
        public string Shipping_Code { get; set; }
        public string Billing_City { get; set; }
        public string Billing_State { get; set; }
        public Created_By Created_By { get; set; }
        public string Fax { get; set; }
        public float? Annual_Revenue { get; set; }
        public string Shipping_Street { get; set; }
    }
}

