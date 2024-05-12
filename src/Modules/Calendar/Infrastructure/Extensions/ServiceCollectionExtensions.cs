using Astrum.Calendar.Infrastructure.Services;
using Astrum.Calendar.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Calendar.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var currentAssembly = typeof(EventService).Assembly;
        services.AddMediatR(currentAssembly);
        services.AddScoped<IAccessorService, AccessorService>();
        services.AddScoped<ICalendarListService, CalendarListService>();
        services.AddScoped<IEventService, EventService>();
    }
}