using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.TrackerProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExternalUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "TrackerProject",
                table: "ExternalUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "TrackerProject",
                table: "ExternalUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "TrackerProject",
                table: "ExternalUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "TrackerProject",
                table: "ExternalUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
