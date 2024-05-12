using Astrum.ITDictionary.Aggregates;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.ITDictionary.Services;

public interface IPracticeService
{
    public Task<Practice?> Get(Guid userId, Guid practiceId);

    public Task<bool> IsFinished(Guid practiceId);

    public Task<Result<FlashCardsView>> StartFlashCards(CreatePracticeRequest request);

    public Task<Result<TestView>> StartTest(CreatePracticeRequest request);

    public Task<Result> FinishPractice(FinishPracticeRequest request);
}