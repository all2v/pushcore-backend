using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Storage.Persistence.Data.Migrations.Storage
{
    /// <inheritdoc />
    public partial class RemoveBucketName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BucketName",
                schema: "Storage",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BucketName",
                schema: "Storage",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
