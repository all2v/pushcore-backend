using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Account.Migrations
{
    /// <inheritdoc />
    public partial class AddVkToSocialNetworks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VK",
                schema: "Account",
                table: "SocialNetworks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VK",
                schema: "Account",
                table: "SocialNetworks");
        }
    }
}
