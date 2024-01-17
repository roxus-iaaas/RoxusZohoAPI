using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets
{

    public class GetListOfAttachments
    {
        public AttachmentDetails[] data { get; set; }
        public Info info { get; set; }
    }

    public class AttachmentDetails
    {
        public Owner Owner { get; set; }
        public DateTime Modified_Time { get; set; }
        public string File_Name { get; set; }
        public DateTime Created_Time { get; set; }
        public string Size { get; set; }
        public Parent_Id Parent_Id { get; set; }
        [JsonProperty("$file_id")]
        public string file_id { get; set; }
        public string type { get; set; }
        [JsonProperty("$se_module")]
        public string se_module { get; set; }
        public Modified_By Modified_By { get; set; }
        public string state { get; set; }
        public string id { get; set; }
        public Created_By Created_By { get; set; }
        public object link_url { get; set; }
    }

    public class Parent_Id
    {
        public string name { get; set; }
        public string id { get; set; }
    }
}
