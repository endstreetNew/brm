﻿using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Sassa.BRM.Models
{
    //Scaffold-DbContext "DATA SOURCE=10.117.123.20:1521/brmtrn;PERSIST SECURITY INFO=True;USER ID=CONTENTSERVER;Password=Password123;" Oracle.EntityFrameworkCore -OutputDir Context -Tables TDW_FILE_LOCATION -f
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DcActivity> DcActivities { get; set; }
        public virtual DbSet<DcBatch> DcBatches { get; set; }
        public virtual DbSet<DcBoxType> DcBoxTypes { get; set; }
        //public virtual DbSet<DcBoxpicked> DcBoxpickeds { get; set; }
        //public virtual DbSet<DcBoxpicklist> DcBoxpicklists { get; set; }
        public virtual DbSet<DcDestruction> DcDestructions { get; set; }
        //public virtual DbSet<DcDistrict> DcDistricts { get; set; }
        //public virtual DbSet<DcDistrictEc> DcDistrictEcs { get; set; }
        //public virtual DbSet<DcDl> DcDls { get; set; }
        public virtual DbSet<DcDocumentImage> DcDocumentImages { get; set; }
        public virtual DbSet<DcDocumentType> DcDocumentTypes { get; set; }
        public virtual DbSet<DcExclusion> DcExclusions { get; set; }
        public virtual DbSet<DcExclusionBatch> DcExclusionBatches { get; set; }
        public virtual DbSet<DcFile> DcFiles { get; set; }
        public virtual DbSet<DcFileDeleted> DcFileDeleteds { get; set; }
        public virtual DbSet<DcFileRec> DcFileRecs { get; set; }
        public virtual DbSet<DcFileRequest> DcFileRequests { get; set; }
        public virtual DbSet<DcGrantDocLink> DcGrantDocLinks { get; set; }
        public virtual DbSet<DcGrantDoctype> DcGrantDoctypes { get; set; }
        public virtual DbSet<DcGrantType> DcGrantTypes { get; set; }
        public virtual DbSet<DcLcType> DcLcTypes { get; set; }
        public virtual DbSet<DcLocalOffice> DcLocalOffices { get; set; }
        public virtual DbSet<DcMerge> DcMerges { get; set; }
        //public virtual DbSet<DcMisboxMissing> DcMisboxMissings { get; set; }
        //public virtual DbSet<DcMissingStatus> DcMissingStatuses { get; set; }
        public virtual DbSet<DcOfficeKuafLink> DcOfficeKuafLinks { get; set; }
        public virtual DbSet<DcPicklist> DcPicklists { get; set; }
        public virtual DbSet<DcPicklistItem> DcPicklistItems { get; set; }
        public virtual DbSet<DcRegion> DcRegions { get; set; }
        public virtual DbSet<DcReqCategory> DcReqCategories { get; set; }
        public virtual DbSet<DcReqCategoryType> DcReqCategoryTypes { get; set; }
        public virtual DbSet<DcReqCategoryTypeLink> DcReqCategoryTypeLinks { get; set; }
        //public virtual DbSet<DcSrdDatum> DcSrdData { get; set; }
        public virtual DbSet<DcStakeholder> DcStakeholders { get; set; }
        public virtual DbSet<DcTransactionType> DcTransactionTypes { get; set; }
        public virtual DbSet<TdwFileLocation> TdwFileLocations { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<IdResult> IdResults { get; set; }

        //public virtual DbSet<SocpenSrdBen> SocpenSrdBens { get; set; }
        //public virtual DbSet<SocpenSrdType> SocpenSrdTypes { get; set; }

        //public virtual DbSet<CustValdatum> CustValdata { get; set; }
        public virtual DbSet<MisLivelinkTbl> MisLivelinkTbls { get; set; }

        public virtual DbSet<DcSocpen> DcSocpen { get; set; }
        //public virtual DbSet<DcCaptureProgress> DcCaptureProgress { get; set; }
        public virtual DbSet<DcFixedServicePoint> DcFixedServicePoints { get; set; }

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
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<IdResult>().HasNoKey();

            modelBuilder.Entity<Application>().HasNoKey();

            modelBuilder.Entity<DcActivity>(entity =>
           {
               entity.ToTable("DC_ACTIVITY");

               entity.Property(e => e.DcActivityId)
                   .HasColumnType("NUMBER(38)")
                   .ValueGeneratedOnAdd()
                   .HasColumnName("DC_ACTIVITY_ID");

               entity.Property(e => e.Activity)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasColumnName("ACTIVITY");

               entity.Property(e => e.ActivityDate)
                   .HasColumnType("DATE")
                   .HasColumnName("ACTIVITY_DATE")
                   .HasDefaultValueSql("CURRENT_DATE ");

               entity.Property(e => e.Area)
                   .IsRequired()
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasColumnName("AREA");

               entity.Property(e => e.OfficeId)
                   .HasColumnType("NUMBER(38)")
                   .HasColumnName("OFFICE_ID");

               entity.Property(e => e.RegionId)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("REGION_ID");

               entity.Property(e => e.Result)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasColumnName("RESULT");

               entity.Property(e => e.UnqFileNo)
                   .HasMaxLength(20)
                   .HasColumnName("UNQ_FILE_NO");

               entity.Property(e => e.Userid)
                   .HasColumnType("NUMBER(38)")
                   .HasColumnName("USERID");

               entity.Property(e => e.Username)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("USERNAME");
           });

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

            modelBuilder.Entity<DcBoxType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_BOX_TYPE");

                entity.Property(e => e.BoxType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BOX_TYPE");

                entity.Property(e => e.BoxTypeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOX_TYPE_ID");

                entity.Property(e => e.IsTransport)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_TRANSPORT");
            });

            modelBuilder.Entity<DcBoxpicked>(entity =>
            {
                entity.HasKey(e => e.UnqNo)
                    .HasName("DC_BOXPICKED_PK");

                entity.ToTable("DC_BOXPICKED");

                entity.HasIndex(e => e.BoxNumber, "INDEX10");

                entity.HasIndex(e => e.UnqPicklist, "INDEX8");

                entity.HasIndex(e => e.BinNumber, "INDEX9");

                entity.Property(e => e.UnqNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("UNQ_NO");

                entity.Property(e => e.ArchiveYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ARCHIVE_YEAR");

                entity.Property(e => e.BinNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BIN_NUMBER");

                entity.Property(e => e.BoxCompleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BOX_COMPLETED")
                    .IsFixedLength(true);

                entity.Property(e => e.BoxNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BOX_NUMBER");

                entity.Property(e => e.BoxReceived)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BOX_RECEIVED")
                    .IsFixedLength(true);

                entity.Property(e => e.UnqPicklist)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UNQ_PICKLIST");
            });

            modelBuilder.Entity<DcBoxpicklist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("DC_BOXPICKLIST");

                entity.Property(e => e.ArchiveYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ARCHIVE_YEAR");

                entity.Property(e => e.BinNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BIN_NUMBER");

                entity.Property(e => e.BoxCompleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BOX_COMPLETED")
                    .IsFixedLength(true);

                entity.Property(e => e.BoxNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BOX_NUMBER");

                entity.Property(e => e.BoxReceived)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BOX_RECEIVED")
                    .IsFixedLength(true);

                entity.Property(e => e.PicklistDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PICKLIST_DATE");

                entity.Property(e => e.PicklistStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PICKLIST_STATUS")
                    .IsFixedLength(true);

                entity.Property(e => e.RegionId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.RegistryType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("REGISTRY_TYPE");

                entity.Property(e => e.UnqNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UNQ_NO");

                entity.Property(e => e.UnqPicklist)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UNQ_PICKLIST");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UPDATED_BY");
            });

            modelBuilder.Entity<DcDestruction>(entity =>
            {
                entity.HasKey(e => e.PensionNo)
                    .HasName("DC_DESTRUCTION_PK");

                entity.ToTable("DC_DESTRUCTION");

                entity.Property(e => e.PensionNo)
                    .HasMaxLength(20)
                    .HasColumnName("PENSION_NO");

                entity.Property(e => e.DestructioDate)
                    .HasMaxLength(20)
                    .HasColumnName("DESTRUCTIO_DATE")
                    .HasDefaultValueSql("TO_CHAR(SYSDATE,'YYYYMMDD')");

                entity.Property(e => e.DestructionBatchId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("DESTRUCTION_BATCH_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ExclusionbatchId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("EXCLUSIONBATCH_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GrantType)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_TYPE");

                entity.Property(e => e.Name)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.RegionId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("REGION_ID")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'Selected' ");

                entity.Property(e => e.StatusDate)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("STATUS_DATE")
                    .HasDefaultValueSql("TO_CHAR(SYSDATE,'YYYYMMDD') ");

                entity.Property(e => e.Surname)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("SURNAME");
            });

            modelBuilder.Entity<DcDistrict>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_DISTRICT");

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT");

                entity.Property(e => e.DistrictName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT_NAME");

                entity.Property(e => e.RegionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");
            });

            modelBuilder.Entity<DcDistrictEc>(entity =>
            {
                entity.HasKey(e => e.DistrictNumber)
                    .HasName("DC_DISTRICT_EC_PK");

                entity.ToTable("DC_DISTRICT_EC");

                entity.Property(e => e.DistrictNumber)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("DISTRICT_NUMBER");

                entity.Property(e => e.DistrictCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.DistrictName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT_NAME");
            });

            modelBuilder.Entity<DcDl>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_DL");

                entity.HasIndex(e => e.Region, "DC_DL_INDX01");

                entity.HasIndex(e => e.BenPenNo, "DC_DL_INDX02");

                entity.HasIndex(e => e.BenNameAndSurname, "DC_DL_INDX03");

                entity.HasIndex(e => e.ChildIdNo, "DC_DL_INDX04");

                entity.HasIndex(e => e.GrantType, "DC_DL_INDX05");

                entity.HasIndex(e => e.GrantAppDate, "DC_DL_INDX06");

                entity.Property(e => e.BenNameAndSurname)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("BEN_NAME_AND_SURNAME");

                entity.Property(e => e.BenPenNo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("BEN_PEN_NO");

                entity.Property(e => e.ChildIdNo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CHILD_ID_NO");

                entity.Property(e => e.ChildNameAndSurname)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CHILD_NAME_AND_SURNAME");

                entity.Property(e => e.CourtOrderDate)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COURT_ORDER_DATE");

                entity.Property(e => e.CourtOrderExpiryPeriodCurrent)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COURT_ORDER_EXPIRY_PERIOD_CURRENT");

                entity.Property(e => e.CourtOrderExtensionDate)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COURT_ORDER_EXTENSION_DATE");

                entity.Property(e => e.CourtOrderNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COURT_ORDER_NUMBER");

                entity.Property(e => e.DistrictCode)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.DistrictDesc)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT_DESC");

                entity.Property(e => e.GrantAppDate)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_APP_DATE");

                entity.Property(e => e.GrantStatus)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_STATUS");

                entity.Property(e => e.GrantType)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_TYPE");

                entity.Property(e => e.NpoName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NPO_NAME");

                entity.Property(e => e.Region)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("REGION");
            });

            modelBuilder.Entity<DcDocumentImage>(entity =>
            {
                entity.ToTable("DC_DOCUMENT_IMAGE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Csnode)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CSNODE");

                entity.Property(e => e.Csurl)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CSURL");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FILENAME");

                entity.Property(e => e.IdNo)
                    .IsRequired()
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("ID_NO");

                entity.Property(e => e.Image)
                    .HasColumnType("BLOB")
                    .HasColumnName("IMAGE");

                entity.Property(e => e.Parentnode)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARENTNODE");

                entity.Property(e => e.Type)
                    .HasPrecision(1)
                    .HasColumnName("TYPE")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<DcDocumentType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_DOCUMENT_TYPE");

                entity.ToTable("DC_DOCUMENT_TYPE");

                entity.Property(e => e.TypeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE_ID");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_NAME");
            });

            modelBuilder.Entity<DcExclusion>(entity =>
            {
                entity.ToTable("DC_EXCLUSIONS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.ExclDate)
                    .HasColumnType("DATE")
                    .HasColumnName("EXCL_DATE");

                entity.Property(e => e.ExclusionBatchId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("EXCLUSION_BATCH_ID");

                entity.Property(e => e.ExclusionType)
                    .HasMaxLength(20)
                    .HasColumnName("EXCLUSION_TYPE");

                entity.Property(e => e.IdNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_NO");

                entity.Property(e => e.RegionId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.Username)
                    .HasMaxLength(40)
                    .HasColumnName("USERNAME");
            });

            modelBuilder.Entity<DcExclusionBatch>(entity =>
            {
                entity.HasKey(e => e.BatchId)
                    .HasName("DC_EXCLUSION_BATCH_PK");

                entity.ToTable("DC_EXCLUSION_BATCH");

                entity.Property(e => e.BatchId)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BATCH_ID");

                entity.Property(e => e.ApprovedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVED_BY");

                entity.Property(e => e.ApprovedDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("APPROVED_DATE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("TO_CHAR(SYSDATE,'YYYYMMDD')");

                entity.Property(e => e.ExclusionYear)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EXCLUSION_YEAR");

                entity.Property(e => e.RegionId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("REGION_ID")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<DcFile>(entity =>
            {
                entity.HasKey(e => e.UnqFileNo);

                entity.ToTable("DC_FILE");

                entity.HasIndex(e => e.BatchNo, "DC_FILE_BATCH_NO_INDEX");

                entity.HasIndex(e => new { e.UnqFileNo, e.RegionId }, "INDEX1");

                entity.HasIndex(e => e.MisBoxno, "INDEX23");

                entity.HasIndex(e => e.BrmBarcode, "INDEX24");

                entity.HasIndex(e => e.MisBoxDate, "INDEX25");

                entity.HasIndex(e => e.ApplicationStatus, "INDEX26");

                entity.HasIndex(e => e.FileStatus, "INDEX27");

                entity.HasIndex(e => e.MisBoxStatus, "INDEX28");

                entity.HasIndex(e => e.TdwBoxno, "INDEX29");

                entity.HasIndex(e => e.Missing, "INDEX30");

                entity.HasIndex(e => e.NonCompliant, "INDEX31");

                entity.HasIndex(e => e.ChildIdNo, "INDEX41");

                entity.HasIndex(e => e.PrintOrder, "INDEX42");

                entity.HasIndex(e => e.RegionId, "INDEX43");

                entity.HasIndex(e => e.FileNumber, "INDEX44");

                entity.HasIndex(e => e.ApplicantNo, "INDEX7");

                entity.HasIndex(e => e.TdwBatch, "DC_FILE_TDWBATCH");

                entity.Property(e => e.UnqFileNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("UNQ_FILE_NO");

                entity.Property(e => e.AltBoxNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ALT_BOX_NO");

                entity.Property(e => e.ApplicantNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("APPLICANT_NO");

                entity.Property(e => e.ApplicationStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("APPLICATION_STATUS");

                entity.Property(e => e.ArchiveYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ARCHIVE_YEAR");

                entity.Property(e => e.BatchAddDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BATCH_ADD_DATE");

                entity.Property(e => e.BatchNo)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BATCH_NO");

                entity.Property(e => e.BoxLocked)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("BOX_LOCKED");

                entity.Property(e => e.BrmBarcode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BRM_BARCODE");

                entity.Property(e => e.ChildIdNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CHILD_ID_NO");

                entity.Property(e => e.Compliant)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COMPLIANT");

                entity.Property(e => e.DocsPresent)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOCS_PRESENT");

                entity.Property(e => e.DocsScanned)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOCS_SCANNED");

                entity.Property(e => e.Exclusions)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EXCLUSIONS");

                entity.Property(e => e.FileComment)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FILE_COMMENT");

                entity.Property(e => e.FileNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FILE_NUMBER");

                entity.Property(e => e.FileStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FILE_STATUS");

                entity.Property(e => e.GrantType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("GRANT_TYPE");

                entity.Property(e => e.Isreview)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ISREVIEW");

                entity.Property(e => e.Lastreviewdate)
                    .HasColumnType("DATE")
                    .HasColumnName("LASTREVIEWDATE");

                entity.Property(e => e.Lctype)
                    .HasColumnType("NUMBER")
                    .HasColumnName("LCTYPE");

                entity.Property(e => e.MiniBoxno)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("MINI_BOXNO");

                entity.Property(e => e.MisBoxDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MIS_BOX_DATE");

                entity.Property(e => e.MisBoxStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_BOX_STATUS");

                entity.Property(e => e.MisBoxno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_BOXNO");

                entity.Property(e => e.MisReboxDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MIS_REBOX_DATE");

                entity.Property(e => e.MisReboxStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_REBOX_STATUS");

                entity.Property(e => e.Missing)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("MISSING");

                entity.Property(e => e.NonCompliant)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("NON_COMPLIANT");

                entity.Property(e => e.OfficeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OFFICE_ID");

                entity.Property(e => e.PrintOrder)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRINT_ORDER");

                entity.Property(e => e.QcDate)
                    .HasColumnType("DATE")
                    .HasColumnName("QC_DATE");

                entity.Property(e => e.QcUserFn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("QC_USER_FN");

                entity.Property(e => e.QcUserLn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("QC_USER_LN");

                entity.Property(e => e.RegionId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.RegionIdFrom)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID_FROM");

                entity.Property(e => e.ScanDatetime)
                    .HasColumnType("DATE")
                    .HasColumnName("SCAN_DATETIME");

                entity.Property(e => e.SrdNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SRD_NO");

                entity.Property(e => e.TdwBoxArchiveYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TDW_BOX_ARCHIVE_YEAR");

                entity.Property(e => e.TdwBoxTypeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TDW_BOX_TYPE_ID");

                entity.Property(e => e.TdwBoxno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TDW_BOXNO");

                entity.Property(e => e.TempBoxNo)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TEMP_BOX_NO");

                entity.Property(e => e.TransDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TRANS_DATE");

                entity.Property(e => e.TransType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRANS_TYPE");

                entity.Property(e => e.Transferred)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TRANSFERRED");

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

                entity.Property(e => e.UserFirstname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USER_FIRSTNAME");

                entity.Property(e => e.UserLastname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USER_LASTNAME");

                entity.HasOne(d => d.BatchNoNavigation)
                    .WithMany(p => p.DcFiles)
                    .HasForeignKey(d => d.BatchNo)
                    .HasConstraintName("FK_DC_BATCH");

                entity.HasOne(d => d.GrantTypeNavigation)
                    .WithMany(p => p.DcFiles)
                    .HasForeignKey(d => d.GrantType)
                    .HasConstraintName("FK_DC_FILE_GRANT");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.DcFiles)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_OFFICE_ID_FILE");

                entity.HasOne(d => d.TransTypeNavigation)
                    .WithMany(p => p.DcFiles)
                    .HasForeignKey(d => d.TransType)
                    .HasConstraintName("FK_DC_FILE_TRANS_TYPE");

                entity.Property(e => e.FspId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("FSP_ID");

                entity.Property(e => e.TdwBatch)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("TDW_BATCH")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.TdwBatchDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TDW_BATCH_DATE");
            });

            modelBuilder.Entity<DcFileDeleted>(entity =>
            {
                entity.HasKey(e => e.UnqFileNo);
                //entity.HasNoKey();

                entity.ToTable("DC_FILE_DELETED");

                entity.Property(e => e.UnqFileNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UNQ_FILE_NO");

                entity.Property(e => e.AltBoxNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ALT_BOX_NO");

                entity.Property(e => e.ApplicantNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("APPLICANT_NO");

                entity.Property(e => e.ApplicationStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("APPLICATION_STATUS");

                entity.Property(e => e.ArchiveYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ARCHIVE_YEAR");

                entity.Property(e => e.BatchAddDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BATCH_ADD_DATE");

                entity.Property(e => e.BatchNo)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BATCH_NO");

                entity.Property(e => e.BrmBarcode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BRM_BARCODE");

                entity.Property(e => e.ChildIdNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CHILD_ID_NO");

                entity.Property(e => e.Compliant)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COMPLIANT");

                entity.Property(e => e.DocsPresent)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOCS_PRESENT");

                entity.Property(e => e.DocsScanned)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOCS_SCANNED");

                entity.Property(e => e.Exclusions)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EXCLUSIONS");

                entity.Property(e => e.FileComment)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FILE_COMMENT");

                entity.Property(e => e.FileNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FILE_NUMBER");

                entity.Property(e => e.FileStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FILE_STATUS");

                entity.Property(e => e.GrantType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_TYPE");

                entity.Property(e => e.Isreview)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ISREVIEW");

                entity.Property(e => e.Lastreviewdate)
                    .HasColumnType("DATE")
                    .HasColumnName("LASTREVIEWDATE");

                entity.Property(e => e.Lctype)
                    .HasColumnType("NUMBER")
                    .HasColumnName("LCTYPE");

                entity.Property(e => e.MisBoxDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MIS_BOX_DATE");

                entity.Property(e => e.MisBoxStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_BOX_STATUS");

                entity.Property(e => e.MisBoxno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_BOXNO");

                entity.Property(e => e.MisReboxDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MIS_REBOX_DATE");

                entity.Property(e => e.MisReboxStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_REBOX_STATUS");

                entity.Property(e => e.Missing)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("MISSING");

                entity.Property(e => e.NonCompliant)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("NON_COMPLIANT");

                entity.Property(e => e.OfficeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OFFICE_ID");

                entity.Property(e => e.PrintOrder)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRINT_ORDER");

                entity.Property(e => e.QcDate)
                    .HasColumnType("DATE")
                    .HasColumnName("QC_DATE");

                entity.Property(e => e.QcUserFn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("QC_USER_FN");

                entity.Property(e => e.QcUserLn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("QC_USER_LN");

                entity.Property(e => e.RegionId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.RegionIdFrom)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID_FROM");

                entity.Property(e => e.ScanDatetime)
                    .HasColumnType("DATE")
                    .HasColumnName("SCAN_DATETIME");

                entity.Property(e => e.SrdNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SRD_NO");

                entity.Property(e => e.TdwBoxArchiveYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TDW_BOX_ARCHIVE_YEAR");

                entity.Property(e => e.TdwBoxTypeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TDW_BOX_TYPE_ID");

                entity.Property(e => e.TdwBoxno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TDW_BOXNO");

                entity.Property(e => e.TempBoxNo)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TEMP_BOX_NO");

                entity.Property(e => e.TransDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TRANS_DATE");

                entity.Property(e => e.TransType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRANS_TYPE");

                entity.Property(e => e.Transferred)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TRANSFERRED");

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

                entity.Property(e => e.UserFirstname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USER_FIRSTNAME");

                entity.Property(e => e.UserLastname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USER_LASTNAME");

                entity.Property(e => e.FspId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("FSP_ID");
            });

            modelBuilder.Entity<DcFileRec>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_FILE_REC");

                entity.HasIndex(e => e.ApplicantNo, "INDEX50");

                entity.HasIndex(e => e.RegionId, "INDEX51");

                entity.HasIndex(e => e.UnqFileNo, "INDEX52")
                    .IsUnique();

                entity.HasIndex(e => e.MisBoxno, "INDEX53");

                entity.HasIndex(e => e.BrmBarcode, "INDEX54");

                entity.HasIndex(e => e.MisBoxDate, "INDEX55");

                entity.HasIndex(e => e.ApplicationStatus, "INDEX56");

                entity.HasIndex(e => e.FileStatus, "INDEX57");

                entity.HasIndex(e => e.MisBoxStatus, "INDEX58");

                entity.HasIndex(e => e.TdwBoxno, "INDEX59");

                entity.Property(e => e.AltBoxNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ALT_BOX_NO");

                entity.Property(e => e.ApplicantNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("APPLICANT_NO");

                entity.Property(e => e.ApplicationStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("APPLICATION_STATUS");

                entity.Property(e => e.ArchiveYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ARCHIVE_YEAR");

                entity.Property(e => e.BatchAddDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BATCH_ADD_DATE");

                entity.Property(e => e.BatchNo)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BATCH_NO");

                entity.Property(e => e.BrmBarcode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BRM_BARCODE");

                entity.Property(e => e.ChildIdNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CHILD_ID_NO");

                entity.Property(e => e.Compliant)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COMPLIANT");

                entity.Property(e => e.DocsPresent)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOCS_PRESENT");

                entity.Property(e => e.DocsScanned)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DOCS_SCANNED");

                entity.Property(e => e.Exclusions)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EXCLUSIONS");

                entity.Property(e => e.FileComment)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FILE_COMMENT");

                entity.Property(e => e.FileNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FILE_NUMBER");

                entity.Property(e => e.FileStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FILE_STATUS");

                entity.Property(e => e.GrantType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_TYPE");

                entity.Property(e => e.Isreview)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ISREVIEW");

                entity.Property(e => e.Lastreviewdate)
                    .HasColumnType("DATE")
                    .HasColumnName("LASTREVIEWDATE");

                entity.Property(e => e.Lctype)
                    .HasColumnType("NUMBER")
                    .HasColumnName("LCTYPE");

                entity.Property(e => e.MisBoxDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MIS_BOX_DATE");

                entity.Property(e => e.MisBoxStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_BOX_STATUS");

                entity.Property(e => e.MisBoxno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_BOXNO");

                entity.Property(e => e.MisReboxDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MIS_REBOX_DATE");

                entity.Property(e => e.MisReboxStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_REBOX_STATUS");

                entity.Property(e => e.Missing)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("MISSING");

                entity.Property(e => e.NonCompliant)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("NON_COMPLIANT");

                entity.Property(e => e.OfficeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OFFICE_ID");

                entity.Property(e => e.PrintOrder)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRINT_ORDER");

                entity.Property(e => e.QcDate)
                    .HasColumnType("DATE")
                    .HasColumnName("QC_DATE");

                entity.Property(e => e.QcUserFn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("QC_USER_FN");

                entity.Property(e => e.QcUserLn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("QC_USER_LN");

                entity.Property(e => e.RegionId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.RegionIdFrom)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID_FROM");

                entity.Property(e => e.ScanDatetime)
                    .HasColumnType("DATE")
                    .HasColumnName("SCAN_DATETIME");

                entity.Property(e => e.SrdNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SRD_NO");

                entity.Property(e => e.TdwBoxArchiveYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TDW_BOX_ARCHIVE_YEAR");

                entity.Property(e => e.TdwBoxTypeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TDW_BOX_TYPE_ID");

                entity.Property(e => e.TdwBoxno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TDW_BOXNO");

                entity.Property(e => e.TempBoxNo)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TEMP_BOX_NO");

                entity.Property(e => e.TransDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TRANS_DATE");

                entity.Property(e => e.TransType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRANS_TYPE");

                entity.Property(e => e.Transferred)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TRANSFERRED");

                entity.Property(e => e.UnqFileNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UNQ_FILE_NO");

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

                entity.Property(e => e.UserFirstname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USER_FIRSTNAME");

                entity.Property(e => e.UserLastname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USER_LASTNAME");
            });

            modelBuilder.Entity<DcFileRequest>(entity =>
            {
                entity.HasKey(e => new { e.IdNo, e.GrantType })
                    .HasName("DC_FILE_REQUEST_PK");

                entity.ToTable("DC_FILE_REQUEST");

                entity.HasIndex(e => e.UnqFileNo, "PK_DC_FILE_REQUEST")
                    .IsUnique();

                entity.Property(e => e.IdNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_NO");

                entity.Property(e => e.GrantType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_TYPE");

                entity.Property(e => e.AppDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APP_DATE");

                entity.Property(e => e.ApplicationStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("APPLICATION_STATUS");

                entity.Property(e => e.BinId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BIN_ID");

                entity.Property(e => e.BoxNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BOX_NUMBER");

                entity.Property(e => e.BrmBarcode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BRM_BARCODE");

                entity.Property(e => e.ClmNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CLM_NUMBER");

                entity.Property(e => e.ClosedBy)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CLOSED_BY");

                entity.Property(e => e.ClosedByAd)
                    .HasMaxLength(225)
                    .IsUnicode(false)
                    .HasColumnName("CLOSED_BY_AD");

                entity.Property(e => e.ClosedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSED_DATE");

                entity.Property(e => e.FileRetrieved)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("FILE_RETRIEVED")
                    .IsFixedLength(true);

                entity.Property(e => e.MisFileNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MIS_FILE_NO");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PickedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PICKED_BY");

                entity.Property(e => e.PicklistNo)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PICKLIST_NO");

                entity.Property(e => e.PicklistStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PICKLIST_STATUS");

                entity.Property(e => e.PicklistType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PICKLIST_TYPE");

                entity.Property(e => e.Position)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("POSITION");

                entity.Property(e => e.ReceivedTdw)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RECEIVED_TDW")
                    .IsFixedLength(true);

                entity.Property(e => e.RegionId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.RegionIdTo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID_TO");

                entity.Property(e => e.RelatedMisFileNo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("RELATED_MIS_FILE_NO");

                entity.Property(e => e.ReqCategory)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REQ_CATEGORY");

                entity.Property(e => e.ReqCategoryDetail)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("REQ_CATEGORY_DETAIL");

                entity.Property(e => e.ReqCategoryType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REQ_CATEGORY_TYPE");

                entity.Property(e => e.RequestedBy)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REQUESTED_BY");

                entity.Property(e => e.RequestedByAd)
                    .HasMaxLength(225)
                    .IsUnicode(false)
                    .HasColumnName("REQUESTED_BY_AD");

                entity.Property(e => e.RequestedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REQUESTED_DATE");

                entity.Property(e => e.RequestedOfficeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("REQUESTED_OFFICE_ID");

                entity.Property(e => e.ScannedBy)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SCANNED_BY");

                entity.Property(e => e.ScannedByAd)
                    .HasMaxLength(225)
                    .IsUnicode(false)
                    .HasColumnName("SCANNED_BY_AD");

                entity.Property(e => e.ScannedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("SCANNED_DATE");

                entity.Property(e => e.ScannedPhysicalInd)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SCANNED_PHYSICAL_IND")
                    .IsFixedLength(true);

                entity.Property(e => e.SendToRequestor)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SEND_TO_REQUESTOR")
                    .IsFixedLength(true);

                entity.Property(e => e.SentTdw)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SENT_TDW");

                entity.Property(e => e.ServBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SERV_BY");

                entity.Property(e => e.Stakeholder)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STAKEHOLDER");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.Surname)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SURNAME");

                entity.Property(e => e.TdwBoxno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TDW_BOXNO");

                entity.Property(e => e.UnqFileNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("UNQ_FILE_NO");

                entity.Property(e => e.UnqPicklist)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UNQ_PICKLIST");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.DcFileRequests)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_FILE_REQUEST_REGION_ID");

                entity.HasOne(d => d.StakeholderNavigation)
                    .WithMany(p => p.DcFileRequests)
                    .HasForeignKey(d => d.Stakeholder)
                    .HasConstraintName("DC_FILE_REQUEST_FK1");

                entity.HasOne(d => d.ReqCategoryNavigation)
                    .WithMany(p => p.DcFileRequests)
                    .HasForeignKey(d => new { d.ReqCategory, d.ReqCategoryType })
                    .HasConstraintName("FK_REQUEST_CATEGORY");
            });

            modelBuilder.Entity<DcGrantDocLink>(entity =>
            {
                entity.HasKey(e => new { e.GrantId, e.TransactionId, e.DocumentId })
                    .HasName("PK_GRAND_DOC_TRANS");

                entity.ToTable("DC_GRANT_DOC_LINK");

                entity.Property(e => e.GrantId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_ID");

                entity.Property(e => e.TransactionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRANSACTION_ID");

                entity.Property(e => e.DocumentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DOCUMENT_ID");

                entity.Property(e => e.CriticalFlag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CRITICAL_FLAG")
                    .IsFixedLength(true);

                entity.Property(e => e.Section)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SECTION");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DcGrantDocLinks)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DOCUMENT_TYPE_LINK");

                entity.HasOne(d => d.Grant)
                    .WithMany(p => p.DcGrantDocLinks)
                    .HasForeignKey(d => d.GrantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GRANT_TYPE_GRANT");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.DcGrantDocLinks)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DC_TRANS_TYPE");
            });

            modelBuilder.Entity<DcGrantDoctype>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_GRANT_DOCTYPE");

                entity.Property(e => e.CsDoctype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CS_DOCTYPE");

                entity.Property(e => e.Docid)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("DOCID");

                entity.Property(e => e.Doctype)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DOCTYPE");

                entity.Property(e => e.Grantid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GRANTID");
            });

            modelBuilder.Entity<DcGrantType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_GRANT_TYPE");

                entity.ToTable("DC_GRANT_TYPE");

                entity.Property(e => e.TypeId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_ID");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_NAME");
            });

            modelBuilder.Entity<DcLcType>(entity =>
            {
                entity.HasKey(e => e.Pk)
                    .HasName("DC_LC_TYPE_PK");

                entity.ToTable("DC_LC_TYPE");

                entity.Property(e => e.Pk)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PK");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");
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

                entity.Property(e => e.ActiveStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVE_STATUS");

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

                entity.Property(e => e.ManualBatch)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'I' ")
                    .HasColumnName("MANUAL_BATCH");
                entity.Property(e => e.RegionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");
            });

            modelBuilder.Entity<DcMerge>(entity =>
            {
                entity.HasKey(e => e.Pk)
                    .HasName("DC_MERGE_PK");

                entity.ToTable("DC_MERGE");

                entity.Property(e => e.Pk)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PK");

                entity.Property(e => e.BrmBarcode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BRM_BARCODE");

                entity.Property(e => e.ParentBrmBarcode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PARENT_BRM_BARCODE");
            });

            modelBuilder.Entity<DcMisboxMissing>(entity =>
            {
                entity.ToTable("DC_MISBOX_MISSING");

                entity.HasIndex(e => e.UnqFileNo, "DC_MISBOX_MISSING_INDEX1")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.BoxNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BOX_NO");

                entity.Property(e => e.CaptureByAd)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CAPTURE_BY_AD");

                entity.Property(e => e.CaptureDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CAPTURE_DATE")
                    .HasDefaultValueSql("CURRENT_DATE");

                entity.Property(e => e.RegionId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.UnqFileNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UNQ_FILE_NO");
            });

            modelBuilder.Entity<DcMissingStatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_MISSING_STATUS");

                entity.Property(e => e.Count)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COUNT");

                entity.Property(e => e.MissingOn)
                    .HasColumnType("DATE")
                    .HasColumnName("MISSING_ON");

                entity.Property(e => e.Quarter)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("QUARTER");

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("REGION");

                entity.Property(e => e.Year)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("YEAR");
            });

            modelBuilder.Entity<DcOfficeKuafLink>(entity =>
            {
                entity.HasKey(e => e.Pk)
                    .HasName("DC_OFFICE_KUAF_LINK_PK");

                entity.ToTable("DC_OFFICE_KUAF_LINK");

                entity.Property(e => e.Pk)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PK");

                entity.Property(e => e.FspId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("FSP_ID");

                entity.Property(e => e.OfficeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OFFICE_ID");

                entity.Property(e => e.Supervisor)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SUPERVISOR");

                entity.Property(e => e.Username)
                    .HasMaxLength(225)
                    .IsUnicode(false)
                    .HasColumnName("USERNAME");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.DcOfficeKuafLinks)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_LOCAL_OFFICE");
            });

            modelBuilder.Entity<DcPicklist>(entity =>
            {
                entity.HasKey(e => e.UnqPicklist)
                    .HasName("DC_PICKLIST_PK");

                entity.ToTable("DC_PICKLIST");

                entity.Property(e => e.UnqPicklist)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("UNQ_PICKLIST");

                entity.Property(e => e.PicklistDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PICKLIST_DATE");

                entity.Property(e => e.PicklistStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PICKLIST_STATUS")
                    .IsFixedLength(true);

                entity.Property(e => e.RegionId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.RegistryType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("REGISTRY_TYPE");

                entity.Property(e => e.RequestedByAd)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REQUESTED_BY_AD");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UPDATED_BY");
            });

            modelBuilder.Entity<DcPicklistItem>(entity =>
            {
                entity.HasKey(e => e.PicklistItemId)
                    .HasName("DC_PICKLIST_ITEM_PK");

                entity.ToTable("DC_PICKLIST_ITEM");

                entity.HasIndex(e => e.BrmNo, "DC_PICKLIST_ITEM_UK1")
                    .IsUnique();

                entity.Property(e => e.PicklistItemId)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PICKLIST_ITEM_ID");

                entity.Property(e => e.Bin)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BIN");

                entity.Property(e => e.Box)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BOX");

                entity.Property(e => e.BrmNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BRM_NO");

                entity.Property(e => e.BvpLc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BVP_LC");

                entity.Property(e => e.ClmNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CLM_NO");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.FolderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FOLDER_ID");

                entity.Property(e => e.GrantType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_TYPE");

                entity.Property(e => e.IdNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_NUMBER");

                entity.Property(e => e.LcType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LC_TYPE");

                entity.Property(e => e.Location)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.LooseCorrespondenceId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOOSE_CORRESPONDENCE_ID");

                entity.Property(e => e.Minibox)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MINIBOX");

                entity.Property(e => e.Position)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("POSITION");

                entity.Property(e => e.Reg)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REG");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.Surname)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SURNAME");

                entity.Property(e => e.UnqPicklist)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UNQ_PICKLIST");

                entity.Property(e => e.Userpicked)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USERPICKED");

                entity.Property(e => e.Year)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("YEAR");
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

            modelBuilder.Entity<DcReqCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK_REQ_CATEGORY");

                entity.ToTable("DC_REQ_CATEGORY");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.CategoryDescr)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CATEGORY_DESCR");
            });

            modelBuilder.Entity<DcReqCategoryType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_CATEGORY_TYPE");

                entity.ToTable("DC_REQ_CATEGORY_TYPE");

                entity.Property(e => e.TypeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE_ID");

                entity.Property(e => e.TypeDescr)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_DESCR");
            });

            modelBuilder.Entity<DcReqCategoryTypeLink>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.TypeId })
                    .HasName("PK_CATEGORY_TYPE_LINK");

                entity.ToTable("DC_REQ_CATEGORY_TYPE_LINK");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.TypeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE_ID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.DcReqCategoryTypeLinks)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CATEGORY");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.DcReqCategoryTypeLinks)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CATEGORY_TYPE");
            });

            modelBuilder.Entity<DcSrdDatum>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DC_SRD_DATA");

                entity.HasIndex(e => e.RegionCode, "DC_SRD_DATA_INDX00");

                entity.HasIndex(e => e.SrdNo, "DC_SRD_DATA_INDX01");

                entity.HasIndex(e => e.IdNo, "DC_SRD_DATA_INDX02");

                entity.HasIndex(e => e.Name, "DC_SRD_DATA_INDX03");

                entity.HasIndex(e => e.Surname, "DC_SRD_DATA_INDX04");

                entity.HasIndex(e => e.LastAppDate, "DC_SRD_DATA_INDX05");

                entity.Property(e => e.IdNo)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("ID_NO");

                entity.Property(e => e.LastAppDate)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("LAST_APP_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.RegionCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REGION_CODE");

                entity.Property(e => e.SrdNo)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("SRD_NO");

                entity.Property(e => e.Surname)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("SURNAME");
            });

            modelBuilder.Entity<DcStakeholder>(entity =>
            {
                entity.HasKey(e => e.StakeholderId)
                    .HasName("DC_STAKEHOLDER_PK");

                entity.ToTable("DC_STAKEHOLDER");

                entity.Property(e => e.StakeholderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STAKEHOLDER_ID");

                entity.Property(e => e.DepartmentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(225)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.RegionId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("SURNAME");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.DcStakeholders)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DC_STAKEHOLDER_FK1");
            });

            modelBuilder.Entity<SocpenSrdBen>(entity =>
            {
                entity.HasKey(e => e.SrdNo)
                    .HasName("SOCPEN_SRD_BEN_PK");

                entity.ToTable("SOCPEN_SRD_BEN", "SASSA");

                entity.HasIndex(e => e.IdNo, "INDEX1");

                entity.HasIndex(e => e.Province, "INDEX2");

                entity.HasIndex(e => e.Region, "INDEX3");

                entity.Property(e => e.SrdNo)
                    .HasPrecision(13)
                    .HasColumnName("SRD_NO");

                entity.Property(e => e.AcceptanceEmployment)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACCEPTANCE_EMPLOYMENT");

                entity.Property(e => e.Address1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS_1");

                entity.Property(e => e.Address2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS_2");

                entity.Property(e => e.Address3)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS_3");

                entity.Property(e => e.Address4)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS_4");

                entity.Property(e => e.AdmittanceDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ADMITTANCE_DATE");

                entity.Property(e => e.AppealDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPEAL_DATE");

                entity.Property(e => e.AppealInd)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("APPEAL_IND");

                entity.Property(e => e.AppointmentDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPOINTMENT_DATE");

                entity.Property(e => e.ArrestDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ARREST_DATE");

                entity.Property(e => e.AssumptionDutyDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ASSUMPTION_DUTY_DATE");

                entity.Property(e => e.Birthdate)
                    .HasColumnType("DATE")
                    .HasColumnName("BIRTHDATE");

                entity.Property(e => e.CellNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CELL_NO");

                entity.Property(e => e.DeathCertificate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DEATH_CERTIFICATE");

                entity.Property(e => e.DeathDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DEATH_DATE");

                entity.Property(e => e.DischargeCertificate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DISCHARGE_CERTIFICATE");

                entity.Property(e => e.DistressedCircumstance1)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("DISTRESSED_CIRCUMSTANCE_1");

                entity.Property(e => e.DistressedCircumstance2)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("DISTRESSED_CIRCUMSTANCE_2");

                entity.Property(e => e.DistressedCircumstance3)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("DISTRESSED_CIRCUMSTANCE_3");

                entity.Property(e => e.DistressedCircumstance4)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("DISTRESSED_CIRCUMSTANCE_4");

                entity.Property(e => e.District)
                    .HasPrecision(4)
                    .HasColumnName("DISTRICT");

                entity.Property(e => e.EducationLevel)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EDUCATION_LEVEL");

                entity.Property(e => e.FileReferenceNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FILE_REFERENCE_NO");

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("GENDER");

                entity.Property(e => e.IdNo)
                    .HasPrecision(13)
                    //.ValueGeneratedOnAdd()
                    .HasColumnName("ID_NO");

                entity.Property(e => e.IdentType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IDENT_TYPE");

                entity.Property(e => e.IdentTypeSpecify)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("IDENT_TYPE_SPECIFY");

                entity.Property(e => e.Initials)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("INITIALS");

                entity.Property(e => e.InstDateFrom)
                    .HasColumnType("DATE")
                    .HasColumnName("INST_DATE_FROM");

                entity.Property(e => e.InstDateTo)
                    .HasColumnType("DATE")
                    .HasColumnName("INST_DATE_TO");

                entity.Property(e => e.InstitutionName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_NAME");

                entity.Property(e => e.Language)
                    .HasPrecision(2)
                    .HasColumnName("LANGUAGE");

                entity.Property(e => e.MaritalStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MARITAL_STATUS");

                entity.Property(e => e.MedicalCertificate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MEDICAL_CERTIFICATE");

                entity.Property(e => e.MilitaryVeteranInd)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MILITARY_VETERAN_IND");

                entity.Property(e => e.Name)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.NoDepChildren)
                    .HasPrecision(2)
                    .HasColumnName("NO_DEP_CHILDREN");

                entity.Property(e => e.OldSocialReliefNo)
                    .HasPrecision(9)
                    .HasColumnName("OLD_SOCIAL_RELIEF_NO");

                entity.Property(e => e.OtherDetail1)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("OTHER_DETAIL_1");

                entity.Property(e => e.OtherDetail2)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("OTHER_DETAIL_2");

                entity.Property(e => e.OtherDetail3)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("OTHER_DETAIL_3");

                entity.Property(e => e.OtherDetail4)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("OTHER_DETAIL_4");

                entity.Property(e => e.OtherIdDoc)
                    .HasPrecision(13)
                    .HasColumnName("OTHER_ID_DOC");

                entity.Property(e => e.Paypoint)
                    .HasPrecision(6)
                    .HasColumnName("PAYPOINT");

                entity.Property(e => e.PoaDate)
                    .HasColumnType("DATE")
                    .HasColumnName("POA_DATE");

                entity.Property(e => e.PoaNo)
                    .HasPrecision(13)
                    .HasColumnName("POA_NO");

                entity.Property(e => e.PoaReason)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("POA_REASON");

                entity.Property(e => e.PostalAddress1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POSTAL_ADDRESS_1");

                entity.Property(e => e.PostalAddress2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POSTAL_ADDRESS_2");

                entity.Property(e => e.PostalAddress3)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POSTAL_ADDRESS_3");

                entity.Property(e => e.PostalAddress4)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POSTAL_ADDRESS_4");

                entity.Property(e => e.PostalCode)
                    .HasPrecision(4)
                    .HasColumnName("POSTAL_CODE");

                entity.Property(e => e.Province)
                    .HasPrecision(2)
                    .HasColumnName("PROVINCE");

                entity.Property(e => e.Recipient)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RECIPIENT");

                entity.Property(e => e.ReferralDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REFERRAL_DATE");

                entity.Property(e => e.ReferralNeeded)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REFERRAL_NEEDED");

                entity.Property(e => e.ReferralNeededDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REFERRAL_NEEDED_DATE");

                entity.Property(e => e.ReferredForTreatment)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REFERRED_FOR_TREATMENT");

                entity.Property(e => e.Region)
                    .HasPrecision(6)
                    .HasColumnName("REGION");

                entity.Property(e => e.ResPostalCode)
                    .HasPrecision(4)
                    .HasColumnName("RES_POSTAL_CODE");

                entity.Property(e => e.SentenceDate)
                    .HasColumnType("DATE")
                    .HasColumnName("SENTENCE_DATE");

                entity.Property(e => e.SocialWorker)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SOCIAL_WORKER");

                entity.Property(e => e.SpouseNo)
                    .HasPrecision(13)
                    .HasColumnName("SPOUSE_NO");

                entity.Property(e => e.Surname)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SURNAME");

                entity.Property(e => e.TelephoneNo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TELEPHONE_NO");

                entity.Property(e => e.TotalDeducted)
                    .HasColumnType("NUMBER(8,2)")
                    .HasColumnName("TOTAL_DEDUCTED");

                entity.Property(e => e.TotalIssued)
                    .HasColumnType("NUMBER(8,2)")
                    .HasColumnName("TOTAL_ISSUED");
            });

            modelBuilder.Entity<SocpenSrdType>(entity =>
            {
                entity.HasKey(e => e.AdabasIsn)
                    .HasName("SOCPEN_SRD_TYPE_PK");

                entity.ToTable("SOCPEN_SRD_TYPE", "SASSA");

                entity.HasIndex(e => e.Region, "INDEX10");

                entity.HasIndex(e => e.ReceiptNo, "INDEX6");

                entity.HasIndex(e => e.SocialReliefNo, "INDEX7");

                entity.HasIndex(e => e.Province, "INDEX8");

                entity.HasIndex(e => e.BenefitType, "INDEX9");

                entity.Property(e => e.AdabasIsn)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("ADABAS_ISN");

                entity.Property(e => e.AdmittanceDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ADMITTANCE_DATE");

                entity.Property(e => e.ApplicationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPLICATION_DATE");

                entity.Property(e => e.ApplicationReason)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("APPLICATION_REASON");

                entity.Property(e => e.ApplicationStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("APPLICATION_STATUS");

                entity.Property(e => e.ApplicationType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("APPLICATION_TYPE");

                entity.Property(e => e.BenefitType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BENEFIT_TYPE");

                entity.Property(e => e.DateApproved)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_APPROVED");

                entity.Property(e => e.DeceasedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DECEASED_DATE");

                entity.Property(e => e.DisasterDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DISASTER_DATE");

                entity.Property(e => e.InstDateFrom)
                    .HasColumnType("DATE")
                    .HasColumnName("INST_DATE_FROM");

                entity.Property(e => e.InstDateTo)
                    .HasColumnType("DATE")
                    .HasColumnName("INST_DATE_TO");

                entity.Property(e => e.InstitutionName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("INSTITUTION_NAME");

                entity.Property(e => e.Paypoint)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("PAYPOINT");

                entity.Property(e => e.PoaDate)
                    .HasColumnType("DATE")
                    .HasColumnName("POA_DATE");

                entity.Property(e => e.PoaNo)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("POA_NO");

                entity.Property(e => e.PoaReason)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("POA_REASON");

                entity.Property(e => e.PreviousSocialRelief)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PREVIOUS_SOCIAL_RELIEF");

                entity.Property(e => e.Province)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("PROVINCE");

                entity.Property(e => e.RandValue)
                    .HasColumnType("NUMBER(7,2)")
                    .HasColumnName("RAND_VALUE");

                entity.Property(e => e.ReceiptNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RECEIPT_NO");

                entity.Property(e => e.Region)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("REGION");

                entity.Property(e => e.SentenceDate)
                    .HasColumnType("DATE")
                    .HasColumnName("SENTENCE_DATE");

                entity.Property(e => e.SocialReliefNo)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SOCIAL_RELIEF_NO");

                entity.Property(e => e.VoucherNo)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("VOUCHER_NO");
            });

            modelBuilder.Entity<DcTransactionType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_TRANSACTION_TYPE");

                entity.ToTable("DC_TRANSACTION_TYPE");

                entity.Property(e => e.TypeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE_ID");

                entity.Property(e => e.ServiceCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SERVICE_CATEGORY");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_NAME");
            });

            modelBuilder.Entity<TdwFileLocation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TDW_FILE_LOCATION");

                entity.HasIndex(e => e.Description, "TDW_FILE_LOCATION_INDX01");

                entity.HasIndex(e => e.Region, "TDW_FILE_LOCATION_INDX02");

                entity.HasIndex(e => e.ContainerCode, "TDW_FILE_LOCATION_INDX03");

                entity.HasIndex(e => e.ContainerAltcode, "TDW_FILE_LOCATION_INDX04");

                entity.HasIndex(e => e.FilefolderCode, "TDW_FILE_LOCATION_INDX05");

                entity.HasIndex(e => e.FilefolderAltcode, "TDW_FILE_LOCATION_INDX06");

                entity.HasIndex(e => e.GrantType, "TDW_FILE_LOCATION_INDX07");

                entity.HasIndex(e => e.Name, "TDW_FILE_LOCATION_INDX08");

                entity.Property(e => e.ContainerAltcode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CONTAINER_ALTCODE");

                entity.Property(e => e.ContainerCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CONTAINER_CODE");

                entity.Property(e => e.Description)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.FilefolderAltcode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FILEFOLDER_ALTCODE");

                entity.Property(e => e.FilefolderCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FILEFOLDER_CODE");

                entity.Property(e => e.GrantType)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_TYPE");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Region)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("REGION");
            });

            modelBuilder.Entity<MisLivelinkTbl>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MIS_LIVELINK_TBL");

                entity.HasIndex(e => e.MiniBoxNumber, "INDEX21");

                entity.HasIndex(e => e.GrantType, "INDEX22");

                entity.HasIndex(e => new { e.FileNumber, e.IdNumber, e.GrantType }, "INDEX49");

                entity.HasIndex(e => e.BinId, "INDX_BIN_ID");

                entity.HasIndex(e => e.BoxNumber, "INDX_BOX_NUMBER");

                entity.HasIndex(e => e.FileNumber, "INDX_FILE_NUMBER");

                entity.HasIndex(e => e.IdNumber, "INDX_IDNUMBER");

                entity.HasIndex(e => e.Position, "INDX_POSITION");

                entity.HasIndex(e => e.RegionId, "INDX_REGION_ID");

                entity.HasIndex(e => e.RegistryType, "INDX_REGISTRY_TYPE");

                entity.HasIndex(e => e.SubRegistryType, "INDX_SUB_REGISTRY_TYPE");

                entity.Property(e => e.AppDate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APP_DATE");

                entity.Property(e => e.BinId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BIN_ID");

                entity.Property(e => e.BoxNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BOX_NUMBER");

                entity.Property(e => e.DateChange)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DATE_CHANGE");

                entity.Property(e => e.FileNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FILE_NUMBER");

                entity.Property(e => e.GrantType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GRANT_TYPE");

                entity.Property(e => e.IdNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID_NUMBER");

                entity.Property(e => e.MiniBoxNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MINI_BOX_NUMBER");

                entity.Property(e => e.MisStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MIS_STATUS");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Position)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POSITION");

                entity.Property(e => e.RecordChange)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("RECORD_CHANGE");

                entity.Property(e => e.RegionId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.RegistryType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REGISTRY_TYPE");

                entity.Property(e => e.SubRegistryType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SUB_REGISTRY_TYPE");

                entity.Property(e => e.Surname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SURNAME");
            });

            modelBuilder.Entity<DcSocpen>(entity =>
            {
                entity.ToTable("DC_SOCPEN");

                entity.HasIndex(e => e.BrmBarcode, "DC_SOCPEN_BRM_BARCODE")
                    .IsUnique();

                entity.HasIndex(e => e.BeneficiaryId, "DC_SOCPEN_IDX01");

                entity.HasIndex(e => e.CaptureReference, "DC_SOCPEN_IDX02");

                entity.HasIndex(e => new { e.BeneficiaryId, e.GrantType, e.ChildId, e.SrdNo }, "DC_SOCPEN_ID_GRANT")
                    .IsUnique();

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

                entity.Property(e => e.Exception)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EXCEPTION");

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

                entity.Property(e => e.OgaDate)
                    .HasColumnType("DATE")
                    .HasColumnName("OGA_DATE");

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

            modelBuilder.Entity<DcFixedServicePoint>(entity =>
            {
                entity.ToTable("DC_FIXED_SERVICE_POINT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.OfficeId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OFFICE_ID");

                entity.Property(e => e.ServicePointName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SERVICE_POINT_NAME");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.DcFixedServicePoints)
                    .HasForeignKey(d => d.OfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0011143");
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


//Scaffold-DbContext "DATA SOURCE=10.117.122.120:1521/brmtrn;PERSIST SECURITY INFO=True;USER ID=CONTENTSERVER;Password=Password123;" Oracle.EntityFrameworkCore -OutputDir Context -Tables TDW_FILE_LOCATION -f
