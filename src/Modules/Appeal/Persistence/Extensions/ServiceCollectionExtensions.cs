using Astrum.Appeal.Repositories;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Appeal.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomDbContext<AppealDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        services.AddScoped<IDbContextInitializer, AppealDbContextContextInitializer>();

        services.AddScoped<IAppealRepository, AppealRepository>();
        services.AddScoped<IAppealCategoryRepository, AppealCategoryRepository>();
    }
}