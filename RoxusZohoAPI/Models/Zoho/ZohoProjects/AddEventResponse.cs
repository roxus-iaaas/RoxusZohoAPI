using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{


    public class AddEventResponse
    {

        public EventData[] events { get; set; }

    }

    public class EventData
    {

        public long? id { get; set; }

        public string title { get; set; }

        public string location { get; set; }

        public string scheduled_on { get; set; }

        public long? scheduled_on_long { get; set; }

        public string reminder { get; set; }

        public string repeat { get; set; }

        public int occurrences { get; set; }

        public int occurred { get; set; }

        public string duration_hour { get; set; }

        public string duration_minutes { get; set; }

        public bool is_open { get; set; }

        public Participant[] participants { get; set; }

    }

    public class Participant
    {

        public string participant_id { get; set; }

        public string participant_person { get; set; }

    }

}
