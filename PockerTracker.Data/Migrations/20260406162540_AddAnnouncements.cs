using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "259420ed-8a24-4af8-85dc-a09604f2a09e", "AQAAAAIAAYagAAAAEKdcR2eWkpqt/65tZickXDp5skkOPiDRN0bgEqzXC3+aEgaALfQI5rHm4ZhfJQV9GA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e882bcf1-2102-4e3f-ae96-8ec7068f9020", "AQAAAAIAAYagAAAAEPAvJe0NiArdEWOJ1UVl49ujfXpGBYyVGhFx2wN7+ojuZt7cN/aa2Y/+nChhbKqiXA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4a65949e-c916-4a59-937f-d7a9783a9c94", "AQAAAAIAAYagAAAAEAzRULhUZtj0uKoWFUEvmFcd62GxWaPVNTHdmlFdRMP5IuFFX9mMI0/BXn6WdPOU+w==" });
        }
    }
}
