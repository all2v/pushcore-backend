using Astrum.Account.Controllers;
using Astrum.Account.Mappers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Astrum.Account.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBackofficeServices(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(AchievementProfile));
            config.AddMaps(typeof(UserProfileProfile));
            config.AddMaps(typeof(RegistrationApplicationProfile));
        });

        services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}