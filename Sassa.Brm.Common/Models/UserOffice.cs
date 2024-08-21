namespace Sassa.Brm.Common.Models
{
    public class UserOffice
    {
        public UserOffice(string officeId, string officeName, string officeType, string regionId, string regionCode, string regionName, decimal? fspId)
        {
            OfficeId = officeId;
            OfficeName = officeName;
            OfficeType = officeType;
            RegionId = regionId;
            RegionCode = regionCode;
            RegionName = regionName;
            FspId = fspId;
        }

        public string OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string OfficeType { get; set; }
        public string RegionId { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public decimal? FspId { get; set; }

    }
}
