using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Context
{
    public partial class DcGrantType
    {
        public DcGrantType()
        {
            DcFiles = new HashSet<DcFile>();
        }

        public string TypeId { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<DcFile> DcFiles { get; set; }
    }
}
