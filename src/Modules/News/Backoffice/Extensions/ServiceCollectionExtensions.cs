using Astrum.News.Controllers;
using Astrum.News.GraphQL;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.News.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBackofficeServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(NewsController).Assembly;

        services.AddMediatR(currentAssembly);
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });

        services.AddGraphQLServer("NewsSchema")
            .AddFiltering()
            .AddSorting()
            .AddQueryType<QueryNews>()
            .AddSubscriptionType<SubscriptionNews>();
    }
}