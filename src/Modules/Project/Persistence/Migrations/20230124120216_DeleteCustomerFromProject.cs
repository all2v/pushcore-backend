using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Projects.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCustomerFromProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Customer_CustomerId",
                schema: "Project",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CustomerId",
                schema: "Project",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "Project",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                schema: "Project",
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
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "Project",
                table: "Projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                schema: "Project",
                table: "AuditHistory",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CustomerId",
                schema: "Project",
                table: "Projects",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Customer_CustomerId",
                schema: "Project",
                table: "Projects",
                column: "CustomerId",
                principalSchema: "Project",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
