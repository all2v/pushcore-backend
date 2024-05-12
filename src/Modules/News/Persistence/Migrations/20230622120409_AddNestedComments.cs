using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.News.Migrations
{
    /// <inheritdoc />
    public partial class AddNestedComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReplyCommentId",
                schema: "News",
                table: "Comments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReplyCommentId",
                schema: "News",
                table: "Comments",
                column: "ReplyCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ReplyCommentId",
                schema: "News",
                table: "Comments",
                column: "ReplyCommentId",
                principalSchema: "News",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ReplyCommentId",
                schema: "News",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ReplyCommentId",
                schema: "News",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReplyCommentId",
                schema: "News",
                table: "Comments");
        }
    }
}
