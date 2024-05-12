using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.CodeRev.Domain.Aggregates;

public class InterviewLanguage : AggregateRootBase<Guid>
{
    public Guid InterviewId { get; set; }
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
}