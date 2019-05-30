using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Source.WebAPI.Migrations.PaymentMigrations
{
    public partial class ChangedPaymentOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.CreateTable(
                name: "PaymentOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    OrderType = table.Column<int>(nullable: false),
                    PayMethod = table.Column<int>(nullable: false),
                    OrderState = table.Column<int>(nullable: false),
                    PaymentOrderId = table.Column<string>(nullable: true),
                    PaidTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOrders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentOrders");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    OrderType = table.Column<int>(nullable: false),
                    PaidTime = table.Column<DateTime>(nullable: true),
                    PayMethod = table.Column<int>(nullable: false),
                    PaymentOrderId = table.Column<string>(nullable: true),
                    Processed = table.Column<bool>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }
    }
}
