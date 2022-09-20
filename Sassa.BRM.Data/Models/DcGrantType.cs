using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcGrantType
    {
        public DcGrantType()
        {
            DcFiles = new HashSet<DcFile>();
            DcGrantDocLinks = new HashSet<DcGrantDocLink>();
        }

        public string TypeId { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<DcFile> DcFiles { get; set; }
        public virtual ICollection<DcGrantDocLink> DcGrantDocLinks { get; set; }
    }
}
