using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcRegion
    {
        public DcRegion()
        {
            DcFileRequests = new HashSet<DcFileRequest>();
            DcLocalOffices = new HashSet<DcLocalOffice>();
        }

        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }

        public virtual ICollection<DcFileRequest> DcFileRequests { get; set; }
        public virtual ICollection<DcLocalOffice> DcLocalOffices { get; set; }
    }
}
