using System;
using System.Collections.Generic;

namespace Sassa.BRM.Models
{
    public partial class DcLocalOffice
    {
        public DcLocalOffice()
        {
            DcBatches = new HashSet<DcBatch>();
            DcFiles = new HashSet<DcFile>();
            DcOfficeKuafLinks = new HashSet<DcOfficeKuafLink>();
            DcFixedServicePoints = new HashSet<DcFixedServicePoint>();
        }

        public string OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string RegionId { get; set; }
        public string OfficeType { get; set; }
        public string District { get; set; }
        public string ActiveStatus { get; set; }
        public string ManualBatch { get; set; }

        public virtual DcRegion Region { get; set; }
        public virtual ICollection<DcBatch> DcBatches { get; set; }
        public virtual ICollection<DcFile> DcFiles { get; set; }
        public virtual ICollection<DcOfficeKuafLink> DcOfficeKuafLinks { get; set; }
        public virtual ICollection<DcFixedServicePoint> DcFixedServicePoints { get; set; }
    }
    //public partial class DcLocalOffice
    //{
    //    public DcLocalOffice()
    //    {
    //        DcBatches = new HashSet<DcBatch>();
    //        DcFiles = new HashSet<DcFile>();
    //        DcOfficeKuafLinks = new HashSet<DcOfficeKuafLink>();
    //    }

    //    public string OfficeId { get; set; }
    //    public string OfficeName { get; set; }
    //    public string RegionId { get; set; }
    //    public string OfficeType { get; set; }
    //    public string District { get; set; }
    //    //public string Paypoint { get; set; }

    //    public virtual DcRegion Region { get; set; }
    //    public virtual ICollection<DcBatch> DcBatches { get; set; }
    //    public virtual ICollection<DcFile> DcFiles { get; set; }
    //    public virtual ICollection<DcOfficeKuafLink> DcOfficeKuafLinks { get; set; }
    //    public string ActiveStatus { get; set; }
    //}
}
