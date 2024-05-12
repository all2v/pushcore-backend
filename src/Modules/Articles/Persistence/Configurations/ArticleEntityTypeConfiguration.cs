using Astrum.Articles.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Articles.Configurations;

public class ArticleEntityTypeConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(e => e.Tags)
            .WithMany()
            .UsingEntity<ArticleTag>();
        //builder.Metadata.FindNavigation(nameof(Article.Tags))
        //    ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsOne(x => x.Content);
        builder.OwnsOne(x => x.Slug);
    }
}