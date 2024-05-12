using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.ITDictionary.Services;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.ITDictionary.Controllers;

[Area("ITDictionary")]
[Route("[area]/[controller]")]
public class PracticesController : ApiBaseController
{
    private readonly IPracticeService _practiceService;
    private readonly ITestQuestionService _questionService;
    private readonly ILogHttpService _logger;

    public PracticesController(ILogHttpService logger, IPracticeService practiceService, ITestQuestionService questionService)
    {
        _logger = logger;
        _practiceService = practiceService;
        _questionService = questionService;
    }

    /// <summary>
    /// Начать практику флеш-карточек
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("flashcards/start")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result<FlashCardsView>), 200)]
    public async Task<Result<FlashCardsView>> StartFlashCards([FromBody] CreatePracticeRequest request)
    {
        var response = await _practiceService.StartFlashCards(request);
        
        return response;
    }

    /// <summary>
    /// Начать тест
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("test/start")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result<TestView>), 200)]
    public async Task<Result<TestView>> StartTest([FromBody] CreatePracticeRequest request)
    {
        var result = await _practiceService.StartTest(request);
        return result;
    }

    /// <summary>
    /// Завершить практику
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("finish")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    public async Task<Result> FinishPractice([FromBody] FinishPracticeRequest request)
    {
        var result = await _practiceService.FinishPractice(request);
        return result;
    }

    /// <summary>
    /// Получить вопрос
    /// </summary>
    /// <param name="practiceId"></param>
    /// <param name="userId"></param>
    [HttpGet("test/questions/{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result<TestQuestionView>), 200)]
    public async Task<Result<TestQuestionView>> GetQuestion([FromRoute] Guid id, [FromQuery] Guid userId)
    {
        var result = await _questionService.GetQuestion(userId, id);
        return result;
    }

    /// <summary>
    /// Получить вопросы
    /// </summary>
    /// <param name="practiceId"></param>
    /// <param name="userId"></param>
    [HttpGet("test/{practiceId:guid}/questions")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result<List<TestQuestionView>>), 200)]
    public async Task<Result<List<TestQuestionView>>> GetQuestions([FromRoute] Guid practiceId, [FromQuery] Guid userId)
    {
        var result = await _questionService.GetQuestions(userId, practiceId);
        return result;
    }

    [HttpPut("test/check")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result<QuestionCheckView>), 200)]
    public async Task<Result<QuestionCheckView>> CheckQuestionAnswer([FromBody] TestQuestionRequest request, [FromServices] IPracticeService practiceService)
    {
        var response = await _questionService.CheckAnswer(request, practiceService);

        return response;
    }
}