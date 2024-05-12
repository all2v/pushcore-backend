using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Articles.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditHistoryChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                schema: "Articles",
                table: "AuditHistory",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                schema: "Articles",
                table: "AuditHistory",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
