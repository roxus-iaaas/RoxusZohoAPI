using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace RoxusZohoAPI.Models.Nintex
{

    public class AddTask244Request
    {

        public string query { get; set; }

        public Variables244 variables { get; set; }

    }

    public class Variables244
    {

        public Task244 task { get; set; }

    }

    public class Task244
    {

        public Task244()
        {

            variables = new List<Variable244>();

        }

        public string queueId { get; set; }

        public string name { get; set; }

        public string wizardCustomName { get; set; }

        public int? priority { get; set; }

        public string tenantId { get; set; }

        public List<Variable244> variables { get; set; }

    }

    public class Variable244
    {

        public string name { get; set; }

        public string value { get; set; }

    }

}
