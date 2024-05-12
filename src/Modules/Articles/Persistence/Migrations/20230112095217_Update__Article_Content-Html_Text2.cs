using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Articles.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArticleContentHtmlText2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content_Text",
                schema: "Articles",
                table: "Articles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(9437184)",
                oldMaxLength: 9437184);

            migrationBuilder.AlterColumn<string>(
                name: "Content_Html",
                schema: "Articles",
                table: "Articles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(9437184)",
                oldMaxLength: 9437184,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content_Text",
                schema: "Articles",
                table: "Articles",
                type: "character varying(9437184)",
                maxLength: 9437184,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Content_Html",
                schema: "Articles",
                table: "Articles",
                type: "character varying(9437184)",
                maxLength: 9437184,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
