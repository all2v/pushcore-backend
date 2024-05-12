using Astrum.Debts.Application.Mappings;
using Astrum.Debts.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Debts.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var currentAssembly = typeof(DebtProfile).Assembly;
            services.AddScoped<IDebtsService, DebtsService>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(currentAssembly);
            });
        }
    }
}
