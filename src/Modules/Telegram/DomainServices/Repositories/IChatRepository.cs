using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
using Astrum.Telegram.Domain.Aggregates;

namespace Astrum.News.Repositories;

public interface IChatRepository : IEntityRepository<TelegramChat, Guid>
{
}