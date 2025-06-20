using System;

namespace RoxusZohoAPI.Models.Nintex
{

    public class AddTask244Response
    {

        public Data data { get; set; }

    }

    public class Data
    {

        public Addtask addTask { get; set; }

    }

    public class Addtask
    {

        public string id { get; set; }

        public string name { get; set; }

        public string queueId { get; set; }

        public string tenantId { get; set; }

        public DateTime? createdAt { get; set; }

        public string state { get; set; }

    }

}
