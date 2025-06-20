namespace RoxusZohoAPI.Models.Nuvoli.Halo
{

    public class ListUsersResponse
    {

        public int? page_no { get; set; }
        
        public int? page_size { get; set; }
        
        public int? record_count { get; set; }
        
        public User[] users { get; set; }
    
    }

    public class User
    {

        public int? id { get; set; }
        
        public string name { get; set; }
        
        public float site_id { get; set; }
        
        public int? site_id_int { get; set; }
        
        public string site_name { get; set; }
        
        public string client_name { get; set; }
        
        public string firstname { get; set; }
        
        public string surname { get; set; }
        
        public string initials { get; set; }
        
        public string emailaddress { get; set; }
        
        public string sitephonenumber { get; set; }
        
        public int? telpref { get; set; }
        
        public bool? inactive { get; set; }
        
        public string colour { get; set; }
        
        public bool? isimportantcontact { get; set; }
        
        public bool? neversendemails { get; set; }
        
        public int? priority_id { get; set; }
        
        public int? linked_agent_id { get; set; }
        
        public bool? isserviceaccount { get; set; }
        
        public bool? ignoreautomatedbilling { get; set; }
        
        public bool? isimportantcontact2 { get; set; }
        
        public int? connectwiseid { get; set; }
        
        public int? autotaskid { get; set; }
        
        public int? messagegroup_id { get; set; }
        
        public string sitetimezone { get; set; }
        
        public int? client_account_manager_id { get; set; }
        
        public string use { get; set; }
        
        public int? key { get; set; }
        
        public int? table { get; set; }
        
        public int? client_id { get; set; }
        
        public int? overridepdftemplatequote { get; set; }
        
        public bool? is_prospect { get; set; }
        
        public string whatsapp_number { get; set; }
    
    }

}
