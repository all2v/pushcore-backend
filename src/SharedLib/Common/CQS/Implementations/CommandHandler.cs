using Astrum.SharedLib.Common.CQS.Interfaces;
using Astrum.SharedLib.Common.Results;

namespace Astrum.SharedLib.Common.CQS.Implementations;

public abstract class CommandHandler<TCommand, TResponse> : RequestMessageHandler<TCommand, TResponse>,
    ICommandHandler<TCommand, TResponse>
    where TCommand : class, ICommand<TResponse>
    where TResponse : class, IResult
{
    public override abstract Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public abstract class CommandResultHandler<TCommand> : CommandHandler<TCommand, Result>
    where TCommand : class, ICommand<Result>
{
}

public abstract class CommandResultHandler<TCommand, TData> : CommandHandler<TCommand, Result<TData>>
    where TCommand : class, ICommand<Result<TData>>
    where TData : class
{
}

// public abstract class CommandPagedResultHandler<TCommand, TData> : CommandHandler<TCommand, PagedResult<TData>>
//     where TCommand : PagedCommand<TData>
//     where TData : class
// {
// }