using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Nimbus
{
   
    public class SiteInventory1Response
    {
        public SiteInventory1Response()
        {
            results = new List<SiteInventoryResult>();
        }

        public List<SiteInventoryResult> results { get; set; }
    }

    public class SiteInventoryResult
    {

        public SiteInventoryResult()
        {
            leaseholdTitleNumbers = new List<LeaseholdTitleNumber>();
        } 

        public string foundBuildingNumber { get; set; }

        public string foundBuildingName { get; set; }

        public string foundPostcode { get; set; }

        public string freeholdTitleNumber { get; set; }

        public List<LeaseholdTitleNumber> leaseholdTitleNumbers { get; set; }

        public bool corporatelyOwned { get; set; }

        public string corporatelyOwnedTitle { get; set; }

        public string corporatelyOwnedTitleTenure { get; set; }

    }

    public class LeaseholdTitleNumber
    {

        public LeaseholdTitleNumber()
        {
            uprNs = new List<string>();
        }

        public string leasehold { get; set; }

        public List<string> uprNs { get; set; }

    }

}
