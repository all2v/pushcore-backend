using Astrum.SampleData.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.SampleData.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBackofficeServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(SampleDataController).Assembly;
    }
}
