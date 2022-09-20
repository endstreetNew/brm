using System;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcMissingStatus
    {
        public string Year { get; set; }
        public string Quarter { get; set; }
        public string Count { get; set; }
        public DateTime MissingOn { get; set; }
        public string Region { get; set; }
    }
}
