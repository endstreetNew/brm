using System;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class SocpenSrdType
    {
        public int AdabasIsn { get; set; }
        public string ReceiptNo { get; set; }
        public string SocialReliefNo { get; set; }
        public string Province { get; set; }
        public string BenefitType { get; set; }
        public string PreviousSocialRelief { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string ApplicationReason { get; set; }
        public DateTime? DateApproved { get; set; }
        public decimal? RandValue { get; set; }
        public string Paypoint { get; set; }
        public string Region { get; set; }
        public DateTime? SentenceDate { get; set; }
        public DateTime? InstDateFrom { get; set; }
        public DateTime? InstDateTo { get; set; }
        public string InstitutionName { get; set; }
        public string ApplicationStatus { get; set; }
        public string VoucherNo { get; set; }
        public string PoaNo { get; set; }
        public string PoaReason { get; set; }
        public DateTime? PoaDate { get; set; }
        public DateTime? DeceasedDate { get; set; }
        public DateTime? AdmittanceDate { get; set; }
        public DateTime? DisasterDate { get; set; }
        public string ApplicationType { get; set; }
    }
}
