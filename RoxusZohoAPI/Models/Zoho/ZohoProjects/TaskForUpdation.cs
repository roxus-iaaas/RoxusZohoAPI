using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{
    public class TaskForUpdation
    {
        // Start Date
        public string start_date { get; set; }

        // Due Date
        public string end_date { get; set; }

        // Status
        public string custom_status { get; set; }

        // Letter Date 1
        public string UDF_DATE1 { get; set; }

        // Letter 1 Status
        public string UDF_CHAR4 { get; set; }

        // Letter Date 2
        public string UDF_DATE2 { get; set; }

        // Letter 2 Status
        public string UDF_CHAR5 { get; set; }

        // Letter Date 3
        public string UDF_DATE3 { get; set; }

        // Letter 3 Status
        public string UDF_CHAR6 { get; set; }

        // Letter Date 4
        public string UDF_DATE4 { get; set; }

        // Letter 4 Status
        public string UDF_CHAR7 { get; set; }

        // Reference
        public string UDF_CHAR57 { get; set; }

        // Wayleave Template
        public string UDF_CHAR2 { get; set; }

        // Letter Reference
        public string UDF_CHAR66 { get; set; }

        // Contact Type
        public string UDF_CHAR11 { get; set; }

        // Wayleave Received Date
        public string UDF_CHAR50 { get; set; }

        // Wayleave Received
        public string UDF_BOOLEAN4 { get; set; }

        // RA Received
        public string UDF_CHAR67 { get; set; }

        // RA Date
        public string UDF_CHAR68 { get; set; }

        // Asbestos Register Received
        public string UDF_CHAR69 { get; set; }

        // Asbestos Register Date
        public string UDF_CHAR64 { get; set; }

        // Asbestos Form Response
        public string UDF_CHAR58 { get; set; }

        // Hold Reason
        public string UDF_MULTI2 { get; set; }

        // Data Phase
        public long? UDF_LONG10 { get; set; }
        
        public string CreateRequestString()
        {
            int countComma = 0;
            string result = "{";
            if (!string.IsNullOrWhiteSpace(UDF_DATE1))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_DATE1\":\"" + UDF_DATE1 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR4))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR4\":\"" + UDF_CHAR4 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR57))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR57\":\"" + UDF_CHAR57 + "\"";
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
            if (!string.IsNullOrWhiteSpace(UDF_DATE2))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_DATE2\":\"" + UDF_DATE2 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR5))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR5\":\"" + UDF_CHAR5 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_DATE3))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_DATE3\":\"" + UDF_DATE3 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR6))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR6\":\"" + UDF_CHAR6 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_DATE4))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_DATE4\":\"" + UDF_DATE4 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR7))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR7\":\"" + UDF_CHAR7 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR66))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR66\":\"" + UDF_CHAR66 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR64))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR64\":\"" + UDF_CHAR64 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR67))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR67\":\"" + UDF_CHAR67 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR11))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR11\":\"" + UDF_CHAR11 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR50))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR50\":\"" + UDF_CHAR50 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_BOOLEAN4))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_BOOLEAN4\":\"" + UDF_BOOLEAN4 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR68))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR68\":\"" + UDF_CHAR68 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR69))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR69\":\"" + UDF_CHAR69 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_CHAR58))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_CHAR58\":\"" + UDF_CHAR58 + "\"";
                countComma++;
            }
            if (!string.IsNullOrWhiteSpace(UDF_MULTI2))
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_MULTI2\":\"" + UDF_MULTI2 + "\"";
                countComma++;
            }
            if (UDF_LONG10.HasValue)
            {
                if (countComma > 0)
                {
                    result += ",";
                }
                result += "\"UDF_LONG10\":" + UDF_LONG10 + "";
                countComma++;
            }
            result += "}";
            return result;
        }
    }
}
