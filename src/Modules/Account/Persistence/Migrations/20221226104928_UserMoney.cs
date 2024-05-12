using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Account.Migrations
{
    /// <inheritdoc />
    public partial class UserMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Money",
                schema: "Account",
                table: "UsersProfiles",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Money",
                schema: "Account",
                table: "UsersProfiles");
        }
    }
}
