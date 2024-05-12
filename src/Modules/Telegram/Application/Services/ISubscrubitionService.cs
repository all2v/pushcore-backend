using Astrum.Telegram.Application.ViewModels.Results;

namespace Astrum.Telegram.Services
{
    public interface ISubscrubitionManager
    {
        Task<ActionResult> SubscribeCompanyChat(string userId, string securityHash, long chatId, string chatName, long userChatId);
    }
}
