using Microsoft.EntityFrameworkCore.Migrations;

namespace Source.WebAPI.Migrations.BaseAuthMigrations
{
    public partial class ModifyBaseUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Base_User_CertNo",
                schema: "Security",
                table: "Base_User");

            migrationBuilder.DropColumn(
                name: "CertNo",
                schema: "Security",
                table: "Base_User");

            migrationBuilder.DropColumn(
                name: "DeptCode",
                schema: "Security",
                table: "Base_User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertNo",
                schema: "Security",
                table: "Base_User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeptCode",
                schema: "Security",
                table: "Base_User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Base_User_CertNo",
                schema: "Security",
                table: "Base_User",
                column: "CertNo",
                unique: true);
        }
    }
}
