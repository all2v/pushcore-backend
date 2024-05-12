using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.CodeRev.Domain.Aggregates;

public class TestTask : AggregateRootBase<Guid>
{
    public List<Interview> Interviews { get; set; }
    
    public List<TaskSolution> TaskSolutions { get; set; }
    public string TaskText { get; set; }
    public string StartCode { get; set; }
    public string Name { get; set; }
    public string TestsCode { get; set; }
    public int RunAttempts { get; set; }
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
}