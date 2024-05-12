using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Account.Migrations
{
    /// <inheritdoc />
    public partial class Timeline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TimelineId",
                schema: "Account",
                table: "UsersProfiles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Timelines",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimelineType = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Timelines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimelineIntervals",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TimelineId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IntervalType = table.Column<int>(type: "integer", nullable: false),
                    AccessTimelineId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_TimelineIntervals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimelineIntervals_Timelines_AccessTimelineId",
                        column: x => x.AccessTimelineId,
                        principalSchema: "Account",
                        principalTable: "Timelines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersProfiles_TimelineId",
                schema: "Account",
                table: "UsersProfiles",
                column: "TimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineIntervals_AccessTimelineId",
                schema: "Account",
                table: "TimelineIntervals",
                column: "AccessTimelineId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersProfiles_Timelines_TimelineId",
                schema: "Account",
                table: "UsersProfiles",
                column: "TimelineId",
                principalSchema: "Account",
                principalTable: "Timelines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersProfiles_Timelines_TimelineId",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.DropTable(
                name: "TimelineIntervals",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Timelines",
                schema: "Account");

            migrationBuilder.DropIndex(
                name: "IX_UsersProfiles_TimelineId",
                schema: "Account",
                table: "UsersProfiles");

            migrationBuilder.DropColumn(
                name: "TimelineId",
                schema: "Account",
                table: "UsersProfiles");
        }
    }
}
