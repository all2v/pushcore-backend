using Astrum.Account.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Account.Configurations;

public class SocialNetworksEntityTypeConfiguration : IEntityTypeConfiguration<SocialNetworks>
{
    #region IEntityTypeConfiguration<SocialNetworks> Members

    public void Configure(EntityTypeBuilder<SocialNetworks> builder)
    {
    }

    #endregion
}