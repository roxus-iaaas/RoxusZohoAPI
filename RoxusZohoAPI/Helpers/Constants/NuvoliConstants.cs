using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers.Constants
{
    public class NuvoliConstants
    {

        #region SharePoint

        public const string AutomationSiteId = "nuvolilimited.sharepoint.com,8a3e9ba4-a956-476a-aa9c-e52390649fff,8521a648-ffde-4a9e-9dd3-5a8185e09d2e";

        public const string MGUserName = "nuvoli";

        public const string MGPassword = "microsoftgraph";

        #endregion

        #region Halo

        #region Auth

        public const string Halo_GrantType = "client_credentials";

        public const string Halo_ClientId = "8c33429f-bfca-45ce-a424-94cce5781d25";

        public const string Halo_ClientSecret = "62201522-2c17-4dde-b6b8-328aef2aedf1-99c38894-a400-47d4-850f-745540540358";

        public const string Halo_Scope = "all:standard";

        #endregion

        #region Halo API

        public const string Halo_Endpoint = "https://support.nuvoli.net";

        #region Get Canned Text by Id

        public const string EmailToSupplier_NewStarter_CannedTextId = "502";

        public const string EmailToSupplier_NewTablet_CannedTextId = "670";

        public const string InternalNote_NewStarter_CannedTextId = "504";

        public const string InternalNote_NewTablet_CannedTextId = "694";

        #endregion

        #region Log to Supplier

        public const string Halo_NHSPS_O2_Supplier_Id = "196";

        #endregion

        #region Email to Supplier

        public const string Halo_EmailSupplier_EmailTo = "hoang.tran@roxus.io";

        #endregion

        #region Internal Note



        #endregion

        #region Delivery Details

        public const string Halo_DeliveryDetails_DeliveryAddressId = "209";

        public const string Halo_DeliveryDetails_ContactNameId = "216";

        public const string Halo_DeliveryDetails_PostcodeId = "204";

        public const string Halo_DeliveryDetails_ContactNumberId = "217";

        public const string Halo_DeliveryDetails_ContactEmailId = "218";

        public const string Halo_DeliveryDetails_ConsignmentId = "192";

        public const string Halo_ConnectionDetails_MobileNumberId = "193";

        #endregion

        #region Connection Details

        public const string Halo_ConnectionDetails_UsernameId = "212";

        public const string Halo_ConnectionDetails_UserEmailAddressId = "252";

        #endregion

        #region Phone Details

        public const string Halo_PhoneDetails_IMEIId = "179";

        public const string Halo_PhoneDetails_AssetTagId = "128";

        #endregion

        #region National Health Service

        public const string NHS_SiteId = "10545";

        public const string NHS_ClientId = "260";

        public const string NHS_Asset_TypeId = "32";

        public const string NHS_Asset_StatusId = "9";

        public const string NHS_Asset_Mobile_FieldId = "8";

        public const string NHS_Asset_Username_FieldId = "242";

        public const string NHS_Asset_Provider_FieldId = "248";

        #endregion

        // GHTBI: Get Halo Ticket by Id

        public const string GTBI_200 = "Get Ticket by Id SUCCESSFULLY";

        public const string GTBI_400 = "Get Ticket by Id FAILED";

        // ETD: Extract Ticket Details

        public const string ETD_200 = "Extract Ticket Details SUCCESSFULLY";

        public const string ETD_400 = "Extract Ticket Details FAILED";

        // UHT: Update Halo Ticket

        public const string UHT_200 = "Update Halo Ticket SUCCESSFULLY";

        public const string UHT_400 = "Update Halo Ticket FAILED";

        // ETA: Execute Ticket Action

        public const string ETA_200 = "Execute Ticket Actions SUCCESSFULLY";

        public const string ETA_400 = "Execute Ticket Actions FAILED";

        // THT: Triage Halo Ticket

        public const string THT_200 = "Triage Halo Ticket SUCCESSFULLY";

        public const string THT_400 = "Triage Halo Ticket FAILED";

        // L2S: Log to Supplier for Halo Ticket

        public const string L2S_200 = "Log to Supplier SUCCESSFULLY";

        public const string L2S_400 = "Log to Supplier FAILED";

        // GTABI: Get Ticket Actions by Id

        public const string GTABI_200 = "Get Ticket Actions by Id SUCCESSFULLY";

        public const string GTABI_400 = "Get Ticket Actions by Id FAILED";

        #endregion

        #endregion

        #region Custom Functions

        // NNSP1: Nuvoli New Starter Phase 1

        public const string NNSP1_200 = "Handle Nuvoli Starter Phase 1 SUCCESSFULLY";

        public const string NNSP1_400 = "Handle Nuvoli Starter Phase 1 FAILED";

        public const string NNSP1_E01 = "[E01] Cannot get Halo Ticket by Id";

        // HEOP2: Handle Email from O2 Phase 2

        public const string HEOP2_200 = "Handle Email from O2 Phase 2 SUCCESSFULLY";

        public const string HEOP2_400 = "Handle Email from O2 Phase 2 FAILED";

        public const string HEOP2_E01 = "[E01] Get Ticket by Id FAILED";

        public const string HEOP2_E02 = "[E02] Update Halo Ticket FAILED";

        // HAP2: Handle Asset for Phase 2

        public const string HAP2_200 = "Handle Asset Phase 2 SUCCESSFULLY";

        public const string HAP2_400 = "Handle Asset Phase 2 FAILED";

        public const string HAP2_E01 = "[E01] Get Ticket by Id FAILED";

        public const string HAP2_E02 = "[E02] List Users FAILED";

        public const string HAP2_E03 = "[E03] Create User FAILED";

        public const string HAP2_E04 = "[E04] List Assets FAILED";

        public const string HAP2_E05 = "[E05] Upsert Asset FAILED";

        public const string HAP2_E06 = "[E06] Create Child Ticket FAILED";

        public const string HAP2_E07 = "[E07] Get Canned Text by Id FAILED";

        public const string HAP2_E08 = "[E08] Create Internal Note FAILED";

        // HSE: Handle Send Email

        public const string HSE_200 = "Handle Send Email SUCCESSFULLY";

        public const string HSE_400 = "Handle Send Email FAILED";

        public const string HSE_E01 = "[E01] Canned Text Id is EMPTY";

        public const string HSE_E02 = "[E02] Get Canned Text by Id FAILED";

        public const string HSE_E03 = "[E03] Get Canned Text by Id FAILED";

        public const string HSE_E04 = "[E04] Get Before Email FAILED";

        public const string HSE_E05 = "[E05] Send Email FAILED";

        #endregion

    }
}
