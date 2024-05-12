namespace Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;

public interface IPasswordGeneratorService
{
    string GenerateRandomPassword();
}