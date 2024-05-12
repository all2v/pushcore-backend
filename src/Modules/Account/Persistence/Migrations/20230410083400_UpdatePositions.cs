using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Account.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                schema: "Account",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Position",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.RenameTable(
                name: "Accounts",
                schema: "Account",
                newName: "Account",
                newSchema: "Account");

            migrationBuilder.AddColumn<Guid>(
                name: "PositionId",
                schema: "Account",
                table: "UsersProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                schema: "Account",
                table: "Account",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersProfiles_PositionId",
                schema: "Account",
                table: "UsersProfiles",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersProfiles_Positions_PositionId",
                schema: "Account",
                table: "UsersProfiles",
                column: "PositionId",
                principalSchema: "Account",
                principalTable: "Positions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersProfiles_Positions_PositionId",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UsersProfiles_PositionId",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                schema: "Account",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "PositionId",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "Account",
                newName: "Accounts",
                newSchema: "Account");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                schema: "Account",
                table: "UsersProfiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                schema: "Account",
                table: "Accounts",
                column: "Id");
        }
    }
}
