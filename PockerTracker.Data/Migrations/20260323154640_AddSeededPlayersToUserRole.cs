using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeededPlayersToUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "7D9B7113-A8F8-4035-99A7-A20DD400F6A3", "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d" },
                    { "7D9B7113-A8F8-4035-99A7-A20DD400F6A3", "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "91d1780a-ce7c-4220-b37f-e0014c2ee1d8", "AQAAAAIAAYagAAAAEK1MQ11FoEcChnvHNqAMMTklYwKq9G8Uxj7WOvPLzlIOFyoVZU0p3kDxSrR4uicahQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d11eda47-a6f6-42d1-be16-30f69d81cb95", "AQAAAAIAAYagAAAAEDo/WrAGC7TXMRimGIEdOE0SoxxCt3VI5K6iCQvF+EcXvQy5n3/Fvl+JqFewO1Rymw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "54299bc8-a36f-47e6-b7bc-3cadc84d83da", "AQAAAAIAAYagAAAAENFJ9D4W8qRKdqDU3MDjvunUk59rbOIkBtuANSoNmL7AzLwiq3t5MUfFwm/xpnUE2w==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7D9B7113-A8F8-4035-99A7-A20DD400F6A3", "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7D9B7113-A8F8-4035-99A7-A20DD400F6A3", "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7e41abec-c481-42ed-a5ae-a62f2a27adc8", "AQAAAAIAAYagAAAAEA7FMWC+tJLIYyV+OtYZbvuJrpg+aEn0nLL8xBqBNTiHBSQF0DG8ZJSoJaUcuoHFmA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9ee08ec3-5da9-4ec6-96a6-f65b7d4bdef7", "AQAAAAIAAYagAAAAEDktosoKBwrnc7xAjX8mTrIJDc9RsvqEWDaovMJhLTfb6MxUe1mIgI7Z4N2qh+8gmg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f62e1609-d3d2-4a47-9707-c08cae426a36", "AQAAAAIAAYagAAAAEOMgOZMWvhgsyKvNmCcKsHxWCOw8WNh0ZF+Zi9iBWcfx5lynrVZkNx17hHTNYQTKyg==" });
        }
    }
}
