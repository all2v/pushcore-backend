using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//using MediatR;

namespace Astrum.News.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // var currentAssembly = typeof(MarketProfile).Assembly;
        //
        // services.AddMediatR(currentAssembly);
        // services.AddAutoMapper(cfg =>
        // {
        //     cfg.AddMaps(currentAssembly);
        // });
    }
}