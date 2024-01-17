using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoProjects
{
    public class GetAllDocumentsResponse
    {
        public string parent_name { get; set; }
        public Total_Resources[] total_resources { get; set; }
        public Preference preference { get; set; }
        public Dataobj[] dataobj { get; set; }
        public string ws_type { get; set; }
        public object[] display_fields { get; set; }
    }

    public class Preference
    {
        public string layout { get; set; }
        public string sort_by { get; set; }
        public Filter[] filters { get; set; }
        public string sort_order { get; set; }
    }

    public class Filter
    {
        public string name { get; set; }
        public bool show { get; set; }
        public string display_name { get; set; }
    }

    public class Total_Resources
    {
        public int no_of_res { get; set; }
    }

    public class Dataobj
    {
        public bool? split { get; set; }
        public string split_value { get; set; }
        public bool? is_rename { get; set; }
        public string author_name { get; set; }
        public bool? is_res_shared { get; set; }
        public string encattr_res_name { get; set; }
        public bool? is_locked { get; set; }
        public bool? is_favorite { get; set; }
        public string encattr_author_name { get; set; }
        public bool? is_fol_author { get; set; }
        public string res_size { get; set; }
        public string res_name { get; set; }
        public string last_modified_by_name { get; set; }
        public string encurl_res_name { get; set; }
        public string docs_preview_url { get; set; }
        public long? created_time_milliseconds { get; set; }
        public string no_preview_class { get; set; }
        public string library_id { get; set; }
        public string shared_time { get; set; }
        public string res_id { get; set; }
        public string download_url { get; set; }
        public bool? is_devfile { get; set; }
        public string encauthor_name { get; set; }
        public bool? is_media { get; set; }
        public string class_name { get; set; }
        public string created_time { get; set; }
        public string enc_res_name { get; set; }
        public bool? is_active { get; set; }
        public string last_modified_time { get; set; }
        public bool? is_folder { get; set; }
        public string encurl_author_name { get; set; }
        public string docs_download_url { get; set; }
        public string last_opened_time { get; set; }
        public string last_modified_by { get; set; }
        public long? last_modified_time_milliseconds { get; set; }
        public string splitvalue { get; set; }
        public string res_extn { get; set; }
        public string res_type { get; set; }
        public string service_type { get; set; }
        public string last_modified_by_encattr_author_name { get; set; }
        public bool? is_video { get; set; }
        public bool? is_audio { get; set; }
        public string mime_type { get; set; }
        public string parent_id { get; set; }
        public string preview_url { get; set; }
        public string last_modified_by_author_name { get; set; }
        public long? last_opened_time_milliseconds { get; set; }
        public string author_id { get; set; }
        public string space_id { get; set; }
        public bool? is_res_viewed { get; set; }
    }

}
