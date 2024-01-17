using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers
{
    public class StringHelpers
    {

        public static string NullToString(object Value)
        {

            // Value.ToString() allows for Value being DBNull, but will also convert int, double, etc.
            return Value == null ? "" : Value.ToString();

            // which will throw if Value isn't actually a string object.
            //return Value == null || Value == DBNull.Value ? "" : (string)Value;

        }

        public static string ReduceCompanyName(string companyName)
        {

            return companyName.Replace("Company", "CO.", StringComparison.InvariantCultureIgnoreCase)
                              .Replace("Limited", "LTD", StringComparison.InvariantCultureIgnoreCase)
                              .Replace("Management", "MGT.", StringComparison.InvariantCultureIgnoreCase)
                              .Replace("Association", "ASSOC.", StringComparison.InvariantCultureIgnoreCase);

        }

    }
}
