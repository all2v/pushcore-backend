using MediatR;

namespace Astrum.SharedLib.Application.Contracts.Infrastructure;

public interface IServiceBus
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : class;
}