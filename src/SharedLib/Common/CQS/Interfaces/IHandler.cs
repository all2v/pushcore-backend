using Astrum.SharedLib.Common.Results;
using MediatR;

namespace Astrum.SharedLib.Common.CQS.Interfaces;

/// <summary>
///     Abstraction over a handler of messages coming from a message broker.
///     Should implement any interfaces to comply with any given message brokers handler object
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResult
{
}