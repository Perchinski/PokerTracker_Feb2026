using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ccec79b0-754d-4d64-bb1e-8395df2af01d", "AQAAAAIAAYagAAAAEJlx3LCq3uzCMmBqA/chB6RamrTRJG4HpNhCe8BvTIzVDUmeg42wUCwLXJId72oH2Q==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1e3f4940-8223-4210-ad5d-1c414d1ad58c", "AQAAAAIAAYagAAAAEAL0uRVyLKmoBj5J7D9O4rSCE3av5/BkawEvgn6YDCi0jpH8Vs1GdWbP5DYiVeK/dw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-4f7e-9d8c-1a2b3c4d5e6f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7bf164a0-3245-4f74-b521-9ed514ca92a0", "AQAAAAIAAYagAAAAEIlBKMdTUqEDdFb3FnpFpMYa7tWbke8SosocOpJZ3MJEAUeSipgjcz8uOXIw3WXMDw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
