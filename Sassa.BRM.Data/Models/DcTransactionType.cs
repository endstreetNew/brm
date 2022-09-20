using System.Collections.Generic;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcTransactionType
    {
        public DcTransactionType()
        {
            DcFiles = new HashSet<DcFile>();
            DcGrantDocLinks = new HashSet<DcGrantDocLink>();
        }

        public decimal TypeId { get; set; }
        public string TypeName { get; set; }
        public string ServiceCategory { get; set; }

        public virtual ICollection<DcFile> DcFiles { get; set; }
        public virtual ICollection<DcGrantDocLink> DcGrantDocLinks { get; set; }
    }
}
