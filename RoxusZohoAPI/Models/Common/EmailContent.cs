using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Common
{
    public class EmailContent
    {

        public string Clients { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string FromName { get; set; }

        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public string Email { get; set; }
    
        public string Password { get; set; }

    }
}
