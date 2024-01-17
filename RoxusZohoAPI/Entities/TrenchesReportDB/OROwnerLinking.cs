using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{
    [Table("OR_OwnerLinkings")]
    public class OROwnerLinking
    {

        public string OwnerId { get; set; }

        public string OwnerName { get; set; }

        public string OwnerType { get; set; }

        public string ReduceCompanyName { get; set; }

        public string CompanyNumber { get; set; }

        public string PropertyAddress { get; set; }

        public string TitleId { get; set; }

        public string TitleNumber { get; set; }

        public string OpenreachId { get; set; }

        public string OpenreachNumber { get; set; }

        public string LetterReference { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}
