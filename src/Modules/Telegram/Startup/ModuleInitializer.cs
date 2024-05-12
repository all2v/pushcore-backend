using Astrum.Infrastructure.Modules;
using Astrum.Telegram.Extensions;
using Astrum.Telegram.Infrastructure;
using Astrum.Telegram.Mappings;
using Astrum.Telegram.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Astrum.Telegram;

public class ModuleInitializer : IModuleInitializer
{
    #region IModuleInitializer Members

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var telegramSection = configuration.GetSection("Telegram");
        services.Configure<TelegramOptions>(telegramSection);
        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(telegramSection["Token"]));
        services.AddHostedService<TelegramBotInitializer>();
        services.AddScoped<ISubscrubitionManager, SubscrubitionManager>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<ITelegramChatService, TelegramChatService>();
        services.AddApplicationServices();
        services.AddPersistenceServices(configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }

    #endregion
}