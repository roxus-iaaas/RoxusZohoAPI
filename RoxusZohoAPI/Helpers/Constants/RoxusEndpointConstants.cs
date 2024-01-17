using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers.Constants
{
    public class RoxusEndpointConstants
    {

        public const string Docmail_SendLetter_TEST = "https://localhost:44321/api/docmail/custom/process-letter";

        public const string Docmail_SendLetter_PRODUCTION = "https://roxusdocmailapi2.azurewebsites.net/api/docmail/custom/process-letter";

        public const string Docmail_SendOpenreachLetter_TEST = "https://localhost:44321/api/docmail/custom/process-openreach-letter";

        public const string Docmail_SendOpenreachLetter_PRODUCTION = "https://roxusdocmailapi2.azurewebsites.net/api/docmail/custom/process-openreach-letter";

        public const string Docmail_BasicAuth_TEST = "c3VwcG9ydEByb3h1cy5pbzpSb3h1c0AyMDIw";

        public const string Docmail_BasicAuth_PRODUCTION = "c3VwcG9ydEByb3h1cy5pbzplRzdjYnA1ZlZIOVM4czNkczlHdg==";

    }
}
