using Astrum.Core.Common.CQS.V2.Interfaces;
using Astrum.Core.Common.Results.V1;

namespace Astrum.Core.Common.CQS.V2.Implementations;

public abstract class Query<TResult> : IQuery<IResult<TResult>> where TResult : class
{
}