using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.PureFinance
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class OutputModel
    {

        public string PersonId { get; set; }

        public string DealId { get; set; }

        public string NoteId { get; set; }

        public string ErrorMessage { get; set; }

    }

}
