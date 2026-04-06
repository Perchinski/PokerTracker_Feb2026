using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAnnouncement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Content", "IsActive", "IsDeleted", "PublishedOn", "Title" },
                values: new object[] { 1, "We are thrilled to launch the new version of our internal poker tracking system. You can now easily create tournaments, manage players, and keep track of your local rankings.\n\nMore features like leaderboards and advanced statistics will be coming soon!", true, false, new DateTime(2026, 4, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Welcome to the New Poker Tracker!" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "34f68282-5102-43e9-9916-74c776c1edcb", "AQAAAAIAAYagAAAAEO4JdZWgVhIUghHbkLtw6nK3OyF/CExoCkJ4lVZlzz+jxKru12fqIlyGZAZYR1T5AQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2f9de94f-ca11-4c0b-9486-ca7588b1c863", "AQAAAAIAAYagAAAAEGzeT0sGCaCT9s+2kJbTrxZ2g/dZ60xDKTO+Hn0e7TYpbFlwlR6wJxwFfpABFMJsbg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a461cf15-12d2-4274-977f-95e0d518aa4a", "AQAAAAIAAYagAAAAEPZRUFkuG9xXPVrmKY+Ve2J844RuuMZ69sg48AET0olY6txcrZKAmhWswABgIu/C0w==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Announcements",
                keyColumn: "Id",
                keyValue: 1);

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
        }
    }
}
