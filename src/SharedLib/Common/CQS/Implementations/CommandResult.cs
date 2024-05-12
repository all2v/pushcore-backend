using Astrum.SharedLib.Common.CQS.Interfaces;
using Astrum.SharedLib.Common.Results;

namespace Astrum.SharedLib.Common.CQS.Implementations;

public abstract class Command<TResponse> : ICommand<TResponse> where TResponse : class, IResult
{
}

public abstract class CommandResult : Command<Result>
{
}

public abstract class CommandResult<TData> : Command<Result<TData>> where TData : class
{
}

// public abstract class PagedCommand<TData> : Command<PagedResult<TData>> where TData : class
// {
//     
// }