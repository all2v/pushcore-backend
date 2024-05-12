using Astrum.Core.Common.CQS.V2.Interfaces;
using IResult = Astrum.Core.Common.Results.V1.IResult;

namespace Astrum.Core.Common.CQS.V2.Implementations;

public abstract class CommandHandler<TCommand> : MessageBaseHandler<TCommand, IResult>, ICommandHandler<TCommand>
    where TCommand : Command
{
    #region ICommandHandler<TCommand> Members

    public override abstract Task<IResult> Handle(TCommand command, CancellationToken cancellationToken = default);

    #endregion
}