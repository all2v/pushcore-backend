using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Recruting.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddApproveField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                schema: "Recruting",
                table: "PracticeRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                schema: "Recruting",
                table: "PracticeRequests");
        }
    }
}
