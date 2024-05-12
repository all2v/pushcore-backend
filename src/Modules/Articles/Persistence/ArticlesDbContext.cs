using Astrum.Articles.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Articles
{
    public class ArticlesDbContext : BaseDbContext
    {
        public ArticlesDbContext(DbContextOptions<ArticlesDbContext> options) : base(options)
        {
        }

        //To add migration with pmc:
        //Add-Migration Migration -Context ArticlesDbContext
        //To update db:
        //Update-Database -Context ArticlesDbContext
        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Articles");
        }
    }
}