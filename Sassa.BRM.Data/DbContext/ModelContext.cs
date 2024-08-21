using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sassa.BRM.Data.Models;

namespace Sassa.BRM.Data.DbContext;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DcDocumentImage> DcDocumentImages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=10.117.123.20:1521/brmtn;PERSIST SECURITY INFO=True;USER ID=CONTENTSERVER;Password=Password123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("CONTENTSERVER")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<DcDocumentImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DC_DOCUMENT_IMAGE_PK");

            entity.ToTable("DC_DOCUMENT_IMAGE");

            entity.HasIndex(e => e.IdNo, "DC_DOCUMENT_IMAGE_INDEX2");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
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
                .HasDefaultValueSql("1")
                .HasColumnType("NUMBER(1)")
                .HasColumnName("TYPE");
            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("URL");
        });
        modelBuilder.HasSequence("ACTIVEVIEWOVERRIDESSEQUENCE");
        modelBuilder.HasSequence("AGENTSEQUENCE");
        modelBuilder.HasSequence("AUDITCOLLECTIONSITEMSSEQ");
        modelBuilder.HasSequence("BRMWAYBIL");
        modelBuilder.HasSequence("CUST_ACTIVE_GRANTS_SEQ");
        modelBuilder.HasSequence("CUST_COMPLY_SEQ");
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
        modelBuilder.HasSequence("SEQ_TDW_BATCH");
        modelBuilder.HasSequence("WORKERQUEUESEQUENCE");
        modelBuilder.HasSequence("WWORKAUDITSEQ");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
