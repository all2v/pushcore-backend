using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // var currentAssembly = typeof(GetRouteQuery).Assembly;

        // services.AddMediatR(currentAssembly);
        // services.AddAutoMapper(cfg =>
        // {
        //     cfg.AddMaps(currentAssembly);
        // });
    }
}