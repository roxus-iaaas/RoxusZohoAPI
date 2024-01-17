using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Entities.RoxusDB
{

    public class DocmailRecord
    {

        [Key]
        public string MailingGUID { get; set; }

        public string TitleId { get; set; }

        public string OpenreachId { get; set; }

        public string ProjectId { get; set; }

        public string TaskId { get; set; }

        public string CustomerId { get; set; }

        public string ContactId { get; set; }

        public string AccountId { get; set; }

        public string RelatedUPRN { get; set; }

        public int? UprnCount { get; set; }

        public string Environment { get; set; }

        public string MailingReference { get; set; }

        public string MailingContent { get; set; }

        public string MailingSubject { get; set; }

        public string MailingType { get; set; }

        public string MailingStatus { get; set; }

        public string DocmailStatus { get; set; }

        public string AddressNameFormat { get; set; }

        public string AddressPanels { get; set; }

        public string Archived { get; set; }

        public string Created { get; set; }

        public string DespatchDate { get; set; }

        public string DespatchType { get; set; }

        public string EnvelopePreference { get; set; }

        public string EstimatedDeliveryDate { get; set; }

        public string HasPaid { get; set; }

        public string IsColour { get; set; }

        public string IsDuplex { get; set; }

        public string MailingDescription { get; set; }

        public string MailingName { get; set; }

        public string MailingProduct { get; set; }

        public string MailingListGUID { get; set; }

        public string MinEnvelopeSize { get; set; }

        public string OrderRef { get; set; }

        public double? PriceExVAT { get; set; }

        public double? PriceIncVAT { get; set; }

        public double? RoxusPriceIncVAT { get; set; }

        public string ReturnManaged { get; set; }

        public string StatusCode { get; set; }

        public string SubmittedForPrinting { get; set; }

        public int? TemplateCount { get; set; }

        public string UseDataIntelligence { get; set; }

        public string UseDotPost { get; set; }

        public string UseOwnReturnAddress { get; set; }

        public double? VAT { get; set; }

        public double? VATRate { get; set; }

        public string BlobUrl { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }

    }

}
