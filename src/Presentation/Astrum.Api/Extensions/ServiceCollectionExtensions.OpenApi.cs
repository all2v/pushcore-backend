using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Astrum.Identity.Configurations;
using Astrum.Infrastructure.Shared.Filters;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Astrum.Api.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static void AddApplicationSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        KeycloakAuthenticationOptions keycloakOptions = new();

        configuration
            .GetSection(KeycloakAuthenticationOptions.Section)
            .Bind(keycloakOptions, opt => opt.BindNonPublicProperties = true);

        services.Configure<KeycloakAuthenticationOptions>(x => configuration
            .GetSection(KeycloakAuthenticationOptions.Section)
            .Bind(x, opt => opt.BindNonPublicProperties = true));


        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.OperationFilter<TagByAreaNameOperationFilter>();
            options.OperationFilter<AuthResponsesOperationFilter>();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Astrum API",
                Description = "Astrum API Description",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Astrum",
                    Email = string.Empty,
                    // TODO change it
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://example.com/license")
                }
            });
            options.ResolveConflictingActions(a => a.First());

            // options.DocInclusionPredicate((docName, description) => true);

            // Define the Bearer auth scheme that's in use
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // must be lower case
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            var openApiSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "Auth",
                Type = SecuritySchemeType.OAuth2,
                Reference = new OpenApiReference
                {
                    Id = "OAuth",
                    Type = ReferenceType.SecurityScheme
                },
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{keycloakOptions.KeycloakUrlRealm}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{keycloakOptions.KeycloakUrlRealm}/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>(),
                    }
                }
            };
            // options.AddSecurityDefinition(openApiSecurityScheme.Reference.Id, openApiSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {jwtSecurityScheme, Array.Empty<string>()},
                // {openApiSecurityScheme, Array.Empty<string>()},
            });

            // var authority = configuration["Authentication:JwtBearer:Authority"];
            // // Scheme Definition 
            // options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            // {
            //     Type = SecuritySchemeType.OAuth2,
            //
            //     Flows = new OpenApiOAuthFlows
            //     {
            //         AuthorizationCode = new OpenApiOAuthFlow
            //         {
            //             AuthorizationUrl = new Uri(authority + "/connect/authorize"),
            //             TokenUrl = new Uri(authority + "/connect/token"),
            //             Scopes =
            //             {
            //                 {"astrum.api", "ASTRUM API - full access"}
            //             }
            //         }
            //     }
            // });
            // // Apply Scheme globally
            // options.AddSecurityRequirement(new OpenApiSecurityRequirement
            // {
            //     {
            //         new OpenApiSecurityScheme
            //         {
            //             Reference = new OpenApiReference
            //                 {Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme}
            //         },
            //         new[] {"astrum.api"}
            //     }
            // });


            //add summaries to swagger
            var canShowSummaries = configuration.GetValue<bool>("Swagger:ShowSummaries");
            if (canShowSummaries)
            {
                var assemblyLocation = Assembly.GetExecutingAssembly().Location;
                var assemblyFolder = Path.GetDirectoryName(assemblyLocation);
                var xmlDocFiles = Directory.GetFiles(assemblyFolder, "*.Backoffice.xml");
                foreach (var xmlDocFile in xmlDocFiles)
                    options.IncludeXmlComments(xmlDocFile);
            }
        });
        services.AddSwaggerGenNewtonsoftSupport();
    }

    public static IApplicationBuilder UseApplicationSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        IdentityServerConfigurationOptions identityServerOptions = new();
        
        configuration
            .GetSection("IdentityServer")
            .Bind(identityServerOptions, opt => opt.BindNonPublicProperties = true);
        
        var ISClient = identityServerOptions.Clients.FirstOrDefault();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // options.SwaggerEndpoint("v1/swagger.json", "Astrum API V1");
            // options.OAuthUsePkce();
            // options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            // options.OAuthScopes("astrum.api");
            // options.OAuthAppName("Astrum API V1");
            // options.OAuthClientId(null);
            // options.OAuthClientSecret(null);
            // options.OAuthUsername("AstrumUsername");
            // options.ShowExtensions();
            // options.EnableFilter();
            // options.EnableValidator();
            // options.EnableDeepLinking();
            // options.EnablePersistAuthorization();
            // options.EnableTryItOutByDefault();

            options.SwaggerEndpoint("v1/swagger.json", "Astrum API V1");
            options.OAuthUsePkce();
            options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            options.OAuthScopes("astrum.api");
            options.OAuthAppName("Astrum API V1");
            options.OAuthClientId(ISClient.ClientId);
            options.OAuthClientSecret(ISClient.ClientSecrets?.FirstOrDefault()?.Value);
            options.OAuthUsername("AstrumUsername");
            options.ShowExtensions();
            options.EnableFilter();
            options.EnableValidator();
            options.EnableDeepLinking();
            options.EnablePersistAuthorization();
            options.EnableTryItOutByDefault();
        });

        return app;
    }
}