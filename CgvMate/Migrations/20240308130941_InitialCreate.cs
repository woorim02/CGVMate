using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CgvMate.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    AreaCode = table.Column<string>(type: "TEXT", nullable: false),
                    AreaName = table.Column<string>(type: "TEXT", nullable: false),
                    IsGiveawayAreaCode = table.Column<bool>(type: "INTEGER", nullable: false),
                    TheaterCount = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.AreaCode);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Index = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailSource = table.Column<string>(type: "TEXT", nullable: false),
                    MovieGroupCd = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "TheaterGiveawayInfos",
                columns: table => new
                {
                    GiveawayIndex = table.Column<string>(type: "TEXT", nullable: false),
                    TheaterCode = table.Column<string>(type: "TEXT", nullable: false),
                    TheaterName = table.Column<string>(type: "TEXT", nullable: false),
                    CountTypeCode = table.Column<string>(type: "TEXT", nullable: false),
                    EncCount = table.Column<string>(type: "TEXT", nullable: false),
                    GiveawayName = table.Column<string>(type: "TEXT", nullable: false),
                    GiveawayRemainCount = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiveTypeCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheaterGiveawayInfos", x => new { x.TheaterCode, x.GiveawayIndex });
                });

            migrationBuilder.CreateTable(
                name: "Theaters",
                columns: table => new
                {
                    TheaterCode = table.Column<string>(type: "TEXT", nullable: false),
                    TheaterName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theaters", x => x.TheaterCode);
                });

            migrationBuilder.CreateTable(
                name: "OpenNotificationInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MovieIndex = table.Column<string>(type: "TEXT", nullable: true),
                    ScreenType = table.Column<string>(type: "TEXT", nullable: false),
                    TheaterCode = table.Column<string>(type: "TEXT", nullable: false),
                    GiveawayIndex = table.Column<string>(type: "TEXT", nullable: false),
                    TheaterGiveawayInfoTheaterCode = table.Column<string>(type: "TEXT", nullable: false),
                    TheaterGiveawayInfoGiveawayIndex = table.Column<string>(type: "TEXT", nullable: false),
                    TargetDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    IsOpen = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanReservation = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenNotificationInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenNotificationInfos_Movies_MovieIndex",
                        column: x => x.MovieIndex,
                        principalTable: "Movies",
                        principalColumn: "Index");
                    table.ForeignKey(
                        name: "FK_OpenNotificationInfos_TheaterGiveawayInfos_TheaterGiveawayInfoTheaterCode_TheaterGiveawayInfoGiveawayIndex",
                        columns: x => new { x.TheaterGiveawayInfoTheaterCode, x.TheaterGiveawayInfoGiveawayIndex },
                        principalTable: "TheaterGiveawayInfos",
                        principalColumns: new[] { "TheaterCode", "GiveawayIndex" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpenNotificationInfos_MovieIndex",
                table: "OpenNotificationInfos",
                column: "MovieIndex");

            migrationBuilder.CreateIndex(
                name: "IX_OpenNotificationInfos_TheaterGiveawayInfoTheaterCode_TheaterGiveawayInfoGiveawayIndex",
                table: "OpenNotificationInfos",
                columns: new[] { "TheaterGiveawayInfoTheaterCode", "TheaterGiveawayInfoGiveawayIndex" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "OpenNotificationInfos");

            migrationBuilder.DropTable(
                name: "Theaters");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "TheaterGiveawayInfos");
        }
    }
}
