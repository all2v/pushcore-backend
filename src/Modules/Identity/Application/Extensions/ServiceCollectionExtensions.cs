using Astrum.Identity.Authorization.Handlers;
using Astrum.Identity.Features.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityApplicationServices(this IServiceCollection services)
    {
        //services.AddAuthorizationCore();
        //services.AddAuthorizationPolicies();

        return services;
    }

    private static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        // services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, UserOperationAuthorizationHandler>();
    }
    
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(LoginCommand).Assembly;

        services.AddMediatR(currentAssembly);
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });
        
    }
}