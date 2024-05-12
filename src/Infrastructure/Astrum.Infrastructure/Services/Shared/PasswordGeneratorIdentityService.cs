using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Astrum.Infrastructure.Services.Shared;

public class PasswordGeneratorIdentityService : IPasswordGeneratorService
{
    private readonly IOptions<IdentityOptions> _identityOptions;

    public PasswordGeneratorIdentityService(IOptions<IdentityOptions> identityOptions)
    {
        _identityOptions = identityOptions;
    }

    #region IPasswordGeneratorService Members

    /// <summary>
    ///     Generates a password according to Identity password options configured
    /// </summary>
    /// <returns></returns>
    public string GenerateRandomPassword()
    {
        var passwordOpt = _identityOptions.Value.Password;
        var requiredDigit = passwordOpt.RequireDigit;
        var requiredUniqueChars = passwordOpt.RequiredUniqueChars;
        var requiredLength = passwordOpt.RequiredLength;
        var requireLowercase = passwordOpt.RequireLowercase;
        var requireNonAlpha = passwordOpt.RequireNonAlphanumeric;
        var requireUppercase = passwordOpt.RequireUppercase;
        return GeneratePassword(requiredLength, requiredUniqueChars, requiredDigit, requireLowercase, requireNonAlpha,
            requireUppercase);
    }

    #endregion

    /// <summary>
    ///     Generates a Random Password
    ///     respecting the given strength requirements.
    /// </summary>
    /// <param name="opts">
    ///     A valid PasswordOptions object
    ///     containing the password strength requirements.
    /// </param>
    /// <returns>A random password</returns>
    private string GeneratePassword(int requiredLength = 10, int requiredUniqueChars = 6, bool requireDigit = true,
        bool requireLowercase = true, bool requireNonAlphanumeric = true, bool requireUppercase = true)
    {
        string[] randomChars =
        {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ", // uppercase 
            "abcdefghijkmnopqrstuvwxyz", // lowercase
            "0123456789", // digits
            @"-._@+\" // non-alphanumeric
        };

        var rand = new Random(Environment.TickCount);
        var chars = new List<char>();

        if (requireUppercase)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[0][rand.Next(0, randomChars[0].Length)]);

        if (requireLowercase)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[1][rand.Next(0, randomChars[1].Length)]);

        if (requireDigit)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[2][rand.Next(0, randomChars[2].Length)]);

        if (requireNonAlphanumeric)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[3][rand.Next(0, randomChars[3].Length)]);

        for (var i = chars.Count;
             i < requiredLength
             || chars.Distinct().Count() < requiredUniqueChars;
             i++)
        {
            var rcs = randomChars[rand.Next(0, randomChars.Length)];
            chars.Insert(rand.Next(0, chars.Count),
                rcs[rand.Next(0, rcs.Length)]);
        }

        return new string(chars.ToArray());
    }
}