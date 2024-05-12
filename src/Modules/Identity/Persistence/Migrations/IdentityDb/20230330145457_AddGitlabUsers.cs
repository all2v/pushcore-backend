using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Astrum.Identity.Migrations.IdentityDb
{
    /// <inheritdoc />
    public partial class AddGitlabUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GitlabUsers",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitlabUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GitlabMappings_GitlabUserId",
                schema: "Identity",
                table: "GitlabMappings",
                column: "GitlabUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GitlabMappings_GitlabUsers_GitlabUserId",
                schema: "Identity",
                table: "GitlabMappings",
                column: "GitlabUserId",
                principalSchema: "Identity",
                principalTable: "GitlabUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GitlabMappings_GitlabUsers_GitlabUserId",
                schema: "Identity",
                table: "GitlabMappings");

            migrationBuilder.DropTable(
                name: "GitlabUsers",
                schema: "Identity");

            migrationBuilder.DropIndex(
                name: "IX_GitlabMappings_GitlabUserId",
                schema: "Identity",
                table: "GitlabMappings");
        }
    }
}
