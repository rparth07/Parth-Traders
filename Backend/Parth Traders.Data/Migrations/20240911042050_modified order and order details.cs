using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parth_Traders.Data.Migrations
{
    public partial class modifiedorderandorderdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2e8d494-40f6-4978-ade6-5975f191df30");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5fc9551-f90b-4791-ad10-2ac88fa75967");

            migrationBuilder.DropColumn(
                name: "BillDate",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "37809fbf-11ce-4ef0-b1fd-e876a9b3973a", "27a15aea-64b4-4023-9d88-5e5c99ebaa54", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ca7a6be9-228e-4885-bb7f-c3ec7e0af840", "947baee5-f9b0-4b97-91fa-dbf949f9b9f7", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37809fbf-11ce-4ef0-b1fd-e876a9b3973a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca7a6be9-228e-4885-bb7f-c3ec7e0af840");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Orders");

            migrationBuilder.AddColumn<DateTime>(
                name: "BillDate",
                table: "OrderDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a2e8d494-40f6-4978-ade6-5975f191df30", "d332ce86-a8af-4cba-8616-36ae96eb7a48", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a5fc9551-f90b-4791-ad10-2ac88fa75967", "9794a61b-e33c-4e94-b995-ee05298b2200", "Administrator", "ADMINISTRATOR" });
        }
    }
}
