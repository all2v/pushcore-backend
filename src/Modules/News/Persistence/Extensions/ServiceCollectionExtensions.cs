using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.News.Mappings;
using Astrum.News.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.News.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddExpressionMapping();
            cfg.AddMaps(typeof(NewsProfile));
        });
        services.AddCustomDbContext<NewsDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        services.AddScoped<IDbContextInitializer, DbContextContextInitializer>();

        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<IAttachmentsRepository, AttachmentsRepository>();
        services.AddScoped<IBannersRepository, BannersRepository>();
        services.AddScoped<ILikesRepository, LikesRepository>();
        services.AddScoped<ICommentsRepository, CommentsRepository>();
        services.AddScoped<IWidgetRepository, WidgetRepository>();
    }
}