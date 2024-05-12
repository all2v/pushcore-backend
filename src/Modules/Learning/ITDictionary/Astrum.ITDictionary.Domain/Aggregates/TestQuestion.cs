using Astrum.SharedLib.Domain.Entities;

namespace Astrum.ITDictionary.Aggregates;

public class TestQuestion : AggregateRootBase<Guid>
{
    public Guid PracticeId { get; set; }

    public Practice Practice { get; set; }

    public Guid TermSourceId { get; set; }

    public Term TermSource { get; set; }
    
    public string Question { get; set; }
    
    public List<QuestionAnswerOption> AnswerOptions { get; set; }

    public bool AnswerIsReceived { get; set; }

    public bool Result { get; set; }
}