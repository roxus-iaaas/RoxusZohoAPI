using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{
    public class TaskForCreation
    {
        public string name { get; set; }
        public string tasklist_id { get; set; }
        // Title CRM
        public string UDF_CHAR1 { get; set; }
        // Wayleave Template
        public string UDF_CHAR2 { get; set; }
        // Title Task
        public string UDF_CHAR82 { get; set; }
        // Title Type
        public string UDF_CHAR83 { get; set; }

        public string CreateRequestString()
        {
            int countComma = 0;
            string result = "{";
            if (!string.IsNullOrWhiteSpace(UDF_CHAR82))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR82\":\"" + UDF_CHAR82 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR83))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR83\":\"" + UDF_CHAR83 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR1))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR1\":\"" + UDF_CHAR1 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR2))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR2\":\"" + UDF_CHAR2 + "\"";
                countComma++;
            }
            result += "}";
            return result;
        }
    }
}
