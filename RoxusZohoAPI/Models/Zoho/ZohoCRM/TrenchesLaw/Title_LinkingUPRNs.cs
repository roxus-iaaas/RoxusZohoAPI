using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoCRM
{
    public class Title_LinkingUPRNs
    {
        public Title_LinkingUPRNs()
        {
            Related_UPRN = new List<Title_RelatedUprn>();
        }
        public string id { get; set; }
        public List<Title_RelatedUprn> Related_UPRN { get; set; }
    }

    public class Title_RelatedUprn
    {
        public string UPRN { get; set; }
    }
}
