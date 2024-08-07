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
    [Migration("20201217123301_SortenDate")]
    partial class SortenDate
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

                    b.Property<string>("ChildIdNo")
                        .HasColumnType("NVARCHAR2(13)")
                        .HasMaxLength(13);

                    b.Property<string>("DateStamp")
                        .HasColumnType("NVARCHAR2(30)")
                        .HasMaxLength(30);

                    b.Property<int>("DocImageId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("IdNo")
                        .HasColumnType("NVARCHAR2(13)")
                        .HasMaxLength(13);

                    b.Property<string>("Reference")
                        .HasColumnType("NVARCHAR2(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Status")
                        .HasColumnType("NVARCHAR2(2000)");

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
#pragma warning restore 612, 618
        }
    }
}
