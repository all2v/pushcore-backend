// using Ardalis.GuardClauses;
// using Astrum.SharedLib.Common.Enumerations;
// using Astrum.SharedLib.Domain.Entities;
// using Astrum.SharedLib.Domain.Interfaces;
// using Astrum.Identity.Domain.Enums;
// using Astrum.Identity.Domain.Events;
//
// namespace Astrum.Identity.Domain.Aggregates;
//
// public class User : AggregateRootBase<Guid>, IDomainEventHandler<UserCreatedEvent>
// {
//     public User(string username, string email, string name)
//     {
//         Guard.Against.NullOrWhiteSpace(username, nameof(username));
//         Guard.Against.NullOrWhiteSpace(email, nameof(email));
//         Guard.Against.NullOrWhiteSpace(name, nameof(name));
//
//         RaiseEvent(new UserCreatedEvent(username, email, name));
//     }
//
//     /// <summary>
//     ///     Gets or sets the user name for this user.
//     /// </summary>
//     public string Username { get; private set; } // TODO map it for case 
//
//     /// <summary>
//     ///     Gets or sets the normalized user name for this user.
//     /// </summary>
//     public string NormalizedUserName { get; set; }
//
//     /// <summary>
//     ///     The user's name
//     /// </summary>
//     public string Name { get; private set; }
//
//     /// <summary>
//     ///     Gets or sets a telephone number for the user.
//     /// </summary>
//     public string PrimaryPhoneNumber { get; private set; } // TODO map it to appl user
//
//     /// <summary>
//     ///     Gets or sets a secondary telephone number for the user.
//     /// </summary>
//     public string SecondaryPhoneNumber { get; private set; } // TODO map it to appl user
//
//     /// <summary>
//     ///     Gets or sets a flag indicating if a user has confirmed their telephone address.
//     /// </summary>
//     /// <value>True if the telephone number has been confirmed, otherwise false.</value>
//     public virtual bool PhoneNumberConfirmed { get; set; }
//
//
//     /// <summary>
//     ///     Gets or sets the email address for this user.
//     /// </summary>
//     public string Email { get; private set; }
//
//     /// <summary>
//     ///     Gets or sets the normalized email address for this user.
//     /// </summary>
//     public string NormalizedEmail { get; set; }
//
//     /// <summary>
//     ///     Gets or sets a flag indicating if a user has confirmed their email address.
//     /// </summary>
//     /// <value>True if the email address has been confirmed, otherwise false.</value>
//     public bool EmailConfirmed { get; set; }
//
//     /// <summary>
//     ///     Gets or sets a salted and hashed representation of the password for this user.
//     /// </summary>
//     public string PasswordHash { get; set; }
//
//     /// <summary>
//     ///     A random value that must change whenever a users credentials change (password changed, login removed)
//     /// </summary>
//     public string SecurityStamp { get; set; }
//
//     /// <summary>
//     ///     A random value that must change whenever a user is persisted to the store
//     /// </summary>
//     public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
//
//     /// <summary>
//     ///     Gets or sets a flag indicating if two factor authentication is enabled for this user.
//     /// </summary>
//     /// <value>True if 2fa is enabled, otherwise false.</value>
//     public virtual bool TwoFactorEnabled { get; set; }
//
//     /// <summary>
//     ///     Gets or sets the date and time, in UTC, when any user lockout ends.
//     /// </summary>
//     /// <remarks>
//     ///     A value in the past means the user is not locked out.
//     /// </remarks>
//     public virtual DateTimeOffset? LockoutEnd { get; set; }
//
//     /// <summary>
//     ///     Gets or sets a flag indicating if the user could be locked out.
//     /// </summary>
//     /// <value>True if the user could be locked out, otherwise false.</value>
//     public virtual bool LockoutEnabled { get; set; }
//
//     /// <summary>
//     ///     Gets or sets the number of failed login attempts for the current user.
//     /// </summary>
//     public virtual int AccessFailedCount { get; set; }
//
//     public IReadOnlyCollection<RolesEnum> Roles { get; private set; }
//
//     /// <summary>
//     ///     Flag indicating whether user is active
//     /// </summary>
//     public bool IsActive { get; private set; }
//
//
//     public DateTimeOffset DateCreated { get; set; }
//     public DateTimeOffset DateModified { get; set; }
//     public string? CreatedBy { get; set; }
//     public string? ModifiedBy { get; set; }
//
//     // public Guid AccountId { get; set; }
//
//     /// <summary>
//     ///     The user's profile picture resource id
//     /// </summary>
//     public Guid? ProfilePictureResourceId { get; set; }
//
//     /// <summary>
//     ///     Flag indicating that user must change his password
//     /// </summary>
//     public bool MustChangePassword { get; set; } = false;
//
//     /// <summary>
//     ///     The user's selected theme
//     /// </summary>
//     public ThemeEnum Theme { get; set; }
//
//     /// <summary>
//     ///     The user's culture
//     /// </summary>
//     public string Culture { get; set; } = "el";
//
//     /// <summary>
//     ///     The user's UI culture
//     /// </summary>
//     public string UICulture { get; set; } = "el";
//
//     #region IDomainEventHandler<UserCreatedEvent> Members
//
//     void IDomainEventHandler<UserCreatedEvent>.Apply(UserCreatedEvent @event)
//     {
//         Username = @event.Username;
//         Name = @event.Name;
//         Email = @event.Email;
//     }
//
//     #endregion
//
//     public void ActivateUser()
//     {
//         IsActive = true;
//         // TODO to raise event
//     }
//
//     public void DeactivateUser()
//     {
//         IsActive = false;
//         // TODO to raise event
//     }
//
//
//     /// <summary>
//     ///     Returns the username for this user.
//     /// </summary>
//     public override string ToString()
//     {
//         return Username;
//     }
// }

