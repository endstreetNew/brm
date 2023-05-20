using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sassa.BRM.Data.ViewModels
{
    /// <summary>
    /// Boxes ready for TDW dispatch.
    /// </summary>
    public class TdwBatch
    {
        public string BoxNo { get; set; }
        public string AltBoxNo { get; set; }
        public int MiniBoxes { get; set; }
        public string Region { get; set; }
        public string Office { get; set; }
        public DateTime? TdwSendDate { get; set; }

    }
}
