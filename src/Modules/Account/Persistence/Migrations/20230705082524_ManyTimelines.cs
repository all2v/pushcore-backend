using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Account.Migrations
{
    /// <inheritdoc />
    public partial class ManyTimelines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersProfiles_Timelines_TimelineId",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UsersProfiles_TimelineId",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.DropColumn(
                name: "TimelineId",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                schema: "Account",
                table: "Timelines",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timelines_UserProfileId",
                schema: "Account",
                table: "Timelines",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timelines_UsersProfiles_UserProfileId",
                schema: "Account",
                table: "Timelines",
                column: "UserProfileId",
                principalSchema: "Account",
                principalTable: "UsersProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timelines_UsersProfiles_UserProfileId",
                schema: "Account",
                table: "Timelines");

            migrationBuilder.DropIndex(
                name: "IX_Timelines_UserProfileId",
                schema: "Account",
                table: "Timelines");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                schema: "Account",
                table: "Timelines");

            migrationBuilder.AddColumn<Guid>(
                name: "TimelineId",
                schema: "Account",
                table: "UsersProfiles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UsersProfiles_TimelineId",
                schema: "Account",
                table: "UsersProfiles",
                column: "TimelineId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersProfiles_Timelines_TimelineId",
                schema: "Account",
                table: "UsersProfiles",
                column: "TimelineId",
                principalSchema: "Account",
                principalTable: "Timelines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
