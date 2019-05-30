using Microsoft.EntityFrameworkCore.Migrations;

namespace Source.WebAPI.Migrations.PaymentMigrations
{
    public partial class ChangedPaymentOrder1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PaymentOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PaymentOrders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PaymentOrders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PaymentOrders");
        }
    }
}
