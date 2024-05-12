using Astrum.Account.Application.ViewModels;
using Astrum.Account.Features.Account.AccountDetails;
using Astrum.Account.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.Account.UserEdit.Commands;

public sealed class UserEditCommand : CommandResult<EditUserResponse>
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }

    public Guid? PositionId { get; set; }

    public string Email { get; set; }

    public string? Address { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string PrimaryPhone { get; set; }

    public string SecondaryPhone { get; set; }

    public IEnumerable<RolesEnum> Roles { get; set; }

    public bool IsActive { get; set; }
}
