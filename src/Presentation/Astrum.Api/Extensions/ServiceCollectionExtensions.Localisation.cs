using Astrum.Infrastructure.Resources.Services;
using Astrum.Infrastructure.Shared.Culture.Providers;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Api.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static void AddApplicationLocalisation(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "");
        services.AddScoped<ILocalizationKeyProvider, LocalizationKeyProvider>();
        services.AddScoped<ILocalizationService, LocalizationService>();
    }

    public static IApplicationBuilder UseApplicationLocalisation(this IApplicationBuilder app,
        IConfiguration configuration)
    {
        var supportedCultures = new[] {"el", "en"};
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        localizationOptions.RequestCultureProviders.Insert(0, new UserProfileRequestCultureProvider());
        app.UseRequestLocalization(localizationOptions);

        return app;
    }
}