using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.RoxusDB
{
    [Table("APITransactionQueues")]
    public class ApiTransactionQueue
    {

        [Key]
        public int Id { get; set; }

        public int TransactionType { get; set; }

        public string TransactionName { get; set; }

        public string ProjectId { get; set; }

        public string TasklistId { get; set; }

        public string TaskId { get; set; }

        public string RequestedBy { get; set; }

        public int Status { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}
