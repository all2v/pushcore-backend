using System.Threading;
using Astrum.Articles.Aggregates;
using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Astrum.Articles;

public class DbContextInitializer : IDbContextInitializer
{
    private readonly ArticlesDbContext _dbContext;
    private readonly ILogger _logger;

    public DbContextInitializer(ArticlesDbContext dbContext, ILogger<DbContextInitializer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.MigrateAsync(cancellationToken);
    }

    public async Task Seed(CancellationToken cancellationToken = default)
    {
        SeedArticlesDbContext();
    }

    private void SeedArticlesDbContext()
    {
        if (!_dbContext.Categories.Any())
        {
            _dbContext.Categories.AddRange(
                new Category(new Guid("5e708c94-965f-458f-96b8-370d8ea8104c")) { Name = "Дизайн" },
                new Category(new Guid("7d6fef20-3793-485b-834f-049d9e4a2b44")) { Name = "Аналитика" },
                new Category(new Guid("134240e2-687c-43f5-8b41-628bbf24927b")) { Name = "Back-end разработка" },
                new Category(new Guid("615cd692-36f4-42c9-a195-5f1d5b5aa311")) { Name = "Front-end разработка" },
                new Category(new Guid("9151f57b-ed20-4c9a-a167-ea725c239da9")) { Name = "DevOps" },
                new Category(new Guid("27e5480a-91cd-45c5-ba03-0d894e068340")) { Name = "Инструкции и гайдлайны" },
                new Category(new Guid("ededcde2-ad3d-4e47-857a-fea9f3d81e62")) { Name = "Другое" }
            );

            _dbContext.SaveChanges();
        }
    }
}