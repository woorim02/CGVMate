using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CgvMate.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descritpion",
                table: "BannedIPs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descritpion",
                table: "BannedIPs");
        }
    }
}
