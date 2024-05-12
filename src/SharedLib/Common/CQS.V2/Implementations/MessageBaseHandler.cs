using MassTransit;
using MediatR;
using IResult = Astrum.Core.Common.Results.V1.IResult;

namespace Astrum.Core.Common.CQS.V2.Implementations;

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