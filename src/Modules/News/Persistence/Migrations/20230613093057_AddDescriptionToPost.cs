using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.News.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "News",
                table: "Posts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "News",
                table: "Posts");
        }
    }
}
