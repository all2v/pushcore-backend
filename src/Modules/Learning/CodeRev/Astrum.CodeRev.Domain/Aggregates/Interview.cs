
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.CodeRev.Domain.Aggregates;

    public class Interview : AggregateRootBase<Guid>
    {
        public string Vacancy { get; set; }
        public string InterviewText { get; set; }
        public long InterviewDurationMs { get; set; }
        public string CreatedByUsername { get; set; }
        public List<TestTask> Tasks { get; set; }
        public List<Invitation> Invitations { get; set; }
        public HashSet<InterviewLanguage> ProgrammingLanguages { get; set; }
        public List<InterviewSolution> InterviewSolutions { get; set; }
    }
