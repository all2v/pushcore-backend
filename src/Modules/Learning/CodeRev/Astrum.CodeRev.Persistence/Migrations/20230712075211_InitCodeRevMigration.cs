using System;
using Astrum.CodeRev.Domain.Aggregates.Draft;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Astrum.CodeRev.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitCodeRevMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CodeRev");

            migrationBuilder.CreateTable(
                name: "AuditHistory",
                schema: "CodeRev",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RowId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TableName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Changed = table.Column<string>(type: "text", nullable: false),
                    Kind = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Username = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interviews",
                schema: "CodeRev",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Vacancy = table.Column<string>(type: "text", nullable: false),
                    InterviewText = table.Column<string>(type: "text", nullable: false),
                    InterviewDurationMs = table.Column<long>(type: "bigint", nullable: false),
                    CreatedByUsername = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Interviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "CodeRev",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewSolutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    NotificationType = table.Column<int>(type: "integer", nullable: false),
                    CreationTimeMs = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewerDrafts",
                schema: "CodeRev",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewSolutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Draft = table.Column<Draft>(type: "jsonb", nullable: true),
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
                    table.PrimaryKey("PK_ReviewerDrafts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                schema: "CodeRev",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskText = table.Column<string>(type: "text", nullable: false),
                    StartCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TestsCode = table.Column<string>(type: "text", nullable: false),
                    RunAttempts = table.Column<int>(type: "integer", nullable: false),
                    ProgrammingLanguage = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterviewLanguages",
                schema: "CodeRev",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgrammingLanguage = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_InterviewLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewLanguages_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalSchema: "CodeRev",
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                schema: "CodeRev",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    InterviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpiredAt = table.Column<long>(type: "bigint", nullable: false),
                    IsSynchronous = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalSchema: "CodeRev",
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewSolutions",
                schema: "CodeRev",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    StartTimeMs = table.Column<long>(type: "bigint", nullable: false),
                    EndTimeMs = table.Column<long>(type: "bigint", nullable: false),
                    TimeToCheckMs = table.Column<long>(type: "bigint", nullable: false),
                    ReviewerComment = table.Column<string>(type: "text", nullable: false),
                    AverageGrade = table.Column<int>(type: "integer", nullable: false),
                    InterviewResult = table.Column<int>(type: "integer", nullable: false),
                    IsSubmittedByCandidate = table.Column<bool>(type: "boolean", nullable: false),
                    InvitedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSynchronous = table.Column<bool>(type: "boolean", nullable: false),
                    ReviewerDraftId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerDraftId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_InterviewSolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewSolutions_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalSchema: "CodeRev",
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewSolutions_ReviewerDrafts_ReviewerDraftId1",
                        column: x => x.ReviewerDraftId1,
                        principalSchema: "CodeRev",
                        principalTable: "ReviewerDrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewTestTask",
                schema: "CodeRev",
                columns: table => new
                {
                    InterviewsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TasksId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewTestTask", x => new { x.InterviewsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_InterviewTestTask_Interviews_InterviewsId",
                        column: x => x.InterviewsId,
                        principalSchema: "CodeRev",
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewTestTask_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalSchema: "CodeRev",
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskSolutions",
                schema: "CodeRev",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewSolutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDone = table.Column<bool>(type: "boolean", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    RunAttemptsLeft = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_TaskSolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskSolutions_InterviewSolutions_InterviewSolutionId",
                        column: x => x.InterviewSolutionId,
                        principalSchema: "CodeRev",
                        principalTable: "InterviewSolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskSolutions_Tasks_TestTaskId",
                        column: x => x.TestTaskId,
                        principalSchema: "CodeRev",
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterviewLanguages_InterviewId",
                schema: "CodeRev",
                table: "InterviewLanguages",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSolutions_InterviewId",
                schema: "CodeRev",
                table: "InterviewSolutions",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewSolutions_ReviewerDraftId1",
                schema: "CodeRev",
                table: "InterviewSolutions",
                column: "ReviewerDraftId1");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewTestTask_TasksId",
                schema: "CodeRev",
                table: "InterviewTestTask",
                column: "TasksId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InterviewId",
                schema: "CodeRev",
                table: "Invitations",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSolutions_InterviewSolutionId",
                schema: "CodeRev",
                table: "TaskSolutions",
                column: "InterviewSolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSolutions_TestTaskId",
                schema: "CodeRev",
                table: "TaskSolutions",
                column: "TestTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditHistory",
                schema: "CodeRev");

            migrationBuilder.DropTable(
                name: "InterviewLanguages",
                schema: "CodeRev");

            migrationBuilder.DropTable(
                name: "InterviewTestTask",
                schema: "CodeRev");

            migrationBuilder.DropTable(
                name: "Invitations",
                schema: "CodeRev");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "CodeRev");

            migrationBuilder.DropTable(
                name: "TaskSolutions",
                schema: "CodeRev");

            migrationBuilder.DropTable(
                name: "InterviewSolutions",
                schema: "CodeRev");

            migrationBuilder.DropTable(
                name: "Tasks",
                schema: "CodeRev");

            migrationBuilder.DropTable(
                name: "Interviews",
                schema: "CodeRev");

            migrationBuilder.DropTable(
                name: "ReviewerDrafts",
                schema: "CodeRev");
        }
    }
}
