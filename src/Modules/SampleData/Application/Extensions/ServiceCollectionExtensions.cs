using Astrum.SampleData.Mappings;
using Astrum.SampleData.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.SampleData.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(SampleContentMapping));
        });
        services.AddScoped<ISampleDataService, SampleDataService>();
        services.AddScoped<ISampleContentService, SampleContentService>();
    }
}
