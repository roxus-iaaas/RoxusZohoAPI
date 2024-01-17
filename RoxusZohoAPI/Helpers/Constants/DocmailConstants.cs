using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers.Constants
{
    public class DocmailConstants
    {
        public const string RoxusDocmailEndpoint = "https://roxusdocmailapi.azurewebsites.net/api/docmail";
        public const string LongDateFormat = "dddd, MMMM dd, yyyy";
        public const string DefaultMailingStatus = "IN PROGRESS";

        #region TrenchesLaw
        public const string TrenchesCustomerId = "13048475";
        public const string TrenchesCustomerCode = "TL";
        public const string Trenches_OuterEnvelopeBlack = "Trenches_Law_Outer_Black";
        public const string Trenches_OuterEnvelopeColour = "Trenches_Law_Outer_Colour";
        public const string Trenches_BusinessReplyEnvelope_1st = "Trenches_Law_DL_1st";
        public const string Trenches_BusinessReplyEnvelope_2nd = "Trenches_Law_DL_2nd";
        public const string Trenches_Wildanet_Wayleave_Template = "Wildanet_Wayleave_Template";

        public const string Trenches_Openreach_OuterEnvelope = "OR_Outer";
        public const string Trenches_Openreach_ReturnEnvelope = "RUAL-YSSH-LZYY_RXS";
        public const string Trenches_Openreach_CompanyAccessAgreementTemplate = "TL_OR_AA_Company";
        public const string Trenches_Openreach_NoRegAccessAgreementTemplate = "TL_OR_AA_No_Reg";
        public const string Trenches_Openreach_PrivateAccessAgreementTemplate = "TL_OR_AA_Private";

        #endregion
    }
}
