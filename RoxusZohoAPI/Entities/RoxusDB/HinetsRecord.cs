using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.RoxusDB
{
    public class HinetsRecord
    {
        public int Id { get; set; }
        public string OpportunityId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleType { get; set; }
        public string ModuleName { get; set; }
        public string ModuleNote { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
