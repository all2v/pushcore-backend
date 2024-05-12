using Astrum.Core.Common.CQS.V2.Interfaces;
using Astrum.Core.Common.Results.V1;

namespace Astrum.Core.Common.CQS.V2.Implementations;

public abstract class QueryHandler<TQuery, TResult> : MessageBaseHandler<TQuery, IResult<TResult>>,
    IQueryHandler<TQuery, IResult<TResult>>
    where TQuery : Query<TResult>
    where TResult : class
{
    #region IQueryHandler<TQuery,IResult<TResult>> Members

    public override abstract Task<IResult<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default);

    #endregion
}