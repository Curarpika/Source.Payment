using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Source.WebAPI.Migrations.Product
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    OrderType = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Snapshot = table.Column<string>(nullable: true),
                    BuyerId = table.Column<Guid>(nullable: true),
                    BuyerIdentity = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    CreditAmount = table.Column<decimal>(nullable: true),
                    PaymentOrderId = table.Column<Guid>(nullable: true),
                    OrderState = table.Column<int>(nullable: false),
                    ExcuteAddress = table.Column<string>(nullable: true),
                    ExcuteDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    ProductType = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Spec = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: true),
                    Credit = table.Column<int>(nullable: true),
                    Discount = table.Column<double>(nullable: false),
                    InStock = table.Column<int>(nullable: true),
                    SupplierId = table.Column<Guid>(nullable: true),
                    Cover = table.Column<byte[]>(nullable: true),
                    Images = table.Column<string>(nullable: true),
                    SupplierIdentity = table.Column<string>(nullable: true),
                    IsAvailable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductOrders");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
