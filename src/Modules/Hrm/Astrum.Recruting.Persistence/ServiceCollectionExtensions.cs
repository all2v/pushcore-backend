using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Recruting.Persistence;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(DbContextExtensions.BaseConnectionName);
        services.AddCustomDbContext<RecrutingDbContext>(connectionString);
        services.AddScoped<IDbContextInitializer, RecrutingDbContextInitializer>();
       
    }
}