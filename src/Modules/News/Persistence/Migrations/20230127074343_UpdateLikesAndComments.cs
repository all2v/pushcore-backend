using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.News.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLikesAndComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From",
                schema: "News",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "News",
                table: "Comments");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                schema: "News",
                table: "Comments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                schema: "News",
                table: "Comments",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                schema: "News",
                table: "Comments",
                column: "PostId",
                principalSchema: "News",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                schema: "News",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                schema: "News",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PostId",
                schema: "News",
                table: "Comments");

            migrationBuilder.AddColumn<Guid>(
                name: "From",
                schema: "News",
                table: "Likes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                schema: "News",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
