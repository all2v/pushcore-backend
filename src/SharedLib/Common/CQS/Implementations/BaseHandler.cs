using Astrum.SharedLib.Common.CQS.Interfaces;
using Astrum.SharedLib.Common.Results;
using MediatR;

namespace Astrum.SharedLib.Common.CQS.Implementations;

public abstract class BaseHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResult
{
    #region IHandler<TRequest,TResponse> Members

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);

    #endregion
}