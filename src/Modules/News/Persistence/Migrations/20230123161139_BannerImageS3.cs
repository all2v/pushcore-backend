using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.News.Migrations
{
    /// <inheritdoc />
    public partial class BannerImageS3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureS3Link",
                schema: "News",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "PictureS3Link",
                schema: "News",
                table: "Banners");

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                schema: "News",
                table: "Widgets",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                schema: "News",
                table: "Banners",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureId",
                schema: "News",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "PictureId",
                schema: "News",
                table: "Banners");

            migrationBuilder.AddColumn<string>(
                name: "PictureS3Link",
                schema: "News",
                table: "Widgets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PictureS3Link",
                schema: "News",
                table: "Banners",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
