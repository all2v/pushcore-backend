using Astrum.Identity.Managers;
using Astrum.Identity.Models;
using Astrum.News.Repositories;
using Astrum.Telegram.Application.ViewModels.Results;
using Astrum.Telegram.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Astrum.Telegram.Services;

public class SubscrubitionManager : ISubscrubitionManager
{
    private readonly IChatRepository сhatRepository;
    private readonly ITelegramBotClient telegramBotClient;
    private readonly ApplicationUserManager userManager;

    public SubscrubitionManager(IChatRepository сhatRepository, ITelegramBotClient telegramBotClient,
        ApplicationUserManager userManager)
    {
        this.сhatRepository = сhatRepository;
        this.telegramBotClient = telegramBotClient;
        this.userManager = userManager;
    }

    public async Task<ActionResult> SubscribeCompanyChat(string userId, string securityHash, long chatId, string chatName, long userChatId)
    {
        var result = new ActionResult();
        var chatWithThisId = await сhatRepository.FirstOrDefaultAsync(c => c.ChatId == chatId);
        if (chatWithThisId != null) 
        {
            result.Errors.Add("Ошибка: чат уже добавлен!");
            return result;
        }
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            result.Errors.Add("Ошибка: не удалось найти пользователя!");
            return result;
        }
        if (!await CheckSecurityHash(user, securityHash, "chatSubscripition")) 
        {
            result.Errors.Add("Ошибка: Неверный токен!");
            return result;
        }

        var newChat = new TelegramChat { ChatId = (int)chatId, Name = chatName };
        await сhatRepository.AddAsync(newChat);
        await сhatRepository.UnitOfWork.SaveChangesAsync();

        result.Succeed = true;
        return result;
    }

    private async Task<bool> CheckSecurityHash(ApplicationUser user, string securityHash, string purpose)
    {
        return await userManager.VerifyUserTokenAsync(user, "TelegramIntegration", purpose, securityHash);
    }
}
