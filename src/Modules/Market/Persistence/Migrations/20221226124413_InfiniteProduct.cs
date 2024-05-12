using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Market.Migrations
{
    /// <inheritdoc />
    public partial class InfiniteProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInfinite",
                schema: "Market",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInfinite",
                schema: "Market",
                table: "Products");
        }
    }
}
