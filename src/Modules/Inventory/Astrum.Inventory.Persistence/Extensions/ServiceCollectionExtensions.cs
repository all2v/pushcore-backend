using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.Inventory.DomainServices.Repositories;
using Astrum.Inventory.Persistence;
using Astrum.Inventory.Persistence.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Inventory.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomDbContext<InventoryDbContext>(configuration.GetConnectionString("BaseConnection"));
            services.AddScoped<IInventoryItemsRepository, InventoryItemsRepository>();
            services.AddScoped<ITemplatesRepository, TemplatesRepository>();
            services.AddScoped<IDbContextInitializer, InventoryDbContextInitializer>();
        }
    }
}
