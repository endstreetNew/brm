using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sassa.BRM.Data.ViewModels
{
    public class ReportHeader
    {
        public string Username { get; set; }
        public string Region { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Records { get; set; } // Total Records

    }
}
