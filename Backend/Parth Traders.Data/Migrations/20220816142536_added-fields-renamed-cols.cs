using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parth_Traders.Data.Migrations
{
    public partial class addedfieldsrenamedcols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityPerUnit",
                table: "Products",
                newName: "PiecesPerUnit");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetails",
                newName: "QuantityPurchased");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderDetails",
                newName: "PricePerPiece");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierName",
                table: "Suppliers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<long>(
                name: "GrandTotal",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierName",
                table: "Suppliers",
                column: "SupplierName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Suppliers_SupplierName",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "GrandTotal",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "PiecesPerUnit",
                table: "Products",
                newName: "QuantityPerUnit");

            migrationBuilder.RenameColumn(
                name: "QuantityPurchased",
                table: "OrderDetails",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "PricePerPiece",
                table: "OrderDetails",
                newName: "Price");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierName",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
