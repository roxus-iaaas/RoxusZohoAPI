using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{

    public class ViewCorporateOwnership
    {

        [Key]
        public string Title_Number { get; set; }

        public string Tenure { get; set; }

        public string Property_Address { get; set; }

        public string District { get; set; }

        public string County { get; set; }

        public string Region { get; set; }

        public string Postcode { get; set; }

        public string Multiple_Address_Indicator { get; set; }

        public string Price_Paid { get; set; }

        public string Proprietor_Name_1 { get; set; }

        public string Company_Registration_No_1 { get; set; }

        public string Proprietorship_Category_1 { get; set; }

        public string Proprietor_1_Address_1 { get; set; }

        public string Proprietor_1_Address_2 { get; set; }

        public string Proprietor_1_Address_3 { get; set; }

        public string Proprietor_Name_2 { get; set; }

        public string Company_Registration_No_2 { get; set; }

        public string Proprietorship_Category_2 { get; set; }

        public string Proprietor_2_Address_1 { get; set; }

        public string Proprietor_2_Address_2 { get; set; }

        public string Proprietor_2_Address_3 { get; set; }

        public string Proprietor_Name_3 { get; set; }

        public string Company_Registration_No_3 { get; set; }

        public string Proprietorship_Category_3 { get; set; }

        public string Proprietor_3_Address_1 { get; set; }

        public string Proprietor_3_Address_2 { get; set; }

        public string Proprietor_3_Address_3 { get; set; }

        public string Proprietor_Name_4 { get; set; }

        public string Company_Registration_No_4 { get; set; }

        public string Proprietorship_Category_4 { get; set; }

        public string Proprietor_4_Address_1 { get; set; }

        public string Proprietor_4_Address_2 { get; set; }

        public string Proprietor_4_Address_3 { get; set; }

        public string Date_Proprietor_Added { get; set; }

        public string Additional_Proprietor_Indicator { get; set; }

    }

}
