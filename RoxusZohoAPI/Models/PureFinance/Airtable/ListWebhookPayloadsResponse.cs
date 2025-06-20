using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.PureFinance.Airtable
{

    public class ListWebhookPayloadsResponse
    {

        public ListWebhookPayloadsResponse()
        {

        }

        public List<WebhookPayload> payloads { get; set; }
        
        public int? cursor { get; set; }
        
        public bool? mightHaveMore { get; set; }
        
        public string payloadFormat { get; set; }
    
    }

    public class WebhookPayload
    {
        
        public DateTime? timestamp { get; set; }
        
        public int? baseTransactionNumber { get; set; }
        
        public ActionMetadata actionMetadata { get; set; }
        
        public string payloadFormat { get; set; }
        
        public object changedTablesById { get; set; }
    
    }

    public class ActionMetadata
    {
        public string source { get; set; }
        public SourceMetadata sourceMetadata { get; set; }
    }

    public class SourceMetadata
    {
        public User user { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        
        public string email { get; set; }
        
        public string permissionLevel { get; set; }

        public string name { get; set; }

        public string profilePicUrl { get; set; }

    }

}
