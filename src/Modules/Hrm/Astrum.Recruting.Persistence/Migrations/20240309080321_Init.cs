using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Astrum.Recruting.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Recruting");

            migrationBuilder.CreateTable(
                name: "AuditHistory",
                schema: "Recruting",
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
                name: "Boards",
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
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HrmUsers",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Srurname = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    TgLogin = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_HrmUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PracticeSkills",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_PracticeSkills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PracticeVacancies",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PracticeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    MaxParticipantsCount = table.Column<int>(type: "integer", nullable: false),
                    TaskUrl = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_PracticeVacancies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vacancies",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    HhId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Salary = table.Column<double>(type: "double precision", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Vacancies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VacancyAreas",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_VacancyAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VacancyCandidates",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandidateResumeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_VacancyCandidates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VacancyRoles",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_VacancyRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VacancyTypes",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_VacancyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkTypes",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_WorkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    BoardId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_Stages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages_Boards_BoardId",
                        column: x => x.BoardId,
                        principalSchema: "Recruting",
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateResumes",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Srurname = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    About = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Salary = table.Column<double>(type: "double precision", nullable: false),
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
                    table.PrimaryKey("PK_CandidateResumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateResumes_HrmUsers_HrmUserId",
                        column: x => x.HrmUserId,
                        principalSchema: "Recruting",
                        principalTable: "HrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserComments",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_UserComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserComments_HrmUsers_HrmUserId",
                        column: x => x.HrmUserId,
                        principalSchema: "Recruting",
                        principalTable: "HrmUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PracticeRequests",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Fio = table.Column<string>(type: "text", nullable: false),
                    TgLogin = table.Column<string>(type: "text", nullable: false),
                    TaskUrl = table.Column<string>(type: "text", nullable: false),
                    About = table.Column<string>(type: "text", nullable: false),
                    HrmUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PracticeVacancyId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_PracticeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PracticeRequests_PracticeVacancies_PracticeVacancyId",
                        column: x => x.PracticeVacancyId,
                        principalSchema: "Recruting",
                        principalTable: "PracticeVacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PracticeSkillPracticeVacancy",
                schema: "Recruting",
                columns: table => new
                {
                    PracticeSkillsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PracticeVacanciesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeSkillPracticeVacancy", x => new { x.PracticeSkillsId, x.PracticeVacanciesId });
                    table.ForeignKey(
                        name: "FK_PracticeSkillPracticeVacancy_PracticeSkills_PracticeSkillsId",
                        column: x => x.PracticeSkillsId,
                        principalSchema: "Recruting",
                        principalTable: "PracticeSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PracticeSkillPracticeVacancy_PracticeVacancies_PracticeVaca~",
                        column: x => x.PracticeVacanciesId,
                        principalSchema: "Recruting",
                        principalTable: "PracticeVacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Responsibilities",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PracticeVacancyId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_Responsibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responsibilities_PracticeVacancies_PracticeVacancyId",
                        column: x => x.PracticeVacancyId,
                        principalSchema: "Recruting",
                        principalTable: "PracticeVacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacancyVacancyArea",
                schema: "Recruting",
                columns: table => new
                {
                    VacanciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    VacancyAreasId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancyVacancyArea", x => new { x.VacanciesId, x.VacancyAreasId });
                    table.ForeignKey(
                        name: "FK_VacancyVacancyArea_Vacancies_VacanciesId",
                        column: x => x.VacanciesId,
                        principalSchema: "Recruting",
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VacancyVacancyArea_VacancyAreas_VacancyAreasId",
                        column: x => x.VacancyAreasId,
                        principalSchema: "Recruting",
                        principalTable: "VacancyAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacancyVacancyRole",
                schema: "Recruting",
                columns: table => new
                {
                    VacanciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    VacancyRolesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancyVacancyRole", x => new { x.VacanciesId, x.VacancyRolesId });
                    table.ForeignKey(
                        name: "FK_VacancyVacancyRole_Vacancies_VacanciesId",
                        column: x => x.VacanciesId,
                        principalSchema: "Recruting",
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VacancyVacancyRole_VacancyRoles_VacancyRolesId",
                        column: x => x.VacancyRolesId,
                        principalSchema: "Recruting",
                        principalTable: "VacancyRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacancyVacancyType",
                schema: "Recruting",
                columns: table => new
                {
                    VacanciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    VacancyTypesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancyVacancyType", x => new { x.VacanciesId, x.VacancyTypesId });
                    table.ForeignKey(
                        name: "FK_VacancyVacancyType_Vacancies_VacanciesId",
                        column: x => x.VacanciesId,
                        principalSchema: "Recruting",
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VacancyVacancyType_VacancyTypes_VacancyTypesId",
                        column: x => x.VacancyTypesId,
                        principalSchema: "Recruting",
                        principalTable: "VacancyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacancyWorkType",
                schema: "Recruting",
                columns: table => new
                {
                    VacanciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkTypesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancyWorkType", x => new { x.VacanciesId, x.WorkTypesId });
                    table.ForeignKey(
                        name: "FK_VacancyWorkType_Vacancies_VacanciesId",
                        column: x => x.VacanciesId,
                        principalSchema: "Recruting",
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VacancyWorkType_WorkTypes_WorkTypesId",
                        column: x => x.WorkTypesId,
                        principalSchema: "Recruting",
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateLinks",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HrmUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StageId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_CandidateLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateLinks_Stages_StageId",
                        column: x => x.StageId,
                        principalSchema: "Recruting",
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobExperiences",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    CandidateResumeId = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_JobExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobExperiences_CandidateResumes_CandidateResumeId",
                        column: x => x.CandidateResumeId,
                        principalSchema: "Recruting",
                        principalTable: "CandidateResumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StageEvents",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EventDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AccountableId = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateLinkId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_StageEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StageEvents_CandidateLinks_CandidateLinkId",
                        column: x => x.CandidateLinkId,
                        principalSchema: "Recruting",
                        principalTable: "CandidateLinks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CandidateLinkHistories",
                schema: "Recruting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StageEventId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_CandidateLinkHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateLinkHistories_StageEvents_StageEventId",
                        column: x => x.StageEventId,
                        principalSchema: "Recruting",
                        principalTable: "StageEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateLinkHistories_StageEventId",
                schema: "Recruting",
                table: "CandidateLinkHistories",
                column: "StageEventId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateLinks_StageId",
                schema: "Recruting",
                table: "CandidateLinks",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateResumes_HrmUserId",
                schema: "Recruting",
                table: "CandidateResumes",
                column: "HrmUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobExperiences_CandidateResumeId",
                schema: "Recruting",
                table: "JobExperiences",
                column: "CandidateResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticeRequests_PracticeVacancyId",
                schema: "Recruting",
                table: "PracticeRequests",
                column: "PracticeVacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticeSkillPracticeVacancy_PracticeVacanciesId",
                schema: "Recruting",
                table: "PracticeSkillPracticeVacancy",
                column: "PracticeVacanciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Responsibilities_PracticeVacancyId",
                schema: "Recruting",
                table: "Responsibilities",
                column: "PracticeVacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_StageEvents_CandidateLinkId",
                schema: "Recruting",
                table: "StageEvents",
                column: "CandidateLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_BoardId",
                schema: "Recruting",
                table: "Stages",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_UserComments_HrmUserId",
                schema: "Recruting",
                table: "UserComments",
                column: "HrmUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyVacancyArea_VacancyAreasId",
                schema: "Recruting",
                table: "VacancyVacancyArea",
                column: "VacancyAreasId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyVacancyRole_VacancyRolesId",
                schema: "Recruting",
                table: "VacancyVacancyRole",
                column: "VacancyRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyVacancyType_VacancyTypesId",
                schema: "Recruting",
                table: "VacancyVacancyType",
                column: "VacancyTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyWorkType_WorkTypesId",
                schema: "Recruting",
                table: "VacancyWorkType",
                column: "WorkTypesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditHistory",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "CandidateLinkHistories",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "JobExperiences",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "PracticeRequests",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "PracticeSkillPracticeVacancy",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "Responsibilities",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "UserComments",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "VacancyCandidates",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "VacancyVacancyArea",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "VacancyVacancyRole",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "VacancyVacancyType",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "VacancyWorkType",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "StageEvents",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "CandidateResumes",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "PracticeSkills",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "PracticeVacancies",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "VacancyAreas",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "VacancyRoles",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "VacancyTypes",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "Vacancies",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "WorkTypes",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "CandidateLinks",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "HrmUsers",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "Stages",
                schema: "Recruting");

            migrationBuilder.DropTable(
                name: "Boards",
                schema: "Recruting");
        }
    }
}
