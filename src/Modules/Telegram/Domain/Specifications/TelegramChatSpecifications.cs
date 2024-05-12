using Ardalis.Specification;
using Astrum.Telegram.Domain.Aggregates;

namespace Astrum.Telegram.Domain.Specifications
{
    public class GetTelegramChatsSpec : Specification<TelegramChat> { }
    public class GetTelegramChatById : GetTelegramChatsSpec
    {
        public GetTelegramChatById(Guid id)
        {
            Query
                .Where(chat => chat.Id == id);
        }
    }
}
