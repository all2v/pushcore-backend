using Astrum.SharedLib.Common.CQS.Interfaces;
using Astrum.SharedLib.Common.Results;

namespace Astrum.SharedLib.Common.CQS.Implementations;


public abstract class Query<TResponse> : IQuery<TResponse> where TResponse : class, IResult
{
}

public abstract class QueryResult<TData> : Query<Result<TData>> where TData : class
{
}

// public abstract class PagedQuery<TData> : Query<PagedResult<TData>> where TData : class
// {
// }