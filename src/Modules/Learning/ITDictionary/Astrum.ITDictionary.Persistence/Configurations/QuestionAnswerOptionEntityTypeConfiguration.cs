using Astrum.ITDictionary.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astrum.ITDictionary.Configurations;

public class QuestionAnswerOptionEntityTypeConfiguration: IEntityTypeConfiguration<QuestionAnswerOption>
{
    public void Configure(EntityTypeBuilder<QuestionAnswerOption> builder)
    {
        builder
            .HasOne(a => a.Question)
            .WithMany(q => q.AnswerOptions)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(a => a.TermSource)
            .WithMany()
            .HasForeignKey(a => a.TermSourceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .Property(a => a.IsCorrect)
            .HasDefaultValue(false);
    }
}