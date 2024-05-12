using Astrum.SharedLib.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Astrum.Identity.Models;

public class ApplicationUserLogin : IdentityUserLogin<Guid>, IAuditableEntity<Guid>
{
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
    
    public Guid Id { get; set; }
    #endregion
}