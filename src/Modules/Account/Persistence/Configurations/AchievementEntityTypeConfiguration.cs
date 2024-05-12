using Astrum.Account.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Account.Configurations;

public class AchievementEntityTypeConfiguration : IEntityTypeConfiguration<Achievement>
{
    #region IEntityTypeConfiguration<Achievement> Members

    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
    }

    #endregion
}