using Astrum.SampleData.Persistence;
using Astrum.SampleData.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.SampleData.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomDbContext<SampleDataDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
            services.AddCustomDbContext<SampleContentDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
            services.AddScoped<ISampleDataRepository, SampleDataRepository>();
            services.AddScoped<ISampleContentRepository, SampleContentRepository>();
        }
    }
}
