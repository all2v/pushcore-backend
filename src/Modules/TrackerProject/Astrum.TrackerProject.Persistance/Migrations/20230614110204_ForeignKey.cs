using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.TrackerProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Teams_TeamId1",
                schema: "TrackerProject",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_TeamId1",
                schema: "TrackerProject",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TeamId1",
                schema: "TrackerProject",
                table: "Projects");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TeamId",
                schema: "TrackerProject",
                table: "Projects",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Teams_TeamId",
                schema: "TrackerProject",
                table: "Projects",
                column: "TeamId",
                principalSchema: "TrackerProject",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Teams_TeamId",
                schema: "TrackerProject",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_TeamId",
                schema: "TrackerProject",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "TeamId1",
                schema: "TrackerProject",
                table: "Projects",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TeamId1",
                schema: "TrackerProject",
                table: "Projects",
                column: "TeamId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Teams_TeamId1",
                schema: "TrackerProject",
                table: "Projects",
                column: "TeamId1",
                principalSchema: "TrackerProject",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
