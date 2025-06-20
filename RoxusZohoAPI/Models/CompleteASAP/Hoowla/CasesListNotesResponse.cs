using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesListNotesResponse
    {

        public CasesListNotesResponse()
        {
            
            Notes = new List<HoowlaNoteDetails>();

        }

        public List<HoowlaNoteDetails> Notes { get; set; }
    
    }

    public class HoowlaNoteDetails
    {

        public int? note_id { get; set; }
        
        public object note_task { get; set; }
        
        public int? note_case { get; set; }
        
        public int? note_user { get; set; }
        
        public int? note_creator { get; set; }
        
        public string note_created { get; set; }
        
        public string note_edited { get; set; }
        
        public int? note_is_alert { get; set; }
        
        public int? notetype_id { get; set; }
        
        public string notetype_type { get; set; }
        
        public string note_text { get; set; }
        
        public int? note_assigned_to_person { get; set; }
        
        public int? note_assigned_to_user { get; set; }
    
    }

}
