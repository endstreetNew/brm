#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcDl
    {
        public string Region { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictDesc { get; set; }
        public string BenPenNo { get; set; }
        public string BenNameAndSurname { get; set; }
        public string ChildIdNo { get; set; }
        public string ChildNameAndSurname { get; set; }
        public string GrantStatus { get; set; }
        public string GrantType { get; set; }
        public string CourtOrderNumber { get; set; }
        public string CourtOrderDate { get; set; }
        public string CourtOrderExpiryPeriodCurrent { get; set; }
        public string CourtOrderExtensionDate { get; set; }
        public string GrantAppDate { get; set; }
        public string NpoName { get; set; }
    }
}
