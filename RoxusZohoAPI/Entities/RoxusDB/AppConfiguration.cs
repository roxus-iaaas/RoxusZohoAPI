using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.RoxusDB
{
    [Table("AppConfiguration")]
    public class AppConfiguration
    {

        [Key]
        public string Id { get; set; }
        
        public string CustomerName { get; set; }
        
        public string AuthEndPoint { get; set; }
        
        public string Platform { get; set; }
        
        public string RefreshToken { get; set; }
        
        public string AccessToken { get; set; }
        
        public string ExpiredTime { get; set; }
        
        public string ClientId { get; set; }
        
        public string ClientSecret { get; set; }
        
        public string TenantId { get; set; }
        
        public string EndPoint { get; set; }
        
        public string Environment { get; set; }

        public string AuthType { get; set; }

        public string RedirectUri { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

    }
}
