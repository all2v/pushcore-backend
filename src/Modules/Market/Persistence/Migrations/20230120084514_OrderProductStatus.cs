using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Market.Migrations
{
    /// <inheritdoc />
    public partial class OrderProductStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                schema: "Market",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_OrderId",
                schema: "Market",
                table: "OrderProducts");

            migrationBuilder.AddColumn<Guid>(
                name: "MarketOrderId",
                schema: "Market",
                table: "OrderProducts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Market",
                table: "OrderProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_MarketOrderId",
                schema: "Market",
                table: "OrderProducts",
                column: "MarketOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_MarketOrderId",
                schema: "Market",
                table: "OrderProducts",
                column: "MarketOrderId",
                principalSchema: "Market",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_MarketOrderId",
                schema: "Market",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_MarketOrderId",
                schema: "Market",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "MarketOrderId",
                schema: "Market",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Market",
                table: "OrderProducts");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId",
                schema: "Market",
                table: "OrderProducts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                schema: "Market",
                table: "OrderProducts",
                column: "OrderId",
                principalSchema: "Market",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
