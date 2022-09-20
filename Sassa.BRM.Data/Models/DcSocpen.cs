using System;
using System.Collections.Generic;

namespace Sassa.BRM.Models
{
    public partial class DcSocpen
    {
        public decimal Id { get; set; }
        public decimal? AdabasIsnMain { get; set; }
        public decimal? AdabasIsnArchive { get; set; }
        public string ApplicationNo { get; set; }
        public string BeneficiaryId { get; set; }
        public string ChildId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string GenderDesc { get; set; }
        public string ChildBirthDate { get; set; }
        public string SchoolGoing { get; set; }
        public string GrantType { get; set; }
        public string RegionId { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? StatusDate { get; set; }
        public string StatusCode { get; set; }
        public string PrimStatus { get; set; }
        public string Prim { get; set; }
        public string SecStatus { get; set; }
        public string Sec { get; set; }
        public string CaptureReference { get; set; }
        public DateTime? CaptureDate { get; set; }
        public DateTime? ScanDate { get; set; }
        public DateTime? SocpenDate { get; set; }
        public string AppStatus { get; set; }
        public DateTime? CsDate { get; set; }
        public DateTime? TdwRec { get; set; }
        public DateTime? TdwDispatch { get; set; }
        public string BrmBarcode { get; set; }
        public string LocalofficeId { get; set; }
        public DateTime? DateReviewed { get; set; }
        public string UniqueId { get; set; }
        public string Paypoint { get; set; }
        public long? SrdNo { get; set; }
        public string AdabasIsnSrd { get; set; }
        public string MisFiles { get; set; }
    }
}
