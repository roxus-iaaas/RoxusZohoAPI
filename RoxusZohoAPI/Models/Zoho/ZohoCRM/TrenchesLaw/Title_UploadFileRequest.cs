using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class Title_UploadFileRequest
    {
        public string TitleId { get; set; }
        public string FileName { get; set; }
        public string FileContent { get; set; }
    }
}
