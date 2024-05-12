using Astrum.ITDictionary.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.ITDictionary.Configurations;

public class TestQuestionEntityTypeConfiguration : IEntityTypeConfiguration<TestQuestion>
{
    public void Configure(EntityTypeBuilder<TestQuestion> builder)
    {
        builder
            .HasOne(q => q.Practice)
            .WithMany()
            .HasForeignKey(q => q.PracticeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(q => q.TermSource)
            .WithMany()
            .HasForeignKey(q => q.TermSourceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .Property(q => q.AnswerIsReceived)
            .HasDefaultValue(false);

        builder
            .Property(q => q.Result)
            .HasDefaultValue(false);
    }
}