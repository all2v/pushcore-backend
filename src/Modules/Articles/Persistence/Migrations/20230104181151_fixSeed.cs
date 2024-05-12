using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Astrum.Articles.Migrations
{
    /// <inheritdoc />
    public partial class fixSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Articles",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("134240e2-687c-43f5-8b41-628bbf24927b"));

            migrationBuilder.DeleteData(
                schema: "Articles",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("27e5480a-91cd-45c5-ba03-0d894e068340"));

            migrationBuilder.DeleteData(
                schema: "Articles",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5e708c94-965f-458f-96b8-370d8ea8104c"));

            migrationBuilder.DeleteData(
                schema: "Articles",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("615cd692-36f4-42c9-a195-5f1d5b5aa311"));

            migrationBuilder.DeleteData(
                schema: "Articles",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7d6fef20-3793-485b-834f-049d9e4a2b44"));

            migrationBuilder.DeleteData(
                schema: "Articles",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9151f57b-ed20-4c9a-a167-ea725c239da9"));

            migrationBuilder.DeleteData(
                schema: "Articles",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ededcde2-ad3d-4e47-857a-fea9f3d81e62"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
