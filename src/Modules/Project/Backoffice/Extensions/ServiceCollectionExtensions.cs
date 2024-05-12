using System.Reflection;
using Astrum.Projects.Controllers;
using Astrum.Projects.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Projects.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBackofficeServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(ProjectsController).Assembly;

        services.AddMediatR(currentAssembly);
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(MemberProfile));
        });
        services.AddMediatR(Assembly.GetExecutingAssembly());
        //services.AddGraphQLServer("MarketSchema")
        //    .AddFiltering()
        //    .AddSorting()
        //    .AddQueryType<QueryMarket>()
        //    .AddSubscriptionType<SubscriptionMarket>();
    }
}