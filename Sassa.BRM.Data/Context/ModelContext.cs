using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sassa.BRM.Data.Context
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DcSocpen> DcSocpens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("DATA SOURCE=10.117.123.20:1521/brmtrn;PERSIST SECURITY INFO=True;USER ID=CONTENTSERVER;Password=Password123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("CONTENTSERVER")
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<DcSocpen>(entity =>
            {
                entity.ToTable("DC_SOCPEN");

                entity.HasIndex(e => e.BrmBarcode, "DC_SOCPEN_BRM_BARCODE")
                    .IsUnique();

                entity.HasIndex(e => e.CaptureReference, "DC_SOCPEN_CLM_NO")
                    .IsUnique();

                entity.HasIndex(e => new { e.BeneficiaryId, e.GrantType, e.ChildId, e.SrdNo }, "DC_SOCPEN_ID_GRANT")
                    .IsUnique();

                entity.HasIndex(e => e.BeneficiaryId, "DC_SOCPEN_ID_NO");

                entity.HasIndex(e => e.SrdNo, "DC_SOCPEN_SRD")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.AdabasIsnSrd)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ADABAS_ISN_SRD");

                entity.Property(e => e.AppStatus)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("APP_STATUS");

                entity.Property(e => e.ApplicationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPLICATION_DATE");

                entity.Property(e => e.ApplicationNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("APPLICATION_NO");

                entity.Property(e => e.BeneficiaryId)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("BENEFICIARY_ID");

                entity.Property(e => e.BrmBarcode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BRM_BARCODE");

                entity.Property(e => e.CaptureDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CAPTURE_DATE");

                entity.Property(e => e.CaptureReference)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CAPTURE_REFERENCE");

                entity.Property(e => e.ChildId)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("CHILD_ID");

                entity.Property(e => e.CsDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CS_DATE");

                entity.Property(e => e.Documents)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOCUMENTS");

                entity.Property(e => e.EcmisFile)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ECMIS_FILE");

                entity.Property(e => e.GrantType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_TYPE");

                entity.Property(e => e.IdHistory)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("ID_HISTORY");

                entity.Property(e => e.LocalofficeId)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("LOCALOFFICE_ID");

                entity.Property(e => e.MisFile)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_FILE");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Paypoint)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("PAYPOINT");

                entity.Property(e => e.RegionId)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.ScanDate)
                    .HasColumnType("DATE")
                    .HasColumnName("SCAN_DATE");

                entity.Property(e => e.SocpenDate)
                    .HasColumnType("DATE")
                    .HasColumnName("SOCPEN_DATE");

                entity.Property(e => e.SrdNo)
                    .HasPrecision(13)
                    .HasColumnName("SRD_NO");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("STATUS_CODE");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SURNAME");

                entity.Property(e => e.TdwRec)
                    .HasColumnType("DATE")
                    .HasColumnName("TDW_REC");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("UNIQUE_ID");
            });

            modelBuilder.HasSequence("ACTIVEVIEWOVERRIDESSEQUENCE");

            modelBuilder.HasSequence("AGENTSEQUENCE");

            modelBuilder.HasSequence("AUDITCOLLECTIONSITEMSSEQ");

            modelBuilder.HasSequence("BRMWAYBIL");

            modelBuilder.HasSequence("CUST_DISTRICTSEQ");

            modelBuilder.HasSequence("DAUDITNEWSEQUENCE");

            modelBuilder.HasSequence("DDOCUMENTCLASSSEQUENCE");

            modelBuilder.HasSequence("DFAVORITESTABSSEQUENCE");

            modelBuilder.HasSequence("DPSINSRTPROPSSEQ");

            modelBuilder.HasSequence("DPSTASKSSEQUENCE");

            modelBuilder.HasSequence("DSOCIALFEEDEVENTSSEQ");

            modelBuilder.HasSequence("DSOCIALFOLLOWERSSEQ");

            modelBuilder.HasSequence("DSTAGINGIMPORTSEQUENCE");

            modelBuilder.HasSequence("DSUGGESTWORDSPENDINGSEQUENCE");

            modelBuilder.HasSequence("DSUGGESTWORDSSEQUENCE");

            modelBuilder.HasSequence("DTREECOREEXTSOURCESEQUENCE");

            modelBuilder.HasSequence("DTREENOTIFYSEQUENCE");

            modelBuilder.HasSequence("ELINKMESSAGESEQUENCE").IsCyclic();

            modelBuilder.HasSequence("FILECACHESEQUENCE");

            modelBuilder.HasSequence("KUAFIDENTITYSEQUENCE");

            modelBuilder.HasSequence("KUAFIDENTITYTYPESEQUENCE");

            modelBuilder.HasSequence("LLEVENTSSEQUENCE");

            modelBuilder.HasSequence("NOTIFYSEQUENCE");

            modelBuilder.HasSequence("OI_STATUS_SEQ");

            modelBuilder.HasSequence("PROVIDERRETRYSEQUENCE");

            modelBuilder.HasSequence("RECD_HOTSEQ");

            modelBuilder.HasSequence("RECD_OPERATIONTRACKINGSEQ");

            modelBuilder.HasSequence("RENDITIONFOLDERSSEQ");

            modelBuilder.HasSequence("RENDITIONMIMETYPERULESSEQ");

            modelBuilder.HasSequence("RENDITIONNODERULESSEQ");

            modelBuilder.HasSequence("RENDITIONQUEUESEQ");

            modelBuilder.HasSequence("RESULTIDSEQUENCE");

            modelBuilder.HasSequence("RETENTIONUPDATEFAILEDSEQNID");

            modelBuilder.HasSequence("RETENTIONUPDATELOGSEQNID");

            modelBuilder.HasSequence("RETENTIONUPDATEORDERSEQNID");

            modelBuilder.HasSequence("RM_HOLDQUERYHISTORYSEQUENCE");

            modelBuilder.HasSequence("RMSEC_DEFINEDRULESEQUENCE");

            modelBuilder.HasSequence("SEARCHSTATSSEQUENCE");

            modelBuilder.HasSequence("SEQ_CUST_REGION_REGNUM");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_ECA");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_FST");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_GAU");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_KZN");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_LIM");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_MPU");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_NCA");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_NWP");

            modelBuilder.HasSequence("SEQ_DC_ALT_BOX_NO_WCA");

            modelBuilder.HasSequence("SEQ_DC_BATCH");

            modelBuilder.HasSequence("SEQ_DC_BOXPICKED");

            modelBuilder.HasSequence("SEQ_DC_FILE");

            modelBuilder.HasSequence("SEQ_DC_FILE_REQUEST");

            modelBuilder.HasSequence("SEQ_DC_PICKLIST");

            modelBuilder.HasSequence("WORKERQUEUESEQUENCE");

            modelBuilder.HasSequence("WWORKAUDITSEQ");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
