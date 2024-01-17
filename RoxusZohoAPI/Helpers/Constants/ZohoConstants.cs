using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers.Constants
{
    public class ZohoConstants
    {

        #region API Messages
        public const string MSG_200 = "OK";
        public const string MSG_201 = "Record created successfully";
        public const string MSG_204 = "NO CONTENT";
        public const string MSG_400 = "FAILED";
        public const string MSG_401 = "You don't have permission to access this resource.";

        // USRN
        public const string MSG_GET_USRR_BY_ID_400 = "Get USRN By Id Failed. Reason: ";
        #endregion

        #region Zoho CRM

        // Authorization
        public const string ZCRM_GET_ACCESS_TOKEN = "Zoho CRM: Get Access Token";
        public const string Trenches_CRM_Key = "dHJlbmNoZXNsYXc6em9ob2NybQ==";
        public const string Trenches_Project_Key = "dHJlbmNoZXNsYXc6em9ob3Byb2plY3Rz";

        // UPRN
        public const string ZCRM_UPRN_PREFIX_URL = "https://crm.zoho.eu/crm/org20069538520/tab/CustomModule1";
        public const string ZCRM_CREATE_UPRN = "Zoho CRM: Create UPRN";
        public const string ZCRM_UPSERT_UPRN = "Zoho CRM: Upsert UPRN";
        public const string ZCRM_UPDATE_UPRN = "Zoho CRM: Update UPRN";
        public const string ZCRM_GET_UPRN_BY_ID = "Zoho CRM: Get UPRN By Id";
        public const string ZCRM_GET_UPRN_BY_NAME = "Zoho CRM: Get UPRN By Name";
        public const string ZCRM_UPRN_LINKING_WITH_TITLE = "Zoho CRM: UPRN - Linking With Title";

        // USRN
        public const string ZCRM_CREATE_USRN = "Zoho CRM: Create USRN";
        public const string ZCRM_UPSERT_USRN = "Zoho CRM: Upsert USRN";
        public const string ZCRM_UPDATE_USRN = "Zoho CRM: Update USRN";
        public const string ZCRM_GET_USRN_BY_ID = "Zoho CRM: Get USRN By Id";
        public const string ZCRM_GET_USRN_BY_NAME = "Zoho CRM: Get USRN By Name";
        public const string ZCRM_USRN_LINKING_WITH_TITLE = "Zoho CRM: USRN - Linking With Title";

        // TITLE
        public const string TRENCHES_ZCRM_TITLE_PREFIX_URL = "https://crm.zoho.eu/crm/org20069538520/tab/CustomModule3";
        public const string ZCRM_CREATE_TITLE = "Zoho CRM: Create Title";
        public const string ZCRM_UPSERT_TITLE = "Zoho CRM: Upsert Title";
        public const string ZCRM_UPDATE_TITLE = "Zoho CRM: Update Title";
        public const string ZCRM_GET_TITLE_BY_ID = "Zoho CRM: Get Title By Id";
        public const string ZCRM_GET_TITLE_BY_NAME = "Zoho CRM: Get Title By Name";
        public const string ZCRM_TITLE_LINKING_WITH_UPRN = "Zoho CRM: Title - Linking With UPRN";
        public const string ZCRM_TITLE_LINKING_WITH_USRN = "Zoho CRM: Title - Linking With USRN";
        public const string ZCRM_TITLE_LINKING_WITH_OWNERS = "Zoho CRM: Title - Linking With Owners";
        public const string ZCRM_TITLE_UPLOAD_ATTACHMENT = "Zoho CRM: Title - Upload Attachment";

        // CONTACT
        public const string TRENCHES_ZCRM_CONTACT_PREFIX_URL = "https://crm.zoho.eu/crm/org20069538520/tab/Contacts";
        public const string ZCRM_CREATE_CONTACT = "Zoho CRM: Create Contact";
        public const string ZCRM_SEARCH_CONTACT_BY_EMAIL = "Zoho CRM: Search Contact By Email";
        public const string ZCRM_UPSERT_CONTACT = "Zoho CRM: Upsert Contact";
        public const string ZCRM_UPDATE_CONTACT = "Zoho CRM: Update Contact";
        public const string ZCRM_GET_CONTACT_BY_ID = "Zoho CRM: Upsert Contact";
        public const string ZCRM_CONTACT_UPLOAD_ATTACHMENT = "Zoho CRM: Contact - Upload Attachment";

        // ACCOUNT
        public const string TRENCHES_ZCRM_ACCOUNT_PREFIX_URL = "https://crm.zoho.eu/crm/org20069538520/tab/Accounts";
        public const string ZCRM_CREATE_ACCOUNT = "Zoho CRM: Create Account";
        public const string ZCRM_UPSERT_ACCOUNT = "Zoho CRM: Upsert Account";
        public const string ZCRM_UPDATE_ACCOUNT = "Zoho CRM: Update Account";
        public const string ZCRM_GET_ALL_ACCOUNTS = "Zoho CRM: Get All Accounts";
        public const string ZCRM_GET_ACCOUNT_BY_ID = "Zoho CRM: Get Account By Id";
        public const string ZCRM_SEARCH_ACCOUNT_BY_COMPANY_NUMBER = "Zoho CRM: Search Account by Company Number";
        public const string ZCRM_ACCOUNT_UPLOAD_ATTACHMENT = "Zoho CRM: Account - Upload Attachment";

        // DEALS
        public const string ZCRM_GET_DEAL_RELATED_PURCHASE_ORDERS = "Zoho CRM: Get Related Purchase Orders from Deal";
        public const string ZCRM_GET_DEAL_RELATED_PROJECT = "Zoho CRM: Get Related Projects from Deal";
        public const string ZCRM_GET_DEAL_ATTACHMENTS = "Zoho CRM: Get Deal Attachments";
        public const string ZCRM_DOWNLOAD_DEAL_ATTACHMENT = "Zoho CRM: Download Deal Attachment";
        #endregion

        #region Zoho Projects
        // Authorization
        public const string ZPRJ_GET_ACCESS_TOKEN = "Zoho Projects: Get Access Token";

        // PORTAL
        public const string ZPRJ_GET_PORTAL_ID = "Zoho Projects: Get Portal Id";

        // PROJECT
        public const string ZPRJ_PROJECT_DESCRIPTION = "Created by Roxus Automation";
        public const string ZPRJ_SEARCH_PROJECTS_IN_PORTAL = "Zoho Projects: Search Projects in Portal";
        public const string ZPRJ_GET_ALL_PROJECTS_IN_PORTAL = "Zoho Projects: Get All Projects in Portal";
        public const string ZPRJ_CREATE_PROJECT = "Zoho Projects: Create Project";
        public const string ZPRJ_GET_FOLDERS_IN_PROJECT = "Zoho Projects: Get Folders in Project";
        public const string ZPRJ_UPLOAD_DOCUMENTS = "Zoho Projects: Upload Document to Project";
        public const string ZPRJ_DELETE_DOCUMENT = "Zoho Projects: Upload Document to Project";

        // TASKLIST
        public const string ZPRJ_TASK_URL = "https://projects.zoho.eu/portal/trencheslimited#taskdetail/{ProjectId}/{TasklistId}/{TaskId}";
        public const string ZPRJ_TASKLIST_UNREGISTERED_ROAD = "[Unregistered Road]";
        public const string ZPRJ_GET_ALL_TASKLISTS_IN_PROJECT = "Zoho Projects: Get All Tasklists in Project";
        public const string ZPRJ_CREATE_TASKLIST = "Zoho Projects: Create Tasklist";

        // TASKS
        public const string ZPRJ_TASK_PREFIX_URL = "https://projects.zoho.eu/portal/trencheslimited#taskdetail";
        public const string ZPRJ_GET_ALL_TASKS_IN_PROJECT = "Zoho Projects: Get All Tasks in Project";
        public const string ZPRJ_SEARCH_TASKS_IN_PROJECT = "Zoho Projects: Search Tasks in Project";
        public const string ZPRJ_GET_TASK_DETAILS = "Zoho Projects: Get Task Details";
        public const string ZPRJ_CREATE_TASK = "Zoho Projects: Create Task";
        public const string ZPRJ_UPDATE_TASK = "Zoho Projects: Update Task";
        public const string ZPRJ_DELETE_TASK = "Zoho Projects: Delete Task";
        public const string ZPRJ_STATUS_LETTER1 = "85664000000582007";
        public const string ZPRJ_STATUS_LETTER2 = "85664000000582013";
        public const string ZPRJ_STATUS_LETTER3 = "85664000000582011";
        public const string ZPRJ_STATUS_REMOVED = "85664000002925881";
        public const string ZPRJ_STATUS_RXSREMOVED = "85664000003396597";
        public const string ZPRJ_STATUS_COMPLETE = "85664000002925879";

        #endregion

        #region Zoho Books
        // ITEMS
        public const string ZBOOKS_GET_ALL_ACTIVE_ITEMS = "Zoho Books: Get All Active Items";

        // PURCHASE ORDERS
        public const string ZBOOKS_GET_PURCHASEORDER_BY_ID = "Zoho Books: Get Purchase Order By Id";
        public const string ZBOOKS_UPDATE_PURCHASE_ORDER = "Zoho Books: Update Purchase Order";
        public const string ZBOOKS_DOWNLOAD_PURCHASE_ORDER_PDF = "Zoho Books: Download PO PDF";

        // ESTIMATES
        public const string ZBOOKS_GET_ESTIMATE_BY_ID = "Zoho Books: Get Estimate By Id";
        public const string ZBOOKS_SEARCH_ESTIMATES = "Zoho Books: Search Estimates";
        public const string ZBOOKS_UPDATE_ESTIMATE = "Zoho Books: Update Estimate";
        public const string ZBOOKS_DOWNLOAD_ESTIMATE_PDF = "Zoho Books: Download Estimate PDF";
        #endregion

        #region Zoho Custom
        // CUT: Create Unregistered Task
        // Error 1: Cannot get Title by Id
        public const string CUSTOM_CUT_ERROR_TT01 = "[CUT-TT01] Cannot get Title by Id";
        public const string CUSTOM_CUT_ERROR_TT02 = "[CUT-TT02] Title Type must be 'Unregistered Road'";
        public const string CUSTOM_CUT_ERROR_TT03 = "[CUT-TT03] Title Name must have correct format 'USRN_{USRN Number}'";
        public const string CUSTOM_CUT_ERROR_TT04 = "[CUT-TT04] Update Task URL in Title FAILED";
        public const string CUSTOM_CUT_ERROR_P01 = "[CUT-P01] Create Project FAILED";
        public const string CUSTOM_CUT_ERROR_TL01 = "[CUT-TL01] Create Task List FAILED";
        public const string CUSTOM_CUT_ERROR_TA01 = "[CUT-TA01] Create Task FAILED";

        // VSP: Validation before Sending Postcard
        public const string CUSTOM_VSP_SUCCESS = "[VSP-200] Postcard Validation SUCCESSFULLY, will send request to Docmail API.";
        public const string CUSTOM_VSP_ERROR = "[VSP-400] Postcard Validation FAILED, please check Desk Ticket for more details.";
        public const string CUSTOM_VSP_ERROR_TA01 = "[VSP-TA01] Cannot get Task Details";
        public const string CUSTOM_VSP_ERROR_TA02 = "[VSP-TA02] Title URL is empty";
        public const string CUSTOM_VSP_ERROR_TT01 = "[VSP-TT01] Task doesn't have valid Title Url";
        public const string CUSTOM_VSP_ERROR_TT02 = "[VSP-TT02] Cannot get Title by Id";
        public const string CUSTOM_VSP_ERROR_TT03 = "[VSP-TT03] Title doesn't have any Related UPRNs";
        public const string CUSTOM_VSP_ERROR_UP01 = "[VSP-UP01] Cannot get UPRN Details";
        public const string CUSTOM_VSP_ERROR_UP02 = "[VSP-UP02] UPRN Address 1 has more than 50 characters";

        // ITD: Insert Trenches Task to DB
        public const string CUSTOM_ITD_SUCCESS = "[ITD-200] Insert Trenches Task to DB SUCCESSFULLY.";
        public const string CUSTOM_ITD_ERROR = "[ITD-400] Insert Trenches Task to DB FAILED, please check Desk Ticket for more details.";
        public const string CUSTOM_ITD_P01 = "[ITD-P01] Cannot Get Project data.";
        public const string CUSTOM_ITD_T01 = "[ITD-T01] Cannot Get Task data.";

        // DTD: Delete Trenches Task from DB
        public const string CUSTOM_DTD_SUCCESS = "[DTD-200] Delete Trenches Task from DB SUCCESSFULLY.";
        public const string CUSTOM_DTD_ERROR = "[DTD-400] Delete Trenches Task from DB FAILED, please check Desk Ticket for more details.";

        // CTV: Check Task Valid to send letters
        public const string CUSTOM_CTV_SUCCESS = "[CTV-200] Task is valid for Sending Letter";
        public const string CUSTOM_CTV_D01 = "[CTV-D01] Due Date is null or empty";
        public const string CUSTOM_CTV_D02 = "[CTV-D02] Current Time is earlier than Due Date";
        public const string CUSTOM_CTV_S01 = "[CTV-S01] Status is not in the Valid Statuses to send Letter";
        public const string CUSTOM_CTV_T01 = "[CTV-T01] Task Name doesn't start with [Automation] or [PIA]";
        public const string CUSTOM_CTV_PIAL3 = "[CTV-PIAL3] Task List is [PIA] and Status is Letter 3, CANNOT SEND LETTER 4";
        public const string CUSTOM_CTV_CSL2 = "[CTV-CSL2] CANNOT SEND LETTER 2 AS LETTER 1 DATE OR LETTER 1 STATUS IS EMPTY";
        public const string CUSTOM_CTV_CSL3 = "[CTV-CSL3] CANNOT SEND LETTER 3 AS LETTER 1, 2 DATE OR LETTER 1, 2 STATUS IS EMPTY";
        public const string CUSTOM_CTV_CSL4 = "[CTV-CSL3] CANNOT SEND LETTER 4 AS LETTER 1, 2, 3 DATE OR LETTER 1, 2, 3 STATUS IS EMPTY";

        // PSL: Process Sending Letters
        public const string CUSTOM_PSL_SUCCESS = "[PSL-200] Send Letter SUCCESSFULLY";
        public const string CUSTOM_PSL_ERROR = "[PSL-400] Send Letter FAILED";
        public const string CUSTOM_PSL_T01 = "[PSL-T01] Cannot get Task Details";
        public const string CUSTOM_PSL_T02 = "[PSL-T02] Invalid Task Name: Task Name must contain Automation or PIA. Please go to Task URL and check: <a href='{TaskUrl}' target='_blank'>{TaskUrl}</a>";
        public const string CUSTOM_PSL_T03 = "[PSL-T03] Cannot Send Letter 4 for Task Type is PIA (POLE, PIT, BP). Please go to Task URL and check: <a href='{TaskUrl}' target='_blank'>{TaskUrl}</a>";
        public const string CUSTOM_PSL_T04 = "[PSL-T04] Task Status is invalid, must be Letter 1, Letter 2, Letter 3 or Letter 4. Please go to Task URL and check: <a href='{TaskUrl}' target='_blank'>{TaskUrl}</a>";
        public const string CUSTOM_PSL_T05 = "[PSL-T05] Update Task Details FAILED.";
        public const string CUSTOM_PSL_T06 = "[PSL-T06] Insert Task to Trenches Reporting DB.";
        public const string CUSTOM_PSL_LT01 = "[PSL-LT01] Letter 1 Date is empty, CANNOT SEND LETTER 2, 3 or 4. Please go to Task URL and check: <a href='{TaskUrl}' target='_blank'>{TaskUrl}</a>";
        public const string CUSTOM_PSL_LT02 = "[PSL-LT02] Letter 2 Date is empty, CANNOT SEND LETTER 3 or 4. Please go to Task URL and check: <a href='{TaskUrl}' target='_blank'>{TaskUrl}</a>";
        public const string CUSTOM_PSL_LT03 = "[PSL-LT03] Letter 3 Date is empty, CANNOT SEND LETTER 4. Please go to Task URL and check: <a href='{TaskUrl}' target='_blank'>{TaskUrl}</a>";
        public const string CUSTOM_PSL_C01 = "[PSL-C01] This client: {clientName} seem to be new, please check it";
        public const string CUSTOM_PSL_TT01 = "[PSL-TT01] Cannot get Title By Id";
        public const string CUSTOM_PSL_TT02 = "[PSL-TT02] Type must not be empty. Please go to Title URL and add data: <a href='{TitleUrl}' target='_blank'>{TitleUrl}</a>";
        public const string CUSTOM_PSL_TT03 = "[PSL-TT03] Reference must not be empty. Please go to Title URL and add data: <a href='{TitleUrl}' target='_blank'>{TitleUrl}</a>";
        public const string CUSTOM_PSL_TT04 = "[PSL-TT04] Project Name must not be empty. Please go to Title URL and add data: <a href='{TitleUrl}' target='_blank'>{TitleUrl}</a>";
        public const string CUSTOM_PSL_TT05 = "[PSL-TT05] Wayleave Template must not be empty. Please go to Title URL and add data: <a href='{TitleUrl}' target='_blank'>{TitleUrl}</a>";
        public const string CUSTOM_PSL_TT06 = "[PSL-TT06] Title must associate with at least 1 Contact or 1 Account. Please go to Title URL and add data: <a href='{TitleUrl}' target='_blank'>{TitleUrl}</a>";
        public const string CUSTOM_PSL_AC01 = "[PSL-AC01] Missing address data (Billing Street, Billing City and Billing Code are required). Please review Account data at: <a href='{AccountUrl}' target='_blank'>{AccountUrl}</a>";
        public const string CUSTOM_PSL_AC02 = "[PSL-AC02] Address field can only contain a maximum of 50 characters. Please review Account data at: <a href='{AccountUrl}' target='_blank'>{AccountUrl}</a>";
        public const string CUSTOM_PSL_AC03 = "[PSL-AC03] Address Name can only contain a maximum of 50 characters. Please review Account data at: <a href='{AccountUrl}' target='_blank'>{AccountUrl}</a>";
        public const string CUSTOM_PSL_CO01 = "[PSL-CO01] Missing address data (Mailing Street, Mailing City and Mailing Code are required). Please review Contact data at: <a href='{ContactUrl}' target='_blank'>{ContactUrl}</a>";
        public const string CUSTOM_PSL_CO02 = "[PSL-CO02] Address field can only contain a maximum of 50 characters. Please review Contact data at: <a href='{ContactUrl}' target='_blank'>{ContactUrl}</a>";

        // POL: Process Openreach Letters
        public const string CUSTOM_POL_SUCCESS = "[POL-200] Send Openreach Letter SUCCESSFULLY";
        public const string CUSTOM_POL_ERROR = "[POL-400] Send Openreach Letter FAILED";
        public const string CUSTOM_POL_T01 = "[POL-T01] Cannot get Task Details";
        public const string CUSTOM_POL_T02 = "[POL-T02] Invalid Task Name: Task Name must contain ORA, ORD or ORH. Please go to Task URL and check: <a href='{TaskUrl}' target='_blank'>{TaskUrl}</a>";
        public const string CUSTOM_POL_T03 = "[POL-T03] Invalid Task Status: Task Status must be Letter 1, Letter 2 or Letter 3. Please go to Task URL and check: <a href='{TaskUrl}' target='_blank'>{TaskUrl}</a>";
        public const string CUSTOM_POL_T04 = "[POL-T04] Update Task Details FAILED.";
        public const string CUSTOM_POL_O01 = "[POL-O01] Cannot get Openreach by Id.";
        public const string CUSTOM_POL_O02 = "[POL-O02] Openreach must associate with at least 1 Contact or 1 Account.";
        public const string CUSTOM_POL_O03 = "[POL-O03] Openreach location must be valid: London, North, Midlands, South & East or Wales & West.";
        public const string CUSTOM_POL_LT01 = "[PSL-LT01] Letter 1 Date is empty, CANNOT SEND LETTER 2 or 3. Please go to Task URL and check: {TaskUrl}";
        public const string CUSTOM_POL_LT02 = "[PSL-LT02] Letter 2 Date is empty, CANNOT SEND LETTER 3. Please go to Task URL and check: {TaskUrl}";
        public const string CUSTOM_POL_LD01 = "[PSL-LD01] Current Date ({CurrentDate}) must be greater than or equal to Letter 1 Date plus 16 days ({NextLetterDate}), CANNOT SEND LETTER 2. Please go to Task URL and check: <a href='{TaskUrl}' target='_blank'>{TaskUrl}</a>";
        public const string CUSTOM_POL_LD02 = "[PSL-LD02] Current Date ({CurrentDate}) must be greater than or equal to Letter 2 Date plus 16 days ({NextLetterDate}), CANNOT SEND LETTER 3. Please go to Task URL and check: <a href='{TaskUrl}' target='_blank'>{TaskUrl}</a>";
        public const string CUSTOM_POL_AC01 = "[POL-AC01] Missing address data (Billing Street, Billing City and Billing Code are required). Please review Account data at: <a href='{AccountUrl}' target='_blank'>{AccountUrl}</a>";
        public const string CUSTOM_POL_AC02 = "[POL-AC02] Address field can only contain a maximum of 50 characters. Please review Account data at: <a href='{AccountUrl}' target='_blank'>{AccountUrl}</a>";
        public const string CUSTOM_POL_AC03 = "[POL-AC03] Address Name can only contain a maximum of 50 characters. Please review Account data at: <a href='{AccountUrl}' target='_blank'>{AccountUrl}</a>";
        public const string CUSTOM_POL_CO01 = "[POL-CO01] Missing address data (Mailing Street, Mailing City and Mailing Code are required). Please review Contact data at: <a href='{ContactUrl}' target='_blank'>{ContactUrl}</a>";
        public const string CUSTOM_POL_CO02 = "[POL-CO02] Address field can only contain a maximum of 50 characters. Please review Contact data at: <a href='{ContactUrl}' target='_blank'>{ContactUrl}</a>";
        public const string CUSTOM_POL_RT01 = "[POL-RT01] Get Related Titles of Openreach FAILED. Please review Openreach SM details at: <a href='{ContactUrl}' target='_blank'>{OpenreachUrl}</a>";

        // RRO: Remove Redundant Occupiers
        public const string CUSTOM_RRO_O1 = "[RRO-O1] There is no Occupier in the title";
        public const string CUSTOM_RRO_O2 = "[RRO-O2] There are only Occupier(s) in the title";
        public const string CUSTOM_RRO_O3 = "[RRO-O3] Occupier(s) are removed SUCCESSFULLY";
        public const string CUSTOM_RR0_400 = "[RRO-400] Remove Occupier(s) from title FAILED";

        // UDP: Upload Documents to Project
        public const string CUSTOM_UDP_SUCCESS = "[UDP-200] Upload Documents to Project SUCCESSFULLY";
        public const string CUSTOM_UDP_O1 = "[UDP-O1] There is no Estimate associate with Deal Id";
        public const string CUSTOM_UDP_O2 = "[UDP-O2] There is no Purchase Orders associate with Deal Id";

        // UAP: Upload Attachments to Project
        public const string CUSTOM_UAP_200 = "[UAP-200] Upload Attachments to Project SUCCESSFULLY";
        public const string CUSTOM_UEP_200 = "[UEP-200] Upload Estimates to Project SUCCESSFULLY";
        public const string CUSTOM_UED_200 = "[UED-200] Upload Estimates to Deal SUCCESSFULLY";
        public const string CUSTOM_UPP_200 = "[UPP-200] Upload POs to Project SUCCESSFULLY";

        // UTD: Update Task Dates
        public const string CUSTOM_UTD_200 = "[UTD-200] Update Task Dates SUCCESSFULLY";
        public const string CUSTOM_UTD_400 = "[UTD-400] Update Task Dates FAILED";
        public const string CUSTOM_UTD_T01 = "[UTD-T01] Get Tasks from Project FAILED";
        public const string CUSTOM_UTD_T02 = "[UTD-T02] Installation Task doesn't have Start Date or Due Date";

        // RPD: Reupload PO Documents
        public const string CUSTOM_RPD_200 = "[RPD-200] Re-upload PO Documents SUCCESSFULLY";
        public const string CUSTOM_RPD_400 = "[RPD-400] Re-upload PO Documents FAILED";

        // UP: Update Project
        public const string CUSTOM_UP_200 = "[UP-200] Update Project SUCCESSFULLY";
        public const string CUSTOM_UP_400 = "[UP-400] Update Project FAILED";

        // UAD: Upload Accommodation Document
        public const string CUSTOM_UAD_200 = "[UAD-200] Update Accommodation Document SUCCESSFULLY";
        public const string CUSTOM_UAD_400 = "[UAD-400] Update Accommodation Document FAILED";

        // CIEE: Create Items Excel and Email
        public const string CUSTOM_CIEE_200 = "[CIEE-200] Create Items Excel and email SUCCESSFULLY";
        public const string CUSTOM_CIEE_400 = "[CIEE-200] Create Items Excel and email FAILED";

        // UIP: Update Item Price
        public const string CUSTOM_UIP_200 = "[UIP-200] Update Item Price SUCCESSFULLY";
        public const string CUSTOM_UIP_400 = "[UIP-400] Update Item Price FAILED";
        public const string CUSTOM_UIP_C01 = "[UIP-C01] Updated Contents is empty, SEND EMAIL TO USER";

        // STO: Sync from Task to Openreach CRM
        public const string CUSTOM_STO_200 = "[STO-200] Sync from Task to Openreach SUCCESSFULLY";
        public const string CUSTOM_STO_400 = "[STO-400] Sync from Task to Openreach FAILED";
        public const string CUSTOM_STO_T01 = "[STO-T01] Get Task Details from Zoho Projects FAILED";
        public const string CUSTOM_STO_T02 = "[STO-T02] Task Name MUST start with OR";
        public const string CUSTOM_STO_O01 = "[STO-O01] Search Openreach by Number FAILED";
        public const string CUSTOM_STO_O02 = "[STO-O01] Update Openreach FAILED";

        // SCD: Sync from CRM to Roxus DB
        public const string CUSTOM_SCD_200 = "[SCD-200] Sync from CRM to DB SUCCESSFULLY";
        public const string CUSTOM_SCD_400 = "[SCD-400] Sync from CRM to DB FAILED";
        public const string CUSTOM_SCD_O01 = "[SCD-O01] Get Openreach Details from Zoho CRM FAILED";
        
        // HTQ: Handle Transaction Queue
        public const string CUSTOM_HTQ_200 = "[HTQ-200] Handle Transaction Queue SUCCESSFULLY";
        public const string CUSTOM_HTQ_400 = "[HTQ-400] Handle Transaction Queue FAILED";

        // ABN: Add Letter Urls to Openreach Note
        public const string CUSTOM_ABN_200 = "[ALO-200] Add Letter Url to Openreach Note SUCCESSFULLY";
        public const string CUSTOM_ABN_400 = "[ALO-400] Add Letter Url to Openreach Note FALED";
        public const string CUSTOM_ABN_E01 = "[ALO-E01] Get Openreach Details by Id FAILED";

        // HRP: Handle Register Purchase
        public const string CUSTOM_HRP_200 = "[HRP_200] Handle Register Purchase SUCCESSFULLY";
        public const string CUSTOM_HRP_400 = "[HRP_400] Handle Register Purchase FAILED";
        public const string CUSTOM_HRP_OK_TO_PURCHASE = "OK TO PURCHASE";
        public const string CUSTOM_HRP_DONT_PURCHASE = "DON'T PURCHASE";

        #endregion

        public const int TransactionQueue_SendORLetter = 1;
        public const int TransactionQueue_SendLetter = 2;

    }
}
