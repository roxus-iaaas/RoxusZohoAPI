using Newtonsoft.Json;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesCreateANoteRequest
    {
        public int case_id { get; set; }
        public int? task_id { get; set; }
        public string note_text { get; set; }
        public int? note_permissions { get; set; }
        public int? note_type { get; set; }
        public bool? note_is_alert { get; set; }
        public int? assign_to_person { get; set; }
        public int? assign_to_user { get; set; }
    }
}
