using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class LandRegPerformEnquiryByPropertyDescRequest
    {
        [JsonProperty("username")]
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonProperty("external_reference")]
        [JsonPropertyName("external_reference")]
        public string ExternalReference { get; set; }
        [JsonProperty("address_name")]
        [JsonPropertyName("address_name")]
        public string AddressName { get; set; }
        [JsonProperty("address_number")]
        [JsonPropertyName("address_number")]
        public string AddressNumber { get; set; }
        [JsonProperty("address_street")]
        [JsonPropertyName("address_street")]
        public string AddressStreet { get; set; }
        [JsonProperty("address_city")]
        [JsonPropertyName("address_city")]
        public string AddressCity { get; set; }
        [JsonProperty("address_postcode")]
        [JsonPropertyName("address_postcode")]
        public string AddressPostcode { get; set; }
    }
}
