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
    [Migration("20210107084043_InternalDoc")]
    partial class InternalDoc
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

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
                        .HasColumnType("NVARCHAR2(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR2(30)")
                        .HasMaxLength(30);

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
                        .HasColumnType("BLOB")
                        .HasMaxLength(400000);

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
                        .HasColumnType("NVARCHAR2(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ChildIdNo")
                        .HasColumnType("NVARCHAR2(13)")
                        .HasMaxLength(13);

                    b.Property<string>("DateStamp")
                        .HasColumnType("NVARCHAR2(30)")
                        .HasMaxLength(30);

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("FileName")
                        .HasColumnType("NVARCHAR2(100)")
                        .HasMaxLength(100);

                    b.Property<string>("IdNo")
                        .HasColumnType("NVARCHAR2(13)")
                        .HasMaxLength(13);

                    b.Property<bool>("InternalDocument")
                        .HasColumnType("NUMBER(1)");

                    b.Property<string>("Reference")
                        .HasColumnType("NVARCHAR2(20)")
                        .HasMaxLength(20);

                    b.Property<string>("RegionCode")
                        .HasColumnType("NVARCHAR2(13)")
                        .HasMaxLength(13);

                    b.Property<string>("RejectReason")
                        .HasColumnType("NVARCHAR2(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Status")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("SupportDocument")
                        .HasColumnType("NVARCHAR2(20)")
                        .HasMaxLength(20);

                    b.Property<string>("User")
                        .HasColumnType("NVARCHAR2(30)")
                        .HasMaxLength(30);

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
                        .HasColumnType("NVARCHAR2(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR2(30)")
                        .HasMaxLength(30);

                    b.HasKey("DocumentTypeId");

                    b.ToTable("DocumentTypes");
                });

            modelBuilder.Entity("Sassa.eDocs.Data.Models.RejectReason", b =>
                {
                    b.Property<int>("RejectReasonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Reason")
                        .HasColumnType("NVARCHAR2(100)")
                        .HasMaxLength(100);

                    b.HasKey("RejectReasonId");

                    b.ToTable("RejectReasons");
                });
#pragma warning restore 612, 618
        }
    }
}
