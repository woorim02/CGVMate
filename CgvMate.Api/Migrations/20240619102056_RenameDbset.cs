using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CgvMate.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameDbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GiveawayEvents",
                table: "GiveawayEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "GiveawayEvents",
                newName: "CgvGiveawayEvents");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "LotteEvents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CgvGiveawayEvents",
                table: "CgvGiveawayEvents",
                column: "EventIndex");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotteEvents",
                table: "LotteEvents",
                column: "EventID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LotteEvents",
                table: "LotteEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CgvGiveawayEvents",
                table: "CgvGiveawayEvents");

            migrationBuilder.RenameTable(
                name: "LotteEvents",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "CgvGiveawayEvents",
                newName: "GiveawayEvents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "EventID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiveawayEvents",
                table: "GiveawayEvents",
                column: "EventIndex");
        }
    }
}
