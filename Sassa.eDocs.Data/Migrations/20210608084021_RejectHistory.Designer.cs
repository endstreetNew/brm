﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using Sassa.eDocs;

namespace Sassa.eDocs.Data.Migrations
{
    [DbContext(typeof(eDocumentContext))]
    [Migration("20210608084021_RejectHistory")]
    partial class RejectHistory
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sassa.eDocs.Data.Models.ApplicationDocument", b =>
                {
                    b.Property<int>("ApplicationDocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApplicationTypeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("ApplicationDocumentId");

                    b.ToTable("ApplicationDocuments");
                });

            modelBuilder.Entity("Sassa.eDocs.Data.Models.ApplicationType", b =>
                {
                    b.Property<int>("ApplcationTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.HasKey("ApplcationTypeId");

                    b.ToTable("ApplicationTypes");
                });

            modelBuilder.Entity("Sassa.eDocs.Data.Models.DocImage", b =>
                {
                    b.Property<int>("DocImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DocumentId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<byte[]>("Image")
                        .HasMaxLength(400000)
                        .HasColumnType("BLOB");

                    b.HasKey("DocImageId");

                    b.ToTable("DocImage");
                });

            modelBuilder.Entity("Sassa.eDocs.Data.Models.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApplicationTypeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("CSNode")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("ChildIdNo")
                        .HasMaxLength(13)
                        .HasColumnType("NVARCHAR2(13)");

                    b.Property<string>("DateStamp")
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("FileName")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("IdNo")
                        .HasMaxLength(13)
                        .HasColumnType("NVARCHAR2(13)");

                    b.Property<bool>("InternalDocument")
                        .HasColumnType("NUMBER(1)");

                    b.Property<int>("LoDocumentTypeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Reference")
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR2(20)");

                    b.Property<string>("RegionCode")
                        .HasMaxLength(13)
                        .HasColumnType("NVARCHAR2(13)");

                    b.Property<string>("RejectReason")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("Status")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("SupportDocument")
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR2(20)");

                    b.Property<string>("User")
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.HasKey("DocumentId");

                    b.HasIndex("ChildIdNo");

                    b.HasIndex("IdNo");

                    b.HasIndex("Reference");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Sassa.eDocs.Data.Models.DocumentType", b =>
                {
                    b.Property<int>("DocumentTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.HasKey("DocumentTypeId");

                    b.ToTable("DocumentTypes");
                });

            modelBuilder.Entity("Sassa.eDocs.Data.Models.LoDocumentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName")
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.Property<string>("DocumentType")
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("LoDocumentTypeId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.ToTable("LoDocumentTypes");
                });

            modelBuilder.Entity("Sassa.eDocs.Data.Models.RejectHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LoDocumentTypeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Reference")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("RejectReason")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.ToTable("RejectHistory");
                });

            modelBuilder.Entity("Sassa.eDocs.Data.Models.RejectReason", b =>
                {
                    b.Property<int>("RejectReasonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Reason")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.HasKey("RejectReasonId");

                    b.ToTable("RejectReasons");
                });
#pragma warning restore 612, 618
        }
    }
}
