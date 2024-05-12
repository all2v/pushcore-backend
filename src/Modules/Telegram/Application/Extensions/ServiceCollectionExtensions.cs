using Astrum.Telegram.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Telegram.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var currentAssembly = typeof(TelegramProfile).Assembly;
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(currentAssembly);
            });
        }
    }
}
