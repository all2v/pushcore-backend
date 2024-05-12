using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.TrackerProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RenameName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortName",
                schema: "TrackerProject",
                table: "Projects",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "TrackerProject",
                table: "Projects",
                newName: "ShortName");
        }
    }
}
