﻿#nullable disable

namespace Sassa.BRM.Models
{
    public partial class DcOfficeKuafLink
    {
        public string OfficeId { get; set; }
        public decimal? KuafId { get; set; }
        public string Supervisor { get; set; }
        public string Username { get; set; }
        public decimal Pk { get; set; }

        public virtual DcLocalOffice Office { get; set; }
    }
}
