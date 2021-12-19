using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmazingProject.Migrations
{
    public partial class addingCreatedPropToModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "people",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "KnownAs",
                table: "people",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "people");

            migrationBuilder.DropColumn(
                name: "KnownAs",
                table: "people");
        }
    }
}
