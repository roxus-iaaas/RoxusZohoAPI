using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{
    public class GetFolderResponse
    {
        public FolderDetails[] folders { get; set; }
    }

    public class FolderDetails
    {
        public string author_name { get; set; }
        public bool is_res_shared { get; set; }
        public string encattr_res_name { get; set; }
        public string enc_res_name { get; set; }
        public object[] shared_users { get; set; }
        public string parent_folder_id { get; set; }
        public bool is_folder { get; set; }
        public string encattr_author_name { get; set; }
        public bool subfolder { get; set; }
        public bool opened { get; set; }
        public string res_name { get; set; }
        public string encurl_res_name { get; set; }
        public bool fetched_data { get; set; }
        public string enc_author_name { get; set; }
        public string res_type { get; set; }
        public bool is_opened { get; set; }
        public object[] children { get; set; }
        public string res_id { get; set; }
        public int scope { get; set; }
        public string author_id { get; set; }
    }
}
