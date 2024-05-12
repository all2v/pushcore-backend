using Astrum.Articles.Repositories;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Articles.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomDbContext<ArticlesDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));

        services.AddScoped<IDbContextInitializer, DbContextInitializer>();
        services.AddScoped<IArticleTagRepository, ArticleTagRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}