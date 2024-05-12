using Astrum.SharedLib.Common.Results;
using MassTransit;
using MediatR;

namespace Astrum.SharedLib.Common.CQS.Implementations;

public abstract class MessageBaseHandler<TRequest, TResponse> : BaseHandler<TRequest, TResponse>, IConsumer<TRequest>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResult
{
    #region IConsumer<TRequest> Members

    public async Task Consume(ConsumeContext<TRequest> context)
    {
        var messageResult = await Handle(context.Message);
        await context.RespondAsync(messageResult);
    }

    #endregion
}