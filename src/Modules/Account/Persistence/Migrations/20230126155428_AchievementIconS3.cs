using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Account.Migrations
{
    /// <inheritdoc />
    public partial class AchievementIconS3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconUrl",
                schema: "Account",
                table: "Achievements");

            migrationBuilder.AddColumn<Guid>(
                name: "IconId",
                schema: "Account",
                table: "Achievements",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconId",
                schema: "Account",
                table: "Achievements");

            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                schema: "Account",
                table: "Achievements",
                type: "text",
                nullable: true);
        }
    }
}
