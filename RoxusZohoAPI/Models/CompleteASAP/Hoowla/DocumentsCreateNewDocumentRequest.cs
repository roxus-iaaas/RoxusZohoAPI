using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class DocumentsCreateNewDocumentRequest
    {
        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonProperty("case_id")]
        [JsonPropertyName("case_id")]
        public string CaseId { get; set; }
        [JsonProperty("task_id")]
        [JsonPropertyName("task_id")]
        public string TaskId { get; set; }
        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public string Data { get; set; }
        [JsonProperty("data_mime_type")]
        [JsonPropertyName("data_mime_type")]
        public string DataMimeType { get; set; }
        [JsonProperty("send_notification_email")]
        [JsonPropertyName("send_notification_email")]
        public string SendNotificationEmail { get; set; }
        [JsonProperty("tag_list")]
        [JsonPropertyName("tag_list")]
        public string TagList { get; set; }
        [JsonProperty("expected_size")]
        [JsonPropertyName("expected_size")]
        public string ExpectedSize { get; set; }
    }
}
