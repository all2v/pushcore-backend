using Astrum.Identity.Models;
using Astrum.ITDictionary.Aggregates;
using Astrum.ITDictionary.Enums;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.ITDictionary.Repositories;
using Astrum.ITDictionary.Specifications;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Astrum.ITDictionary.Services;

public class PracticeService : IPracticeService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITestQuestionService _questionService;
    private readonly ITermConstructorService _constructorService;

    private readonly IPracticeRepository _practiceRepository;

    private readonly Random _random;
    private readonly IMapper _mapper;

    public static readonly int[] PossibleTestCount = { 10, 20, 30, 40 };
    public static readonly int PossibleAnswers = 3;

    public PracticeService(UserManager<ApplicationUser> userManager, ITestQuestionService questionService,
        ITermConstructorService constructorService, IPracticeRepository practiceRepository, Random random,
        IMapper mapper)
    {
        _userManager = userManager;
        _questionService = questionService;
        _constructorService = constructorService;
        _practiceRepository = practiceRepository;
        _random = random;
        _mapper = mapper;
    }

    public async Task<Result<TestView>> GetTest(Guid userId, Guid practiceId)
    {
        var spec = new GetPracticesByIdsSpec(userId, practiceId);
        var practice = await _practiceRepository.FirstOrDefaultAsync(spec);
        if (practice == null) return Result.NotFound("Тест не найден");
        var questions = await _questionService.GetQuestions(userId, practiceId);
        if (questions.Failed) return Result.Error(questions.MessageWithErrors);
        return Result.Success(MapTest(practice, questions.Data));
    }

    public async Task<Practice?> Get(Guid userId, Guid practiceId)
    {
        var spec = new GetPracticesByIdsSpec(userId, practiceId);
        var practice = await _practiceRepository.FirstOrDefaultAsync(spec);
        return practice;
    }

    public async Task<bool> IsFinished(Guid practiceId)
    {
        var spec = new GetPracticeByIdSpec(practiceId);
        var practice = await _practiceRepository.FirstOrDefaultAsync(spec);
        return practice is { IsFinished: true };
    }

    public async Task<Result<FlashCardsView>> StartFlashCards(CreatePracticeRequest request)
    {
        if (request.Type != PracticeType.FlashCard)
            return Result.Error("Неверные входные параметры для создания практики.");

        var terms = await _constructorService.GetSelectedTerms(request.UserId);
        if (terms.Count < request.QuestionsCount)
            return Result.Error("Недостаточно терминов для создания практики");

        var practiceResult = await GeneratePractice(request);
        if (practiceResult.Failed) return Result.Error("Ошибка при создании флешкаточек.");

        var flashCards = new FlashCardsView()
        {
            Id = practiceResult.Data.Id,
            TermIds = GenerateFlashCardsSequence(terms, request.QuestionsCount),
        };
        return Result.Success(flashCards);
    }

    private List<Guid> GenerateFlashCardsSequence(IEnumerable<Term> terms, int length)
    {
        return terms
            .OrderBy(t => _random.Next())
            .Take(length)
            .Select(t => t.Id)
            .ToList();
    }

    public async Task<Result<TestView>> StartTest(CreatePracticeRequest request)
    {
        if (request.Type != PracticeType.DefinitionAndTerms && request.Type != PracticeType.TermAndDefinitions)
            return Result.Error("Неверные входные параметры для создания практики.");

        var terms = await _constructorService.GetSelectedTerms(request.UserId);
        if (terms.Count < request.QuestionsCount)
            return Result.Error("Ошибка. Недостаточно терминов для создания теста");

        var practiceResult = await GeneratePractice(request);
        if (practiceResult.Failed) return Result.Error("Ошибка при создании теста.");
        var practice = practiceResult.Data;
        var questionsResult =
            await _questionService.GenerateQuestions(practice, terms, request.QuestionsCount, PossibleAnswers);
        if (questionsResult.IsSuccess) return Result.Success(MapTest(practice, questionsResult.Data));
        try
        {
            await _practiceRepository.DeleteAsync(practice);
            await _practiceRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result<TestView>.Error(e.Message, questionsResult.MessageWithErrors);
        }

        return Result.Error(questionsResult.MessageWithErrors);
    }

    private async Task<Result<Practice>> GeneratePractice(CreatePracticeRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result.NotFound("Пользователь не найден.");

        var practice = _mapper.Map<Practice>(request);
        await _practiceRepository.AddAsync(practice);
        try
        {
            await _practiceRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Error(e.Message, "Ошибка при создании практики.");
        }

        return Result.Success(practice);
    }

    public async Task<Result> FinishPractice(FinishPracticeRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return Result.NotFound("Пользователь не найден.");
        var spec = new GetPracticeByIdSpec(request.PracticeId);
        var practice = await _practiceRepository.FirstOrDefaultAsync(spec);
        if (practice == null)
            return Result.NotFound("Практика не найдена.");
        practice.IsFinished = true;

        try
        {
            await _practiceRepository.UpdateAsync(practice);
            await _practiceRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Error(e.Message, "Ошибка при зачтении практики.");
        }

        return Result.Success();
    }

    private TestView MapTest(Practice practice, List<TestQuestionView> questions)
    {
        return new TestView
        {
            PracticeId = practice.Id,
            Type = practice.Type,
            Questions = questions,
            QuestionsCount = questions.Count,
        };
    }
}