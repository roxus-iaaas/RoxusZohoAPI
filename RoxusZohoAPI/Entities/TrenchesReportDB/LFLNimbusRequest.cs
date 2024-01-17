using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{

    [Table("LFL_NimbusRequests")]
    public class LFLNimbusRequest
    {

        [Key]
        public long UPRN { get; set; }

        public string Name { get; set; }

        public string StatusCode { get; set; }

        public string Endpoint { get; set; }

        public string Response { get; set; }

        public DateTime? Created { get; set; }

    }

}
