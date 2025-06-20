using Newtonsoft.Json;
using System.Collections.Generic;

namespace RoxusZohoAPI.Models.Nuvoli.Halo
{
    
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UpsertAssetData
    {

        public UpsertAssetData()
        {

            users = new List<AssetUser>();

            fields = new List<UpsertAssetField>();

        }

        public List<AssetUser> users { get; set; }

        public int? assettype_id { get; set; }

        public int? id { get; set; }
        
        public int? site_id { get; set; }
        
        public string inventory_number { get; set; }
        
        public int? status_id { get; set; }
        
        public List<UpsertAssetField> fields { get; set; }

    }

    public class AssetUser
    {
        public int? id { get; set; }
    }

    public class UpsertAssetField
    {

        public string id { get; set; }
        
        public string value { get; set; }
    
    }

}
