using Newtonsoft.Json;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class LandRegPerformEnquiryByPropertyDescResponse
    {
        [JsonProperty("title_number")]
        public string TitleNumber { get; set; }
        [JsonProperty("tenure_type_code")]
        public string TenureTypeCode { get; set; }
        [JsonProperty("building_number")]
        public string BuildingNumber { get; set; }
        [JsonProperty("building_name")]
        public string BuildingName { get; set; }
        [JsonProperty("sub_building_name")]
        public string SubBuildingName { get; set; }
        [JsonProperty("city_name")]
        public string CityName { get; set; }
        [JsonProperty("street_name")]
        public string StreetName { get; set; }
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
    }

    public class LandRegPerformEnquiryByPropertyDescErrorResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
