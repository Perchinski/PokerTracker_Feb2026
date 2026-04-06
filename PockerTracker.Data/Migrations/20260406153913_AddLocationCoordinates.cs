using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 36.1126, -115.1767 });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 40.712800000000001, -74.006 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e3b93edc-5129-4b0b-9ff5-6caece2ef53d", "AQAAAAIAAYagAAAAEKuAXYuCXV2nMLxJlUGgn5p1lJr6BtqbNW61TvChAKy3M6pTcSCTu5Z7inGl4xbcvw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5565c233-b822-41df-a1af-e92e4604d895", "AQAAAAIAAYagAAAAEK8jAuKmNhThPZVXaL2Ou+fIx021P6GWAYY0AFQE0R8iC8dpF+k+uEXkTB4YxAy3wg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b7356b33-0f1e-4be9-97ec-b8cb4b100033", "AQAAAAIAAYagAAAAEP52fhgPl2wBwLjV0+VhZvlwc6n+t4DpuFWoa18T9JqfcXdJtzu4YcwB2+U5dLCCxA==" });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { null, null });
        }
    }
}
