using System;
using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class CasesListDocumentEntitiesResponse
    {
        public int? document_entity_id { get; set; }
        public int? document_entity_is_multiple { get; set; }
        public DateTime? document_entity_created_at { get; set; }
        public int? entity_id { get; set; }
        public string entity_name { get; set; }
        public string entity_name_multiple { get; set; }
        public string entity_slug { get; set; }
        public int? entity_company_type { get; set; }
        public DateTime? entity_created_at { get; set; }
    }
}