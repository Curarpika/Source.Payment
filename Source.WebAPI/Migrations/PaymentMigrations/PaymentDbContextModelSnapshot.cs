﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Source.Payment.Models;

namespace Source.WebAPI.Migrations.PaymentMigrations
{
    [DbContext(typeof(PaymentDbContext))]
    partial class PaymentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Source.Payment.PaymentOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("Content");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int>("OrderState");

                    b.Property<int>("OrderType");

                    b.Property<DateTime?>("PaidTime");

                    b.Property<int>("PayMethod");

                    b.Property<string>("PaymentOrderId");

                    b.Property<int>("Quantity");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedTime");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("PaymentOrders");
                });
#pragma warning restore 612, 618
        }
    }
}
