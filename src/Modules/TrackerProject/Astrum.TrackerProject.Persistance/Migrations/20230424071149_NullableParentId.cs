using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.TrackerProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class NullableParentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Issues_ParentId",
                schema: "TrackerProject",
                table: "Issues");

            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                schema: "TrackerProject",
                table: "Issues",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Issues_ParentId",
                schema: "TrackerProject",
                table: "Issues",
                column: "ParentId",
                principalSchema: "TrackerProject",
                principalTable: "Issues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Issues_ParentId",
                schema: "TrackerProject",
                table: "Issues");

            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                schema: "TrackerProject",
                table: "Issues",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Issues_ParentId",
                schema: "TrackerProject",
                table: "Issues",
                column: "ParentId",
                principalSchema: "TrackerProject",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
