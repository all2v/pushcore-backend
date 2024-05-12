using Astrum.SharedLib.Common.Results;
using MediatR;

namespace Astrum.SharedLib.Common.CQS.Interfaces;

public interface IQuery<out TResponse> : IRequest<TResponse> 
    where TResponse : class, IResult
{
}