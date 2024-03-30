﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShoeCatalog.DataModels.Data;

#nullable disable

namespace ShoeCatalog.DataAccess.Data.Migrations
{
    [DbContext(typeof(ShoeDbContext))]
    [Migration("20230928140414_FirstMigrationCompelte")]
    partial class FirstMigrationCompelte
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ShoeCatalog.DataAccess.Models.Brand", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("ShoeCatalog.DataAccess.Models.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ShoeCatalog.DataAccess.Models.Shoe", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BrandId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("ImageFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Size")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("Shoes");
                });

            modelBuilder.Entity("ShoeCatalog.DataAccess.Models.ShoeCategory", b =>
                {
                    b.Property<string>("ShoeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ShoeId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ShoeCategories");
                });

            modelBuilder.Entity("ShoeCatalog.DataAccess.Models.Shoe", b =>
                {
                    b.HasOne("ShoeCatalog.DataAccess.Models.Brand", "Brand")
                        .WithMany("Shoes")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("ShoeCatalog.DataAccess.Models.ShoeCategory", b =>
                {
                    b.HasOne("ShoeCatalog.DataAccess.Models.Category", "Category")
                        .WithMany("ShoeCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShoeCatalog.DataAccess.Models.Shoe", "Shoe")
                        .WithMany("ShoeCategories")
                        .HasForeignKey("ShoeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Shoe");
                });

            modelBuilder.Entity("ShoeCatalog.DataAccess.Models.Brand", b =>
                {
                    b.Navigation("Shoes");
                });

            modelBuilder.Entity("ShoeCatalog.DataAccess.Models.Category", b =>
                {
                    b.Navigation("ShoeCategories");
                });

            modelBuilder.Entity("ShoeCatalog.DataAccess.Models.Shoe", b =>
                {
                    b.Navigation("ShoeCategories");
                });
#pragma warning restore 612, 618
        }
    }
}