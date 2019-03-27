﻿// <auto-generated />
using System;
using DatabasePerTenantPOC.Data.CatalogDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatabasePerTenantPOC.Migrations.CatalogDb
{
    [DbContext(typeof(CatalogDbContext))]
    [Migration("20190319192923_update name1")]
    partial class updatename1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DatabasePerTenantPOC.Data.CatalogDB.Databases", b =>
                {
                    b.Property<string>("ServerName")
                        .HasMaxLength(128);

                    b.Property<string>("DatabaseName")
                        .HasMaxLength(128);

                    b.Property<string>("ElasticPoolName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("RecoveryState")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("ServiceObjective")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("ServerName", "DatabaseName");

                    b.ToTable("Databases");
                });

            modelBuilder.Entity("DatabasePerTenantPOC.Data.CatalogDB.ElasticPools", b =>
                {
                    b.Property<string>("ServerName");

                    b.Property<string>("ElasticPoolName");

                    b.Property<int>("DatabaseDtuMax");

                    b.Property<int>("DatabaseDtuMin");

                    b.Property<int>("Dtu");

                    b.Property<string>("Edition")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("RecoveryState")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("StorageMB");

                    b.HasKey("ServerName", "ElasticPoolName");

                    b.ToTable("ElasticPools");
                });

            modelBuilder.Entity("DatabasePerTenantPOC.Data.CatalogDB.Servers", b =>
                {
                    b.Property<string>("ServerName")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("RecoveryState")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("ServerName");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("DatabasePerTenantPOC.Data.CatalogDB.Tenants", b =>
                {
                    b.Property<byte[]>("TenantId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("RecoveryState")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("ServicePlan")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("'standard'")
                        .HasMaxLength(30);

                    b.Property<string>("TenantName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("TenantId")
                        .HasName("PK__Tenants__2E9B47E15565CFCB");

                    b.HasIndex("TenantName")
                        .HasName("IX_Tenants_TenantName");

                    b.ToTable("Tenants");
                });
#pragma warning restore 612, 618
        }
    }
}
