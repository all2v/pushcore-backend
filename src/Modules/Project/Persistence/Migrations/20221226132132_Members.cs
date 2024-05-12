using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Projects.Migrations
{
    /// <inheritdoc />
    public partial class Members : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Manager_ManagerId",
                schema: "Project",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Products_ProductId1",
                schema: "Project",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Manager",
                schema: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ManagerId",
                schema: "Project",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProductId1",
                schema: "Project",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                schema: "Project",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Members",
                schema: "Project",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                schema: "Project",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "Member",
                schema: "Project",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsManager = table.Column<bool>(type: "boolean", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Member_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "Project",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Member_ProjectId",
                schema: "Project",
                table: "Member",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Member",
                schema: "Project");

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                schema: "Project",
                table: "Projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<List<Guid>>(
                name: "Members",
                schema: "Project",
                table: "Projects",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                schema: "Project",
                table: "Projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Manager",
                schema: "Project",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ManagerId",
                schema: "Project",
                table: "Projects",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProductId1",
                schema: "Project",
                table: "Projects",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Manager_ManagerId",
                schema: "Project",
                table: "Projects",
                column: "ManagerId",
                principalSchema: "Project",
                principalTable: "Manager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Products_ProductId1",
                schema: "Project",
                table: "Projects",
                column: "ProductId1",
                principalSchema: "Project",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
