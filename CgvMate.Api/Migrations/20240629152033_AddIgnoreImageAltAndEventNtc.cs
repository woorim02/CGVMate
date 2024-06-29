using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CgvMate.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIgnoreImageAltAndEventNtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventNtc",
                table: "LotteEvents");

            migrationBuilder.DropColumn(
                name: "ImageAlt",
                table: "LotteEvents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventNtc",
                table: "LotteEvents",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImageAlt",
                table: "LotteEvents",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
