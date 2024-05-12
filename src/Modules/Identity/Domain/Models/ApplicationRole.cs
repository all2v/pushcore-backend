using Astrum.SharedLib.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Astrum.Identity.Models;

public class ApplicationRole : IdentityRole<Guid>, IAuditableEntity<Guid>
{
    public ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;

    #region IAuditableEntity Members

    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; private set; }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    //public new Guid Id { get; set; }
    #endregion
}