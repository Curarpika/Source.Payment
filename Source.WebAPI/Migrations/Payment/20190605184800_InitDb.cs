using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Source.WebAPI.Migrations.Payment
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    OrderId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
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
        }
    }
}
