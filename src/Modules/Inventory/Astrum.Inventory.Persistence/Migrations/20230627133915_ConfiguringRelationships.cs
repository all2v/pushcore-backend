using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Inventory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguringRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Templates_TemplateId",
                schema: "Inventories",
                table: "InventoryItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "TemplateId",
                schema: "Inventories",
                table: "InventoryItems",
                type: "uuid",
                nullable: true,
                defaultValue: null,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Templates_TemplateId",
                schema: "Inventories",
                table: "InventoryItems",
                column: "TemplateId",
                principalSchema: "Inventories",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Templates_TemplateId",
                schema: "Inventories",
                table: "InventoryItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "TemplateId",
                schema: "Inventories",
                table: "InventoryItems",
                type: "uuid",
                nullable: true,
                defaultValue: null, 
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Templates_TemplateId",
                schema: "Inventories",
                table: "InventoryItems",
                column: "TemplateId",
                principalSchema: "Inventories",
                principalTable: "Templates",
                principalColumn: "Id");
        }
    }
}
