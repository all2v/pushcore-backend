using Astrum.Core.Common.CQS.V2.Interfaces;
using MediatR;
using IResult = Astrum.Core.Common.Results.V1.IResult;

namespace Astrum.Core.Common.CQS.V2.Implementations;

public abstract class BaseHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResult
{
    #region IHandler<TRequest,TResponse> Members

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);

    #endregion
}