using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers.Constants
{
    public class CommonConstants
    {
        public const string MSG_200 = "SUCCESS";
        public const string MSG_400 = "FAILED";
        public const string MSG_401 = "UNAUTHORIZED";

        public const string PlainDateTimeFormat = "yyyyMMddTHHmmss";
        public const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        public const string TaskDateFormat = "MM-dd-yyyy";
        public const int SaltSize = 16;
        public const string BigChangeApp = "BigChange";

        public const string Outlook_Email_SmtpServer = "smtp-mail.outlook.com";
        public const string Zoho_Email_SmtpServer = "smtppro.zoho.com";
        public const string Gmail_SmtpServer = "smtp.gmail.com";

        public const string Email_Username = "hello@roxus.io";
        public const string Email_Password = "frxjhkydvgbcyvrt";
        public const int SmtpPort = 587;

        public const string MyEmail_Username = "hoagsun@gmail.com";
        public const string MyEmail_Password = "kkhgttmqyogyooux";

        #region Companies House
        public const string CompaniesHouseKey = "QURFMW9pU1RaLXppRGNRSWx6VUFVazVoRU1zWkRTRWNnTkFaUm83ODo=";
        public const string CompaniesHouseEndpoint = "https://api.company-information.service.gov.uk/search/companies?q=";
        #endregion

        #region Kryon
        public const string RowDelimiter = "###^#^###";
        public const string ColumnDelimiter = "~~*~*~~";
        #endregion

        #region Blob Storage

        public const string Roxus_ClientSecret = "-j-8Q~5QUGFLH5h1vy~SnyNy6z4THyyjN2vepcn1";

        public const string Roxus_BlobStorage = "roxusautomationlogs";

        #endregion

    }
}
