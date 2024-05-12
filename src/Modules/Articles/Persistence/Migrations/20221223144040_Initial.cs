using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Astrum.Articles.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Articles");

            migrationBuilder.CreateTable(
                name: "AuditHistory",
                schema: "Articles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RowId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TableName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Changed = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Kind = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Username = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "Articles",
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
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                schema: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReadingTime = table.Column<int>(type: "integer", nullable: false),
                    CoverUrl = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "Articles",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Articles",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "Articles",
                        principalTable: "Articles",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "Articles",
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DateDeleted", "DateModified", "IsDeleted", "ModifiedBy", "Name", "Version" },
                values: new object[,]
                {
                    { new Guid("134240e2-687c-43f5-8b41-628bbf24927b"), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8277), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8277), new TimeSpan(0, 0, 0, 0, 0)), false, null, "Back-end разработка", -1 },
                    { new Guid("27e5480a-91cd-45c5-ba03-0d894e068340"), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8287), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8288), new TimeSpan(0, 0, 0, 0, 0)), false, null, "Инструкции и гайдлайны", -1 },
                    { new Guid("5e708c94-965f-458f-96b8-370d8ea8104c"), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8269), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8272), new TimeSpan(0, 0, 0, 0, 0)), false, null, "Дизайн", -1 },
                    { new Guid("615cd692-36f4-42c9-a195-5f1d5b5aa311"), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8280), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8280), new TimeSpan(0, 0, 0, 0, 0)), false, null, "Front-end разработка", -1 },
                    { new Guid("7d6fef20-3793-485b-834f-049d9e4a2b44"), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8274), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8275), new TimeSpan(0, 0, 0, 0, 0)), false, null, "Аналитика", -1 },
                    { new Guid("9151f57b-ed20-4c9a-a167-ea725c239da9"), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8283), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8283), new TimeSpan(0, 0, 0, 0, 0)), false, null, "DevOps", -1 },
                    { new Guid("ededcde2-ad3d-4e47-857a-fea9f3d81e62"), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8407), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2022, 12, 23, 14, 40, 40, 736, DateTimeKind.Unspecified).AddTicks(8408), new TimeSpan(0, 0, 0, 0, 0)), false, null, "Другое", -1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorId",
                schema: "Articles",
                table: "Articles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                schema: "Articles",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ArticleId",
                schema: "Articles",
                table: "Tags",
                column: "ArticleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditHistory",
                schema: "Articles");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "Articles");

            migrationBuilder.DropTable(
                name: "Articles",
                schema: "Articles");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "Articles");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "Articles");
        }
    }
}
