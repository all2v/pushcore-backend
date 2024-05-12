using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Appeal.Migrations
{
    /// <inheritdoc />
    public partial class JoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Appeals_AppealId",
                schema: "Appeal",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_AppealId",
                schema: "Appeal",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "AppealId",
                schema: "Appeal",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "AppealAppealCategory",
                schema: "Appeal",
                columns: table => new
                {
                    AppealsId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppealAppealCategory", x => new { x.AppealsId, x.CategoriesId });
                    table.ForeignKey(
                        name: "FK_AppealAppealCategory_Appeals_AppealsId",
                        column: x => x.AppealsId,
                        principalSchema: "Appeal",
                        principalTable: "Appeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppealAppealCategory_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalSchema: "Appeal",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppealAppealCategory_CategoriesId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                column: "CategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppealAppealCategory",
                schema: "Appeal");

            migrationBuilder.AddColumn<Guid>(
                name: "AppealId",
                schema: "Appeal",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_AppealId",
                schema: "Appeal",
                table: "Categories",
                column: "AppealId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Appeals_AppealId",
                schema: "Appeal",
                table: "Categories",
                column: "AppealId",
                principalSchema: "Appeal",
                principalTable: "Appeals",
                principalColumn: "Id");
        }
    }
}
