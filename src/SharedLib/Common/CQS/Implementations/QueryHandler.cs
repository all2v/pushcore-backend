using Astrum.SharedLib.Common.CQS.Interfaces;
using Astrum.SharedLib.Common.Results;

namespace Astrum.SharedLib.Common.CQS.Implementations;

public abstract class QueryHandler<TQuery, TResponse> : RequestMessageHandler<TQuery, TResponse>,
    IQueryHandler<TQuery, TResponse>
    where TQuery : class, IQuery<TResponse>
    where TResponse : class, IResult
{
    public override abstract Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
}

public abstract class QueryResultHandler<TQuery, TData> : QueryHandler<TQuery, Result<TData>>
    where TQuery : QueryResult<TData>
    where TData : class
{
}

// public abstract class QueryPagedResultHandler<TQuery, TData> : QueryHandler<TQuery, PagedResult<TData>>
//     where TQuery : PagedQuery<TData>
//     where TData : class
// {
// }
