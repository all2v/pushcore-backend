using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Recruting.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStageCardModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Boards_BoardId",
                schema: "Recruting",
                table: "Stages");

            migrationBuilder.DropTable(
                name: "Boards",
                schema: "Recruting");

            migrationBuilder.DropIndex(
                name: "IX_Stages_BoardId",
                schema: "Recruting",
                table: "Stages");

            migrationBuilder.AddColumn<Guid>(
                name: "KanbanBoardId",
                schema: "Recruting",
                table: "Stages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StageCardId",
                schema: "Recruting",
                table: "StageEvents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StageId",
                schema: "Recruting",
                table: "StageEvents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "KanbanBoards",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    VacancyId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_KanbanBoards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StageCards",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StageId = table.Column<Guid>(type: "uuid", nullable: false),
                    HrmUserId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_StageCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StageCards_HrmUsers_HrmUserId",
                        column: x => x.HrmUserId,
                        principalSchema: "Recruting",
                        principalTable: "HrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StageCards_Stages_StageId",
                        column: x => x.StageId,
                        principalSchema: "Recruting",
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stages_KanbanBoardId",
                schema: "Recruting",
                table: "Stages",
                column: "KanbanBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_StageEvents_StageCardId",
                schema: "Recruting",
                table: "StageEvents",
                column: "StageCardId");

            migrationBuilder.CreateIndex(
                name: "IX_StageCards_HrmUserId",
                schema: "Recruting",
                table: "StageCards",
                column: "HrmUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StageCards_StageId",
                schema: "Recruting",
                table: "StageCards",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_StageEvents_StageCards_StageCardId",
                schema: "Recruting",
                table: "StageEvents",
                column: "StageCardId",
                principalSchema: "Recruting",
                principalTable: "StageCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_KanbanBoards_KanbanBoardId",
                schema: "Recruting",
                table: "Stages",
                column: "KanbanBoardId",
                principalSchema: "Recruting",
                principalTable: "KanbanBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StageEvents_StageCards_StageCardId",
                schema: "Recruting",
                table: "StageEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_KanbanBoards_KanbanBoardId",
                schema: "Recruting",
                table: "Stages");

            migrationBuilder.DropTable(
                name: "KanbanBoards",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "StageCards",
                schema: "Recruting");

            migrationBuilder.DropIndex(
                name: "IX_Stages_KanbanBoardId",
                schema: "Recruting",
                table: "Stages");

            migrationBuilder.DropIndex(
                name: "IX_StageEvents_StageCardId",
                schema: "Recruting",
                table: "StageEvents");

            migrationBuilder.DropColumn(
                name: "KanbanBoardId",
                schema: "Recruting",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "StageCardId",
                schema: "Recruting",
                table: "StageEvents");

            migrationBuilder.DropColumn(
                name: "StageId",
                schema: "Recruting",
                table: "StageEvents");

            migrationBuilder.CreateTable(
                name: "Boards",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DateModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    VacancyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stages_BoardId",
                schema: "Recruting",
                table: "Stages",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Boards_BoardId",
                schema: "Recruting",
                table: "Stages",
                column: "BoardId",
                principalSchema: "Recruting",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
