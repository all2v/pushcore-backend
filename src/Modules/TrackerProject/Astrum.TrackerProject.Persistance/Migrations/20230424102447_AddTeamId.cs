using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.TrackerProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Projects_ProjectId",
                schema: "TrackerProject",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_ProjectId",
                schema: "TrackerProject",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "TeamId",
                schema: "TrackerProject",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "TeamId",
                schema: "TrackerProject",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TeamId1",
                schema: "TrackerProject",
                table: "Projects");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ProjectId",
                schema: "TrackerProject",
                table: "Teams",
                column: "ProjectId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Projects_ProjectId",
                schema: "TrackerProject",
                table: "Teams",
                column: "ProjectId",
                principalSchema: "TrackerProject",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
