using Astrum.SharedLib.Common.Results;

namespace Astrum.SharedLib.Common.CQS.Interfaces;

public interface IQueryHandler<in TQuery, TResponse> : IHandler<TQuery, TResponse>
    where TQuery : class, IQuery<TResponse>
    where TResponse : class, IResult
{
}