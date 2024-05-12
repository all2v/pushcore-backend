using Ardalis.Specification;
using Astrum.News.Aggregates;
using Astrum.SharedLib.Domain.Interfaces;
using Astrum.SharedLib.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Astrum.News.Repositories;

/// <summary>
///     Implementation of <see cref="IExampleRepository" /> which allows persistence on both EventStore and relational
///     store.
/// </summary>
public class AttachmentsRepository : EFRepository<PostFileAttachment, Guid, NewsDbContext>,
    IAttachmentsRepository
{
    public AttachmentsRepository(NewsDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context,
    specificationEvaluator)
    {
    }
}