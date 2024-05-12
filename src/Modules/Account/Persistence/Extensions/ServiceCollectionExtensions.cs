using Astrum.Account.Application.Repositories;
using Astrum.Account.Persistence.Repositories;
using Astrum.Account.Repositories;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Account.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomDbContext<AccountDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        services.AddScoped<IDbContextInitializer, DbContextContextInitializer>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
        services.AddScoped<IAchievementRepository, AchievementRepository>();
        services.AddScoped<ISocialNetworksRepository, SocialNetworksRepository>();
        services.AddScoped<ITimelineRepository, TimelineRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IRegistrationApplicationRepository, RegistrationApplicationRepository>();
        services.AddScoped<ICustomFieldRepository, CustomFieldRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }
}