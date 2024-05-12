using Ardalis.Specification;
using Astrum.SharedLib.Domain.Interfaces;
using Astrum.SharedLib.Persistence.Repositories;
using Astrum.Telegram.Domain.Aggregates;
using Astrum.Telegram.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Astrum.News.Repositories;

/// <summary>
///     Implementation of <see cref="IExampleRepository" /> which allows persistence on both EventStore and relational
///     store.
/// </summary>
public class ChatRepository : EFRepository<TelegramChat, Guid, TelegramDbContext>,
    IChatRepository
{
    public ChatRepository(TelegramDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context,
    specificationEvaluator)
    {
    }
}