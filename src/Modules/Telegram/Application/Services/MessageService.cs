using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Astrum.Telegram.Services;

public class MessageService : IMessageService
{
    private readonly ITelegramBotClient telegramBotClient;
    private readonly ISubscrubitionManager subscrubitionManager;
    //private readonly ILogger logger;

    private static long? botId;
    public MessageService(ITelegramBotClient telegramBotClient, ISubscrubitionManager subscrubitionManager)
        //ILogger<MessageService> logger)
    {
        this.telegramBotClient = telegramBotClient;
        this.subscrubitionManager = subscrubitionManager;
        //this.logger = logger;
    }

    public async Task HandleUpdate(Update update)
    {
        if (update.Type is UpdateType.MyChatMember)
        {
            await HandleNewChatMember(update.ChatMember);
        }
        else if (update.Type is UpdateType.Message or UpdateType.EditedMessage)
        {
            await HandleMessage(update.Message ?? update.EditedMessage);
        }
    }

    private async Task HandleNewChatMember(ChatMemberUpdated? update)
    {
        if (update is null) return;

        if (botId is null)
        {
            var me = await telegramBotClient.GetMeAsync();
            botId = me.Id;
        }
        if (botId == update.NewChatMember.User.Id && update.NewChatMember.Status == ChatMemberStatus.Member)
        {
            //var instruction = subscrubitionManager.CreateSubscribeInstruction(update.Chat.Type);
            //if (instruction != null)
            //{
            //    await SendMessage(update.Chat.Id, instruction);
            //}
        }
    }

    private async Task HandleMessage(Message? message)
    {
        if (message is null) return;

        var chat = message.Chat;

        if (message.Text?.StartsWith("/start") ?? false)
        {
            string msg = "";
            if (message.Chat.Id < 0)//chatId < 0 - значит это групповой чат.
            {
                var subscriptionStatusMsg = await CheckIfCompanySubscriptionRequestAndSubscribe(message);
                if (subscriptionStatusMsg != null)
                    msg = subscriptionStatusMsg;
            }
            if (msg != "")
                await telegramBotClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: msg,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }
        else if (message.ReplyToMessage != null)
        {
            //todo
        }
    }

    private async Task<string> CheckIfCompanySubscriptionRequestAndSubscribe(Message message)
    {
        var valid = CheckValidationMessage(message.Text);
        if(valid == false)
            return "Чат не привязан - Токен не валиден!";
        var splittedCommand = message.Text.Split(" ");
        var command = splittedCommand[1];
        var guidLength = 36;
        var userId = command.Substring(0, guidLength);
        var securityHash = command.Substring(guidLength);
        var result = await subscrubitionManager.SubscribeCompanyChat(userId,
            securityHash, message.Chat.Id, message.Chat.Title, message.From.Id);
        if (!result.Succeed)
            return string.Join(", ", result.Errors);
        return "Чат успешно подписан на уведомления!";
    }

    public bool CheckValidationMessage(string text)
    {
        var splittedCommand = text.Split(" ");
        if (splittedCommand.Length != 2)
            return false;
        var command = splittedCommand[1];
        var guidLength = 36;
        if (command.Length < guidLength)
            return false;
        else
            return true;
    }

    public async Task<int> SendMessage(long chatId, string text)
    {
        try
        {
            var message = await telegramBotClient.SendTextMessageAsync(chatId, text, ParseMode.Html);
            return message?.MessageId ?? 0;
        }
        catch (Exception ex)
        {
            //logger.LogError(ex.Message);
            return 0;
        }
    }

    public async Task<int> SendPhoto(long chatId, Stream photoStr, string photoName, string text)
    {
        try
        {
            var file = new InputOnlineFile(photoStr, photoName);
            var message = await telegramBotClient.SendPhotoAsync(chatId, file, text, ParseMode.Html);
            return message?.MessageId ?? 0;
        }
        catch (Exception ex)
        {
            //logger.LogError(ex.Message);
            return 0;
        }
    }

    public async Task<int> SendPhotoes(long chatId, List<(Stream, string)> photoes)
    {
        try
        {
            var files = photoes.Select(p => new InputMediaPhoto(new InputMedia(p.Item1, p.Item2)));
            var messages = await telegramBotClient.SendMediaGroupAsync(chatId, files);
            return messages?.FirstOrDefault()?.MessageId ?? 0;
        }
        catch (Exception ex)
        {
            //logger.LogError(ex.Message);
            return 0;
        }
    }
}
