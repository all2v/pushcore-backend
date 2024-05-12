using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Account.Migrations
{
    /// <inheritdoc />
    public partial class AddedRequisites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequisiteBank",
                schema: "Account",
                table: "UsersProfiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequisiteNumberPhone",
                schema: "Account",
                table: "UsersProfiles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequisiteBank",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.DropColumn(
                name: "RequisiteNumberPhone",
                schema: "Account",
                table: "UsersProfiles");
        }
    }
}
