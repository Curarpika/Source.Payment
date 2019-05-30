using Microsoft.EntityFrameworkCore.Migrations;

namespace Source.WebAPI.Migrations.BaseAuthMigrations
{
    public partial class changedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Credit",
                schema: "Security",
                table: "Base_User",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ExternalName",
                schema: "Security",
                table: "Base_User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalType",
                schema: "Security",
                table: "Base_User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                schema: "Security",
                table: "Base_User");

            migrationBuilder.DropColumn(
                name: "ExternalName",
                schema: "Security",
                table: "Base_User");

            migrationBuilder.DropColumn(
                name: "ExternalType",
                schema: "Security",
                table: "Base_User");
        }
    }
}
