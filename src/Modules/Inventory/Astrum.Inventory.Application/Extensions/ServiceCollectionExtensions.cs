using Astrum.Inventory.Application.Mappings;
using Astrum.Inventory.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Inventory.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var currentAssembly = typeof(InventoryProfile).Assembly;
            services.AddMediatR(currentAssembly);
            services.AddScoped<IInventoryItemsService, InventoryItemsService>();
            services.AddScoped<ITemplatesService, TemplatesService>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(currentAssembly);
            });
        }
    }
}
