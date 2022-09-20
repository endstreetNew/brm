using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Context
{
    public partial class DcRegion
    {
        public DcRegion()
        {
            DcLocalOffices = new HashSet<DcLocalOffice>();
        }

        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }

        public virtual ICollection<DcLocalOffice> DcLocalOffices { get; set; }
    }
}
