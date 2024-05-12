using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.News.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using Astrum.Telegram.Persistence;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomDbContext<TelegramDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        //services.AddScoped<IDbContextInitializer, DbContextContextInitializer>();

        services.AddScoped<IChatRepository, ChatRepository>();
    }
}