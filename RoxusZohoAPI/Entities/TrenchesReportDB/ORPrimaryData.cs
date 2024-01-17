using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.TrenchesReportDB
{

    [Table("OR_PrimaryData")]
    public class ORPrimaryData
    {

        [Key]
        public string Openreach_SM { get; set; }

        public string THP { get; set; }

        public string THP_Slab { get; set; }

        public string OR_Primary_Account { get; set; }

        public string Programme_Segment { get; set; }

        public string Programme_Lead { get; set; }

        public string PON_Project_ID { get; set; }

        public long Parent_UPRN { get; set; }

        public DateTime? Building_Construction_Date { get; set; }

        public string SAU_ID_Name { get; set; }

        public string FTTP_Build_Program { get; set; }

        public string Deployment_QTR { get; set; }

        public string Parent_Account_Category { get; set; }

        public string Post_Code { get; set; }

        public string OR_Parent_Account { get; set; }

        public string Desk_Third_Party_Supplier { get; set; }

        public string MDU_Child_Record_Number { get; set; }

        public string OR_Account_Owner { get; set; }

        public string SAU_ID { get; set; }

        public string FND_Region { get; set; }

        public string Enablement_Patch_v2 { get; set; }

        public string Programme_Workstream { get; set; }

        public string Supplier_Region { get; set; }

        public string Enablement_Patch { get; set; }

        public string Address { get; set; }

        public string Google_Maps { get; set; }

        public string Title { get; set; }

        public string OR_Salesforce { get; set; }

        public string Wayleave_Received { get; set; }

        public DateTime? Wayleave_Date { get; set; }

        public DateTime? OR_Status_Date { get; set; }

        public DateTime? OR_Salesforce_Date { get; set; }

        public string Asbestos_Response { get; set; }

        public string OR_Status { get; set; }

        public DateTime? Asbestos_Response_Date { get; set; }

        public string Asbestos_Response_Detail { get; set; }

        public DateTime? Asbestos_Sent_Date { get; set; }

        public string Asbestos_Register { get; set; }

        public DateTime? Asbestos_Reveiced_Date { get; set; }

        public string Survey_Estimate_Number { get; set; }

        public DateTime? RA_Date { get; set; }

        public string Survey { get; set; }

        public string Route_Approval { get; set; }

        public string Removed_Reason { get; set; }

        public string Hold_Reason { get; set; }

        public string Agent { get; set; }

        public string Registration_Status { get; set; }

        public string Contact_Type { get; set; }

        public string OR_MDU_Owner_Name { get; set; }

        public string OR_Secondary_Owner { get; set; }

        public int? Google_Maps_Processed { get; set; }

        public int? Nimbus_UPRN_Processed { get; set; }

        public int? Nimbus_Title_Purchased { get; set; }

        public string Title_Created { get; set; }

        public string Title_Linking_SM { get; set; }

        public string Title_Created_ID { get; set; }

        public int? UPRN_UK_Processed { get; set; }

        public string UPRN_Lat { get; set; }

        public string UPRN_Long { get; set; }

        public int? Check_My_Postcode_Processed { get; set; }

        public string Typical_Construction { get; set; }

        public int? Phase { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}
