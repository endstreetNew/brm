using System;
using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcCaptureProgress
    {
        public decimal Id { get; set; }
        public string Quarter { get; set; }
        public decimal? Total { get; set; }
        public decimal? Captured { get; set; }
        public decimal? Missing { get; set; }
        public string RegionId { get; set; }
    }
}
