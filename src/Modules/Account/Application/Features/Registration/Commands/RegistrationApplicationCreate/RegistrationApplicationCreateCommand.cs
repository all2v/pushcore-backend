using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Registration.Commands.RegistrationApplicationCreate;

public class RegistrationApplicationCreateCommand : CommandResult<RegistrationApplicationResponse>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}