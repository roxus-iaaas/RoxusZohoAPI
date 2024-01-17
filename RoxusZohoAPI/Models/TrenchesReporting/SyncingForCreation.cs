using RoxusZohoAPI.Entities.TrenchesReportDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.TrenchesReporting
{
    public class SyncingForCreation
    {
        public string ModuleName { get; set; }
        public string ModuleId { get; set; }
        public SyncingAction Action { get; set; }
        public SyncingStatus Status { get; set; }
        public double ZohoDate { get; set; }
    }
}
