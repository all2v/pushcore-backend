using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.Projects.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Projects.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomDbContext<ProjectDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        services.AddScoped<IDbContextInitializer, DbContextInitializer>();

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomFieldRepository, CustomFieldRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
    }
}