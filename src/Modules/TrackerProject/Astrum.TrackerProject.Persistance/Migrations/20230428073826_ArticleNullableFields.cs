using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.TrackerProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ArticleNullableFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Articles_ParentId",
                schema: "TrackerProject",
                table: "Articles");

            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                schema: "TrackerProject",
                table: "Articles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Articles_ParentId",
                schema: "TrackerProject",
                table: "Articles",
                column: "ParentId",
                principalSchema: "TrackerProject",
                principalTable: "Articles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Articles_ParentId",
                schema: "TrackerProject",
                table: "Articles");

            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                schema: "TrackerProject",
                table: "Articles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Articles_ParentId",
                schema: "TrackerProject",
                table: "Articles",
                column: "ParentId",
                principalSchema: "TrackerProject",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
