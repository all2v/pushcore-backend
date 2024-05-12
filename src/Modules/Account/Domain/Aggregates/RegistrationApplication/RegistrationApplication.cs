using Astrum.Account.Enums;
using Astrum.Account.Events;
using Astrum.SharedLib.Domain.Entities;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Account.Aggregates;

public class RegistrationApplication : AggregateRootBase<Guid>,
    IDomainEventHandler<ApplicationStatusChangedEvent>
{
    private ApplicationStatus _status;
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public ApplicationStatus Status
    {
        get => _status;
        set => RaiseEvent(new ApplicationStatusChangedEvent(value)); 
    }

    public void Apply(ApplicationStatusChangedEvent @event)
    {
        _status = @event.Status;
    }
}