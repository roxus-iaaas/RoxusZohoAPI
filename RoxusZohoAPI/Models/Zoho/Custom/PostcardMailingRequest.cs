using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.Custom
{
    public class PostcardMailingRequest
    {
        public PostcardMailingRequest()
        {
            Postcards = new List<PostcardDetail>();
        }
        public List<PostcardDetail> Postcards { get; set; }
        public string TaskUrl { get; set; }
        public string TitleId { get; set; }
        public string TitleNumber { get; set; }
        public string ProjectName { get; set; }
        public string Credential { get; set; }
        public string ProjectId { get; set; }
        public string TaskId { get; set; }
    }

    public class PostcardDetail
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string FullName { get; set; }
        public string Custom1 { get; set; }
    }
}
