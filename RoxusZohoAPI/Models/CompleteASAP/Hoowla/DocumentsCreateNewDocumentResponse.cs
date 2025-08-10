using Newtonsoft.Json;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class DocumentsCreateNewDocumentResponse
    {
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }
        [JsonProperty("file_size_match")]
        public string FileSizeMatch { get; set; }
        [JsonProperty("http_code")]
        public string HttpCode { get; set; }
    }
}
