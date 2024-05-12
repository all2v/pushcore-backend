using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Astrum.ITDictionary.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fixQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestions_Terms_PracticeId",
                schema: "ITDictionary",
                table: "TestQuestions");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestions_TermSourceId",
                schema: "ITDictionary",
                table: "TestQuestions",
                column: "TermSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestions_Terms_TermSourceId",
                schema: "ITDictionary",
                table: "TestQuestions",
                column: "TermSourceId",
                principalSchema: "ITDictionary",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestions_Terms_TermSourceId",
                schema: "ITDictionary",
                table: "TestQuestions");

            migrationBuilder.DropIndex(
                name: "IX_TestQuestions_TermSourceId",
                schema: "ITDictionary",
                table: "TestQuestions");

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestions_Terms_PracticeId",
                schema: "ITDictionary",
                table: "TestQuestions",
                column: "PracticeId",
                principalSchema: "ITDictionary",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
