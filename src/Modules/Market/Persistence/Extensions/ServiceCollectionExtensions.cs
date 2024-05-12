using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.Market.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Market.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(DbContextExtensions.BaseConnectionName);
        services.AddCustomDbContext<MarketDbContext>(connectionString);
        services.AddScoped<IDbContextInitializer, DbContextContextInitializer>();

        services.AddScoped<IMarketOrderRepository, MarketOrderRepository>();
        services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        services.AddScoped<IMarketBasketRepository, MarketBasketRepository>();
        services.AddScoped<IMarketProductRepository, MarketProductRepository>();
    }
}