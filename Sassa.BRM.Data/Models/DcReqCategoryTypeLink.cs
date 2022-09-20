using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcReqCategoryTypeLink
    {
        public DcReqCategoryTypeLink()
        {
            DcFileRequests = new HashSet<DcFileRequest>();
        }

        public decimal CategoryId { get; set; }
        public decimal TypeId { get; set; }

        public virtual DcReqCategory Category { get; set; }
        public virtual DcReqCategoryType Type { get; set; }
        public virtual ICollection<DcFileRequest> DcFileRequests { get; set; }
    }
}
