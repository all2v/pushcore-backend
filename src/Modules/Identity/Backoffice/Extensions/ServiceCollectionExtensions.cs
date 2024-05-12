using Astrum.Identity.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBackofficeServices(this IServiceCollection services)
    {
        // services.AddAutoMapper(cfg =>
        // {
        //     cfg.AddMaps(typeof(AuthController));
        // });
        services.AddMediatR(typeof(AuthController));
    }
}