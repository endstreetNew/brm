using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcReqCategoryType
    {
        public DcReqCategoryType()
        {
            DcReqCategoryTypeLinks = new HashSet<DcReqCategoryTypeLink>();
        }

        public decimal TypeId { get; set; }
        public string TypeDescr { get; set; }

        public virtual ICollection<DcReqCategoryTypeLink> DcReqCategoryTypeLinks { get; set; }
    }
}
