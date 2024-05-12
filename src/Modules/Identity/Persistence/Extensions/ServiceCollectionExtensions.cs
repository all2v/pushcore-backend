using Astrum.Identity.Application.Repositories;
using Astrum.Identity.Configurations;
using Astrum.Identity.Factories;
using Astrum.Identity.Managers;
using Astrum.Identity.Models;
using Astrum.Identity.Persistence.Repositories.EntityRepositories;
using Astrum.Identity.Persistence.TokenProviders;
using Astrum.Identity.Repositories;
using Astrum.Identity.Repositories.EntityRepositories;
using Astrum.Identity.Services;
using Astrum.Identity.Validators;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.SharedLib.Persistence.Extensions;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.DbContexts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Astrum.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddIdentityPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCustomizedIdentity(configuration);
        //services.AddCustomizedIdentityServer(configuration);

        services.AddIdentityServices();
    }

    public static void AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddScoped<IGitLabMappingsRepository, GitLabMappingsRepository>();
        services.AddScoped<IGitLabUsersRepository, GitLabUsersRepository>();
    }

    private static IServiceCollection AddCustomizedIdentityServer(this IServiceCollection services,
        IConfiguration configuration)
    {
        var migrationsAssembly = typeof(IdentityDbContext).Assembly.GetName().Name;
        var connectionString = configuration.GetConnectionString(DbContextExtensions.BaseConnectionName);
        var configurationSchemaName = "Identity_Configuration"; // TODO to config
        var configurationMigrationHistoryTableName = $"__{configurationSchemaName}_MigrationsHistory";
        var persistedGrantSchemaName = "Identity_PersistedGrant"; // TODO to config
        var persistedGrantMigrationHistoryTableName = $"__{persistedGrantSchemaName}_MigrationsHistory";

        services.AddIdentityServerDbPersistence(configuration);

        var keyManagementOptions = new KeyManagementOptions
        {
            // set path where to store keys
            KeyPath = "/home/shared/keys",

            // new key every 30 days
            RotationInterval = TimeSpan.FromDays(30),

            // announce new key 2 days in advance in discovery
            PropagationTime = TimeSpan.FromDays(2),

            // keep old key for 7 days in discovery for validation of tokens
            RetentionDuration = TimeSpan.FromDays(7),

            SigningAlgorithms = new[]
            {
                // RS256 for older clients (with additional X.509 wrapping)
                new SigningAlgorithmOptions(SecurityAlgorithms.RsaSha256) {UseX509Certificate = true},

                // PS256
                new SigningAlgorithmOptions(SecurityAlgorithms.RsaSsaPssSha256),

                // ES256
                new SigningAlgorithmOptions(SecurityAlgorithms.EcdsaSha256)
            }
        };

        services.AddIdentityServer(options =>
            {
                options.EmitStaticAudienceClaim = false;
                options.Authentication.CoordinateClientLifetimesWithUserSession = true;
                options.Events = new EventsOptions
                {
                    RaiseErrorEvents = true,
                    RaiseInformationEvents = true,
                    RaiseFailureEvents = true,
                    RaiseSuccessEvents = true
                };
                options.ServerSideSessions.UserDisplayNameClaimType = "name";
                options.KeyManagement = keyManagementOptions;
            })
            .AddConfigurationStore<ConfigurationDbContext>(options =>
            {
                options.ConfigureDbContext = b => b.UseNpgsql(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly)
                        .MigrationsHistoryTable(configurationMigrationHistoryTableName, configurationSchemaName));

                // this enables automatic token cleanup. this is optional.
                options.DefaultSchema = configurationSchemaName;
                options.EnablePooling = true;
            })
            .AddConfigurationStoreCache()
            .AddOperationalStore<PersistedGrantDbContext>(options =>
            {
                options.ConfigureDbContext = b => b.UseNpgsql(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly)
                        .MigrationsHistoryTable(persistedGrantMigrationHistoryTableName, persistedGrantSchemaName));
                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
                options.DefaultSchema = persistedGrantSchemaName;
                options.EnablePooling = true;
            })
            .AddServerSideSessions()
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<CustomProfileService>()
            .AddResourceOwnerValidator<UserValidator>();

        var a = configuration.GetRequiredSection("IdentityServer");

        services.Configure<IdentityServerConfigurationOptions>(configuration.GetSection("IdentityServer"));

        return services;
    }

    public static void AddIdentityServerDbPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbContextInitializer, IdentityServerDbContextInitializer>();
    }

    public static void AddIdentityDbPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddMediatR(typeof(IdentityDbContext));
        services.AddCustomDbContext<IdentityDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        services.AddScoped<IDbContextInitializer, IdentityDbContextInitializer>();
    }

    public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddIdentityDbPersistence(configuration);

        var passwordOptions = new PasswordOptions
        {
            RequireDigit = false,
            RequiredLength = 4, // TODO to 10
            RequireNonAlphanumeric = false,
            RequireUppercase = false,
            RequireLowercase = false,
            RequiredUniqueChars = 0
        };
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password = passwordOptions;
                //options.ClaimsIdentity.UserNameClaimType = JwtRegisteredClaimNames.Sub;
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<TelegramIntegrationTokenProvider<ApplicationUser>>("TelegramIntegration")
            .AddUserManager<ApplicationUserManager>()
            .AddSignInManager<ApplicationSignInManager>()
            .AddRoleManager<ApplicationRoleManager>();
        //.AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password = passwordOptions;
        });

        services.AddScoped<UserManager<ApplicationUser>, ApplicationUserManager>();
        services.AddScoped<SignInManager<ApplicationUser>, ApplicationSignInManager>();
        services.AddScoped<RoleManager<ApplicationRole>, ApplicationRoleManager>();

        return services;
    }
}