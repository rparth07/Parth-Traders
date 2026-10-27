using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parth_Traders.Data.Migrations
{
    public partial class addedratingandskufieldinproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c617cb89-6dba-4e65-a646-c311a8c29b8b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f532dcf0-64d9-40f3-adad-1307506f73b3");

            migrationBuilder.AddColumn<int>(
                name: "ProductRating",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductSku",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a2e8d494-40f6-4978-ade6-5975f191df30", "d332ce86-a8af-4cba-8616-36ae96eb7a48", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a5fc9551-f90b-4791-ad10-2ac88fa75967", "9794a61b-e33c-4e94-b995-ee05298b2200", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductSku",
                table: "Products",
                column: "ProductSku",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductSku",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2e8d494-40f6-4978-ade6-5975f191df30");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5fc9551-f90b-4791-ad10-2ac88fa75967");

            migrationBuilder.DropColumn(
                name: "ProductRating",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductSku",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c617cb89-6dba-4e65-a646-c311a8c29b8b", "8a2a9a39-a9d3-4299-ab0e-68683e854805", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f532dcf0-64d9-40f3-adad-1307506f73b3", "5646f8c9-4f4b-4e75-85c8-baa35377614a", "Administrator", "ADMINISTRATOR" });
        }
    }
}
