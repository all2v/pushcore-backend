using System;
using Astrum.Api.Extensions;
using Astrum.Api.Middleware;
using Astrum.Api.Models;
using Astrum.Infrastructure;
using Astrum.Infrastructure.Modules;
using Astrum.Infrastructure.Resources.Services;
using Astrum.Infrastructure.Services;
using Astrum.Infrastructure.Shared.Culture.Providers;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Application.Extensions;
using Astrum.SharedLib.Persistence.Extensions;
using Astrum.TrackerProject.Application.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpRaven;
using GlobalConfiguration = Astrum.Infrastructure.GlobalConfiguration;

namespace Astrum.Api;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;
        HostEnvironment = webHostEnvironment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment HostEnvironment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        GlobalConfiguration.WebRootPath = HostEnvironment.WebRootPath;
        GlobalConfiguration.ContentRootPath = HostEnvironment.ContentRootPath;
        //IdentityModelEventSource.ShowPII = true;

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddHttpContextAccessor();

        //ROUTING
        services.AddRouting(options => options.LowercaseUrls = true);

        //LOCALIZATION
        services.AddApplicationLocalisation();

        //CACHING
        services.AddResponseCaching();

        services.AddCustomSwagger(Configuration);
        //services.AddApplicationSwagger(Configuration);

        // TODO specify after docker-compose initiating
        services.AddCors(options =>
        {
            // this defines a CORS policy called "default"
            options.AddPolicy(Cors.Policy, policy =>
            {
                // policy.WithOrigins("https://localhost:5003")
                policy.WithOrigins("https://localhost:3000", "https://git.66bit.ru")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        services.AddTaskTracking(Configuration);

        services.AddCustomAuthentication(Configuration);
        //services.AddCustomAuthorization(Configuration);
        services.AddCustomizedMvc();

        services.AddSwaggerGenNewtonsoftSupport();
        services.AddGraphQLServer()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
            .ConfigureSchema(sb => sb.ModifyOptions(opts => opts.StrictValidation = false));

        services.AddInMemorySubscriptions();
        // services.AddRedisSubscriptions((sp) => ConnectionMultiplexer.Connect("localhost:7003"));
        services.AddLogging(config =>
        {
            config.AddDebug();
            config.AddConsole();
        });
        services.AddApplicationServices();
        services.AddInfrastructure(Configuration);
        services.AddGitLabAuthentication(Configuration.GetSection("Gitlab"));
        // services.AddKeycloak(Configuration.GetSection("Oidc"));

        //services.Configure<GitlabSettings>(Configuration.GetSection("Gitlab"));
        // services.Configure<OpenIdConnectOptions>(Configuration.GetSection("Oidc"));

        services.AddScoped<ILocalizationKeyProvider, LocalizationKeyProvider>();

        services.AddSingleton<IRavenClient>(new RavenClient(Configuration["Sentry:Dsn"]));
        services.AddScoped<SentryService>();

        var postgreOptions = new PostgreSqlStorageOptions
        {
            InvisibilityTimeout = TimeSpan.FromMinutes(15)
        };
        services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString(DbContextExtensions.BaseConnectionName), postgreOptions));

        // services.AddScoped<UserHelper>();

        // services.AddScoped<SlugRouteValueTransformer>();
        // services.AddSingleton(services);
        services.Configure<ApiBehaviorOptions>(options => options.SuppressInferBindingSourcesForParameters = true); // TODO to each module?

        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddAntDesign();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        app.UseHttpLogging();

        if (env.IsDevelopment() || env.EnvironmentName == "DockerCompose" || env.IsProduction())
        {
            app.UseDeveloperExceptionPage();
            app.UseApplicationSwagger(configuration);
            //app.UseHttpsRedirection();
        }

        if (env.EnvironmentName == "Local")
        {
            app.UseApplicationSwagger(configuration);
            app.UseFileServer();
        }

        var basePath = configuration.GetValue<string>("BasePath");
        if (!string.IsNullOrWhiteSpace(basePath))
            app.UsePathBase(basePath);

        app.UseCors(Cors.Policy);
        app.UseRouting();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseAuthentication();
        app.UseAuthorization();

        //app.UseMustChangePassword();

        app.UseApplicationLocalisation(configuration);

        app.UseResponseCaching();

        app.UseStaticFiles();
        app.UseSentry();
        
        var moduleInitializers = app.ApplicationServices.GetServices<IModuleInitializer>();
        foreach (var moduleInitializer in moduleInitializers)
            moduleInitializer.Configure(app, env);

        app.UseHangfireServer();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGraphQL("/graphql");
            // endpoints.MapDynamicControllerRoute<SlugRouteValueTransformer>("/{**slug}");
            // endpoints.MapControllerRoute(
            //     "areas",
            //     "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            // endpoints.MapControllerRoute(
            // "default",
            // "{controller=Home}/{action=Index}/{id?}")
            endpoints.MapControllers()/*.RequireAuthorization()*/;
            endpoints.MapRazorPages();
            endpoints.MapBlazorHub();
            endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "Astrun Jobs",
                Authorization = new[]
                {
                    new HangfireAuthorizationFilter("Admin", "SuperAdmin")
                }
            });
            endpoints.MapFallbackToPage("/_Host");
            // .RequireAuthorization("ApiScope");
        });

        RecurringJob.AddOrUpdate<ISynchronisationService>(task => task.SynchroniseArticles(),
            cronExpression: Cron.HourInterval(1), queue: "default");
        RecurringJob.AddOrUpdate<ISynchronisationService>(task => task.SynchroniseIssues(),
            cronExpression: Cron.HourInterval(1), queue: "default");
        RecurringJob.AddOrUpdate<ISynchronisationService>(task => task.SynchroniseProjects(),
            cronExpression: Cron.Daily(1), queue: "default");
    }
}