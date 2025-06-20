using Newtonsoft.Json;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{


    public class PeopleCreateAPersonCardRequest
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string person_title { get; set; }

        public string person_fname { get; set; }

        public string person_lname { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string person_email { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string person_dob { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string person_website { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string person_source { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string person_ni { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool person_is_company { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string person_company { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? person_type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string address_line1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string address_line2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string address_city { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string address_county { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string address_postcode { get; set; }

    }

}
