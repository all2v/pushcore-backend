using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Projects.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CoverId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverUrl",
                schema: "Project",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "CoverImageId",
                schema: "Project",
                table: "Products",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageId",
                schema: "Project",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                schema: "Project",
                table: "Products",
                type: "text",
                nullable: true);
        }
    }
}
