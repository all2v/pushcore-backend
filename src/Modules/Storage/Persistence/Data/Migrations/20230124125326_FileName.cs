using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Storage.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class FileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OriginalName",
                schema: "Storage",
                table: "Files",
                newName: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                schema: "Storage",
                table: "Files",
                newName: "OriginalName");
        }
    }
}
