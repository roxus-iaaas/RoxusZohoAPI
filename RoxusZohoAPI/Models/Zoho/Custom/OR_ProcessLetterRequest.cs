using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.Custom
{
   

    public class OR_ProcessLetterRequest
    {

        public OR_ProcessLetterRequest()
        {

            AccountData = new List<OR_AccountLetter>();

            ContactData = new List<OR_ContactLetter>();

        }

        public int LetterNumber { get; set; }

        public string TaskUrl { get; set; }

        public string OpenreachId { get; set; }

        public string OpenreachNumber { get; set; }

        public string ProjectId { get; set; }

        public string TasklistId { get; set; }

        public string TaskId { get; set; }

        public string Environment { get; set; }

        public string MailingType { get; set; }

        public string CustomerId { get; set; }

        public string MailingStatus { get; set; }

        public string MailingContent { get; set; }

        public List<OR_AccountLetter> AccountData { get; set; }

        public List<OR_ContactLetter> ContactData { get; set; }

    }

    public class OR_AccountLetter : AccountLetter
    {

        public string LetterTemplate { get; set; }

        public string AccessAgreementTemplate { get; set; }

        public List<OwnerLinking> OwnerLinkings { get; set; }

    }

    public class OR_ContactLetter : ContactLetter
    {

        public string LetterTemplate { get; set; }

        public string AccessAgreementTemplate { get; set; }

        public List<OwnerLinking> OwnerLinkings { get; set; }

    }

    public class OwnerLinking
    {

        public string OpenreachReference { get; set; }

        public string PropertyAddress { get; set; }

        public string TitleNumber { get; set; }

    }

}
