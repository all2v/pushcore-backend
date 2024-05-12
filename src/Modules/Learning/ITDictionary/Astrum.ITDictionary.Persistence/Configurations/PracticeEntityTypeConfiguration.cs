using Astrum.ITDictionary.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.ITDictionary.Configurations;

public class PracticeEntityTypeConfiguration: IEntityTypeConfiguration<Practice>
{
    public void Configure(EntityTypeBuilder<Practice> builder)
    {
        builder
            .Property(p => p.IsFinished)
            .HasDefaultValue(false);
    }
}