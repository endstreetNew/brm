using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Context
{
    public partial class DcLocalOffice
    {
        public DcLocalOffice()
        {
            DcBatches = new HashSet<DcBatch>();
        }

        public string OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string RegionId { get; set; }
        public string OfficeType { get; set; }
        public string District { get; set; }

        public virtual DcRegion Region { get; set; }
        public virtual ICollection<DcBatch> DcBatches { get; set; }
    }
}
