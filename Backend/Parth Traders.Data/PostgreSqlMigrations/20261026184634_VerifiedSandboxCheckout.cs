using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parth_Traders.Data.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class VerifiedSandboxCheckout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentDrafts",
                columns: table => new
                {
                    PayPalOrderId = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false),
                    ItemsJson = table.Column<string>(type: "text", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionId = table.Column<string>(type: "text", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDrafts", x => x.PayPalOrderId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TransactionId",
                table: "Orders",
                column: "TransactionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDrafts");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TransactionId",
                table: "Orders");
        }
    }
}
