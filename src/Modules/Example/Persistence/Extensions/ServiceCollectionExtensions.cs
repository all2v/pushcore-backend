using Astrum.Example.Repositories;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Example.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomDbContext<ExampleDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        services.AddScoped<IDbContextInitializer, DbContextContextInitializer>();
        services.AddScoped<IExampleRepository, ExampleRepository>();
    }
}