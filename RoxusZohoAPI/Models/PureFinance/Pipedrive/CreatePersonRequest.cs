using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.PureFinance.Pipedrive
{
    
    [JsonObject(ItemNullValueHandling=NullValueHandling.Ignore)]
    public class CreatePersonRequest
    {

        public CreatePersonRequest()
        {

            email = new List<PersonEmail>();

            phone = new List<PersonPhone>();

        }

        public string name { get; set; }

        public string owner_id { get; set; }

        public List<PersonEmail> email { get; set; }

        public List<PersonPhone> phone { get; set; }

        // Contact Type
        [JsonProperty("f57e9a008207e6740053fdd5bc432885b700ae57")]
        public string ContactType { get; set; }

        // Contact Source
        [JsonProperty("3eb90ef5db1bbd083c973d524734cdd83f5ef1b4")]
        public string ContactSource {get; set; }

        // Marketing Consent
        [JsonProperty("f33b5a9f1b7c8813129cf609965c51b564656a9b")]
        public string MarketingConsent { get; set; }

        // Client DOB
        [JsonProperty("7d6558578040f7cc612be534eb66258c17f5f73f")]
        public string ClientDOB { get; set; }

    }

    public class PersonEmail
    {

        public string value { get; set; }

        public bool? primary { get; set; }

        public string label { get; set; }

    }

    public class PersonPhone
    {

        public string value { get; set; }

        public bool? primary { get; set; }

        public string label { get; set; }

    }

}
