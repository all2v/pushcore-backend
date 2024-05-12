// <auto-generated />
using System;
using Astrum.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Astrum.Articles.Persistence.Migrations
{
    [DbContext(typeof(ArticlesDbContext))]
    [Migration("20230111142455_Update__Article_Content")]
    partial class UpdateArticleContent
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Articles")
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Astrum.Articles.Aggregates.Article", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<Guid>("AuthorId")
                    .HasColumnType("uuid");

                b.Property<Guid>("CategoryId")
                    .HasColumnType("uuid");

                b.Property<string>("CoverUrl")
                    .HasColumnType("text");

                b.Property<string>("CreatedBy")
                    .HasColumnType("text");

                b.Property<string>("Content_Text")
                        .IsRequired()
                        .HasColumnType("text");

                b.Property<string>("Content_Html")
                        .IsRequired()
                        .HasColumnType("text");

                b.Property<DateTimeOffset>("DateCreated")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTimeOffset?>("DateDeleted")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTimeOffset>("DateModified")
                    .HasColumnType("timestamp with time zone");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<bool>("IsDeleted")
                    .HasColumnType("boolean");

                b.Property<string>("ModifiedBy")
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<int>("ReadingTime")
                    .HasColumnType("integer");

                b.Property<int>("Version")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.HasIndex("AuthorId");

                b.HasIndex("CategoryId");

                b.ToTable("Articles", "Articles");
            });

            modelBuilder.Entity("Astrum.Articles.Aggregates.Author", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<string>("CreatedBy")
                    .HasColumnType("text");

                b.Property<DateTimeOffset>("DateCreated")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTimeOffset?>("DateDeleted")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTimeOffset>("DateModified")
                    .HasColumnType("timestamp with time zone");

                b.Property<bool>("IsDeleted")
                    .HasColumnType("boolean");

                b.Property<string>("ModifiedBy")
                    .HasColumnType("text");

                b.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b.Property<int>("Version")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.ToTable("Authors", "Articles");
            });

            modelBuilder.Entity("Astrum.Articles.Aggregates.Category", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<string>("CreatedBy")
                    .HasColumnType("text");

                b.Property<DateTimeOffset>("DateCreated")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTimeOffset?>("DateDeleted")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTimeOffset>("DateModified")
                    .HasColumnType("timestamp with time zone");

                b.Property<bool>("IsDeleted")
                    .HasColumnType("boolean");

                b.Property<string>("ModifiedBy")
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<int>("Version")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.ToTable("Categories", "Articles");
            });

            modelBuilder.Entity("Astrum.Articles.Aggregates.Tag", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<Guid?>("ArticleId")
                    .HasColumnType("uuid");

                b.Property<string>("CreatedBy")
                    .HasColumnType("text");

                b.Property<DateTimeOffset>("DateCreated")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTimeOffset?>("DateDeleted")
                    .HasColumnType("timestamp with time zone");

                b.Property<DateTimeOffset>("DateModified")
                    .HasColumnType("timestamp with time zone");

                b.Property<bool>("IsDeleted")
                    .HasColumnType("boolean");

                b.Property<string>("ModifiedBy")
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("text");

                b.Property<int>("Version")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.HasIndex("ArticleId");

                b.ToTable("Tags", "Articles");
            });

            modelBuilder.Entity("Astrum.SharedLib.Persistence.Models.Audit.AuditHistory", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                b.Property<string>("Changed")
                    .IsRequired()
                    .HasMaxLength(2048)
                    .HasColumnType("character varying(2048)");

                b.Property<DateTimeOffset>("Created")
                    .HasColumnType("timestamp with time zone");

                b.Property<int>("Kind")
                    .HasColumnType("integer");

                b.Property<string>("RowId")
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnType("character varying(128)");

                b.Property<string>("TableName")
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnType("character varying(128)");

                b.Property<string>("Username")
                    .HasMaxLength(128)
                    .HasColumnType("character varying(128)");

                b.HasKey("Id");

                b.ToTable("AuditHistory", "Articles");
            });

            modelBuilder.Entity("Astrum.Articles.Aggregates.Article", b =>
            {
                b.HasOne("Astrum.Articles.Aggregates.Author", "Author")
                    .WithMany()
                    .HasForeignKey("AuthorId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Astrum.Articles.Aggregates.Category", "Category")
                    .WithMany()
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired();

                b.OwnsOne("Astrum.Articles.Aggregates.ArticleContent", "Content", b1 =>
                {
                    b1.Property<Guid>("ArticleId")
                        .HasColumnType("uuid");

                    b1.Property<string>("Html")
                        .HasMaxLength(9437184)
                        .HasColumnType("character varying(9437184)");

                    b1.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(9437184)
                        .HasColumnType("character varying(9437184)");

                    b1.HasKey("ArticleId");

                    b1.ToTable("Articles", "Articles");

                    b1.WithOwner()
                        .HasForeignKey("ArticleId");
                });

                b.Navigation("Author");

                b.Navigation("Category");

                b.Navigation("Content")
                    .IsRequired();
            });

            modelBuilder.Entity("Astrum.Articles.Aggregates.Tag", b =>
            {
                b.HasOne("Astrum.Articles.Aggregates.Article", null)
                    .WithMany("Tags")
                    .HasForeignKey("ArticleId");
            });

            modelBuilder.Entity("Astrum.Articles.Aggregates.Article", b =>
            {
                b.Navigation("Tags");
            });
#pragma warning restore 612, 618
        }
    }
}
