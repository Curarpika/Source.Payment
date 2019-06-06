﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Source.Product.Models;

namespace Source.WebAPI.Migrations.Product
{
    [DbContext(typeof(ProductDbContext))]
    [Migration("20190606132851_InitDb")]
    partial class InitDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Source.Product.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Cover");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int?>("Credit");

                    b.Property<double>("Discount");

                    b.Property<string>("Images");

                    b.Property<int?>("InStock");

                    b.Property<bool>("IsAvailable");

                    b.Property<string>("Name");

                    b.Property<int?>("Price");

                    b.Property<int?>("ProductType");

                    b.Property<string>("Spec");

                    b.Property<Guid?>("SupplierId");

                    b.Property<string>("SupplierIdentity");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Unit");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Source.Product.Models.ProductOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("Amount");

                    b.Property<Guid?>("BuyerId");

                    b.Property<string>("BuyerIdentity");

                    b.Property<string>("Contact");

                    b.Property<string>("Content");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<decimal?>("CreditAmount");

                    b.Property<string>("ExcuteAddress");

                    b.Property<DateTime?>("ExecuteDateTime");

                    b.Property<DateTime?>("ExecutedDatedTime");

                    b.Property<int>("OrderState");

                    b.Property<int?>("OrderType");

                    b.Property<Guid?>("PaymentOrderId");

                    b.Property<int?>("Quantity");

                    b.Property<string>("Snapshot");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.ToTable("ProductOrders");
                });
#pragma warning restore 612, 618
        }
    }
}