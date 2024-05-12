using Astrum.Account.Events;
using Astrum.Identity.Contracts;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Account.EventHandlers;

public class AccountCreatedEventHandler : IDomainEventHandler<AccountCreatedEvent>
{
    private readonly IApplicationUserService _applicationUserService;

    public AccountCreatedEventHandler(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    #region IDomainEventHandler<AccountCreatedEvent> Members

    void IDomainEventHandler<AccountCreatedEvent>.Apply(AccountCreatedEvent @event)
    {
        Handle(@event).Wait();
    }

    #endregion

    public async Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken = default)
    {
        // var userResult = await _applicationUserService.GetUserAsync(notification.UserId, cancellationToken);
        // notification.Account.User = userResult.Value;
    }
}