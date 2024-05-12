using Astrum.SharedLib.Common.Results;
using MediatR;

namespace Astrum.SharedLib.Common.CQS.Implementations;

public abstract class RequestMessageHandler<TRequest, TResponse> : MessageBaseHandler<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResult

{
}