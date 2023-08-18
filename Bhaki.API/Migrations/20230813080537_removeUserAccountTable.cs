using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bhaki.API.Migrations
{
    public partial class removeUserAccountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_UserAccount_UserAccountId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropIndex(
                name: "IX_Student_UserAccountId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Student");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Registration",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "OutstandingAmount",
                table: "Registration",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PaidAmount",
                table: "Registration",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Registration",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Registration");

            migrationBuilder.DropColumn(
                name: "OutstandingAmount",
                table: "Registration");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Registration");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Registration");

            migrationBuilder.AddColumn<Guid>(
                name: "UserAccountId",
                table: "Student",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OutstandingAmount = table.Column<double>(type: "float", nullable: false),
                    PaidAmount = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_UserAccountId",
                table: "Student",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_UserAccount_UserAccountId",
                table: "Student",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
