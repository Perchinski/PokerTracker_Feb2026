using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedLocationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "City", "ImageUrl", "IsActive", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "3600 S Las Vegas Blvd", "Las Vegas", "https://images.unsplash.com/photo-1590059530490-25251624c478?w=800", true, false, "Bellagio Casino" },
                    { 2, "123 Main Street", "New York", "https://images.unsplash.com/photo-1596484552834-6a58f850e0a1?w=800", true, false, "Downtown Poker Club" },
                    { 0, "No address", "No city", null, false, false, "Dummy Location" }
                });

            migrationBuilder.Sql("UPDATE Tournaments SET LocationId = 0 WHERE LocationId IS NULL");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.UpdateData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 2,
                column: "LocationId",
                value: 2);

            migrationBuilder.Sql("DELETE FROM Locations WHERE Id = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Tournaments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cba0abc1-cd41-45a1-9e85-96dafd4afde7", "AQAAAAIAAYagAAAAENClDR/lJOcqWyRZ9T2a0BWz9hpo0bQldLqZV2v6zPBUbnC8i1QHaSeyc52yKVrsqQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0108a345-dbda-49dd-9f4b-77e9bd0a319a", "AQAAAAIAAYagAAAAEB75V9ls6QezZxYNkVrEPZQCqh1qd3MxTn1UkHj9V8JLROiuuZlogdYZcA7/vc0zeQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fc3f2e94-1308-4926-972b-32c99898f574", "AQAAAAIAAYagAAAAENaXCFwgC8UgaxoBCdjstppRHMWRcuu6GPfAAdDDQrLsfHKjLMI7NN4U042jxxk6wg==" });

            migrationBuilder.UpdateData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: 2,
                column: "LocationId",
                value: null);
        }
    }
}
