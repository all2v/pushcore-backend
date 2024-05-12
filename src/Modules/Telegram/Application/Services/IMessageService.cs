using Telegram.Bot.Types;

namespace Astrum.Telegram.Services
{
    public interface IMessageService
    {
        Task HandleUpdate(Update update);
        Task<int> SendMessage(long chatId, string text);
        Task<int> SendPhoto(long chatId, Stream photoStr, string photoName, string text);
        Task<int> SendPhotoes(long chatId, List<(Stream, string)> photoes);
    }
}
