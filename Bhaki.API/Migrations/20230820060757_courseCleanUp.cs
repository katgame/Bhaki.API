using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bhaki.API.Migrations
{
    public partial class courseCleanUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalDescription",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Firearm",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Course");

            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                table: "Course",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Course");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalDescription",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Firearm",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
