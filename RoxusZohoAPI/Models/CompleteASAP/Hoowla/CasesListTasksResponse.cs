using System.Collections.Generic;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{

    public class CasesListTasksResponse
    {
    
        public List<HoowlaTaskDetails> Tasks { get; set; }
    
    }

    public class HoowlaTaskDetails
    {

        public int? task_id { get; set; }
        
        public int? task_group { get; set; }
        
        public string task_name { get; set; }
        
        public string task_parent { get; set; }
        
        public int? task_status { get; set; }
        
        public string task_created { get; set; }
        
        public string task_completed { get; set; }
        
        public string task_edited { get; set; }
        
        public string task_expected { get; set; }
        
        public string taskgroup_name { get; set; }
        
        public Status[] statuses { get; set; }
    
    }

    public class Status
    {
        
        public int? status_id { get; set; }
        
        public string status_name { get; set; }
        
        public string status_created { get; set; }
        
        public string status_completed { get; set; }
        
        public int? status_completed_by { get; set; }
        
        public bool? status_compulsory { get; set; }
        
        public int? status_parent { get; set; }
        
        public object status_due_date { get; set; }
        
        public object status_assigned_to { get; set; }
        
        public object status_assigned_to_name { get; set; }
        
        public object status_assigned_to_email { get; set; }
    
    }

}
