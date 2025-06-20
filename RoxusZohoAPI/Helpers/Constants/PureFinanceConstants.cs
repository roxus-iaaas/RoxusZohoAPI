using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers.Constants
{
    public class PureFinanceConstants
    {

        #region Airtable Constants

        public const string Airtable_APIKey = "UHVyZUZpbmFuY2U6QWlydGFibGU=";

        public const string Airtable_ComparisonTool_BaseId = "appMS0j83uIDy63wI";

        public const string Airtable_WebhookId = "achZ5XKsJi92OlKpu";

        public const string Airtable_GetToken_200 = "Get Airtable Token SUCCESSFULLY";

        public const string Airtable_GetToken_400 = "Get Airtable Token FAILED";

        #endregion

        #region Pipedrive Constants

        public const string PipedriveApiKey = "b818e82ffa5bb20fc4a0afeadec5b85f1a4812a4";

        public const string PipedriveEndpointV1 = "https://api.pipedrive.com/v1";

        #region Deal

        public const string Deal_Pipeline_SecondCharge = "12";

        public const string Deal_Pipeline_SpecialistPropertyFinance = "1";
        
        public const string Deal_Pipeline_EquityRelease = "14";

        public const string Deal_SecondCharge_Enquiry_Id = "60";

        public const string Deal_Specialist_Enquiry_Id = "1";

        public const string Deal_Equity_Enquiry_Id = "80";

        public const string Deal_Owner_Zara_Id = "13734698";

        public const string Deal_Client_Key = "bc3d135c086aa32235d4ca6563cd8296fdb86659"; // Person Id

        public const string Deal_EnquiryType_Key = "584179c1c26612544a0c9664eea87ed2429a8718"; // Enquiry Type Id

        public const string Deal_EnquiryType_SecondCharge = "151";

        public const string Deal_EnquiryType_InvoiceFinance = "18";

        public const string Deal_EnquiryType_BridgingFinance = "17";

        public const string Deal_EnquiryType_EquityRelease = "161";

        public const string Deal_EnquirySource_Key = "7d12f9d04f486789f2b15632777ce2b0af8e0ffc"; // Enquiry Source Id

        public const string Deal_EnquirySource_SecondCharge = "527";

        public const string Deal_EnquirySource_InvoiceFinance = "529";

        public const string Deal_EnquirySource_BridgingFinance = "526";

        public const string Deal_EnquirySource_EquityRelease = "528";

        public const string Deal_EnquiryDate_Key = "e9f867eabf4bd3942f165531a2abd2593ec1358c"; // Enquiry Date

        public const string Deal_EnquiryLoanAmount_Key = "689c1a61ef1137c4661836775bf1dbff7c848030"; // Currency

        public const string Deal_LoanPurpose_Key = "b87299ca8ceb9858584a0e92f75f2e4bf5724e0a"; // Comma Separated String - LEAVE IT FOR NOW

        public const string Deal_LoanPurpose_HomeImprovements_Refurbishments = "444"; // Home Improvements, Refurbishments

        public const string Deal_LoanPurpose_Consolidation = "445"; // Debt Consolidation

        public const string Deal_LoanPurpose_BusinessPurposes = "446"; 

        public const string Deal_LoanPurpose_AuctionPurchase = "447"; // Auction

        public const string Deal_LoanPurpose_ChainBreak = "448"; // Chain break

        public const string Deal_LoanPurpose_InvestmentPurchase = "449";

        public const string Deal_LoanPurpose_Rebridge = "450";

        public const string Deal_LoanPurpose_HolidayHomePurchase = "505"; // Holiday

        public const string Deal_LoanPurpose_Gift2FamilyMember = "506"; // Family gift

        public const string Deal_LoanPurpose_LivingExpenses = "507";

        public const string Deal_LoanPurpose_AddRemovePartyFromCurrentMortgage = "508";

        public const string Deal_LoanPurpose_ProductTransfer = "509";

        public const string Deal_LoanPurpose_ResidentialHomePurchase = "510"; 

        public const string Deal_LoanPurpose_Remortgage = "511"; // Remortgage

        public const string Deal_LoanPurpose_OffsetFacilityRequired = "512";

        public const string Deal_LoanPurpose_DevelopmentLoan = "514";

        public const string Note_Second_Charge = "Second Charge Enquiry";

        public const string Note_Content_Format = "<p style='margin-top:0cm;'>Hello,</p>" +
            "<p style='margin-top:0cm;'>Someone has completed a $DealType$ enquiry, please find their details below:</p>" +
            "<table cellpadding='0' cellspacing='0' border='0' class='page_table'>" +
            "<tbody>$TableContent$</tbody></table>";

        public const string Note_Row_Format = "<tr><td style=border-width:1pt;border-style:solid;border-color:windowtext;padding:3.75pt;box-sizing:border-box><p><span>" +
            "$RowName$" +
            "</span><td style='border-top:1pt solid windowtext;border-right:1pt solid windowtext;border-bottom:1pt solid windowtext;border-left-style:none;padding:3.75pt;box-sizing:border-box'><p><span>" +
            "$RowContent$" +
            "</span>";

        #endregion

        #region Person

        public const string Person_ContactSource_Key = "3eb90ef5db1bbd083c973d524734cdd83f5ef1b4"; // Enquiry Date

        public const string Person_ContactSource_ViaWeb = "91"; // via Web

        public const string Person_ContactType_Client = "1"; // Client

        public const string Person_MarketingConsent_Key = "f33b5a9f1b7c8813129cf609965c51b564656a9b";

        public const string Person_MarketingConsents_DoNotContact = "192";

        public const string Person_MarketingConsents_Consent = "191";

        public const string Person_Owner_Id = "13734698"; // Zara Harris

        #endregion

        #endregion

        #region Custom Function

        public const string Airtable_RefreshWebhook_200 = "Refresh Airtable Webhook SUCCESSFULLY";

        public const string Airtable_RefreshWebhook_400 = "Refresh Airtable Webhook FAILED";

        public const string Custom_SyncAirtableToPipedrive_200 = "[SA2P_200] Sync Airtable to Pipedrive SUCCESSFULLY";

        public const string Custom_SyncAirtableToPipedrive_400 = "[SA2P_400] Sync Airtable to Pipedrive FAILED";

        public const string Custom_SyncAirtableToPipedrive_GetWebhookPayload_400 = "[SA2P_E01] Get Webhook Payload FAILED";

        public const string Custom_SyncAirtableToPipedrive_NoWebhookPayload = "[SA2P_E02] There is no Webhook Payload";

        public const string Custom_SyncAirtableToPipedrive_CreatePerson_400 = "[SA2P_E03] Create Pipedrive Person FAILED";

        public const string Custom_SyncAirtableToPipedrive_CreateDeal_400 = "[SA2P_E04] Create Pipedrive Deal FAILED";

        public const string Custom_SyncAirtableToPipedrive_CreateNote_400 = "[SA2P_E04] Create Pipedrive Note for Deal FAILED";

        #endregion

        #region Second Charge Applications

        public const string Second_Table_Id = "tblSTXOHkWkX2KP90";

        public const string Second_Id = "fldS2FbusDhEsiZxY";

        public const string Second_LoanAmount = "flduGEC44WzDAHwg5";

        public const string Second_LoanPurpose = "fld4C2UApmahVg0IR";

        public const string Second_LoanPurposeOther = "fldxOUkH6qlgx7Fqd";

        public const string Second_LoanTermLengthYears = "fld5bhNNIyIhZqHZ9";

        public const string Second_PropertyType = "fldUlXMQRLftT4KBK";

        public const string Second_Homeowner = "fldlcQWxLsaanCWkj";

        public const string Second_EstimatedPropertyValue = "fldxyLg8otpUw3rVJ";

        public const string Second_CurrentMortgageBalance = "fldmYuEXjmU0pEFdt";

        public const string Second_ExistingMortgageType = "fldi9KlgFu8STTDee";

        public const string Second_MonthlyMortgagePayment = "fldhy0nVn86mlaxqa";

        public const string Second_EstimatedMortgageTermRemainingYears = "fldb36j5uGuA6xPk1";

        public const string Second_AddressLine1 = "fldoyo1tu0UH3JLHQ";

        public const string Second_AddressLine2 = "fldzBGeMXHxYRfCea";

        public const string Second_AddressLine3 = "fld0BNAXQtcNGKpBG";

        public const string Second_City = "fld1HdH9lVtbXh8bA";

        public const string Second_Postcode = "fldivBuQC4GBV7fUq";

        public const string Second_Country = "fldHEjG9hwygdXyCs";

        public const string Second_ExistingLender = "fld61YrbJjqUdjYr8";

        public const string Second_DateOfBirth = "fldYGB7cwp2x582Ii";

        public const string Second_AnnualIncome = "fldobNkyYBE91RvrG";

        public const string Second_EmploymentStatus = "fldbePnQI0L7y4S4d";

        public const string Second_TimeWithCurrentEmployer = "fldP8Q2eybX505YcC";

        public const string Second_Title = "flddMfwxKDHRviwAM";

        public const string Second_TitleOther = "fldSUrqVbkb0PYiar";

        public const string Second_FirstName = "fldiy7m0PS75DByaV";

        public const string Second_LastName = "fld5MQrCIhT5EDnyW";

        public const string Second_Telephone = "fldxaBZVazN1iBwOS";

        public const string Second_Email = "fldlpDo7U6vQCxC2F";

        public const string Second_JointApplication = "fldsYC4vKxhMMgpPn";

        public const string Second_JATitle = "fldzNqqeSkjUmou73";

        public const string Second_JATitleOther = "fldk4Zl0C1lgJoaZD";

        public const string Second_JAFirstName = "fldPmqDFQ6ouiVPaf";

        public const string Second_JALastName = "fldl2Q6yflag4Uujf";

        public const string Second_JADateOfBirth = "fld9CMW19a6F6aBcZ";

        public const string Second_JAAnnualIncome = "fldUWnlDJzI7pClDZ";

        public const string Second_JAEmploymentStatus = "fldcOiDftFDuwhCXy";

        public const string Second_JATimeWithCurrentEmployer = "fldirBHupqyOFuJ7M";

        public const string Second_Website = "fldt1lqLdR1M5Z6wS";

        public const string Second_RequestedCallbackTime = "fldLSNLmgzUrQBwJ6";

        public const string Second_ExcludeFromMarketing = "fldaWieKXRbY78Jhb";

        public const string Second_UniqueSessionId = "fld1wiOgqYRO6YXIj";

        public const string Second_DateAndTime = "fldzPK614yskZGXVP";

        #endregion

        #region Invoice Finance Applications

        public const string Invoice_Table_Id = "tblQFCVuJHtcBjJSw";

        public const string Invoice_Id = "fldQOkihRoqT1RTgu";

        public const string Invoice_BusinessName = "fld2oH1nO7jwuPUrn";

        public const string Invoice_WhatSectorIsYourBusiness = "fldS7CTDgwoIsDEkg";

        public const string Invoice_Previous12Months = "fldssjJRtHIS9gqZB";

        public const string Invoice_Projected12Months = "fldaymkIMsW2iLIsN";

        public const string Invoice_MajorityOfSales = "fldvAzruvbuv6Gz9J";

        public const string Invoice_Title = "fldbyUDk9oQ64Rqji";

        public const string Invoice_TitleOther = "fldQG6xIA5kfoxcTX";

        public const string Invoice_FirstName = "fldgkMtNeDgkcasTr";

        public const string Invoice_LastName = "fld3yvyp722kdchhs";

        public const string Invoice_Telephone = "fldvWg6IzkWgRaqxo";

        public const string Invoice_Email = "fldjbivUjRE5b6wLb";

        public const string Invoice_Website = "fldrN0xyCCa1Ey0fo";

        public const string Invoice_RequestedCallbackTime = "fldJEsS9Fk3GpaqsC";

        public const string Invoice_ExcludeFromMarketing = "fld8IXlxmCkdGHD0H";

        public const string Invoice_UniqueSessionId = "fldZiXV3PJ03FxRrP";

        public const string Invoice_DateAndTime = "fldxBpdOtjBzyfREl";

        #endregion

        #region Bridging Loan Applications

        public const string BridgingLoan_Table_Id = "tblfKYvKtRMXn79E4";

        public const string BridgingLoan_Id = "fldfTGSxByJENFj22";

        public const string BridgingLoan_LoanAmount = "fldRxFj7dR1DV4QL9";

        public const string BridgingLoan_LoanPurpose = "fldrt3BDyhChgDkdV";

        public const string BridgingLoan_LoanPurposeOther = "fldUFV1KflNgSuZVh";

        public const string BridgingLoan_LoanTermLengthYears = "flds2iuQRtahkN1ud";

        public const string BridgingLoan_TypeOfBorrower = "fldhcYtT0GHter46O";

        public const string BridgingLoan_ExitStrategy = "fldbwDWKmGWdCkSqt";

        public const string BridgingLoan_PropertyType = "fldI3RDAUnCaIZgPn";

        public const string BridgingLoan_EstimatedPropertyValue = "fldUpMXbxoRURqLqN";

        public const string BridgingLoan_CurrentMortgageBalance = "fldJPvl0shm0K1ZIx";

        public const string BridgingLoan_AddressLine1 = "fldLppIwDVmHo65cU";

        public const string BridgingLoan_AddressLine2 = "fldWsHVP6CZYcCWJe";

        public const string BridgingLoan_AddressLine3 = "fldnsOh0ZoEN17J6K";

        public const string BridgingLoan_City = "fldoyeocuQVbiEsGE";

        public const string BridgingLoan_Postcode = "fldFmCbTLZ8Bguzpu";

        public const string BridgingLoan_Country = "fld4vkncqr0gykS7w";

        public const string BridgingLoan_AdditionalSecurityAddress = "fldlM58wDJiFi7OP1";

        public const string BridgingLoan_SAPropertyType = "fldD6nkKvntb2589E";

        public const string BridgingLoan_SAEstimatedPropertyValue = "fldDN3tOrmVIrs4QS";

        public const string BridgingLoan_SACurrentMortgageBalance = "fldrRvSN9OAWPPQU0";

        public const string BridgingLoan_SAAddressLine1 = "fldBCgiJHpYJ4nMNd";

        public const string BridgingLoan_SAAddressLine2 = "fldZuRahhdOZn4ZW8";

        public const string BridgingLoan_SAAddressLine3 = "fldrAZHvnILZeO6R7";

        public const string BridgingLoan_SACity = "flddjDf2GTVXgmlJo";

        public const string BridgingLoan_SAPostcode = "fldlhfI16kyBbtAT1";

        public const string BridgingLoan_SACountry = "fldaXYbYnCawlPm0E";

        public const string BridgingLoan_Title = "fldADgdATy9RQFQ5Q";

        public const string BridgingLoan_TitleOther = "fldfLs7YkfD0alCFv";

        public const string BridgingLoan_FirstName = "fldFp833YNz5YYSFZ";

        public const string BridgingLoan_LastName = "fldsDR8FRcl5Z0H30";

        public const string BridgingLoan_DateOfBirth = "fldlxCOfFkuxqvmdm";

        public const string BridgingLoan_Telephone = "fldU1CGYjuf1DYQjW";

        public const string BridgingLoan_Email = "fldIgE5a31XQXUWxJ";

        public const string BridgingLoan_Website = "fldQSm7OmMtMqmq1W";

        public const string BridgingLoan_RequestedCallbackTime = "fld8JOsppumrbYQea"; // 16:00

        public const string BridgingLoan_ExcludeFromMarketing = "fldxNjVN6MDYsv3Mf";

        public const string BridgingLoan_UniqueSessionId = "fldonjvjzTjOrlhdn";

        public const string BridgingLoan_DateAndTime = "fldWGLN4dtUkk3hqT"; // 2024-06-28T12:33:08.000Z

        #endregion

        #region Equity Release Applications

        public const string Equity_Table_Id = "tblIJPTSFsyeUqOoY";

        public const string Equity_Id = "fldISxgFN9vVkYYMW";

        public const string Equity_LoanAmount = "fldkwwHfpsNUsnvv3";

        public const string Equity_LoanPurpose = "fldUsUZLKSoyNWZXP";

        public const string Equity_LoanPurposeOther = "fldnEMpSrWzxpNEFb";

        public const string Equity_HomeOwner = "fldb2I1I6YorfiVzh";

        public const string Equity_EstimatedPropertyValue = "fldnoDljJZDboJqaH";

        public const string Equity_CurrentMortgageBalance = "fldcOmJ8ES8hhkEsr";

        public const string Equity_AddressLine1 = "fldeog6EPw8YVpKWO";

        public const string Equity_AddressLine2 = "fldpryjXidLfJVBt8";

        public const string Equity_AddressLine3 = "fldQrFF8bZq4yqoQE";

        public const string Equity_City = "fldRx5MkGrHsPX7qy";

        public const string Equity_Postcode = "fld8ltz1XAUSNNe9o";

        public const string Equity_Country = "fldxubLkC2Mx5DxRq";

        public const string Equity_Title = "fld3C7BI59V8nYvPK";

        public const string Equity_TitleOther = "fldIKjv6wQphHEhpp";

        public const string Equity_FirstName = "fld8oZrbaolmvhxpT";

        public const string Equity_LastName = "fldVCIwN3N7mwjmNU";

        public const string Equity_DateOfBirth = "fldOwtcnRVgOXO1Xg";

        public const string Equity_Gender = "fldhBnxZZ9MRWhJzP";

        public const string Equity_Telephone = "fldn0t46v51iahv3Q";

        public const string Equity_Email = "fldbfvtifCJ7udBhD";

        public const string Equity_JointApplication = "fldiOu9G53v3EWo4l";

        public const string Equity_JATitle = "fldpDivpdQxbe4tm1";

        public const string Equity_JATitleOther = "fldaURqbXxzxB49eB";

        public const string Equity_JAFirstName = "fldFciIQbCCLaBOpd";

        public const string Equity_JALastName = "fldbSIbJARoxWAtyd";

        public const string Equity_JADateOfBirth = "fldZsE1cuGkWYQArX";

        public const string Equity_JAGender = "fldExPcWYpResz2oj";

        public const string Equity_Website = "fldjRdvWynf3XF5LQ";

        public const string Equity_RequestedCallbackTime = "fldBIFQxB58IIhvY4";

        public const string Equity_ExcludeFromMarketing = "fld0MajVinpfZOIw9";

        public const string Equity_UniqueSessionId = "fldRmaTrLu55YEWXh";

        public const string Equity_DateAndTime = "fldpFCbcp4GBRmWaN";

        #endregion

    }
}
