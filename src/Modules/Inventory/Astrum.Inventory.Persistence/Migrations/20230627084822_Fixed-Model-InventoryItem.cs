using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Inventory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedModelInventoryItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateName",
                schema: "Inventories",
                table: "InventoryItems");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                schema: "Inventories",
                table: "InventoryItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateId",
                schema: "Inventories",
                table: "InventoryItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_TemplateId",
                schema: "Inventories",
                table: "InventoryItems",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Templates_TemplateId",
                schema: "Inventories",
                table: "InventoryItems",
                column: "TemplateId",
                principalSchema: "Inventories",
                principalTable: "Templates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Templates_TemplateId",
                schema: "Inventories",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_TemplateId",
                schema: "Inventories",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Index",
                schema: "Inventories",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                schema: "Inventories",
                table: "InventoryItems");

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                schema: "Inventories",
                table: "InventoryItems",
                type: "text",
                nullable: true);
        }
    }
}
