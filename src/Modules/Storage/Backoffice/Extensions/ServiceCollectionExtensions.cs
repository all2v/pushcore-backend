using Astrum.Storage.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Storage.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBackofficeServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(StorageController).Assembly;

        services.AddMediatR(currentAssembly);
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });
    }
}