using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.PureFinance.Airtable;
using RoxusZohoAPI.Models.PureFinance.Pipedrive;
using RoxusZohoAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.PureFinance
{

    public class PureFinanceCustomService : IPureFinanceCustomService
    {

        private readonly IRoxusLoggingRepository _roxusLoggingRepository;
        private readonly IAirtableService _airtableService;
        private readonly IPipedriveService _pipedriveService;

        public PureFinanceCustomService(IRoxusLoggingRepository roxusLoggingRepository,
            IAirtableService airtableService, IPipedriveService pipedriveService)
        {
            _roxusLoggingRepository = roxusLoggingRepository;
            _airtableService = airtableService;
            _pipedriveService = pipedriveService;
        }

        public async Task<ApiResultDto<string>> SyncFromAirtable2Pipedrive
            (AirtablePayload airtablePayload)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = PureFinanceConstants.Custom_SyncAirtableToPipedrive_400
            };

            string errorMessage = string.Empty;
            string createDealId = string.Empty;
            string createPersonId = string.Empty;
            string createNoteId = string.Empty;
            int currentCursor = 42;
            string tableName = string.Empty;
            string rowId = string.Empty;

            try
            {

                // STEP 1: Get Webhook Payload
                // STEP 1.1: Get Current Cursor Number
                string clientName = "C5A528DA-1B8B-4E38-9F7E-52A3E6CB17A0";
                string platformName = "Airtable";
                var integrationLog = await _roxusLoggingRepository
                    .GetLatestIntegrationLogByCursor(clientName, platformName);
                
                if (integrationLog != null)
                {
                    var webhookCursor = integrationLog.WebhookCursor;
                    if (webhookCursor.HasValue)
                    {
                        currentCursor = webhookCursor.Value + 1;
                    }
                }

                // STEP 1.2: Get Webhook Payload Data
                var inputBase = airtablePayload._base;
                string inputBaseId = inputBase.id;

                var inputWebhook = airtablePayload.webhook;
                string inputWebhookId = inputWebhook.id;

                var getPayloadResponse = await _airtableService.ListWebhookPayloads
                    (inputBaseId, inputWebhookId,
                    currentCursor);

                if (getPayloadResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = PureFinanceConstants.Custom_SyncAirtableToPipedrive_GetWebhookPayload_400;
                    return apiResult;
                }

                var payloadList = getPayloadResponse.Data.payloads;
                if (payloadList.Count() == 0)
                {
                    apiResult.Message = PureFinanceConstants.Custom_SyncAirtableToPipedrive_NoWebhookPayload;
                    return apiResult;
                }

                var firstPayload = payloadList[0];
                string payloadDataStr = JsonConvert.SerializeObject(firstPayload);

                var payloadObject = JObject.Parse(payloadDataStr);

                var changedTable = payloadObject["changedTablesById"];

                string changedTableId = string.Empty;
                foreach (JProperty item in changedTable.Children())
                {
                    changedTableId = item.Name;
                    break;
                }

                var createdRecord = changedTable[changedTableId]["createdRecordsById"];

                string createdRecordId = string.Empty;
                foreach (JProperty item in createdRecord.Children())
                {
                    createdRecordId = item.Name;
                    break;
                }

                var cellValues = createdRecord[createdRecordId];
                var cellValuesByFieldId = cellValues["cellValuesByFieldId"];

                // STEP 2: Extract all the necessary information from the table
                string firstName = string.Empty;
                string lastName = string.Empty;
                string title = string.Empty;
                string pipeline = string.Empty;
                string stageId = string.Empty;
                string titleOther = string.Empty;
                string dealTitle = string.Empty;
                string enquiryType = string.Empty;
                DateTime enquiryDate = DateTime.UtcNow;
                string phone = string.Empty;
                string email = string.Empty;
                decimal enquiryLoanAmount = 0;
                string owner = PureFinanceConstants.Person_Owner_Id;
                string enquirySource = string.Empty;
                string enquiryLoanPurpose = string.Empty;
                string contactType = PureFinanceConstants.Person_ContactType_Client;
                string dateOfBirth = string.Empty;
                string contactSource = PureFinanceConstants.Person_ContactSource_ViaWeb;
                string person = string.Empty;
                string marketingConsent = string.Empty;

                // For Note
                string enquiryLoanPurposeOther = string.Empty;
                string enquiryLoanTermLenth = string.Empty;
                string propertyType = string.Empty;
                string homeOwner = string.Empty;
                string propertyValue = string.Empty;
                string mortgageBalance = string.Empty;
                string mortgageType = string.Empty;
                string mortgagePayment = string.Empty;
                string mortgageTermRemaining = string.Empty;
                string existingLender = string.Empty;
                string annualIncome = string.Empty;
                string employmentStatus = string.Empty;
                string timeInCurrentEmployment = string.Empty;
                string addressLine1 = string.Empty;
                string addressLine2 = string.Empty;
                string addressLine3 = string.Empty;
                string city = string.Empty;
                string country = string.Empty;
                string postcode = string.Empty;
                string jointApplication = string.Empty;
                string jaTitle = string.Empty;
                string jaTitleOther = string.Empty;
                string jaFirstName = string.Empty;
                string jaLastName = string.Empty;
                string jaDateOfBirth = string.Empty;
                string jaAnnualIncome = string.Empty;
                string jaEmploymentStatus = string.Empty;
                string jaTimeInCurrentEmployment = string.Empty;
                string callbackTime = string.Empty;
                string gender = string.Empty;
                string jaGender = string.Empty;
                string typeOfBorrower = string.Empty;
                string exitStrategy = string.Empty;
                string additionalSecurityAddress = string.Empty;
                string additionalSecurityPropertyType = string.Empty;
                string additionalSecurityPropertyValue = string.Empty;
                string additionalSecurityMortgageBalance = string.Empty;
                string additionalSecurityAddressLine1 = string.Empty;
                string additionalSecurityAddressLine2 = string.Empty;
                string additionalSecurityAddressLine3 = string.Empty;
                string additionalSecurityCity = string.Empty;
                string additionalSecurityCountry = string.Empty;
                string additionalSecurityPostcode = string.Empty;
                string businessName = string.Empty;
                string whatSector = string.Empty;
                string previous12Months = string.Empty;
                string projected12Months = string.Empty;
                string majorityOfSales = string.Empty;

                string contactDateOfBirth = string.Empty;

                string noteContent = PureFinanceConstants.Note_Content_Format;
                string allRows = "";
                string dealType = "";

                if (changedTableId == PureFinanceConstants.Second_Table_Id)
                {
                    dealType = "Second Charge";
                    tableName = "Second charge applications";

                    if (cellValuesByFieldId[PureFinanceConstants.Second_Id] != null)
                    {
                        rowId = (string)cellValuesByFieldId[PureFinanceConstants.Second_Id];
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_Title] != null)
                    {
                        title = (string)cellValuesByFieldId[PureFinanceConstants.Second_Title];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Title")
                            .Replace("$RowContent$", title);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_TitleOther] != null)
                    {
                        titleOther = (string)cellValuesByFieldId[PureFinanceConstants.Second_TitleOther];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Title Other")
                            .Replace("$RowContent$", titleOther);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_FirstName] != null)
                    {
                        firstName = (string)cellValuesByFieldId[PureFinanceConstants.Second_FirstName];
                        firstName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(firstName.ToLower());
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "First Name")
                            .Replace("$RowContent$", firstName);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_LastName] != null)
                    {
                        lastName = (string)cellValuesByFieldId[PureFinanceConstants.Second_LastName];
                        lastName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lastName.ToLower());
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Last Name")
                            .Replace("$RowContent$", lastName);
                    }

                    dealTitle = $"{firstName} {lastName} Second Charge";
                    enquiryType = PureFinanceConstants.Deal_EnquiryType_SecondCharge;
                    enquirySource = PureFinanceConstants.Deal_EnquirySource_SecondCharge;
                    pipeline = PureFinanceConstants.Deal_Pipeline_SecondCharge;
                    stageId = PureFinanceConstants.Deal_SecondCharge_Enquiry_Id;

                    if (cellValuesByFieldId[PureFinanceConstants.Second_Telephone] != null)
                    {
                        phone = (string)cellValuesByFieldId[PureFinanceConstants.Second_Telephone];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Telephone")
                            .Replace("$RowContent$", phone);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_Email] != null)
                    {
                        email = (string)cellValuesByFieldId[PureFinanceConstants.Second_Email];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Email")
                            .Replace("$RowContent$", email);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_LoanAmount] != null)
                    {
                        enquiryLoanAmount = decimal.Parse((string)cellValuesByFieldId[PureFinanceConstants.Second_LoanAmount]);
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Amount")
                            .Replace("$RowContent$", enquiryLoanAmount.ToString());
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_LoanPurpose] != null)
                    {
                        string loanPurpose = (string)cellValuesByFieldId[PureFinanceConstants.Second_LoanPurpose];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Purpose")
                            .Replace("$RowContent$", loanPurpose);

                        var purposes = loanPurpose.Split(",");
                        var purposeSet = new HashSet<string>();
                        foreach (var purpose in purposes)
                        {
                            if (purpose.Contains("Auction", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_AuctionPurchase);
                            }
                            if (purpose.Contains("Chain Break", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_ChainBreak);
                            }
                            if (purpose.Contains("Refurbishment", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_HomeImprovements_Refurbishments);
                            }
                            if (purpose.Contains("Remortgage", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_Remortgage);
                            }
                            if (purpose.Contains("Debt Consolidation", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_Consolidation);
                            }
                            if (purpose.Contains("Family gift", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_Gift2FamilyMember);
                            }
                            if (purpose.Contains("Holiday", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_HolidayHomePurchase);
                            }
                        }
                        if (purposeSet.Count > 0)
                        {
                            enquiryLoanPurpose = string.Join(",", purposeSet);
                        }
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_LoanPurposeOther] != null)
                    {
                        enquiryLoanPurposeOther = (string)cellValuesByFieldId[PureFinanceConstants.Second_LoanPurposeOther];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Purpose Other")
                            .Replace("$RowContent$", enquiryLoanPurposeOther);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_LoanTermLengthYears] != null)
                    {
                        enquiryLoanTermLenth = (string)cellValuesByFieldId[PureFinanceConstants.Second_LoanTermLengthYears];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Term Length")
                            .Replace("$RowContent$", enquiryLoanTermLenth);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_PropertyType] != null)
                    {
                        propertyType = (string)cellValuesByFieldId[PureFinanceConstants.Second_PropertyType];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Property Type")
                            .Replace("$RowContent$", propertyType);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_Homeowner] != null)
                    {
                        homeOwner = (string)cellValuesByFieldId[PureFinanceConstants.Second_Homeowner];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Homeowner?")
                            .Replace("$RowContent$", homeOwner);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_EstimatedPropertyValue] != null)
                    {
                        propertyValue = (string)cellValuesByFieldId[PureFinanceConstants.Second_EstimatedPropertyValue];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Property Value")
                            .Replace("$RowContent$", propertyValue);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_CurrentMortgageBalance] != null)
                    {
                        mortgageBalance = (string)cellValuesByFieldId[PureFinanceConstants.Second_CurrentMortgageBalance];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Mortgage Balance")
                            .Replace("$RowContent$", mortgageBalance);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_ExistingMortgageType] != null)
                    {
                        mortgageType = (string)cellValuesByFieldId[PureFinanceConstants.Second_ExistingMortgageType];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Mortgage Type")
                            .Replace("$RowContent$", mortgageType);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_MonthlyMortgagePayment] != null)
                    {
                        mortgagePayment = (string)cellValuesByFieldId[PureFinanceConstants.Second_MonthlyMortgagePayment];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Mortgage Payment")
                            .Replace("$RowContent$", mortgagePayment);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_EstimatedMortgageTermRemainingYears] != null)
                    {
                        mortgageTermRemaining = (string)cellValuesByFieldId[PureFinanceConstants.Second_EstimatedMortgageTermRemainingYears];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Mortgage Term Remaining")
                            .Replace("$RowContent$", mortgageTermRemaining);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_ExistingLender] != null)
                    {
                        existingLender = (string)cellValuesByFieldId[PureFinanceConstants.Second_ExistingLender];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Existing Lender")
                            .Replace("$RowContent$", existingLender);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_DateOfBirth] != null)
                    {
                        dateOfBirth = (string)cellValuesByFieldId[PureFinanceConstants.Second_DateOfBirth];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Date Of Birth")
                            .Replace("$RowContent$", dateOfBirth);
                        contactDateOfBirth = DateTime.ParseExact(dateOfBirth,
                            "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_AnnualIncome] != null)
                    {
                        annualIncome = (string)cellValuesByFieldId[PureFinanceConstants.Second_AnnualIncome];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Annual Income")
                            .Replace("$RowContent$", annualIncome);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_EmploymentStatus] != null)
                    {
                        employmentStatus = (string)cellValuesByFieldId[PureFinanceConstants.Second_EmploymentStatus];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Employment Status")
                            .Replace("$RowContent$", employmentStatus);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_TimeWithCurrentEmployer] != null)
                    {
                        timeInCurrentEmployment = (string)cellValuesByFieldId[PureFinanceConstants.Second_TimeWithCurrentEmployer];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Time In Current Employment")
                            .Replace("$RowContent$", timeInCurrentEmployment);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_AddressLine1] != null)
                    {
                        addressLine1 = (string)cellValuesByFieldId[PureFinanceConstants.Second_AddressLine1];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Address Line 1")
                            .Replace("$RowContent$", addressLine1);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_AddressLine2] != null)
                    {
                        addressLine2 = (string)cellValuesByFieldId[PureFinanceConstants.Second_AddressLine2];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Address Line 2")
                            .Replace("$RowContent$", addressLine2);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_AddressLine3] != null)
                    {
                        addressLine3 = (string)cellValuesByFieldId[PureFinanceConstants.Second_AddressLine3];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Address Line 3")
                            .Replace("$RowContent$", addressLine3);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_City] != null)
                    {
                        city = (string)cellValuesByFieldId[PureFinanceConstants.Second_City];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "City")
                            .Replace("$RowContent$", city);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_Country] != null)
                    {
                        country = (string)cellValuesByFieldId[PureFinanceConstants.Second_Country];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Country")
                            .Replace("$RowContent$", country);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_Postcode] != null)
                    {
                        postcode = (string)cellValuesByFieldId[PureFinanceConstants.Second_Postcode];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Postcode")
                            .Replace("$RowContent$", postcode);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_JointApplication] != null)
                    {
                        jointApplication = (string)cellValuesByFieldId[PureFinanceConstants.Second_JointApplication];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Joint Application?")
                            .Replace("$RowContent$", jointApplication);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_JATitle] != null)
                    {
                        jaTitle = (string)cellValuesByFieldId[PureFinanceConstants.Second_JATitle];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Title")
                            .Replace("$RowContent$", jaTitle);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_JATitleOther] != null)
                    {
                        jaTitleOther = (string)cellValuesByFieldId[PureFinanceConstants.Second_JATitleOther];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Title Other")
                            .Replace("$RowContent$", jaTitleOther);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_JAFirstName] != null)
                    {
                        jaFirstName = (string)cellValuesByFieldId[PureFinanceConstants.Second_JAFirstName];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Firstname")
                            .Replace("$RowContent$", jaFirstName);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_JALastName] != null)
                    {
                        jaLastName = (string)cellValuesByFieldId[PureFinanceConstants.Second_JALastName];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Lastname")
                            .Replace("$RowContent$", jaLastName);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_JADateOfBirth] != null)
                    {
                        jaDateOfBirth = (string)cellValuesByFieldId[PureFinanceConstants.Second_JADateOfBirth];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Date Of Birth")
                            .Replace("$RowContent$", jaDateOfBirth);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_JAAnnualIncome] != null)
                    {
                        jaAnnualIncome = (string)cellValuesByFieldId[PureFinanceConstants.Second_JAAnnualIncome];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Annual Income")
                            .Replace("$RowContent$", jaAnnualIncome);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_JAEmploymentStatus] != null)
                    {
                        jaEmploymentStatus = (string)
                            cellValuesByFieldId[PureFinanceConstants.Second_JAEmploymentStatus];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Employment Status")
                            .Replace("$RowContent$", jaEmploymentStatus);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_JATimeWithCurrentEmployer] != null)
                    {
                        jaTimeInCurrentEmployment = (string)
                            cellValuesByFieldId[PureFinanceConstants.Second_JATimeWithCurrentEmployer];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Time In Current Employment")
                            .Replace("$RowContent$", jaTimeInCurrentEmployment);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_RequestedCallbackTime] != null)
                    {
                        callbackTime = (string)
                            cellValuesByFieldId[PureFinanceConstants.Second_RequestedCallbackTime];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Callback time")
                            .Replace("$RowContent$", callbackTime);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_DateAndTime] != null)
                    {
                        enquiryDate = DateTime.Parse((string)cellValuesByFieldId[PureFinanceConstants.Second_DateAndTime]);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Second_ExcludeFromMarketing] != null)
                    {
                        string excludeFromMarketing = (string)
                            cellValuesByFieldId[PureFinanceConstants.Second_ExcludeFromMarketing];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Exclude from marketing")
                            .Replace("$RowContent$", excludeFromMarketing);
                        if (excludeFromMarketing.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                        {
                            marketingConsent = PureFinanceConstants.Person_MarketingConsents_DoNotContact;
                        }
                        else
                        {
                            marketingConsent = PureFinanceConstants.Person_MarketingConsents_Consent;
                        }
                    }
                }
                else if (changedTableId == PureFinanceConstants.Equity_Table_Id)
                {
                    dealType = "Equity Release";
                    tableName = "Equity release applications";

                    if (cellValuesByFieldId[PureFinanceConstants.Equity_Id] != null)
                    {
                        rowId = (string)cellValuesByFieldId[PureFinanceConstants.Equity_Id];
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_Title] != null)
                    {
                        title = (string)cellValuesByFieldId[PureFinanceConstants.Equity_Title];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Title")
                            .Replace("$RowContent$", title);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_TitleOther] != null)
                    {
                        titleOther = (string)cellValuesByFieldId[PureFinanceConstants.Equity_TitleOther];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Title Other")
                            .Replace("$RowContent$", titleOther);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_FirstName] != null)
                    {
                        firstName = (string)cellValuesByFieldId[PureFinanceConstants.Equity_FirstName];
                        firstName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(firstName.ToLower());
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "First Name")
                            .Replace("$RowContent$", firstName);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_LastName] != null)
                    {
                        lastName = (string)cellValuesByFieldId[PureFinanceConstants.Equity_LastName];
                        lastName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lastName.ToLower());
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Last Name")
                            .Replace("$RowContent$", lastName);
                    }

                    dealTitle = $"{firstName} {lastName} Equity Release";
                    enquiryType = PureFinanceConstants.Deal_EnquiryType_EquityRelease;
                    enquirySource = PureFinanceConstants.Deal_EnquirySource_EquityRelease;
                    pipeline = PureFinanceConstants.Deal_Pipeline_EquityRelease;
                    stageId = PureFinanceConstants.Deal_Equity_Enquiry_Id;

                    if (cellValuesByFieldId[PureFinanceConstants.Equity_DateOfBirth] != null)
                    {
                        dateOfBirth = (string)cellValuesByFieldId[PureFinanceConstants.Equity_DateOfBirth];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Date Of Birth")
                            .Replace("$RowContent$", dateOfBirth);
                        contactDateOfBirth = DateTime.ParseExact(dateOfBirth,
                            "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_Gender] != null)
                    {
                        gender = (string)cellValuesByFieldId[PureFinanceConstants.Equity_Gender];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Gender")
                            .Replace("$RowContent$", gender);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_Telephone] != null)
                    {
                        phone = (string)cellValuesByFieldId[PureFinanceConstants.Equity_Telephone];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Telephone")
                            .Replace("$RowContent$", phone);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_Email] != null)
                    {
                        email = (string)cellValuesByFieldId[PureFinanceConstants.Equity_Email];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Email")
                            .Replace("$RowContent$", email);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_LoanAmount] != null)
                    {
                        enquiryLoanAmount = decimal.Parse((string)cellValuesByFieldId[PureFinanceConstants.Equity_LoanAmount]);
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Amount")
                            .Replace("$RowContent$", enquiryLoanAmount.ToString());
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_LoanPurpose] != null)
                    {
                        string loanPurpose = (string)cellValuesByFieldId[PureFinanceConstants.Equity_LoanPurpose];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Purpose")
                            .Replace("$RowContent$", loanPurpose);

                        var purposes = loanPurpose.Split(",");
                        var purposeSet = new HashSet<string>();
                        foreach (var purpose in purposes)
                        {
                            if (purpose.Contains("Auction", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_AuctionPurchase);
                            }
                            if (purpose.Contains("Chain Break", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_ChainBreak);
                            }
                            if (purpose.Contains("Refurbishment", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_HomeImprovements_Refurbishments);
                            }
                            if (purpose.Contains("Remortgage", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_Remortgage);
                            }
                            if (purpose.Contains("Debt Consolidation", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_Consolidation);
                            }
                            if (purpose.Contains("Family gift", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_Gift2FamilyMember);
                            }
                            if (purpose.Contains("Holiday", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_HolidayHomePurchase);
                            }
                        }
                        if (purposeSet.Count > 0)
                        {
                            enquiryLoanPurpose = string.Join(",", purposeSet);
                        }
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_LoanPurposeOther] != null)
                    {
                        enquiryLoanPurposeOther = (string)cellValuesByFieldId[PureFinanceConstants.Equity_LoanPurposeOther];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Purpose Other")
                            .Replace("$RowContent$", enquiryLoanPurposeOther);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_HomeOwner] != null)
                    {
                        homeOwner = (string)cellValuesByFieldId[PureFinanceConstants.Equity_HomeOwner];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Homeowner?")
                            .Replace("$RowContent$", homeOwner);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_EstimatedPropertyValue] != null)
                    {
                        propertyValue = (string)cellValuesByFieldId[PureFinanceConstants.Equity_EstimatedPropertyValue];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Property Value")
                            .Replace("$RowContent$", propertyValue);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_CurrentMortgageBalance] != null)
                    {
                        mortgageBalance = (string)cellValuesByFieldId[PureFinanceConstants.Equity_CurrentMortgageBalance];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Mortgage Balance")
                            .Replace("$RowContent$", mortgageBalance);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_AddressLine1] != null)
                    {
                        addressLine1 = (string)cellValuesByFieldId[PureFinanceConstants.Equity_AddressLine1];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Address Line 1")
                            .Replace("$RowContent$", addressLine1);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_AddressLine2] != null)
                    {
                        addressLine2 = (string)cellValuesByFieldId[PureFinanceConstants.Equity_AddressLine2];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Address Line 2")
                            .Replace("$RowContent$", addressLine2);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_AddressLine3] != null)
                    {
                        addressLine3 = (string)cellValuesByFieldId[PureFinanceConstants.Equity_AddressLine3];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Address Line 3")
                            .Replace("$RowContent$", addressLine3);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_City] != null)
                    {
                        city = (string)cellValuesByFieldId[PureFinanceConstants.Equity_City];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "City")
                            .Replace("$RowContent$", city);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_Country] != null)
                    {
                        country = (string)cellValuesByFieldId[PureFinanceConstants.Equity_Country];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Country")
                            .Replace("$RowContent$", country);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_Postcode] != null)
                    {
                        postcode = (string)cellValuesByFieldId[PureFinanceConstants.Equity_Postcode];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Postcode")
                            .Replace("$RowContent$", postcode);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_JointApplication] != null)
                    {
                        jointApplication = (string)cellValuesByFieldId[PureFinanceConstants.Equity_JointApplication];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Joint Application?")
                            .Replace("$RowContent$", jointApplication);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_JATitle] != null)
                    {
                        jaTitle = (string)cellValuesByFieldId[PureFinanceConstants.Equity_JATitle];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Title")
                            .Replace("$RowContent$", jaTitle);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_JATitleOther] != null)
                    {
                        jaTitleOther = (string)cellValuesByFieldId[PureFinanceConstants.Equity_JATitleOther];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Title Other")
                            .Replace("$RowContent$", jaTitleOther);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_JAFirstName] != null)
                    {
                        jaFirstName = (string)cellValuesByFieldId[PureFinanceConstants.Equity_JAFirstName];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Firstname")
                            .Replace("$RowContent$", jaFirstName);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_JALastName] != null)
                    {
                        jaFirstName = (string)cellValuesByFieldId[PureFinanceConstants.Equity_JALastName];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Lastname")
                            .Replace("$RowContent$", jaLastName);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_JADateOfBirth] != null)
                    {
                        jaDateOfBirth = (string)cellValuesByFieldId[PureFinanceConstants.Equity_JADateOfBirth];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Date Of Birth")
                            .Replace("$RowContent$", jaDateOfBirth);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_JAGender] != null)
                    {
                        jaGender = (string)cellValuesByFieldId[PureFinanceConstants.Equity_JAGender];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "JA Gender")
                            .Replace("$RowContent$", jaGender);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_RequestedCallbackTime] != null)
                    {
                        callbackTime = (string)cellValuesByFieldId[PureFinanceConstants.Equity_RequestedCallbackTime];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Callback time")
                            .Replace("$RowContent$", callbackTime);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Equity_ExcludeFromMarketing] != null)
                    {
                        string excludeFromMarketing = (string)
                            cellValuesByFieldId[PureFinanceConstants.Equity_ExcludeFromMarketing];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Exclude from marketing")
                            .Replace("$RowContent$", excludeFromMarketing);
                        if (excludeFromMarketing.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                        {
                            marketingConsent = PureFinanceConstants.Person_MarketingConsents_DoNotContact;
                        }
                        else
                        {
                            marketingConsent = PureFinanceConstants.Person_MarketingConsents_Consent;
                        }
                    }
                }
                else if (changedTableId == PureFinanceConstants.BridgingLoan_Table_Id)
                {
                    dealType = "Bridging Loan";
                    tableName = "Bridging loan applications";

                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Id] != null)
                    {
                        rowId = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Id];
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Title] != null)
                    {
                        title = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Title];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Title")
                            .Replace("$RowContent$", title);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_TitleOther] != null)
                    {
                        titleOther = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_TitleOther];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Title Other")
                            .Replace("$RowContent$", titleOther);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_FirstName] != null)
                    {
                        firstName = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_FirstName];
                        firstName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(firstName.ToLower());
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "First Name")
                            .Replace("$RowContent$", firstName);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_LastName] != null)
                    {
                        lastName = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_LastName];
                        lastName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lastName.ToLower());
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Last Name")
                            .Replace("$RowContent$", lastName);
                    }

                    dealTitle = $"{firstName} {lastName} Bridge";
                    enquiryType = PureFinanceConstants.Deal_EnquiryType_BridgingFinance;
                    enquirySource = PureFinanceConstants.Deal_EnquirySource_BridgingFinance;
                    pipeline = PureFinanceConstants.Deal_Pipeline_SpecialistPropertyFinance;
                    stageId = PureFinanceConstants.Deal_Specialist_Enquiry_Id;

                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_DateOfBirth] != null)
                    {
                        dateOfBirth = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_DateOfBirth];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Date Of Birth")
                            .Replace("$RowContent$", dateOfBirth);
                        contactDateOfBirth = DateTime.ParseExact(dateOfBirth,
                            "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Telephone] != null)
                    {
                        phone = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Telephone];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Telephone")
                            .Replace("$RowContent$", phone);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Email] != null)
                    {
                        email = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Email];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Email")
                            .Replace("$RowContent$", email);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_LoanAmount] != null)
                    {
                        enquiryLoanAmount = decimal.Parse((string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_LoanAmount]);
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Amount")
                            .Replace("$RowContent$", enquiryLoanAmount.ToString());
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_LoanPurpose] != null)
                    {
                        string loanPurpose = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_LoanPurpose];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Purpose")
                            .Replace("$RowContent$", loanPurpose);

                        var purposes = loanPurpose.Split(",");
                        var purposeSet = new HashSet<string>();
                        foreach (var purpose in purposes)
                        {
                            if (purpose.Contains("Auction", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_AuctionPurchase);
                            }
                            if (purpose.Contains("Chain Break", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_ChainBreak);
                            }
                            if (purpose.Contains("Refurbishment", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_HomeImprovements_Refurbishments);
                            }
                            if (purpose.Contains("Remortgage", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_Remortgage);
                            }
                            if (purpose.Contains("Debt Consolidation", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_Consolidation);
                            }
                            if (purpose.Contains("Family gift", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_Gift2FamilyMember);
                            }
                            if (purpose.Contains("Holiday", StringComparison.InvariantCultureIgnoreCase))
                            {
                                purposeSet.Add(PureFinanceConstants.Deal_LoanPurpose_HolidayHomePurchase);
                            }
                        }
                        if (purposeSet.Count > 0)
                        {
                            enquiryLoanPurpose = string.Join(",", purposeSet);
                        }
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_LoanPurposeOther] != null)
                    {
                        enquiryLoanPurposeOther = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_LoanPurposeOther];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Purpose Other")
                            .Replace("$RowContent$", enquiryLoanPurposeOther);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_LoanTermLengthYears] != null)
                    {
                        enquiryLoanTermLenth = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_LoanTermLengthYears];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Loan Term Length")
                            .Replace("$RowContent$", enquiryLoanTermLenth);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_TypeOfBorrower] != null)
                    {
                        typeOfBorrower = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_TypeOfBorrower];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Type Of Borrower")
                            .Replace("$RowContent$", typeOfBorrower);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_ExitStrategy] != null)
                    {
                        exitStrategy = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_ExitStrategy];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Exit Strategy")
                            .Replace("$RowContent$", exitStrategy);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_PropertyType] != null)
                    {
                        propertyType = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_PropertyType];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Property Type")
                            .Replace("$RowContent$", propertyType);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_EstimatedPropertyValue] != null)
                    {
                        propertyValue = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_EstimatedPropertyValue];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Property Value")
                            .Replace("$RowContent$", propertyValue);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_CurrentMortgageBalance] != null)
                    {
                        mortgageBalance = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_CurrentMortgageBalance];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Mortgage Balance")
                            .Replace("$RowContent$", mortgageBalance);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_AddressLine1] != null)
                    {
                        addressLine1 = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_AddressLine1];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Address Line 1")
                            .Replace("$RowContent$", addressLine1);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_AddressLine2] != null)
                    {
                        addressLine2 = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_AddressLine2];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Address Line 2")
                            .Replace("$RowContent$", addressLine2);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_AddressLine3] != null)
                    {
                        addressLine3 = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_AddressLine3];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Address Line 3")
                            .Replace("$RowContent$", addressLine3);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_City] != null)
                    {
                        city = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_City];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "City")
                            .Replace("$RowContent$", city);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Country] != null)
                    {
                        country = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Country];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Country")
                            .Replace("$RowContent$", country);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Postcode] != null)
                    {
                        postcode = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Postcode];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Postcode")
                            .Replace("$RowContent$", postcode);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_AdditionalSecurityAddress] != null)
                    {
                        additionalSecurityAddress = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_AdditionalSecurityAddress];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Additional Security Address?")
                            .Replace("$RowContent$", additionalSecurityAddress);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SAPropertyType] != null)
                    {
                        additionalSecurityPropertyType = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SAPropertyType];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Additional Security Property Type")
                            .Replace("$RowContent$", additionalSecurityPropertyType);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SAEstimatedPropertyValue] != null)
                    {
                        additionalSecurityPropertyValue = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SAEstimatedPropertyValue];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Additional Security Property Value")
                            .Replace("$RowContent$", additionalSecurityPropertyValue);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SACurrentMortgageBalance] != null)
                    {
                        additionalSecurityMortgageBalance = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SACurrentMortgageBalance];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Additional Security Mortgage Balance")
                            .Replace("$RowContent$", additionalSecurityMortgageBalance);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SAAddressLine1] != null)
                    {
                        additionalSecurityAddressLine1 = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SAAddressLine1];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Additional Security Address Line 1")
                            .Replace("$RowContent$", additionalSecurityAddressLine1);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SAAddressLine2] != null)
                    {
                        additionalSecurityAddressLine2 = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SAAddressLine2];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Additional Security Address Line 2")
                            .Replace("$RowContent$", additionalSecurityAddressLine2);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SAAddressLine3] != null)
                    {
                        additionalSecurityAddressLine3 = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SAAddressLine3];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Additional Security Address Line 3")
                            .Replace("$RowContent$", additionalSecurityAddressLine3);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SACity] != null)
                    {
                        additionalSecurityCity = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SACity];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Additional Security City")
                            .Replace("$RowContent$", additionalSecurityCity);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SACountry] != null)
                    {
                        additionalSecurityCountry = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_SACountry];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Additional Security Country")
                            .Replace("$RowContent$", additionalSecurityCountry);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Postcode] != null)
                    {
                        additionalSecurityPostcode = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_Postcode];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Additional Security Postcode")
                            .Replace("$RowContent$", additionalSecurityPostcode);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_RequestedCallbackTime] != null)
                    {
                        callbackTime = (string)cellValuesByFieldId[PureFinanceConstants.BridgingLoan_RequestedCallbackTime];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Callback time")
                            .Replace("$RowContent$", callbackTime);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.BridgingLoan_ExcludeFromMarketing] != null)
                    {
                        string excludeFromMarketing = (string)
                            cellValuesByFieldId[PureFinanceConstants.BridgingLoan_ExcludeFromMarketing];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Exclude from marketing")
                            .Replace("$RowContent$", excludeFromMarketing);
                        if (excludeFromMarketing.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                        {
                            marketingConsent = PureFinanceConstants.Person_MarketingConsents_DoNotContact;
                        }
                        else
                        {
                            marketingConsent = PureFinanceConstants.Person_MarketingConsents_Consent;
                        }
                    }
                }
                else if (changedTableId == PureFinanceConstants.Invoice_Table_Id)
                {
                    dealType = "Invoice Finance";
                    tableName = "Invoice finance applications";

                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_Id] != null)
                    {
                        rowId = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_Id];
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_Title] != null)
                    {
                        title = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_Title];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Title")
                            .Replace("$RowContent$", title);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_TitleOther] != null)
                    {
                        titleOther = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_TitleOther];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Title Other")
                            .Replace("$RowContent$", titleOther);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_FirstName] != null)
                    {
                        firstName = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_FirstName];
                        firstName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(firstName.ToLower());
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "First Name")
                            .Replace("$RowContent$", firstName);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_LastName] != null)
                    {
                        lastName = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_LastName];
                        lastName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lastName.ToLower());
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Last Name")
                            .Replace("$RowContent$", lastName);
                    }

                    dealTitle = $"{firstName} {lastName} Invoice Finance";
                    enquiryType = PureFinanceConstants.Deal_EnquiryType_InvoiceFinance;
                    enquirySource = PureFinanceConstants.Deal_EnquirySource_InvoiceFinance;
                    pipeline = PureFinanceConstants.Deal_Pipeline_SpecialistPropertyFinance;
                    stageId = PureFinanceConstants.Deal_Specialist_Enquiry_Id;

                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_Telephone] != null)
                    {
                        phone = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_Telephone];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Telephone")
                            .Replace("$RowContent$", phone);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_Email] != null)
                    {
                        email = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_Email];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Email")
                            .Replace("$RowContent$", email);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_BusinessName] != null)
                    {
                        businessName = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_BusinessName];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Business Name")
                            .Replace("$RowContent$", businessName);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_WhatSectorIsYourBusiness] != null)
                    {
                        whatSector = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_WhatSectorIsYourBusiness];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "What Sector Is Your Business")
                            .Replace("$RowContent$", whatSector);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_Previous12Months] != null)
                    {
                        previous12Months = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_Previous12Months];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Previous 12 Months")
                            .Replace("$RowContent$", previous12Months);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_Projected12Months] != null)
                    {
                        projected12Months = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_Projected12Months];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Projected 12 Months")
                            .Replace("$RowContent$", projected12Months);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_RequestedCallbackTime] != null)
                    {
                        callbackTime = (string)cellValuesByFieldId[PureFinanceConstants.Invoice_RequestedCallbackTime];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Callback time")
                            .Replace("$RowContent$", callbackTime);
                    }
                    if (cellValuesByFieldId[PureFinanceConstants.Invoice_ExcludeFromMarketing] != null)
                    {
                        string excludeFromMarketing = (string)
                            cellValuesByFieldId[PureFinanceConstants.Invoice_ExcludeFromMarketing];
                        allRows += $"{PureFinanceConstants.Note_Row_Format}"
                            .Replace("$RowName$", "Exclude from marketing")
                            .Replace("$RowContent$", excludeFromMarketing);
                        if (excludeFromMarketing.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                        {
                            marketingConsent = PureFinanceConstants.Person_MarketingConsents_DoNotContact;
                        }
                        else
                        {
                            marketingConsent = PureFinanceConstants.Person_MarketingConsents_Consent;
                        }
                    }
                }
                noteContent = noteContent.Replace("$DealType$", dealType)
                    .Replace("$TableContent$", allRows);

                // STEP 3: Create Integration Log
                var input = new InputModel()
                {
                    RowId = rowId,
                    TableName = tableName
                };
                var integrationLogForCreation = new IntegrationLog()
                {
                    CustomerName = Guid.Parse("C5A528DA-1B8B-4E38-9F7E-52A3E6CB17A0"),
                    CreatedTime = DateTime.UtcNow,
                    InputSummary = JsonConvert.SerializeObject(input),
                    PlatformName = "Airtable",
                    RequestType = "Webhook",
                    WebhookCursor = currentCursor
                };
                await _roxusLoggingRepository.CreateIntegrationLog
                    (integrationLogForCreation);

                // STEP 4: Search for Person in Pipedrive
                var searchPersonsResponse = await _pipedriveService.SearchPersonsByEmail(email);
                if (searchPersonsResponse.Code == ResultCode.OK)
                {
                    var searchPersons = searchPersonsResponse.Data.data.items;
                    if (searchPersons.Count() > 0)
                    {
                        createPersonId = searchPersons[0].item.id.ToString();
                    }
                }

                // STEP 5: Create Person in Pipedrive
                if (string.IsNullOrEmpty(createPersonId))
                {
                    var createPersonRequest = new CreatePersonRequest()
                    {
                        name = $"{firstName} {lastName}",
                        ContactSource = contactSource,
                        ContactType = contactType,
                        owner_id = owner,
                        MarketingConsent = marketingConsent,
                        ClientDOB = contactDateOfBirth
                    };
                    var personPhone = new PersonPhone()
                    {
                        label = "mobile",
                        value = phone,
                        primary = true
                    };
                    var personPhones = new List<PersonPhone>();
                    personPhones.Add(personPhone);
                    createPersonRequest.phone = personPhones;

                    var personEmail = new PersonEmail()
                    {
                        label = "work",
                        value = email,
                        primary = true
                    };
                    var personEmails = new List<PersonEmail>();
                    personEmails.Add(personEmail);
                    createPersonRequest.email = personEmails;

                    string createPersonRequestStr = JsonConvert.SerializeObject(createPersonRequest);

                    var createPersonResponse = await _pipedriveService.CreatePerson(createPersonRequest);

                    if (createPersonResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = PureFinanceConstants.Custom_SyncAirtableToPipedrive_CreatePerson_400;
                        errorMessage = PureFinanceConstants.Custom_SyncAirtableToPipedrive_CreatePerson_400;
                        return apiResult;
                    }

                    var createPersonDetails = createPersonResponse.Data.data;
                    createPersonId = createPersonDetails.id.ToString();
                }

                // STEP 6: Create Deal in Pipedrive
                var createDealRequest = new CreateDealRequest()
                {
                    Client = createPersonId,
                    pipeline_id = pipeline,
                    EnquiryDate = enquiryDate.ToString("yyyy-MM-dd"),
                    EnquiryLoanAmount = enquiryLoanAmount,
                    EnquirySource = enquirySource,
                    EnquiryType = enquiryType,
                    LoanPurpose = enquiryLoanPurpose,
                    person_id = createPersonId,
                    stage_id = stageId,
                    title = dealTitle,
                    user_id = PureFinanceConstants.Deal_Owner_Zara_Id.ToString(),
                };
                string createDealRequestStr = JsonConvert.SerializeObject(createDealRequest);
                var createDealResponse = await _pipedriveService.CreateDeal(createDealRequest);

                if (createDealResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = PureFinanceConstants.Custom_SyncAirtableToPipedrive_CreateDeal_400;
                    errorMessage = PureFinanceConstants.Custom_SyncAirtableToPipedrive_CreateDeal_400;
                    return apiResult;
                }

                var createDealDetails = createDealResponse.Data.data;
                createDealId = createDealDetails.id.ToString();

                // STEP 7: Create Note in Deal
                var createNoteRequest = new CreateNoteRequest()
                {
                    deal_id = createDealId,
                    content = noteContent,
                    pinned_to_deal_flag = "1",
                    user_id = PureFinanceConstants.Deal_Owner_Zara_Id.ToString()
                };
                string createNoteRequestStr = JsonConvert.SerializeObject(createNoteRequest);
                var createNoteResponse = await _pipedriveService.CreateNote(createNoteRequest);

                if (createNoteResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = PureFinanceConstants.Custom_SyncAirtableToPipedrive_CreateNote_400;
                    errorMessage = PureFinanceConstants.Custom_SyncAirtableToPipedrive_CreateNote_400;
                    return apiResult;
                }
                var createNoteDetails = createNoteResponse.Data.data;
                createNoteId = createNoteDetails.id.ToString();

                apiResult.Code = ResultCode.OK;
                apiResult.Message = PureFinanceConstants.Custom_SyncAirtableToPipedrive_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            finally
            {
                // STEP 8: Update Integration 
                var outputModel = new OutputModel();
                string result = string.Empty;
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    outputModel.ErrorMessage = errorMessage;
                    result = "FAILED";
                }
                else
                {
                    outputModel.DealId = createDealId;
                    if (!string.IsNullOrEmpty(createPersonId))
                    {
                        outputModel.PersonId = createPersonId;
                    }
                    outputModel.NoteId = createNoteId;
                    result = "SUCCESS";
                }
                string outputModelStr = JsonConvert.SerializeObject(outputModel);

                await _roxusLoggingRepository.UpdateIntegrationLog(currentCursor, outputModelStr, result, tableName, rowId);

            }

        }

        public async Task<ApiResultDto<string>> MassUpdateTable()
        {

            var apiResult = new ApiResultDto<string>();

            try
            {

                // STEP 1: Get all records with EMPTY INPUT 1
                var emptyLogs = await _roxusLoggingRepository.GetEmptyIntegrationLogs();

                foreach (var log in emptyLogs)
                {
                    string input = log.InputSummary;
                    var inputSummary = JsonConvert.DeserializeObject<PureInputSummary>(input);
                    // string tableName = inputSummary.
                }

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

    }

}
