using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Inventory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedInventoryItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                schema: "Inventories",
                table: "InventoryItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "Inventories",
                table: "InventoryItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
