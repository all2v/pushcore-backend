using MediatR;
using IResult = Astrum.Core.Common.Results.V1.IResult;

namespace Astrum.Core.Common.CQS.V2.Interfaces;

public interface IQueryHandler<in TQuery, TResponse> : IHandler<TQuery, TResponse>
    where TQuery : class, IRequest<TResponse>
    where TResponse : class, IResult
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
}