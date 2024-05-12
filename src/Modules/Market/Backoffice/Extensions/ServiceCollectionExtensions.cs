using Astrum.Market.Controllers;
using Astrum.Market.GraphQL;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Market.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBackofficeServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(BasketController).Assembly;

        services.AddMediatR(currentAssembly);
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });
        services.AddGraphQLServer("MarketSchema")
            .AddFiltering()
            .AddSorting()
            .AddQueryType<QueryMarket>();
    }
}