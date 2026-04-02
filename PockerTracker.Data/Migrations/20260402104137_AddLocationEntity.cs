using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Tournaments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_LocationId",
                table: "Tournaments",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Locations_LocationId",
                table: "Tournaments",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Locations_LocationId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_LocationId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Tournaments");

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
    }
}
