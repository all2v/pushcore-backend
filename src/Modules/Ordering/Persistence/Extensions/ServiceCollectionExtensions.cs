using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.Ordering.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Ordering.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDatabasePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomDbContext<OrderingDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        services.AddScoped<IDbContextInitializer, DbContextContextInitializer>();

        services.AddScoped<IOrderRepository, OrderRepository>();
    }
}