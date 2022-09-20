using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcDocumentType
    {
        public DcDocumentType()
        {
            DcGrantDocLinks = new HashSet<DcGrantDocLink>();
        }

        public decimal TypeId { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<DcGrantDocLink> DcGrantDocLinks { get; set; }
    }
}
