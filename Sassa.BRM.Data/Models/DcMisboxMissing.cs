using System;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcMisboxMissing
    {
        public string UnqFileNo { get; set; }
        public string BoxNo { get; set; }
        public string RegionId { get; set; }
        public string CaptureByAd { get; set; }
        public decimal Id { get; set; }
        public DateTime? CaptureDate { get; set; }
    }
}
