using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcReqCategory
    {
        public DcReqCategory()
        {
            DcReqCategoryTypeLinks = new HashSet<DcReqCategoryTypeLink>();
            DcStakeholders = new HashSet<DcStakeholder>();
        }

        public decimal CategoryId { get; set; }
        public string CategoryDescr { get; set; }

        public virtual ICollection<DcReqCategoryTypeLink> DcReqCategoryTypeLinks { get; set; }
        public virtual ICollection<DcStakeholder> DcStakeholders { get; set; }
    }
}
