using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.RoxusDB
{
    [Table("APILogging")]
    public class ApiLogging
    {

        [Key]
        public string Id { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public string ApplicationName { get; set; }

        public string CustomerName { get; set; }

        public string ApplicationId { get; set; }

        public string Status { get; set; }

        public string CreatedDate { get; set; }

        public string HttpMethod { get; set; }

        public string ApiName { get; set; }

        public string Endpoint { get; set; }

    }
}
