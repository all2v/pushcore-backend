using Astrum.SharedLib.Application.Contracts.Infrastructure;
using MediatR;
using IMediator = MassTransit.Mediator.IMediator;

namespace Astrum.Infrastructure.Services;

/// <summary>
///     Abstraction over the implementation specifics of a message broker transmission
///     Concrete implementation of <see cref="IServiceBus" /> which uses MassTransit's
///     <see cref="MassTransit.Mediator.IMediator" />
/// </summary>
public class ServiceBusMediator : IServiceBus
{
    private readonly IMediator _mediator;

    public ServiceBusMediator(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region IServiceBus Members

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default) where TResponse : class
    {
        var client = _mediator.CreateRequestClient<IRequest<TResponse>>();
        cancellationToken.ThrowIfCancellationRequested();
        var response = await client.GetResponse<TResponse>(request, cancellationToken);
        return response.Message;
    }

    #endregion
}