using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Tournaments",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d", 0, "11a7daca-28ac-48e7-91bc-d8f3f70ac341", "player1@pokertracker.com", true, false, null, "PLAYER1@POKERTRACKER.COM", "PLAYER1@POKERTRACKER.COM", "AQAAAAIAAYagAAAAEO2f5THhBdWgjHxujBvqfZtFJPocBrEXZNCVd+CMoYblAh7Jag4JWcdWox1aHANGIg==", null, false, "STATIC-SECURITY-STAMP-P1", false, "player1@pokertracker.com" },
                    { "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f", 0, "7ea10f76-1065-43ad-a73e-dbe48dff938b", "player2@pokertracker.com", true, false, null, "PLAYER2@POKERTRACKER.COM", "PLAYER2@POKERTRACKER.COM", "AQAAAAIAAYagAAAAEOpIlWxOHUy1wthNueSweO7rcMaa2iyEvkpymi3RcRhscdLIftMTOzWWqmJ99tQ/MQ==", null, false, "STATIC-SECURITY-STAMP-P2", false, "player2@pokertracker.com" }
                });

            migrationBuilder.InsertData(
                table: "Tournaments",
                columns: new[] { "Id", "CreatorId", "Date", "Description", "FormatId", "ImageUrl", "IsDeleted", "Name", "Status", "WinnerId" },
                values: new object[,]
                {
                    { 1, "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d", new DateTime(2026, 3, 14, 20, 0, 0, 0, DateTimeKind.Unspecified), "Casual no-limit game. $20 buy-in, winner takes all!", 1, "https://images.unsplash.com/photo-1511193311914-0346f16efe90?w=800", false, "Friday Night Holdem", 0, null },
                    { 2, "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f", new DateTime(2026, 2, 8, 19, 0, 0, 0, DateTimeKind.Unspecified), "Knockout format — collect a bounty for every player you eliminate.", 6, "https://images.unsplash.com/photo-1609902726285-00668009f004?w=800", false, "Weekend Bounty Bash", 2, "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d" }
                });

            migrationBuilder.InsertData(
                table: "PlayersTournaments",
                columns: new[] { "PlayerId", "TournamentId" },
                values: new object[,]
                {
                    { "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d", 1 },
                    { "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d", 2 },
                    { "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f", 1 },
                    { "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlayersTournaments",
                keyColumns: new[] { "PlayerId", "TournamentId" },
                keyValues: new object[] { "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d", 1 });

            migrationBuilder.DeleteData(
                table: "PlayersTournaments",
                keyColumns: new[] { "PlayerId", "TournamentId" },
                keyValues: new object[] { "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d", 2 });

            migrationBuilder.DeleteData(
                table: "PlayersTournaments",
                keyColumns: new[] { "PlayerId", "TournamentId" },
                keyValues: new object[] { "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f", 1 });

            migrationBuilder.DeleteData(
                table: "PlayersTournaments",
                keyColumns: new[] { "PlayerId", "TournamentId" },
                keyValues: new object[] { "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f", 2 });

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true);
        }
    }
}
