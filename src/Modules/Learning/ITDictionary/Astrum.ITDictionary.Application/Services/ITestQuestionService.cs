using Astrum.ITDictionary.Aggregates;
using Astrum.ITDictionary.Enums;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.ITDictionary.Services;

public interface ITestQuestionService
{
    public Task<Result<List<TestQuestionView>>> GenerateQuestions(Practice practice, List<Term> terms,
        int questionsCount, int answerOptions);

    public Task<Result<TestQuestionView>> GetQuestion(Guid userId, Guid practiceId);
    
    public Task<Result<List<TestQuestionView>>> GetQuestions(Guid userId, Guid practiceId);

    public Task<Result<QuestionCheckView>> CheckAnswer(TestQuestionRequest request, IPracticeService practiceService);
}