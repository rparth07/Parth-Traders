using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parth_Traders.Data.Migrations
{
    public partial class modifiedratingdatatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37809fbf-11ce-4ef0-b1fd-e876a9b3973a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca7a6be9-228e-4885-bb7f-c3ec7e0af840");

            migrationBuilder.AlterColumn<double>(
                name: "ProductRating",
                table: "Products",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8ee518ab-ea93-4717-b2b5-b91a27eb7bab", "e7fcd958-1476-4f68-b5e5-4641dae2a19f", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9e775af8-0046-480b-a089-890b71eea770", "c6ac895b-89d3-4606-bedd-b5c51aab4c7c", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ee518ab-ea93-4717-b2b5-b91a27eb7bab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e775af8-0046-480b-a089-890b71eea770");

            migrationBuilder.AlterColumn<int>(
                name: "ProductRating",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "37809fbf-11ce-4ef0-b1fd-e876a9b3973a", "27a15aea-64b4-4023-9d88-5e5c99ebaa54", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ca7a6be9-228e-4885-bb7f-c3ec7e0af840", "947baee5-f9b0-4b97-91fa-dbf949f9b9f7", "Administrator", "ADMINISTRATOR" });
        }
    }
}
