using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.PureFinance.Pipedrive
{

    public class SearchPersonsResponse
    {

        public bool? success { get; set; }
        
        public PersonData data { get; set; }
        
        public Person_Additional_Data additional_data { get; set; }
    
    }

    public class PersonData
    {

        public PersonItem[] items { get; set; }
    
    }

    public class PersonItem
    {

        public decimal? result_score { get; set; }
        
        public Item item { get; set; }
    
    }

    public class Item
    {

        public int? id { get; set; }
        
        public string type { get; set; }
        
        public string name { get; set; }
        
        public string[] phones { get; set; }
        
        public string[] emails { get; set; }
        
        public string primary_email { get; set; }
        
        public int? visible_to { get; set; }
        
        public Owner owner { get; set; }
        
        public object organization { get; set; }
        
        public object[] custom_fields { get; set; }
        
        public object[] notes { get; set; }
        
        public string update_time { get; set; }
    
    }

    public class Owner
    {

        public int? id { get; set; }
    
    }

    public class Person_Additional_Data
    {
        
        public Pagination pagination { get; set; }
    
    }

    public class Pagination
    {
        
        public int start { get; set; }
        
        public int limit { get; set; }
        
        public bool? more_items_in_collection { get; set; }
    
    }

}
