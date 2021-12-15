using Microsoft.EntityFrameworkCore.Migrations;

namespace AmazingProject.Migrations
{
    public partial class AddUsernameAndPass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "people",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "people",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "people",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "people");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "people");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "people");
        }
    }
}
