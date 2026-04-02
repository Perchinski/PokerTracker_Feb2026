using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationWithImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Locations",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Locations");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3a1a5412-9001-4cf1-9598-28e2c416b5e4", "AQAAAAIAAYagAAAAEIQ3VLcVQVEoNg6PnSXDcpHSYMFXz/1xtyQ2Skb89UrZp3ctoxYdEOprxQ9zzJNU3A==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "572d30a2-0c05-47e0-9acf-ad929bc88721", "AQAAAAIAAYagAAAAENTQtkyMlUiSkL5pY3tqhOcdozvkN50Vtjbu3fSvmO/0fYHx6+Rye9Ow72cMdQ9evA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "160132c3-51db-47d3-92cf-80010b4f3cca", "AQAAAAIAAYagAAAAELFkaUzycP/YnWyuZvdvq3VGXJhMpjF4zoGSVJ2Wl3xD5y2vT+HqFVB7OHDZWBhxlA==" });
        }
    }
}
