using MediatR;
using IResult = Astrum.Core.Common.Results.V1.IResult;

namespace Astrum.Core.Common.CQS.V2.Interfaces;

public interface ICommandHandler<in TCommand> : IHandler<TCommand, IResult>
    where TCommand : class, IRequest, IRequest<IResult>
{
}