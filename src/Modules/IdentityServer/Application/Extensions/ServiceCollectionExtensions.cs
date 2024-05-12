using System.Reflection;
using Api.Application.Authorization;
using Astrum.Account.Aggregates;
using Astrum.Example.Extensions;
using Astrum.IdentityServer.Application.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(UserMappings));
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
    }
}