using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class Title_LinkingOwners
    {
        public Title_LinkingOwners()
        {
            Related_Contacts = new List<Title_RelatedContacts>();
        }
        public string id { get; set; }
        public List<Title_RelatedContacts> Related_Contacts { get; set; }
    }

    public class Title_RelatedContacts
    {
        public string Contact_Name { get; set; }
        public string Account_Name { get; set; }
    }
}
