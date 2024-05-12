using Astrum.Storage.Models;
using Astrum.Storage.Mappings;
using Astrum.Storage.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Storage.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var currentAssembly = typeof(StorageProfile).Assembly;
        services.AddScoped<IFileStorage, S3Storage>();
        services.Configure<S3StorageRequisites>(configuration.GetSection(S3StorageRequisites.Section));
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });
    }
}