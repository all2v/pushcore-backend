using Ardalis.Specification;
using Astrum.News.Aggregates;
using Astrum.News.Specifications;
using Astrum.SharedLib.Domain.Interfaces;
using Astrum.SharedLib.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Astrum.News.Repositories;

/// <summary>
///     Implementation of <see cref="IExampleRepository" /> which allows persistence on both EventStore and relational
///     store.
/// </summary>
public class NewsRepository : EFRepository<Post, Guid, NewsDbContext>,
    INewsRepository
{
    public NewsRepository(NewsDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context,
    specificationEvaluator)
    {
    }

    public async Task<List<Post>> IncludedListBySpecAsync(ISpecification<Post> spec, 
        CancellationToken cancellationToken = default)
    {
        var res = await ListAsync(spec, cancellationToken);
        res.ForEach(p => p.Comments = p.Comments.Where(c => c.ReplyCommentId == null).ToList());
        return res;
    }

    public async Task<Post> FirstOrDefaultBySpecAsync(ISpecification<Post> spec,
        CancellationToken cancellationToken = default)
    {
        var res = await FirstOrDefaultAsync(spec, cancellationToken);
        res.Comments = res.Comments.Where(c => c.ReplyCommentId == null).ToList();
        return res;
    }

    //TODO: may be make ToListAsync virtual
    //public async override Task<List<Post>> ToListAsync(ISpecification<Post> spec,
    //    CancellationToken cancellationToken = default)
    //{
    //    var res = await ListAsync(spec, cancellationToken);
    //    res.ForEach(p => p.Comments = p.Comments.Where(c => c.ReplyCommentId == null));
    //    return res;
    //}
}