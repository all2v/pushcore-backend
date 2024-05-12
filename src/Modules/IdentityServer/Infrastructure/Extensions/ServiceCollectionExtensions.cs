using Api.Application.Authorization.Abstractions;
using Api.Application.Authorization.Abstractions.Impl;
using Astrum.IdentityServer.Application.Services;
using Astrum.IdentityServer.DomainServices.Services;
using Astrum.IdentityServer.Infrastructure.Services;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.IdentityServer.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // var currentAssembly = typeof().Assembly;
        //
        // services.AddMediatR(currentAssembly);
        // services.AddAutoMapper(cfg =>
        // {
        //     cfg.AddMaps(currentAssembly);
        // });
        services.AddApplicationAuthentication(configuration);
        // requires confidential client
        services.AddKeycloakAdminHttpClient(configuration);

        // based on token forwarding HttpClient middleware
        // services.AddKeycloakProtectionHttpClient(configuration);

        // services.AddScoped<ICurrentUserService, CurrentUserService>();
        // services.AddScoped<IIdentityService, IdentityService>();
        // services.AddScoped<IUserService, KeycloakUserService>();
        // services.AddScoped<IKeycloakExtendedClient, KeycloakExtendedClient>();
    }

    public static void AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<KeycloakAuthenticationOptions>(x => configuration
            .GetSection(KeycloakAuthenticationOptions.Section)
            .Bind(x, opt => opt.BindNonPublicProperties = true));
        services.AddKeycloakAuthentication(configuration);

        services.AddAuthorization(o =>
        {
            // o.FallbackPolicy = new AuthorizationPolicyBuilder()
            //     .RequireAuthenticatedUser()
            //     .Build();
            
            // o.AddPolicy("ProtectedResource", b =>
            // {
            //     // b.AddRequirements(new DecisionRequirement("workspaces", "workspaces:read"));
            //     b.RequireProtectedResource("workspaces", "workspaces:read");
            // });
            //
            // o.AddPolicy("RealmRole", b =>
            // {
            //     // b..AddRequirements(new RealmAccessRequirement("SuperManager"));
            //     b.RequireRealmRoles("SuperManager");
            // });
            //
            // o.AddPolicy("ClientRole", b =>
            // {
            //     // b.AddRequirements(new ResourceAccessRequirement(default, "Manager"));
            //     b.RequireResourceRoles("Manager");
            // });
            // o.AddPolicy("IsAdmin", b =>
            // {
            //     b.RequireRole("r-admin");
            // });
        }).AddKeycloakAuthorization(configuration);
        services.AddSingleton<IAuthorizationPolicyProvider, ProtectedResourcePolicyProvider>();
    }
}