using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.ZohoDesk
{

    public class CreateTicketResponse
    {
        public DateTime? modifiedTime { get; set; }
        
        public object subCategory { get; set; }

        public string statusType { get; set; }
        
        public string subject { get; set; }
        
        public DateTime? dueDate { get; set; }
        
        public string departmentId { get; set; }
        
        public string channel { get; set; }
        
        public object onholdTime { get; set; }
        
        public string language { get; set; }
        
        public Source source { get; set; }
        
        public object resolution { get; set; }
        
        public object[] sharedDepartments { get; set; }
        
        public object closedTime { get; set; }
        
        public string approvalCount { get; set; }
        
        public bool? isOverDue { get; set; }
        
        public bool? isTrashed { get; set; }
        
        public DateTime? createdTime { get; set; }
        
        public string id { get; set; }
        
        public bool? isResponseOverdue { get; set; }
        
        public DateTime? customerResponseTime { get; set; }
        
        public object productId { get; set; }
        
        public string contactId { get; set; }
        
        public string threadCount { get; set; }
        
        public object[] secondaryContacts { get; set; }
        
        public string priority { get; set; }
        
        public string classification { get; set; }
        
        public string commentCount { get; set; }
        
        public string taskCount { get; set; }
        
        public object accountId { get; set; }
        
        public string phone { get; set; }
        
        public string webUrl { get; set; }
        
        public bool? isSpam { get; set; }
        
        public string status { get; set; }
        
        public object[] entitySkills { get; set; }
        
        public string ticketNumber { get; set; }
        
        public Customfields customFields { get; set; }
        
        public bool? isArchived { get; set; }
        
        public string description { get; set; }
        
        public string timeEntryCount { get; set; }
        
        public object channelRelatedInfo { get; set; }
        
        public object responseDueDate { get; set; }
        
        public bool? isDeleted { get; set; }
        
        public string modifiedBy { get; set; }
        
        public string email { get; set; }
        
        public LayoutDetails layoutDetails { get; set; }
        
        public object channelCode { get; set; }
        
        public Cf cf { get; set; }
        
        public string slaId { get; set; }
        
        public string layoutId { get; set; }
        
        public object assigneeId { get; set; }
        
        public object teamId { get; set; }
        
        public string attachmentCount { get; set; }
        
        public bool isEscalated { get; set; }
        
        public object category { get; set; }

    }

    public class Source
    {
        
        public object appName { get; set; }
        
        public object extId { get; set; }
        
        public object permalink { get; set; }
        
        public string type { get; set; }
        
        public object appPhotoURL { get; set; }
    
    }
    public class Customfields
    {
      
        public object SingleLine1 { get; set; }
        
        public object ErrorCode { get; set; }
    
    }

    public class LayoutDetails
    {
        
        public string id { get; set; }
        
        public string layoutName { get; set; }
    
    }

    public class Cf
    {
        
        public object cf_error_code { get; set; }
        
        public object cf_single_line_1 { get; set; }
    
    }

}
