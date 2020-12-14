﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace BunningsProductCatalog.Repository.Migrations
{
  [DbContext(typeof(BunningsProductCatalogContext))]
    partial class BunningsProductCatalogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BunningsProductCatalog.Domain.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompanyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            CompanyId = 1,
                            CompanyCode = "A",
                            CompanyName = "Company A"
                        },
                        new
                        {
                            CompanyId = 2,
                            CompanyCode = "B",
                            CompanyName = "Company B"
                        });
                });

            modelBuilder.Entity("BunningsProductCatalog.Domain.Models.CompanyProduct", b =>
                {
                    b.Property<int>("CompanyProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("ProductSku")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("CompanyProductId");

                    b.HasIndex("CompanyId");

                    b.ToTable("CompanyProducts");
                });

            modelBuilder.Entity("BunningsProductCatalog.Domain.Models.CompanyProductBarcode", b =>
                {
                    b.Property<int>("CompanyProductBarcodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("CompanyProductId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.HasKey("CompanyProductBarcodeId");

                    b.HasIndex("CompanyProductId");

                    b.HasIndex("SupplierId");

                    b.ToTable("CompanyProductBarcodes");
                });

            modelBuilder.Entity("BunningsProductCatalog.Domain.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("SupplierCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.HasKey("SupplierId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("BunningsProductCatalog.Domain.Models.CompanyProduct", b =>
                {
                    b.HasOne("BunningsProductCatalog.Domain.Models.Company", "Company")
                        .WithMany("CompanyProducts")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("BunningsProductCatalog.Domain.Models.CompanyProductBarcode", b =>
                {
                    b.HasOne("BunningsProductCatalog.Domain.Models.CompanyProduct", "CompanyProduct")
                        .WithMany("CompanyProductBarcodes")
                        .HasForeignKey("CompanyProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BunningsProductCatalog.Domain.Models.Supplier", "Supplier")
                        .WithMany("CompanyProductBarcode")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("BunningsProductCatalog.Domain.Models.Supplier", b =>
                {
                    b.HasOne("BunningsProductCatalog.Domain.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
