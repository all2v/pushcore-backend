using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Market.Migrations
{
    /// <inheritdoc />
    public partial class BasketProductChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_MarketBasket_BasketId",
                schema: "Market",
                table: "BasketProducts");

            migrationBuilder.DropIndex(
                name: "IX_BasketProducts_BasketId",
                schema: "Market",
                table: "BasketProducts");

            migrationBuilder.AddColumn<Guid>(
                name: "MarketBasketId",
                schema: "Market",
                table: "BasketProducts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                schema: "Market",
                table: "AuditHistory",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.CreateIndex(
                name: "IX_BasketProducts_MarketBasketId",
                schema: "Market",
                table: "BasketProducts",
                column: "MarketBasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_MarketBasket_MarketBasketId",
                schema: "Market",
                table: "BasketProducts",
                column: "MarketBasketId",
                principalSchema: "Market",
                principalTable: "MarketBasket",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_MarketBasket_MarketBasketId",
                schema: "Market",
                table: "BasketProducts");

            migrationBuilder.DropIndex(
                name: "IX_BasketProducts_MarketBasketId",
                schema: "Market",
                table: "BasketProducts");

            migrationBuilder.DropColumn(
                name: "MarketBasketId",
                schema: "Market",
                table: "BasketProducts");

            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                schema: "Market",
                table: "AuditHistory",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_BasketProducts_BasketId",
                schema: "Market",
                table: "BasketProducts",
                column: "BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_MarketBasket_BasketId",
                schema: "Market",
                table: "BasketProducts",
                column: "BasketId",
                principalSchema: "Market",
                principalTable: "MarketBasket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
