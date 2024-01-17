using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.CompanyHouse
{

    public class SearchCompaniesResponse
    {
        public int? start_index { get; set; }

        public Item[] items { get; set; }

        public int? items_per_page { get; set; }

        public int? page_number { get; set; }

        public int? total_results { get; set; }

        public string kind { get; set; }

    }

    public class Item
    {

        public string[] description_identifier { get; set; }

        public string title { get; set; }

        public string kind { get; set; }

        public string company_status { get; set; }

        public string company_number { get; set; }

        public Address address { get; set; }

        public string address_snippet { get; set; }

        public Links links { get; set; }

        public string company_type { get; set; }

        public string date_of_creation { get; set; }

        public string description { get; set; }

    }

    public class Address
    {

        public string address_line_1 { get; set; }

        public string premises { get; set; }

        public string locality { get; set; }

        public string postal_code { get; set; }

    }

    public class Links
    {

        public string self { get; set; }

    }

}
