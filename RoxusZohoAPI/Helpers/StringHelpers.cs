using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers
{
    public class StringHelpers
    {

        public static string Base64Encode(string input)
        {

            // Decode the Base64 string to a byte array
            byte[] byteArray = Encoding.UTF8.GetBytes(input);

            // Encode byte array to a Base64 string
            string base64Encoded = Convert.ToBase64String(byteArray);

            return base64Encoded;
        }

        public static string Base64Decode(string encoded)
        {

            // Decode the Base64 string to a byte array
            byte[] byteArray = Convert.FromBase64String(encoded);

            // Convert byte array back to a string
            string originalString = Encoding.UTF8.GetString(byteArray);

            return originalString;
        }

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

        public static string ExtractUKPostcode(string address)
        {

            // Define a regex pattern to match UK postcodes
            string pattern = @"[A-Z]{1,2}\d{1,2}[A-Z]?[-\s]?\d[A-Z]{2}";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Find the match
            Match match = regex.Match(address);

            // Return the cleaned postcode with a space included
            if (match.Success)
            {
                string postcode = match.Value;

                // Replace any hyphen with a space
                postcode = postcode.Replace("-", " ");

                // Ensure the format with space: insert space before the last three characters if it's missing
                if (!postcode.Contains(" ") && postcode.Length > 3)
                {
                    postcode = postcode.Insert(postcode.Length - 3, " ");
                }

                return postcode.ToUpper();
            }

            // If nothing is found, return an appropriate message or an empty string
            return string.Empty;

        }

        public static string ExtractUKPhone(string text)
        {

            // Regex pattern to match UK postcodes
            string phonePattern = @"(?:\+44|0)(?:\s?\d\s?|\d){9,10}";
            Match match = Regex.Match(text, phonePattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Value;
            }
            return null;

        }

        public static string ExtractNHSServiceDeskURL(string text)
        {

            // Regex pattern to match UK postcodes
            string urlPattern = @"https://servicedesk\.propertyservices\.nhs\.uk/WorkOrder\.do\?woMode=viewWO&woID=\d+&PORTALID=\d+";
            Match match = Regex.Match(text, urlPattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Value;
            }
            return null;

        }

        public static Tuple<string, string> ExtractReferenceAndNumber(string text)
        {

            // Regular expression patterns
            string mobilePattern = @"\b(\+44|0044|0?7)\d{9}\b"; // Matches numbers starting with +44, 0044, or 07
            string referencePattern = @"Reference Number = (\w+)"; // Captures the reference number

            // Extract mobile number
            var mobileMatch = Regex.Match(text, mobilePattern);
            if (!mobileMatch.Success)
                return null;

            string mobileNumber = mobileMatch.Value;

            // Extract reference number
            var referenceMatch = Regex.Match(text, referencePattern);
            if (!referenceMatch.Success)
                return null;

            string referenceNumber = referenceMatch.Groups[1].Value;

            return Tuple.Create(mobileNumber, referenceNumber);

        }

        public static string ConvertEmailToName(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Regular expression to validate an email format
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(input, emailPattern))
                return input;

            // Split the input by '@' to separate the local part from the domain
            string localPart = input.Split('@')[0];

            // Replace hyphens with spaces
            localPart = localPart.Replace('-', ' ');

            // Split the modified local part by '.' to get individual name components
            string[] nameParts = localPart.Split('.');

            // Join the name parts with a space
            string name = string.Join(" ", nameParts);

            return name;
        }

        public static string ExtractRequestId(string input)
        {

            // Define regex pattern to match the request ID
            string pattern = @"\[Request ID :##(RE-\d+)##\]";

            // Match the pattern in the input string
            Match match = Regex.Match(input, pattern);

            // Check if match is successful and return the captured group
            if (match.Success)
            {
                return match.Groups[0].Value; // Returns the whole match including the brackets
            }
            
            return string.Empty;
        
        }

        public static string ExtractEmail(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            // This regex is for basic email extraction
            string pattern = @"[a-zA-Z0-9._%'+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            Match match = regex.Match(input);
            if (match.Success)
            {
                return match.Value; // Returns the first matched email
            }

            return null;
        }

    }
}
