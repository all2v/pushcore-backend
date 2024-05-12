using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.TrackerProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ProjectNotMapped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Projects_ProjectId",
                schema: "TrackerProject",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ProjectId",
                schema: "TrackerProject",
                table: "Articles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Articles_ProjectId",
                schema: "TrackerProject",
                table: "Articles",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Projects_ProjectId",
                schema: "TrackerProject",
                table: "Articles",
                column: "ProjectId",
                principalSchema: "TrackerProject",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
