using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.PureFinance.Pipedrive
{

    public class GetDealByIdResponse
    {
        
        public bool? success { get; set; }
        
        public Data data { get; set; }
        
        public Additional_Data additional_data { get; set; }
        
        public object related_objects { get; set; }
    
    }

    public class Data
    {

        public int? id { get; set; }
        
        public Creator_User_Id creator_user_id { get; set; }
        
        public User_Id user_id { get; set; }
        
        public Person_Id person_id { get; set; }
        
        public object org_id { get; set; }
        
        public int stage_id { get; set; }
        
        public string title { get; set; }
        
        public int value { get; set; }
        
        public object acv { get; set; }
        
        public object mrr { get; set; }
        
        public object arr { get; set; }
        
        public string currency { get; set; }
        
        public string add_time { get; set; }
        
        public string update_time { get; set; }
        
        public object stage_change_time { get; set; }
        
        public bool? active { get; set; }
        
        public bool? deleted { get; set; }
        
        public string status { get; set; }
        
        public object probability { get; set; }
        
        public object next_activity_date { get; set; }
        
        public object next_activity_time { get; set; }
        
        public object next_activity_id { get; set; }
        
        public object last_activity_id { get; set; }
        
        public object last_activity_date { get; set; }
        
        public string lost_reason { get; set; }
        
        public string visible_to { get; set; }
        
        public string close_time { get; set; }
        
        public int? pipeline_id { get; set; }
        
        public object won_time { get; set; }
        
        public object first_won_time { get; set; }
        
        public string lost_time { get; set; }
        
        public int? products_count { get; set; }
        
        public int? files_count { get; set; }
        
        public int? notes_count { get; set; }
        
        public int? followers_count { get; set; }
        
        public int? email_messages_count { get; set; }
        
        public int? activities_count { get; set; }
        
        public int? done_activities_count { get; set; }
        
        public int? undone_activities_count { get; set; }
        
        public int? participants_count { get; set; }
        
        public object expected_close_date { get; set; }
        
        public object last_incoming_mail_time { get; set; }
        
        public object last_outgoing_mail_time { get; set; }
        
        public object label { get; set; }
        
        public object local_won_date { get; set; }
        
        public string local_lost_date { get; set; }
        
        public string local_close_date { get; set; }
        
        public string origin { get; set; }
        
        public object origin_id { get; set; }
        
        public object channel { get; set; }
        
        public object channel_id { get; set; }
        
        public int? stage_order_nr { get; set; }
        
        public string person_name { get; set; }
        
        public object org_name { get; set; }
        
        public object next_activity_subject { get; set; }
        
        public object next_activity_type { get; set; }
        
        public object next_activity_duration { get; set; }
        
        public object next_activity_note { get; set; }
        
        public string formatted_value { get; set; }
        
        public int? weighted_value { get; set; }
        
        public string formatted_weighted_value { get; set; }
        
        public string weighted_value_currency { get; set; }
        
        public object rotten_time { get; set; }
        
        public object acv_currency { get; set; }
        
        public object mrr_currency { get; set; }
        
        public object arr_currency { get; set; }
        
        public string owner_name { get; set; }
        
        public string cc_email { get; set; }
        
        public bool? org_hidden { get; set; }

        public bool? person_hidden { get; set; }

        public Average_Time_To_Won average_time_to_won { get; set; }

        public int? average_stage_progress { get; set; }

        public Age age { get; set; }
        
        public object last_activity { get; set; }
        
        public object next_activity { get; set; }
    
    }

    public class Creator_User_Id
    {

        public int? id { get; set; }
        
        public string name { get; set; }
        
        public string email { get; set; }
        
        public int has_pic { get; set; }
        public object pic_hash { get; set; }
        public bool active_flag { get; set; }
        public int value { get; set; }
    }

    public class User_Id
    {
        public int id { get; set; }
        
        public string name { get; set; }
        
        public string email { get; set; }
        
        public int? has_pic { get; set; }
        
        public object pic_hash { get; set; }
        
        public bool? active_flag { get; set; }
        
        public int? value { get; set; }
    
    }

    public class Person_Id
    {

        public bool? active_flag { get; set; }
        
        public string name { get; set; }
        
        public Email[] email { get; set; }
        
        public Phone[] phone { get; set; }
        
        public int? owner_id { get; set; }
        
        public int? value { get; set; }
    
    }

    public class Email
    {
        
        public string label { get; set; }
        
        public string value { get; set; }
        
        public bool? primary { get; set; }
    
    }

    public class Phone
    {
        
        public string label { get; set; }
        
        public string value { get; set; }
        
        public bool? primary { get; set; }
    
    }

    public class Email1
    {

        public string label { get; set; }
        
        public string value { get; set; }
        
        public bool? primary { get; set; }
    
    }

    public class Phone1
    {
        public string label { get; set; }
        public string value { get; set; }
        public bool primary { get; set; }
    }

    public class Average_Time_To_Won
    {

        public int? y { get; set; }
        
        public int? m { get; set; }
        
        public int? d { get; set; }
        
        public int? h { get; set; }
        
        public int? i { get; set; }
        
        public int? s { get; set; }
        
        public int? total_seconds { get; set; }
    
    }

    public class Age
    {
        
        public int? y { get; set; }

        public int? m { get; set; }
        
        public int? d { get; set; }
        
        public int? h { get; set; }
        
        public int? i { get; set; }
        
        public int? s { get; set; }
        
        public int? total_seconds { get; set; }
    
    }

    public class Additional_Data
    {
        public string dropbox_email { get; set; }
    }

}
