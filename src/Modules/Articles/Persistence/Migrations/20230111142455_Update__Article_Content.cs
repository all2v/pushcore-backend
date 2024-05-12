using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Articles.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArticleContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                schema: "Articles",
                table: "Articles",
                newName: "Content_Text");

            migrationBuilder.AddColumn<string>(
                name: "Content_Html",
                schema: "Articles",
                table: "Articles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content_Html",
                schema: "Articles",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "Content_Text",
                schema: "Articles",
                table: "Articles",
                newName: "Content");
        }
    }
}
