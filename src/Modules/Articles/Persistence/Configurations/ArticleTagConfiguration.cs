using Astrum.Articles.Aggregates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Articles.Persistence.Configurations;

public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
{
    public void Configure(EntityTypeBuilder<ArticleTag> builder)
    {
        builder.HasOne<Tag>()
            .WithMany(e=>e.ArticleTags)
            .HasForeignKey(e => e.TagId);
        builder.HasOne<Article>()
            .WithMany()
            .HasForeignKey(e => e.ArticleId);
    }
}
