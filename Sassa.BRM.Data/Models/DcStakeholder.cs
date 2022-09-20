using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcStakeholder
    {
        public DcStakeholder()
        {
            DcFileRequests = new HashSet<DcFileRequest>();
        }

        public decimal StakeholderId { get; set; }
        public decimal DepartmentId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string RegionId { get; set; }

        public virtual DcReqCategory Department { get; set; }
        public virtual ICollection<DcFileRequest> DcFileRequests { get; set; }
    }
}
