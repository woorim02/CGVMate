using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CgvMate.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Boards` (
                    `Id` varchar(255) NOT NULL,
                    `Title` varchar(100) NOT NULL,
                    `Description` longtext NOT NULL,
                    PRIMARY KEY (`Id`)
                ) CHARSET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `CgvCuponEvents` (
                    `EventId` varchar(255) NOT NULL,
                    `StartDateTime` datetime(6) NOT NULL,
                    `EventName` longtext NOT NULL,
                    `ImageSource` longtext NOT NULL,
                    `Period` longtext NOT NULL,
                    PRIMARY KEY (`EventId`)
                ) CHARSET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `CgvGiveawayEvents` (
                    `EventIndex` varchar(255) NOT NULL,
                    `Title` longtext NOT NULL,
                    `Period` longtext NOT NULL,
                    `DDay` longtext NOT NULL,
                    `Views` int NOT NULL,
                    PRIMARY KEY (`EventIndex`)
                ) CHARSET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `LotteEvents` (
                    `EventID` varchar(255) NOT NULL,
                    `EventName` longtext NOT NULL,
                    `EventClassificationCode` longtext NOT NULL,
                    `EventTypeCode` longtext NOT NULL,
                    `EventTypeName` longtext NOT NULL,
                    `ProgressStartDate` longtext NOT NULL,
                    `ProgressEndDate` longtext NOT NULL,
                    `ImageUrl` longtext NOT NULL,
                    `ImageDivisionCode` int NOT NULL,
                    `CinemaID` longtext NOT NULL,
                    `CinemaName` longtext NOT NULL,
                    `CinemaAreaCode` longtext NOT NULL,
                    `CinemaAreaName` longtext NOT NULL,
                    `DevTemplateYN` int NOT NULL,
                    `CloseNearYN` int NOT NULL,
                    `RemainsDayCount` int NOT NULL,
                    `EventWinnerYN` int NOT NULL,
                    `EventSeq` int NOT NULL,
                    `EventCntnt` longtext NOT NULL,
                    `Views` int NOT NULL,
                    PRIMARY KEY (`EventID`)
                ) CHARSET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `LotteGiveawayEventKeywords` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Keyword` varchar(255) NOT NULL,
                    PRIMARY KEY (`Id`),
                    UNIQUE KEY `IX_LotteGiveawayEventKeywords_Keyword` (`Keyword`)
                ) CHARSET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `LotteGiveawayEventModels` (
                    `EventID` varchar(255) NOT NULL,
                    `FrGiftID` longtext NOT NULL,
                    `FrGiftNm` longtext NOT NULL,
                    `HasNext` tinyint(1) NOT NULL,
                    PRIMARY KEY (`EventID`)
                ) CHARSET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `MegaboxCuponEvents` (
                    `EventNo` varchar(255) NOT NULL,
                    `StartDateTime` datetime(6) NOT NULL,
                    `Title` longtext NOT NULL,
                    `Date` longtext NOT NULL,
                    `ImageUrl` longtext NOT NULL,
                    PRIMARY KEY (`EventNo`)
                ) CHARSET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `MegaboxGiveawayEvents` (
                    `ID` varchar(255) NOT NULL,
                    `Title` longtext NOT NULL,
                    `ViewCount` int NOT NULL,
                    PRIMARY KEY (`ID`)
                ) CHARSET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Users` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `UserName` varchar(100) NOT NULL,
                    `Email` varchar(255) NOT NULL,
                    `PasswordHash` longtext NOT NULL,
                    `PasswordSalt` longtext NOT NULL,
                    `Roles` longtext NOT NULL,
                    PRIMARY KEY (`Id`),
                    UNIQUE KEY `IX_Users_Email` (`Email`)
                ) CHARSET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Posts` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `No` int NOT NULL,
                    `BoardId` varchar(255) NOT NULL,
                    `WriterIP` longtext,
                    `WriterName` longtext,
                    `WriterPasswordHash` longtext,
                    `WriterPasswordSalt` longtext,
                    `UserId` int NOT NULL,
                    `Title` varchar(100) NOT NULL,
                    `Content` longtext NOT NULL,
                    `DateCreated` datetime(6) NOT NULL,
                    `ViewCount` int NOT NULL,
                    `Upvote` int NOT NULL,
                    `Downvote` int NOT NULL,
                    PRIMARY KEY (`Id`),
                    KEY `FK_Posts_Boards_BoardId` (`BoardId`),
                    KEY `FK_Posts_Users_UserId` (`UserId`),
                    UNIQUE KEY `IX_Posts_BoardId_No` (`BoardId`,`No`)
                ) CHARSET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Comments` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `PostId` int NOT NULL,
                    `ParentCommentId` int NOT NULL,
                    `WriterIP` longtext,
                    `WriterName` longtext,
                    `WriterPasswordHash` longtext,
                    `WriterPasswordSalt` longtext,
                    `UserId` int NOT NULL,
                    `Content` longtext NOT NULL,
                    `DateCreated` datetime(6) NOT NULL,
                    PRIMARY KEY (`Id`),
                    KEY `FK_Comments_Comments_ParentCommentId` (`ParentCommentId`),
                    KEY `FK_Comments_Posts_PostId` (`PostId`),
                    KEY `FK_Comments_Users_UserId` (`UserId`)
                ) CHARSET=utf8mb4;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CgvCuponEvents");

            migrationBuilder.DropTable(
                name: "CgvGiveawayEvents");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "LotteEvents");

            migrationBuilder.DropTable(
                name: "LotteGiveawayEventKeywords");

            migrationBuilder.DropTable(
                name: "LotteGiveawayEventModels");

            migrationBuilder.DropTable(
                name: "MegaboxCuponEvents");

            migrationBuilder.DropTable(
                name: "MegaboxGiveawayEvents");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
