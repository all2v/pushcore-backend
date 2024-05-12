using Astrum.Telegram.Infrastructure;
using Astrum.Telegram.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Astrum.Telegram;

internal class TelegramBotInitializer : IHostedService
{
    private readonly ITelegramBotClient telegramBotClient;
    private readonly IServiceProvider serviceProvider;
    //private readonly ILogger logger;
    private readonly IOptions<TelegramOptions> options;
    private readonly CancellationTokenSource cancellationTokenSource = new();
//ILogger<TelegramBotInitializer> logger, 
    public TelegramBotInitializer(ITelegramBotClient telegramBotClient, IServiceProvider serviceProvider,
        IOptions<TelegramOptions> options)
    {
        this.telegramBotClient = telegramBotClient;
        this.serviceProvider = serviceProvider;
        //this.logger = logger;
        this.options = options;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var defaultHandler = new DefaultUpdateHandler(HandleUpdate, HandleError);
        if (options.Value.Receive)
        {
            telegramBotClient.StartReceiving(defaultHandler, cancellationToken: cancellationTokenSource.Token);
        }
        return Task.CompletedTask;
    }

    private async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var bot = scope.ServiceProvider.GetService<IMessageService>();
            await bot?.HandleUpdate(update);
        }
        catch (Exception e)
        {
            await HandleError(client, e, cancellationToken);
        }
    }

    private Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        //logger.LogError(exception, exception.Message);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
        return Task.CompletedTask;
    }
}
