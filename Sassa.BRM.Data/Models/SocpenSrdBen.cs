using System;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class SocpenSrdBen
    {
        public long SrdNo { get; set; }
        public long? IdNo { get; set; }
        public byte? Province { get; set; }
        public int? Region { get; set; }
        public byte? District { get; set; }
        public int? Paypoint { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public byte? Language { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public byte? NoDepChildren { get; set; }
        public DateTime? ReferralDate { get; set; }
        public string SocialWorker { get; set; }
        public string FileReferenceNo { get; set; }
        public long? SpouseNo { get; set; }
        public DateTime? AppealDate { get; set; }
        public string AppealInd { get; set; }
        public string MedicalCertificate { get; set; }
        public string DischargeCertificate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string DeathCertificate { get; set; }
        public DateTime? ArrestDate { get; set; }
        public DateTime? AdmittanceDate { get; set; }
        public DateTime? SentenceDate { get; set; }
        public DateTime? InstDateFrom { get; set; }
        public DateTime? InstDateTo { get; set; }
        public string InstitutionName { get; set; }
        public string DistressedCircumstance1 { get; set; }
        public string DistressedCircumstance2 { get; set; }
        public string DistressedCircumstance3 { get; set; }
        public string DistressedCircumstance4 { get; set; }
        public string ReferredForTreatment { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string AcceptanceEmployment { get; set; }
        public DateTime? AssumptionDutyDate { get; set; }
        public string OtherDetail1 { get; set; }
        public string OtherDetail2 { get; set; }
        public string OtherDetail3 { get; set; }
        public string OtherDetail4 { get; set; }
        public string ReferralNeeded { get; set; }
        public DateTime? ReferralNeededDate { get; set; }
        public decimal? TotalDeducted { get; set; }
        public decimal? TotalIssued { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PostalAddress1 { get; set; }
        public string PostalAddress2 { get; set; }
        public string PostalAddress3 { get; set; }
        public byte? PostalCode { get; set; }
        public string TelephoneNo { get; set; }
        public long? OtherIdDoc { get; set; }
        public string CellNo { get; set; }
        public long? PoaNo { get; set; }
        public DateTime? PoaDate { get; set; }
        public string PoaReason { get; set; }
        public string Recipient { get; set; }
        public byte? ResPostalCode { get; set; }
        public string IdentType { get; set; }
        public string IdentTypeSpecify { get; set; }
        public int? OldSocialReliefNo { get; set; }
        public string Address4 { get; set; }
        public string PostalAddress4 { get; set; }
        public string MilitaryVeteranInd { get; set; }
        public string EducationLevel { get; set; }
    }
}
