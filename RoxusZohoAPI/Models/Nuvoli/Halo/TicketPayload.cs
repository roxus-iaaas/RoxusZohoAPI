using System;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class TicketPayload
    {
        public string id { get; set; }

        public Ticket ticket { get; set; }
    
    }

    public class Ticket
    {
        public int? id { get; set; }
        
    }

}
