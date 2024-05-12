using Astrum.Ordering.Mappings;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Ordering.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(OrderProfile).Assembly;

        services.AddMediatR(currentAssembly);
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });
        services.AddValidatorsFromAssembly(currentAssembly);
    }
}