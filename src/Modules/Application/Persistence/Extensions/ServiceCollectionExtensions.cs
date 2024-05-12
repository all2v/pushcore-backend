using Astrum.Application.EventStore;
using Astrum.Application.Repositories;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.SharedLib.Application.Contracts.Infrastructure.EventStore;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using Astrum.SharedLib.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabasePersistence(configuration);
        services.AddEventStore();
        services.AddRepositories();
    }

    private static void AddDatabasePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomDbContext<ApplicationDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        services.AddCustomDbContext<EventStoreDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));

        services.AddScoped<IDbContextInitializer, DbContextInitializer>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IESRepository<,>), typeof(ESRepository<,>)); 
        services.AddScoped<IApplicationConfigurationRepository, ApplicationConfigurationRepository>();
    }

    private static void AddEventStore(this IServiceCollection services)
    {
        services.AddScoped<IEventStore, EFEventStore>();
        services.AddScoped<IEventStoreSnapshotProvider, EFEventStoreSnapshotProvider>();
        services.AddScoped<IRetroactiveEventsService, RetroactiveEventsService>();
    }
}