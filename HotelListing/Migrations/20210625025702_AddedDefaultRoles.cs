using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2cd0bade-fdb6-4f43-8b9a-f8d213d9b824", "638c1b80-7714-49ac-b4a1-382c83bdbe78", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "57857da1-ec9e-4dd7-a263-80dcbccf6555", "5fcbfbf5-a784-4f76-a3c9-a37a12dcc1ea", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2cd0bade-fdb6-4f43-8b9a-f8d213d9b824");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57857da1-ec9e-4dd7-a263-80dcbccf6555");
        }
    }
}
