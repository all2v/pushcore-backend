using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.TrackerProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class CustomFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueTags",
                schema: "TrackerProject");

            migrationBuilder.AddColumn<string>(
                name: "AssigneeId",
                schema: "TrackerProject",
                table: "Issues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                schema: "TrackerProject",
                table: "Issues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "TrackerProject",
                table: "Issues",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_AssigneeId",
                schema: "TrackerProject",
                table: "Issues",
                column: "AssigneeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_ExternalUsers_AssigneeId",
                schema: "TrackerProject",
                table: "Issues",
                column: "AssigneeId",
                principalSchema: "TrackerProject",
                principalTable: "ExternalUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_ExternalUsers_AssigneeId",
                schema: "TrackerProject",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_AssigneeId",
                schema: "TrackerProject",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "AssigneeId",
                schema: "TrackerProject",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "Priority",
                schema: "TrackerProject",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "TrackerProject",
                table: "Issues");

            migrationBuilder.CreateTable(
                name: "IssueTags",
                schema: "TrackerProject",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IssueId = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueTags_Issues_IssueId",
                        column: x => x.IssueId,
                        principalSchema: "TrackerProject",
                        principalTable: "Issues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssueTags_IssueId",
                schema: "TrackerProject",
                table: "IssueTags",
                column: "IssueId");
        }
    }
}
