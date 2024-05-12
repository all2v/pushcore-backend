using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.Recruting.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTableLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CandidateResumeId",
                schema: "Recruting",
                table: "VacancyCandidates",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "VacancyId",
                schema: "Recruting",
                table: "VacancyCandidates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VacancyCandidates_VacancyId",
                schema: "Recruting",
                table: "VacancyCandidates",
                column: "VacancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyCandidates_Vacancies_VacancyId",
                schema: "Recruting",
                table: "VacancyCandidates",
                column: "VacancyId",
                principalSchema: "Recruting",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacancyCandidates_Vacancies_VacancyId",
                schema: "Recruting",
                table: "VacancyCandidates");

            migrationBuilder.DropIndex(
                name: "IX_VacancyCandidates_VacancyId",
                schema: "Recruting",
                table: "VacancyCandidates");

            migrationBuilder.DropColumn(
                name: "VacancyId",
                schema: "Recruting",
                table: "VacancyCandidates");

            migrationBuilder.AlterColumn<Guid>(
                name: "CandidateResumeId",
                schema: "Recruting",
                table: "VacancyCandidates",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
