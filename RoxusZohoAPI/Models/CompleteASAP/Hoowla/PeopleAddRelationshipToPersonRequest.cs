using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class PeopleAddRelationshipToPersonRequest
    {

        public string person_id { get; set; }

        public List<HoowlaRelationship> relationships { get; set; }

    }

    public class HoowlaRelationship
    {

        public int? id { get; set; }

        public int? type { get; set; } // 1 = Married, 2 = Working for

    }

}
