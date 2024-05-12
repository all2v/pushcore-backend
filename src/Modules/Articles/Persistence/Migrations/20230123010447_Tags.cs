using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Articles.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Tags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Articles_ArticleId",
                schema: "Articles",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ArticleId",
                schema: "Articles",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                schema: "Articles",
                table: "Tags");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "Articles",
                table: "Tags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                schema: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "Articles",
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "Articles",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CategoryId",
                schema: "Articles",
                table: "Tags",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_ArticleId",
                schema: "Articles",
                table: "ArticleTag",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_TagId",
                schema: "Articles",
                table: "ArticleTag",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Categories_CategoryId",
                schema: "Articles",
                table: "Tags",
                column: "CategoryId",
                principalSchema: "Articles",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Categories_CategoryId",
                schema: "Articles",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "ArticleTag",
                schema: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Tags_CategoryId",
                schema: "Articles",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "Articles",
                table: "Tags");

            migrationBuilder.AddColumn<Guid>(
                name: "ArticleId",
                schema: "Articles",
                table: "Tags",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ArticleId",
                schema: "Articles",
                table: "Tags",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Articles_ArticleId",
                schema: "Articles",
                table: "Tags",
                column: "ArticleId",
                principalSchema: "Articles",
                principalTable: "Articles",
                principalColumn: "Id");
        }
    }
}
