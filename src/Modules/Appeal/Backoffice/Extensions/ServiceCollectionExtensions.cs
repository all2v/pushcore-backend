using Astrum.Appeal.Controllers;
using Astrum.Appeal.GraphQL;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Appeal.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBackofficeServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(AppealController).Assembly;

        services.AddMediatR(currentAssembly);
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });
        services.AddGraphQLServer("AppealSchema")
            .AddFiltering()
            .AddSorting()
            .AddQueryType<QueryAppeal>();
    }
}