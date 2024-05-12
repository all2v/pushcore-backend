using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Projects.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOnDeletingMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Projects_ProjectId",
                schema: "Project",
                table: "Member");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Projects_ProjectId",
                schema: "Project",
                table: "Member",
                column: "ProjectId",
                principalSchema: "Project",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Projects_ProjectId",
                schema: "Project",
                table: "Member");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Projects_ProjectId",
                schema: "Project",
                table: "Member",
                column: "ProjectId",
                principalSchema: "Project",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
