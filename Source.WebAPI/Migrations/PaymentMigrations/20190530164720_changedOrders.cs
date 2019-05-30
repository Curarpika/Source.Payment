using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Source.WebAPI.Migrations.PaymentMigrations
{
    public partial class changedOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "PaymentOrders",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "PaymentOrders");
        }
    }
}
