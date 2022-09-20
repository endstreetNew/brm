using System;

#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcBoxpicklist
    {
        public string UnqPicklist { get; set; }
        public string RegionId { get; set; }
        public string RegistryType { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime PicklistDate { get; set; }
        public string PicklistStatus { get; set; }
        public string UnqNo { get; set; }
        public string BinNumber { get; set; }
        public string BoxNumber { get; set; }
        public string BoxReceived { get; set; }
        public string BoxCompleted { get; set; }
        public string ArchiveYear { get; set; }
    }
}
