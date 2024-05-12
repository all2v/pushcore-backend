using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.TrackerProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class IssueId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueComments_Issues_IssueId",
                schema: "TrackerProject",
                table: "IssueComments");

            migrationBuilder.AlterColumn<string>(
                name: "IssueId",
                schema: "TrackerProject",
                table: "IssueComments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueComments_Issues_IssueId",
                schema: "TrackerProject",
                table: "IssueComments",
                column: "IssueId",
                principalSchema: "TrackerProject",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueComments_Issues_IssueId",
                schema: "TrackerProject",
                table: "IssueComments");

            migrationBuilder.AlterColumn<string>(
                name: "IssueId",
                schema: "TrackerProject",
                table: "IssueComments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueComments_Issues_IssueId",
                schema: "TrackerProject",
                table: "IssueComments",
                column: "IssueId",
                principalSchema: "TrackerProject",
                principalTable: "Issues",
                principalColumn: "Id");
        }
    }
}
