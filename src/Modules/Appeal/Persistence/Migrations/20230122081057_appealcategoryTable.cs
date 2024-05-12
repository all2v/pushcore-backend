using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Appeal.Migrations
{
    /// <inheritdoc />
    public partial class appealcategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppealAppealCategory_Appeals_AppealsId",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_AppealAppealCategory_Categories_CategoriesId",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppealAppealCategory",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                newName: "AppealId");

            migrationBuilder.RenameColumn(
                name: "AppealsId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                newName: "AppealCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AppealAppealCategory_CategoriesId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                newName: "IX_AppealAppealCategory_AppealId");

            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                schema: "Appeal",
                table: "AuditHistory",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "Appeal",
                table: "AppealAppealCategory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Appeal",
                table: "AppealAppealCategory",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateCreated",
                schema: "Appeal",
                table: "AppealAppealCategory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDeleted",
                schema: "Appeal",
                table: "AppealAppealCategory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateModified",
                schema: "Appeal",
                table: "AppealAppealCategory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Appeal",
                table: "AppealAppealCategory",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "Appeal",
                table: "AppealAppealCategory",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "Appeal",
                table: "AppealAppealCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppealAppealCategory",
                schema: "Appeal",
                table: "AppealAppealCategory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppealAppealCategory_AppealCategoryId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                column: "AppealCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppealAppealCategory_Appeals_AppealId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                column: "AppealId",
                principalSchema: "Appeal",
                principalTable: "Appeals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppealAppealCategory_Categories_AppealCategoryId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                column: "AppealCategoryId",
                principalSchema: "Appeal",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppealAppealCategory_Appeals_AppealId",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_AppealAppealCategory_Categories_AppealCategoryId",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppealAppealCategory",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropIndex(
                name: "IX_AppealAppealCategory_AppealCategoryId",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropColumn(
                name: "DateModified",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "Appeal",
                table: "AppealAppealCategory");

            migrationBuilder.RenameColumn(
                name: "AppealId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                newName: "CategoriesId");

            migrationBuilder.RenameColumn(
                name: "AppealCategoryId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                newName: "AppealsId");

            migrationBuilder.RenameIndex(
                name: "IX_AppealAppealCategory_AppealId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                newName: "IX_AppealAppealCategory_CategoriesId");

            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                schema: "Appeal",
                table: "AuditHistory",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppealAppealCategory",
                schema: "Appeal",
                table: "AppealAppealCategory",
                columns: new[] { "AppealsId", "CategoriesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppealAppealCategory_Appeals_AppealsId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                column: "AppealsId",
                principalSchema: "Appeal",
                principalTable: "Appeals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppealAppealCategory_Categories_CategoriesId",
                schema: "Appeal",
                table: "AppealAppealCategory",
                column: "CategoriesId",
                principalSchema: "Appeal",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
