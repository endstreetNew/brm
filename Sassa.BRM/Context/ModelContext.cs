using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Sassa.BRM.Context
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

        public virtual DbSet<DcBatch> DcBatches { get; set; }
        public virtual DbSet<DcLocalOffice> DcLocalOffices { get; set; }
        public virtual DbSet<DcRegion> DcRegions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("DATA SOURCE=10.117.122.120:1521/brmtrn;PERSIST SECURITY INFO=True;USER ID=CONTENTSERVER;Password=Password123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("CONTENTSERVER")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<DcBatch>(entity =>
            {
                entity.HasKey(e => e.BatchNo);

                entity.ToTable("DC_BATCH");

                entity.HasIndex(e => e.UpdatedBy, "INDEX17");

                entity.HasIndex(e => e.OfficeId, "INDEX18");

                entity.HasIndex(e => e.BatchStatus, "INDEX19");

                entity.HasIndex(e => e.RegType, "INDEX20");

                entity.Property(e => e.BatchNo)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BATCH_NO");

                entity.Property(e => e.BatchComment)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("BATCH_COMMENT");

                entity.Property(e => e.BatchCurrent)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BATCH_CURRENT")
                    .IsFixedLength(true);

                entity.Property(e => e.BatchStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BATCH_STATUS");

                entity.Property(e => e.BrmWaybill)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BRM_WAYBILL");

                entity.Property(e => e.CourierName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("COURIER_NAME");

                entity.Property(e => e.NoOfFiles)
                    .HasColumnType("NUMBER")
                    .HasColumnName("NO_OF_FILES");

                entity.Property(e => e.OfficeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OFFICE_ID");

                entity.Property(e => e.RegType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REG_TYPE");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("NUMBER")
                    .HasColumnName("UPDATED_BY");

                entity.Property(e => e.UpdatedByAd)
                    .HasMaxLength(225)
                    .IsUnicode(false)
                    .HasColumnName("UPDATED_BY_AD");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("UPDATED_DATE");

                entity.Property(e => e.WaybillDate)
                    .HasColumnType("DATE")
                    .HasColumnName("WAYBILL_DATE");

                entity.Property(e => e.WaybillNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("WAYBILL_NO");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.DcBatches)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_BATCH_LOCAL_OFFICE");
            });

            modelBuilder.Entity<DcLocalOffice>(entity =>
            {
                entity.HasKey(e => e.OfficeId);

                entity.ToTable("DC_LOCAL_OFFICE");

                entity.HasIndex(e => e.OfficeName, "INDEX45");

                entity.HasIndex(e => e.RegionId, "INDEX46");

                entity.HasIndex(e => e.OfficeType, "INDEX47");

                entity.HasIndex(e => e.District, "INDEX48");

                entity.Property(e => e.OfficeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OFFICE_ID");

                entity.Property(e => e.District)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT");

                entity.Property(e => e.OfficeName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("OFFICE_NAME");

                entity.Property(e => e.OfficeType)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("OFFICE_TYPE");

                entity.Property(e => e.RegionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.DcLocalOffices)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_LOCAL_OFFICE_REGION");
            });

            modelBuilder.Entity<DcRegion>(entity =>
            {
                entity.HasKey(e => e.RegionId);

                entity.ToTable("DC_REGION");

                entity.HasIndex(e => new { e.RegionName, e.RegionCode, e.RegionId }, "INDEX40");

                entity.Property(e => e.RegionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.RegionCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REGION_CODE");

                entity.Property(e => e.RegionName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("REGION_NAME");
            });

            modelBuilder.HasSequence("ACTIVEVIEWOVERRIDESSEQUENCE");

            modelBuilder.HasSequence("AGENTSEQUENCE");

            modelBuilder.HasSequence("AUDITCOLLECTIONSITEMSSEQ");

            modelBuilder.HasSequence("CUST_DISTRICTSEQ");

            modelBuilder.HasSequence("DAUDITNEWSEQUENCE");

            modelBuilder.HasSequence("DDOCUMENTCLASSSEQUENCE");

            modelBuilder.HasSequence("DFAVORITESTABSSEQUENCE");

            modelBuilder.HasSequence("DMU_DEMO_SEQ");

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
