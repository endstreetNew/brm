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
    public class TdwBatchViewModel
    {
        public int TdwBatchNo { get; set; }
        public string BoxNo { get; set; }
        public string RegType { get; set; }
        public int Boxes { get; set; }
        public int MiniBoxes { get; set; }
        public int Files { get; set; }
        public string Region { get; set; }
        public string User { get; set; }
        public DateTime? TdwSendDate { get; set; }
        public bool IsSelected { get; set; }

    }
}
//i)	The batch inventory must contain:

//i)	Batch ID
//ii)	the list of boxes in the batch
//iii)	The name of operator/manager who dispatched the batch.
//iv)	The date the batch was dispatched.
//v)	The name of the region the batch belongs to
