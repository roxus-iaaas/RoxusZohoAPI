using Newtonsoft.Json;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesAddPersonToACaseRequest
    {
        public int person_id { get; set; }

        public int case_id { get; set; }

        public int case_side { get; set; }
    }

}
