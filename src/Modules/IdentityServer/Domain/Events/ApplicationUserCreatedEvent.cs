using Astrum.Account.Aggregates;
using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.SharedLib.Domain.Events;
using Astrum.SharedLib.Domain.Interfaces;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Astrum.IdentityServer.Domain.Events;

public class ApplicationUserCreatedEvent : DomainEventBase<string>
{
    public ApplicationUserCreatedEvent(string aggregateId, int aggregateVersion,
        UserViewModel user, string phoneNumber, RegistrationApplication application = null, 
        string password = null) : base(aggregateId, aggregateVersion)
    {
        User = user;
        Application = application;
        PhoneNumber = phoneNumber;
        Password = password;
    }

    public UserViewModel User { get; }
    public RegistrationApplication? Application { get; }
    public string PhoneNumber { get; }
    public string Password { get; }

    public override IDomainEvent<string> WithAggregate(string aggregateId, int aggregateVersion)
    {
        return new ApplicationUserCreatedEvent(aggregateId, aggregateVersion, User, PhoneNumber, Application, Password);
    }
}