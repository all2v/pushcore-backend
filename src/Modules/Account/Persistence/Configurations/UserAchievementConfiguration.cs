using Astrum.Account.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.Account.Configurations;

public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
{
    #region IEntityTypeConfiguration<UserAchievement> Members

    public void Configure(EntityTypeBuilder<UserAchievement> builder)
    {
        // builder
        //     .HasMany(x => x.Achievements)
        //     .WithOne()
        //     .IsRequired();
        builder.Metadata.FindNavigation(nameof(UserAchievement.Achievements))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    #endregion
}