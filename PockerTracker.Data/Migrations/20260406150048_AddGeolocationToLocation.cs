using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGeolocationToLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Locations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Locations",
                type: "float",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Locations");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a65cbb5f-a8bd-48be-a5f4-52f8cb12395f", "AQAAAAIAAYagAAAAEDsYVxRfJ1f/T4Psb/rKlvh21lkkWuQcZSPdCjq+5U7PWSUDQq2NuLpe+HfgFViWZA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "87ae5aa5-f8ad-4f5c-a928-bdd4afc58cac", "AQAAAAIAAYagAAAAEKZ7n7gqxn3jm889/qFN7TYlY0dZ4gvWWzmOMlyyYGzmBZme1SsyawiMO8lX8OpLsg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "44be5c45-91b8-4766-bd62-82fee15ac946", "AQAAAAIAAYagAAAAEFETYREGeimwQLIJglHnTSYTIGgqCxrvY1fgzjKju54bcWvxcDUTocNP1NtVHyCDEQ==" });
        }
    }
}
