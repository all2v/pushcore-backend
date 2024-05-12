using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Articles.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverUrl",
                schema: "Articles",
                table: "Articles");

            migrationBuilder.AddColumn<Guid>(
                name: "CoverImageId",
                schema: "Articles",
                table: "Articles",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageId",
                schema: "Articles",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                schema: "Articles",
                table: "Articles",
                type: "text",
                nullable: true);
        }
    }
}
