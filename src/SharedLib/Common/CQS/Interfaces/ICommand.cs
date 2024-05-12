using Astrum.SharedLib.Common.Results;
using MediatR;

namespace Astrum.SharedLib.Common.CQS.Interfaces;

public interface ICommand<out TResponse> : IRequest<TResponse>
    where TResponse : class, IResult
{
}
public interface ICommand: ICommand<Result>
{
}