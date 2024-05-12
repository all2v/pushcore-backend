using Astrum.Account.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Astrum.Account.Configurations;

public class UserProfileEntityTypeConfiguration : IEntityTypeConfiguration<UserProfile>
{
    #region IEntityTypeConfiguration<UserProfile> Members

    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder
            .HasMany(profile => profile.Achievements)
            .WithMany(achievement => achievement.Users)
            .UsingEntity(j => j.ToTable("UserAchievement"));

        builder.Property(profile => profile.Competencies)
            .HasConversion(
                c => JsonConvert.SerializeObject(c),
                c => JsonConvert.DeserializeObject<List<string>>(c));
    }

    #endregion
}