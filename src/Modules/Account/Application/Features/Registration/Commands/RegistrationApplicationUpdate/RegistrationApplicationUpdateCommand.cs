using Astrum.SharedLib.Common.CQS.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdate;

public class RegistrationApplicationUpdateCommand : CommandResult<RegistrationApplicationResponse>
{
    [FromRoute]
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}