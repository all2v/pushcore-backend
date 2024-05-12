using Astrum.SharedLib.Common.Results;
using Astrum.Telegram.Application.ViewModels;

namespace Astrum.Telegram.Services
{
    public interface ITelegramChatService
    {
        Task<Result<List<ChatForm>>> GetChats(CancellationToken cancellationToken = default);
        Task<Result<ChatForm>> GetChatById(Guid id, CancellationToken cancellationToken = default);
        Task<Result<ChatForm>> DeleteChat(Guid id, CancellationToken cancellationToken = default);
    }
}
