using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Context
{
    public partial class DcTransactionType
    {
        public DcTransactionType()
        {
            DcFiles = new HashSet<DcFile>();
        }

        public decimal TypeId { get; set; }
        public string TypeName { get; set; }
        public string ServiceCategory { get; set; }

        public virtual ICollection<DcFile> DcFiles { get; set; }
    }
}
