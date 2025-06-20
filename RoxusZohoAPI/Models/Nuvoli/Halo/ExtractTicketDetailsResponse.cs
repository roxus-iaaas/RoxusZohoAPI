using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class ExtractTicketDetailsResponse
    {

        public string username { get; set; }

        public string userEmailAddress { get; set; }

        public string mobileUsersLineManager { get; set; }

        public string preferredDeliveryAddress { get; set; }

        public string deliveryContactNumber { get; set; }

        public string nhsTicketUrl { get; set; }

        public string postcode { get; set; }

    }

}
