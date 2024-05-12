using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Calendar.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(cfg =>
        {
            // cfg.AddMaps(typeof(AppealProfile));
        });

        // services.AddCustomDbContext<CalendarDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        // services.AddScoped<IDbContextInitializer, DbContextContextInitializer>();
    }
}