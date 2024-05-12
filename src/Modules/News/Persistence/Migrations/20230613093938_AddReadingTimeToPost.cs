using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.News.Migrations
{
    /// <inheritdoc />
    public partial class AddReadingTimeToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReadingTime",
                schema: "News",
                table: "Posts",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadingTime",
                schema: "News",
                table: "Posts");
        }
    }
}
