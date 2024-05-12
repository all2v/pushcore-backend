using Astrum.SharedLib.Common.CQS.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.Registration.Commands.RegistrationApplicationDelete;

public class RegistrationApplicationDeleteCommand : CommandResult<RegistrationApplicationResponse>
{
    public RegistrationApplicationDeleteCommand([FromRoute] Guid Id)
    {
        this.Id = Id;
    }

    public Guid Id { get; init; }

    public void Deconstruct([FromRoute] out Guid Id)
    {
        Id = this.Id;
    }
}