using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Account.Migrations
{
    /// <inheritdoc />
    public partial class AvatarAndCoverImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "AvatarImageId",
                schema: "Account",
                table: "UsersProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CoverImageId",
                schema: "Account",
                table: "UsersProfiles",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarImageId",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.DropColumn(
                name: "CoverImageId",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                schema: "Account",
                table: "UsersProfiles",
                type: "text",
                nullable: true);
        }
    }
}
