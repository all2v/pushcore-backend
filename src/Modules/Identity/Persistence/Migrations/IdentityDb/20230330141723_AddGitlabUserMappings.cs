using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Identity.Migrations.IdentityDb
{
    /// <inheritdoc />
    public partial class AddGitlabUserMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                schema: "Identity",
                table: "AuditHistory",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.CreateTable(
                name: "GitlabMappings",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    GitlabUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitlabMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GitlabMappings_Users_UserID",
                        column: x => x.UserID,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GitlabMappings_UserID",
                schema: "Identity",
                table: "GitlabMappings",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GitlabMappings",
                schema: "Identity");

            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                schema: "Identity",
                table: "AuditHistory",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
