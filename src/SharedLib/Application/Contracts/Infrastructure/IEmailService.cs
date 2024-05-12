using Astrum.SharedLib.Application.Models.Email;

namespace Astrum.SharedLib.Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task<bool> SendEmail(Email email);
}