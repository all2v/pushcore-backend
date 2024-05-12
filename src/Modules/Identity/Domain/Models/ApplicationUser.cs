using Astrum.SharedLib.Common.Enumerations;
using Astrum.SharedLib.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Astrum.Identity.Models;

public abstract class BaseApplicationUser : IdentityUser<Guid>, IAuditableEntity<Guid>
{
    #region IAuditableEntity Members

    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }

    #endregion

    public bool IsDeleted { get; private set; }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}

public class ApplicationUser : BaseApplicationUser
{
    #region Public Properties

    /// <summary>
    ///     The user's name
    /// </summary>
    public string Name { get; set; } = null!;

    // public Guid AccountId { get; set; }

    /// <summary>
    ///     The user's secondary phone number
    /// </summary>
    public string? SecondaryPhoneNumber { get; set; }

    /// <summary>
    ///     The user's profile picture resource id
    /// </summary>
    public Guid? ProfilePictureResourceId { get; set; }

    /// <summary>
    ///     Flag indicating whether user is active
    /// </summary>
    public bool IsActive { get; set; } = false;

    /// <summary>
    ///     Flag indicating that user must change his password
    /// </summary>
    public bool MustChangePassword { get; set; } = false;

    /// <summary>
    ///     The user's selected theme
    /// </summary>
    public ThemeEnum Theme { get; set; }

    /// <summary>
    ///     The user's culture
    /// </summary>
    public string Culture { get; set; } = "el";

    /// <summary>
    ///     The user's UI culture
    /// </summary>
    public string UICulture { get; set; } = "el";

    /// <summary>
    ///     Navigation property for the roles this user belongs to.
    /// </summary>
    public ICollection<ApplicationUserRole> Roles { get; } = new List<ApplicationUserRole>();

    //public virtual ICollection<UserAchievement> UserAchievements { get; } = new List<UserAchievement>();

    #endregion
}