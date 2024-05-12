namespace Astrum.Identity.DTOs;

public class LoginRequestDTO
{
    /// <summary>
    ///     Username or email for authentication
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    ///     Password for authentication
    /// </summary>
    public string Password { get; set; }

    public bool RememberMe { get; set; } //TODO[CH]: Consider removing this?

    public string? ReturnUrl { get; set; }
}