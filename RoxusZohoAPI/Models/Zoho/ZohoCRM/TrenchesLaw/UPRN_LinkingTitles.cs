using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class UPRN_LinkingTitles
    {
        public UPRN_LinkingTitles()
        {
            UPRN_Titles = new List<UPRN_RelatedTitles>();
        }
        public string id { get; set; }
        public List<UPRN_RelatedTitles> UPRN_Titles { get; set; }
    }

    public class UPRN_RelatedTitles
    {
        public string Title { get; set; }
    }
}
