using Astrum.ITDictionary.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.ITDictionary.Configurations;

public class UserTermEntityTypeConfiguration : IEntityTypeConfiguration<UserTerm>
{
    public void Configure(EntityTypeBuilder<UserTerm> builder)
    {
        builder
            .HasKey(u => new { u.UserId, u.TermId });

        builder
            .HasOne(u => u.Term)
            .WithMany()
            .HasForeignKey(u => u.TermId);

        builder
            .Property(u => u.IsSelected)
            .HasDefaultValue(false);
    }
}