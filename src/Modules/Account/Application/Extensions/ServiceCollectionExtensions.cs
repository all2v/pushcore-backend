using System.Reflection;
using Astrum.Account.Application.Services;
using Astrum.Account.DomainServices.Extensions;
using Astrum.Account.Features.Achievement.Commands.AchievementCreate;
using Astrum.Account.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Account.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        services.AddDomainServices();
        services.AddMediatR(currentAssembly);
        services.AddAutoMapper(x => x.AddMaps(currentAssembly));
        services.AddTransient<IAchievementService, AchievementService>();
        services.AddTransient<IUserProfileService, UserProfileService>();
        services.AddTransient<IUserAchievementService, UserAchievementService>();
        services.AddTransient<IContactsService, ContactsService>();
        services.AddTransient<ISocialNetworksService, SocialNetworksService>();
        services.AddTransient<IRegistrationApplicationService, RegistrationApplicationService>();
        services.AddTransient<ITimelineService, TimelineService>();
        services.AddTransient<ICustomFieldService, CustomFieldService>();
        services.AddTransient<IPositionsService, PositionsService>();
        services.AddTransient<ITransactionService, TransactionService>();
    }
}