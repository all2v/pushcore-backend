namespace Astrum.Identity.Models;

public static class ApplicationUserClaims
{
    #region Constants

    public const string MustChangePasswordClaimType = "user:mustchangepassword";
    public const string FullNameClaimType = "user:fullname";
    public const string ProfilePictureClaimType = "user:avatar";
    public const string ThemeClaimType = "user:theme";
    public const string CultureClaimType = "localization:culture";
    public const string UiCultureClaimType = "localization:uiculture";

    #endregion
}