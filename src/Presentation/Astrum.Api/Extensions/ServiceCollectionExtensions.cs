using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Reflection;
using System.Threading.Tasks;
using Ardalis.RouteAndBodyModelBinding;
using Astrum.Api.Configurations;
using Astrum.Infrastructure;
using Astrum.Infrastructure.Extensions;
using Astrum.Infrastructure.Integrations.YouTrack.Mappings;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.Logging;
using Astrum.Logging.Repositories;
using Astrum.Logging.Services;
using Astrum.Infrastructure.Resources.Services;
using Astrum.Infrastructure.Shared.Filters;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Common.Options;
using Astrum.SharedLib.Persistence.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;
using Astrum.Infrastructure.Integrations.YouTrack.Services;

namespace Astrum.Api.Extensions;

/// <summary>
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSharedServices();
        services.AddInfrastructureServices(configuration);
        
        services.AddEmailOptions(configuration);
        services.AddValidatorsFromAssemblyContaining<Program>();
        
        services.AddScoped<ITaskTrackerService, YouTrackService>();
        services.AddScoped<ITrackerRequestService, YoutrackRequestService>();
        //services.AddScoped<IAuthenticationStateAccessor, AuthenticationStateAccessor>()
        services.AddBlazoredLocalStorage();
    }

    private static void AddEmailOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOptions>(x => configuration.GetSection("Email").Bind(x));
    }

    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomizedMvc(this IServiceCollection services)
    {
        var modules = GlobalConfiguration.Modules;
        services
            .AddControllers(options =>
            {
                options.UseRoutePrefix("api");
                //options.Filters.Add(new AuthorizeAttribute());
                //var policy = new AuthorizationPolicyBuilder()
                //            .RequireAuthenticatedUser().AddAuthenticationSchemes("Bearer").Build();
                //options.Filters.Add(new AuthorizeFilter(policy));
                options.ModelBinderProviders.InsertRouteAndBodyBinding();
                // options.Conventions.Add(new DashedRoutingConvention());
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            })
            .AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            // .AddDataAnnotationsLocalization(options =>
            // {
            //     options.DataAnnotationLocalizerProvider = (type, factory) =>
            //     {
            //         var assemblyName = new AssemblyName(typeof(SharedResources).GetTypeInfo().Assembly.FullName);
            //         return factory.Create("SharedResources", assemblyName.Name);
            //     };
            // })
            .LoadModules(modules); // TODO to test
        services.AddEndpointsApiExplorer();

        return services;
    }

    private static IServiceCollection AddApplicationCookie(this IServiceCollection services)
    {
        var cookieAuthenticationEvents = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = async context =>
            {
                // TODO to api.domain.ru/v1/...
                if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase) &&
                    context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("test response with exception"); // TODO 
                }
            },
            OnRedirectToAccessDenied = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase) &&
                    context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Task.CompletedTask;
                }

                return Task.CompletedTask;
            }
        };

        services.ConfigureApplicationCookie(options =>
        {
            options.ForwardAuthenticate = JwtBearerDefaults.AuthenticationScheme;
            // options.AccessDeniedPath = "/Shared/AccessDenied";
            // options.Cookie.Name = "Astrum.AUTH";
            // options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            // options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            // options.SlidingExpiration = true;
            // options.LoginPath = new PathString("/auth/login");
            // options.LogoutPath = new PathString("/logoff");
            options.Events = cookieAuthenticationEvents;
        });
        return services;
    }

    //public static ConfigureHostBuilder ConfigureLogger(this ConfigureHostBuilder builder)
    //{
    //    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    //    var basePath = Directory.GetCurrentDirectory();
    //    var configuration = AppConfigurations.Get(basePath, environment);

    //    Log.Logger = new LoggerConfiguration()
    //        .ReadFrom.Configuration(configuration)
    //        // .EnrichWithEventType()
    //        // .Enrich.WithProperty("Version", ReflectionUtils.GetAssemblyVersion<Program>());
    //        .CreateBootstrapLogger();

    //    builder.UseSerilog((context, services, loggerConfiguration) =>
    //        loggerConfiguration
    //            .ReadFrom.Configuration(context.Configuration)
    //            .ReadFrom.Services(services));

    //    return builder;
    //}

    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var audience = configuration["Authentication:JwtBearer:Audience"];

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddCookie(options =>
            //{
            //    options.Events.OnRedirectToAccessDenied =
            //        options.Events.OnRedirectToLogin = c =>
            //        {
            //            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //            return Task.FromResult<object>(null);
            //        };
            //})
            //.AddCookie()
            .AddJwtBearer(options =>
            {
                //options.Authority = configuration["Authentication:JwtBearer:Authority"];
                //options.Audience = audience;
                // options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = false,
                    //ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],

                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                     new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = false,
                    //ValidAudience = audience,
                    //ValidAudiences = new[] {audience},
                    //ValidTypes = new[] { "at+jwt" },
                    // Validate the token expiry
                    // ValidateLifetime = true
                    // If you want to allow a certain amount of clock drift, set that here
                    // ClockSkew = TimeSpan.Zero
                };
            });
            //.AddGoogleOpenIdConnect(options =>
            //{
            //    options.ClientId = configuration.GetSection("Google-Auth:client_id").Value;
            //    options.ClientSecret = configuration.GetSection("Google-Auth:client_secret").Value;
            //})
        // services.AddAuthentication(options =>
        //     {
        //         // custom scheme defined in .AddPolicyScheme() below
        //         options.DefaultScheme = "JWT_OR_COOKIE";
        //         options.DefaultChallengeScheme = "JWT_OR_COOKIE";
        //     })
        //     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        //     {
        //         options.LoginPath = "/login";
        //         options.ExpireTimeSpan = TimeSpan.FromDays(1);
        //     })
        //     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        //     {
        //         options.Authority = configuration["Authentication:JwtBearer:Authority"];
        //         options.Audience = audience;
        //         options.RequireHttpsMetadata = false;
        //         options.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             // Validate the JWT Issuer (iss) claim
        //             ValidateIssuer = true,
        //             ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],
        //
        //             // The signing key must match!
        //             // ValidateIssuerSigningKey = true,
        //             // IssuerSigningKey =
        //             // new SymmetricSecurityKey(
        //             // Encoding.UTF8.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),
        //
        //             // Validate the JWT Audience (aud) claim
        //             ValidateAudience = true,
        //             ValidAudience = audience,
        //             ValidAudiences = new[] {audience},
        //             ValidTypes = new[] {"at+jwt"},
        //             // Validate the token expiry
        //             ValidateLifetime = true
        //             // If you want to allow a certain amount of clock drift, set that here
        //             // ClockSkew = TimeSpan.Zero
        //         };
        //     })
        //     .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
        //     {
        //         // runs on each request
        //         options.ForwardDefaultSelector = context =>
        //         {
        //             // filter by auth type
        //             string authorization = context.Request.Headers[HeaderNames.Authorization];
        //             if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
        //                 return JwtBearerDefaults.AuthenticationScheme;
        //
        //             // otherwise always check for cookie auth
        //             return CookieAuthenticationDefaults.AuthenticationScheme;
        //         };
        //     });
        // .AddGoogle(options =>
        // {
        //     options.SignInScheme = IdentityServerConstants.JwtRequestClientKey;
        //
        //     options.ClientId = configuration["Authentication:Google:ClientId"];
        //     options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        // });

        //services.AddApplicationCookie();
    }

    public static void AddTaskTracking(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<TrackerProjectProfile>();
            cfg.AddProfile<TrackerArticleProfile>();
        });
    }

    public static void AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var apiAudience = configuration["Authentication:JwtBearer:Audience"];
        services.AddAuthorization(options =>
        {
            //options.FallbackPolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();
            //options.AddPolicy("ApiScope", policy =>
            //{
            //    policy.RequireAuthenticatedUser();
            //    policy.RequireClaim("scope", apiAudience);
            //});
        });
    }

    public static void AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
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
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            var authority = configuration["Authentication:JwtBearer:Authority"];
            // Scheme Definition 
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,

                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(authority + "/connect/authorize"),
                        TokenUrl = new Uri(authority + "/connect/token"),
                        Scopes =
                         {
                             {"astrum.api", "ASTRUM API - full access"}
                         }
                    }
                }
            });
            // Apply Scheme globally
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                             {Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme}
                     },
                     new[] {"astrum.api"}
                 }
             });


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
}
