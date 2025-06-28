using System;

namespace RoxusZohoAPI.Models.CompleteASAP.Hoowla
{
    public class UsersListUsersEmployeesResponse
    {
        public int? user_id { get; set; }
        public string user_email { get; set; }
        public string user_title { get; set; }
        public string user_fname { get; set; }
        public string user_lname { get; set; }
        public DateTime? user_dob { get; set; }
        public int? user_trading_style { get; set; }
        public string user_job_title { get; set; }
        public bool? user_blocked { get; set; }
        public int? branch_id { get; set; }
        public string branch_name { get; set; }
        public string exchange_status { get; set; }
        public string premission_level { get; set; }
    }
}