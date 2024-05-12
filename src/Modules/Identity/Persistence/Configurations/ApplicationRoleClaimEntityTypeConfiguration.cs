using Astrum.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Identity.Configurations;

public class ApplicationRoleClaimEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    #region IEntityTypeConfiguration<ApplicationRoleClaim> Members

    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
    {
        builder.ToTable("RoleClaims", "Identity");
    }

    #endregion
}