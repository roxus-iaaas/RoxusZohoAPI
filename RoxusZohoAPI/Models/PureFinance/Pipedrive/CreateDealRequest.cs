using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.PureFinance.Pipedrive
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CreateDealRequest
    {

        public string user_id { get; set; }

        public string person_id { get; set; }

        public string stage_id { get; set; }

        public string pipeline_id { get; set; }

        public string title { get; set; }

        [JsonProperty("584179c1c26612544a0c9664eea87ed2429a8718")]
        public string EnquiryType { get; set; }

        [JsonProperty("bc3d135c086aa32235d4ca6563cd8296fdb86659")]
        public string Client { get; set; }

        [JsonProperty("e9f867eabf4bd3942f165531a2abd2593ec1358c")]
        public string EnquiryDate { get; set; }

        [JsonProperty("689c1a61ef1137c4661836775bf1dbff7c848030")]
        public decimal? EnquiryLoanAmount { get; set; }

        [JsonProperty("7d12f9d04f486789f2b15632777ce2b0af8e0ffc")]
        public string EnquirySource { get; set; }

        [JsonProperty("b87299ca8ceb9858584a0e92f75f2e4bf5724e0a")]
        public string LoanPurpose { get; set; }

    }

}
