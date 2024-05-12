using Astrum.Identity.Application.Contracts;
using Astrum.Identity.Contracts;
using Astrum.Identity.Infrastructure.Services;
using Astrum.Identity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddScoped<IUserContextAccessor, UserContextAccessor>();
        services.AddScoped<IApplicationUserAccessor, ApplicationUserAccessor>(); 
        services.AddScoped<IIdentityJwtGenerator, IdentityJwtGenerator>();
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<IGitlabMappingService, GitlabMappingService>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(UserProfile));
        });
        return services;
    }
}