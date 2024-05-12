using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.SharedLib.Persistence.Extensions;
using Astrum.Storage.Mappings;
using Astrum.Storage.Persistance.Repositories;
using Astrum.Storage.Repositories;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Storage.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddExpressionMapping();
            cfg.AddMaps(typeof(StorageProfile));
        });
        services.AddCustomDbContext<StorageDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        services.AddScoped<IDbContextInitializer, DbContextContextInitializer>();

        services.AddScoped<IFileRepository, FileRepository>();
    }
}