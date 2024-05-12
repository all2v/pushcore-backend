using Astrum.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Identity.Configurations;

public class ApplicationUserTokenEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
{
    #region IEntityTypeConfiguration<ApplicationUserToken> Members

    public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
    {
        builder.HasKey(o => new {o.LoginProvider, o.Value, o.UserId});
        builder.ToTable("UserTokens", "Identity");
    }

    #endregion
}