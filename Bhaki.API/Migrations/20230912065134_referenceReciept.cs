using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bhaki.API.Migrations
{
    /// <inheritdoc />
    public partial class referenceReciept : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecieptReference",
                table: "Registration",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecieptReference",
                table: "Registration");
        }
    }
}
