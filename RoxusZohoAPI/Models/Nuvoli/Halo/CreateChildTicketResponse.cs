using System;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class CreateChildTicketResponse
    {
        public int? id { get; set; }
        public DateTime? dateoccurred { get; set; }
        public string summary { get; set; }
        public string details { get; set; }
        public int? status_id { get; set; }
        public int? tickettype_id { get; set; }
        
    }

}
