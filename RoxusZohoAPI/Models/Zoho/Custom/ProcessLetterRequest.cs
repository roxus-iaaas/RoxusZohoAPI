using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Zoho.Custom
{
    public class ProcessLetterRequest
    {

        public ProcessLetterRequest()
        {

            AccountData = new List<AccountLetter>();

            ContactData = new List<ContactLetter>();

        }

        public string TaskUrl { get; set; }

        public string TitleId { get; set; }

        public string TitleNumber { get; set; }

        public string RelatedUPRN { get; set; }

        public int UprnCount { get; set; }

        public string ProjectId { get; set; }

        public string TaskId { get; set; }

        public string Environment { get; set; }

        public string MailingType { get; set; }

        public string CustomerId { get; set; }

        public string MailingStatus { get; set; }

        public string MailingContent { get; set; }

        public List<AccountLetter> AccountData { get; set; }

        public List<ContactLetter> ContactData { get; set; }

    }

    public class AccountLetter
    {

        public string AccountId { get; set; }

        public string MailingName { get; set; }

        public string MailingDescription { get; set; }

        public string OuterEnvelope { get; set; }

        public string BaseTemplate { get; set; }

        public string WayleaveTemplate { get; set; }

        public string MapImage { get; set; }

        public string ReplyEnvelope { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string Address4 { get; set; }

        public string Address5 { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string FullName { get; set; }

        public string CompanyName { get; set; }

        public string Custom1 { get; set; }

        public string Custom2 { get; set; }

        public string Custom3 { get; set; }

        public string Custom4 { get; set; }

        public string Custom5 { get; set; }

        public string Custom6 { get; set; }

        public string Custom7 { get; set; }

        public string Custom8 { get; set; }

        public string Custom9 { get; set; }

        public string Custom10 { get; set; }

    }

    public class ContactLetter
    {

        public string ContactId { get; set; }

        public string MailingName { get; set; }

        public string MailingDescription { get; set; }

        public string OuterEnvelope { get; set; }

        public string BaseTemplate { get; set; }

        public string WayleaveTemplate { get; set; }

        public string MapImage { get; set; }

        public string ReplyEnvelope { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string Address4 { get; set; }

        public string Address5 { get; set; }

        public string Address6 { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string FullName { get; set; }

        public string Custom1 { get; set; }

        public string Custom2 { get; set; }

        public string Custom3 { get; set; }

        public string Custom4 { get; set; }

        public string Custom5 { get; set; }

        public string Custom6 { get; set; }

        public string Custom7 { get; set; }

        public string Custom8 { get; set; }

        public string Custom9 { get; set; }

        public string Custom10 { get; set; }

    }
}
