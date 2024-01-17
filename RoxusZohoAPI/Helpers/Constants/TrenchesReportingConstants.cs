using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers.Constants
{
    public class TrenchesReportingConstants
    {
        /* Syncing */
        public const string MSG_SYNCING_200 = "Syncing Record SUCCESSFULLY!";
        public const string MSG_SYNCING_400 = "Syncing Record FAILED!";
        public const string MSG_SYNCING_EXIST = "Syncing Record is already EXISTED!";

        /* Lightning Fibre */


        /* OpenReach */
        public const string MSG_OR_OWNERSHIP_200 = "Handle Openreach Ownership SUCCESSFULLY!";
        public const string MSG_OR_OWNERSHIP_400 = "Handle Openreach Ownership FAILED!";

        public const string OpenreachLayoutId = "262304000017649001";

        public const string MSG_OR_OWNERLINKING_200 = "Handle Openreach Ownership Linking SUCCESSFULLY!";
        public const string MSG_OR_OWNERLINKING_400 = "Handle Openreach Ownership Linking FAILED!";

        public const string MSG_OR_CREATEACCESSAGREEMENT_200 = "Create Access Agreement SUCCESSFULLY!";
        public const string MSG_OR_CREATEACCESSAGREEMENT_400 = "Create Access Agreement FAILED!";

        public const string MSG_OR_HANDLELETTERREFERENCE_200 = "Handle Letter Reference SUCCESSFULLY!";
        public const string MSG_OR_HANDLELETTERREFERENCE_400 = "Handle Letter Reference FAILED!";

        public const string AccessAgreementFolder = "Access Agreement";
        public const string TemplateFolder = "Template";
        public const string OutputFolder = "Output";
        public const string AccessAgreement_Company_Template = "TL_OR_AA_Company.docx";
        public const string AccessAgreement_Private_Template = "TL_OR_AA_Private.docx";

    }
}
