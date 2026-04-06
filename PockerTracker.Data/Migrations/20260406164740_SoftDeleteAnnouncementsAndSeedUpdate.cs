using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteAnnouncementsAndSeedUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Announcements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e04ae5ce-4417-427f-8806-8060797258fc", "AQAAAAIAAYagAAAAEKQ6Cbt/FQUYjh1OyJbkrEadUAuIDBhqaKzd7R4N3BhMwEPN0aI0mlSOJGVYJtfkCg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b2b84bcb-a012-417f-bc78-cf43d2336256", "AQAAAAIAAYagAAAAEG5ck2Qv4/iVGAJt5e+fVW33We/u5l5bfWo7GFstGMy8jYJP8k0kI/SEKWXE6ibkoA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "42e4cca7-7800-4b1a-88ba-57f1b321cba1", "AQAAAAIAAYagAAAAEHKDKg4YFUIenDJDuLsF4HHHc5dAt/Fm2EyomeJi/9ChpaiZLt7rBusXNnN++IpY3Q==" });

            migrationBuilder.UpdateData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2027, 3, 14, 20, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Announcements");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5acb1583-e456-4b46-9a8e-66400087fd11", "AQAAAAIAAYagAAAAEKdH7jsd4obzjFyE21MfNGdGHiAukuOmuVa043KFT6HoeNVJkO0ofxP8VPowdHVduA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1b437c88-05a7-4299-8f80-a8f1be295ee4", "AQAAAAIAAYagAAAAEPRyXMGp/aDMGQEmT2D3wqHC7iBvOJo5tDWsMgDrDuZapy80g+YqniJ3N+JbS4GDpQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9c18cec2-1db2-4c82-9046-3ae008e1247c", "AQAAAAIAAYagAAAAEKBnypWpaxctCjN/QhVa21CsyVEoc7Mrajf4Ny3DY9MNP1NUwcZ2XFmrBPR74mHc4A==" });

            migrationBuilder.UpdateData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2026, 3, 14, 20, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
