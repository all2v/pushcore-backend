using Astrum.Calendar.Controllers;
using Astrum.Calendar.GraphQL;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Calendar.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBackofficeServices(this IServiceCollection services)
    {   var currentAssembly = typeof(CalendarController).Assembly;
        services.AddMediatR(currentAssembly);

        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });
        services.AddGraphQLServer("CalendarSchema")
            .AddFiltering()
            .AddSorting()
            .AddQueryType<QueryCalendar>()
            .AddSubscriptionType<SubscriptionCalendar>();
    }
}