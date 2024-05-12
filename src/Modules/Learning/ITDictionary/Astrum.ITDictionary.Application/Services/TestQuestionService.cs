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

public class TestQuestionService : ITestQuestionService
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ITermRepository _termRepository;
    private readonly ITestQuestionRepository _questionRepository;

    private readonly IMapper _mapper;
    private readonly Random _random;


    public TestQuestionService(ITermRepository termRepository, IMapper mapper, Random random,
        ITestQuestionRepository questionRepository, UserManager<ApplicationUser> userManager)
    {
        _termRepository = termRepository;
        _mapper = mapper;
        _random = random;
        _questionRepository = questionRepository;
        _userManager = userManager;
    }

    public async Task<Result<TestQuestionView>> GetQuestion(Guid userId, Guid questionId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result.NotFound("Пользователь не найден.");
        var spec = new GetQuestionByIdSpec(questionId);
        var question = await _questionRepository.FirstOrDefaultAsync(spec);
        if (question == null)
            return Result.NotFound("Вопрос не найден.");

        return Result.Success(_mapper.Map<TestQuestionView>(question));
    }

    public async Task<Result<List<TestQuestionView>>> GetQuestions(Guid userId, Guid practiceId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result.NotFound("Пользователь не найден.");
        var spec = new GetQuestionsByPracticeIdSpec(practiceId);
        var questions = await _questionRepository.ListAsync(spec);
        return Result.Success(_mapper.Map<List<TestQuestionView>>(questions));
    }

    public async Task<Result<List<TestQuestionView>>> GenerateQuestions(Practice practice, List<Term> userTerms,
        int questionsCount,
        int answerOptions)
    {
        if (practice.Type == PracticeType.FlashCard)
            return Result.Error("Невозможно сгенерировать вопрос для флешкарточек.");
        if (questionsCount > userTerms.Count)
            return Result.Error("Недостаточно терминов для создания вопросов.");

        var shuffledTerms = userTerms
            .OrderBy(x => _random.Next())
            .ToList();
        var questions = new List<TestQuestion>();
        for (var i = 0; i < questionsCount; i++)
        {
            var result = await GenerateQuestion(practice, shuffledTerms[i], answerOptions);
            if (result.Failed)
                return Result.Error("Ошибка при создании вопроса.", result.MessageWithErrors);
            var question = result.Data;
            questions.Add(question);
        }

        await _questionRepository.AddRangeAsync(questions);
        try
        {
            await _questionRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Error(e.Message, "Ошибка при создании вопросов");
        }

        return Result.Success(_mapper.Map<List<TestQuestionView>>(questions));
    }

    private async Task<Result<TestQuestion>> GenerateQuestion(Practice practice, Term term, int answerOptions)
    {
        var termsSpec = new GetTermsByCategoryIdSpec(term.CategoryId);
        var terms = (await _termRepository.ListAsync(termsSpec))
            .Where(e => e.Id != term.Id)
            .OrderBy(e => _random.Next())
            .Take(answerOptions - 1)
            .ToList();
        if (terms.Count < answerOptions - 1)
            return Result.Error("Недостаточно терминов для генерации вопроса.");

        var question = new TestQuestion
        {
            PracticeId = practice.Id,
            TermSourceId = term.Id,
            AnswerOptions = MapAnswerOptions(practice.Type, terms, term, answerOptions),
            Question = practice.Type switch
            {
                PracticeType.DefinitionAndTerms => term.Definition,
                PracticeType.TermAndDefinitions => term.Name,
                _ => "",
            },
        };
        return Result.Success(question);
    }

    private List<QuestionAnswerOption> MapAnswerOptions(PracticeType practiceType, List<Term> terms,
        Term termSource, int countAnswerOptions)
    {
        var answerOptions = new QuestionAnswerOption[countAnswerOptions];
        for (var i = 0; i < countAnswerOptions - 1; i++)
            answerOptions[i] = MapAnswerOption(practiceType, terms[i], false);

        answerOptions[countAnswerOptions - 1] = MapAnswerOption(practiceType, termSource, true);

        return answerOptions
            .OrderBy(x => _random.Next())
            .ToList();
    }

    private QuestionAnswerOption MapAnswerOption(PracticeType practiceType, Term termSource, bool isCorrect)
    {
        var answerOption = new QuestionAnswerOption
        {
            IsCorrect = isCorrect,
            TermSourceId = termSource.Id,
        };
        answerOption.Answer = practiceType switch
        {
            PracticeType.DefinitionAndTerms => termSource.Name,
            PracticeType.TermAndDefinitions => termSource.Definition,
            _ => answerOption.Answer
        };

        return answerOption;
    }

    public async Task<Result<QuestionCheckView>> CheckAnswer(TestQuestionRequest request, IPracticeService practiceService)
    {
        var spec = new GetQuestionByIdsSpec(request.UserId, request.TestQuestionId);
        var question = await _questionRepository.FirstOrDefaultAsync(spec);
        if (question == null) return Result.NotFound("Вопрос не найден.");
        if (await practiceService.IsFinished(question.PracticeId)) return Result.Error("Практика уже завершена.");
        if (question.AnswerIsReceived) return Result.Error("Ответ на вопрос уже был получен.");
        var answerOption = question.AnswerOptions.FirstOrDefault(e => e.Id == request.TestOptionId);
        if (answerOption == null) return Result.NotFound("Вариат ответа не найден.");

        var checkResult = answerOption.IsCorrect;

        question.AnswerIsReceived = true;
        question.Result = checkResult;
        try
        {
            await _questionRepository.UpdateAsync(question);
            await _questionRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return Result.Error(e.Message, "Ошибка при проверке вопроса.");
        }

        if (await CanTestBeFinished(question.PracticeId))
            await practiceService.FinishPractice(new FinishPracticeRequest
            {
                PracticeId = question.PracticeId, 
                UserId = request.UserId
            });

        var questionCheck = new QuestionCheckView
        {
            UserId = request.UserId,
            QuestionId = request.TestQuestionId,
            CheckingResult = checkResult,
        };
        return Result.Success(questionCheck);
    }

    private async Task<bool> CanTestBeFinished(Guid practiceId)
    {
        var spec = new GetQuestionsByPracticeIdSpec(practiceId);
        var questions = await _questionRepository.ListAsync(spec);
        return questions.All(q => q.AnswerIsReceived);
    }
}