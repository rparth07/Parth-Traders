using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parth_Traders.Data.Migrations
{
    public partial class renamedorderdetailtablefields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "08668c52-dbbb-4e2b-bccb-296a5d75a980");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e856b174-d7e8-409f-a6af-e018db088851");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "OrderDetails",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "QuantityPurchased",
                table: "OrderDetails",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "PricePerPiece",
                table: "OrderDetails",
                newName: "Price");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8f72fba8-51f1-4316-b54a-f73e62d4a2d9", "d5b020c1-ad65-4a8d-bacc-2781761178e0", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c0b2294b-6e24-4e40-9d33-e13eb1bfd891", "2f0650bc-571f-457a-9f25-4904c5c1f36b", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f72fba8-51f1-4316-b54a-f73e62d4a2d9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c0b2294b-6e24-4e40-9d33-e13eb1bfd891");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "OrderDetails",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetails",
                newName: "QuantityPurchased");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderDetails",
                newName: "PricePerPiece");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "08668c52-dbbb-4e2b-bccb-296a5d75a980", "fb3f6db1-4824-4f11-aa4d-d715c47e4786", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e856b174-d7e8-409f-a6af-e018db088851", "83b2b2ca-3475-4c5e-b346-3b5db9eaa2a1", "Administrator", "ADMINISTRATOR" });
        }
    }
}
