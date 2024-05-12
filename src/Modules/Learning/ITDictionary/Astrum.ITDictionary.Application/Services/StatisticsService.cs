using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;
using Astrum.ITDictionary.Enums;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.ITDictionary.Repositories;
using Astrum.ITDictionary.Specifications;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.ITDictionary.Services;

public class StatisticsService: IStatisticsService
{
    private readonly IPracticeService _practiceService;
    private readonly IPracticeRepository _repository;
    private readonly ITestQuestionRepository _questionRepository;

    private readonly IMapper _mapper;

    public static int LastPracticesCount = 4;
    
    public StatisticsService(IPracticeService practiceService, IPracticeRepository repository, IMapper mapper, ITestQuestionRepository questionRepository)
    {
        _practiceService = practiceService;
        _repository = repository;
        _mapper = mapper;
        _questionRepository = questionRepository;
    }
    
    public async Task<Result<StatisticsSummary>> GetSummary(Guid userId)
    {
        var spec = new GetFinishedPracticesSpec(userId);
        var practices = await _repository.ListAsync(spec);
        var practicesTypes = practices.GroupBy(p => p.Type);

        var responseModel = new StatisticsSummary()
        {
            UserId = userId,
            CountCompleted = practices.Count,
            Practices = new List<PracticeView>(),
        };
        
        foreach (var type in practicesTypes)
        {
            var practiceView = new PracticeView
            {
                Type = type.Key,
                CountCompleted = type.Count()
            };
            responseModel.Practices.Add(practiceView);
        }

        return Result.Success(responseModel);
    }

    public async Task<Result<TestStatisticsDetails>> GetTestStatsWithDetails(Guid userId, PracticeType type)
    {
        var spec = new GetFinishedPracticesSpec(userId);
        var practices = (await _repository.ListAsync(spec))
            .Where(p => p.Type == type)
            .ToList();
        var questionsSpec = new GetQuestionByPracticeIdsSpec(practices.Select(p => p.Id).ToList());
        var questions = await _questionRepository.ListAsync(questionsSpec);
        var responseModel = new TestStatisticsDetails()
        {
            PracticeType = type,
            SuccessRate = CalculateSuccessRate(questions),
            LastTestsStats = new List<TestStatistics>(),
        };

        practices = practices
            .OrderByDescending(p => p.DateModified)
            .Take(LastPracticesCount)
            .ToList();
        questions = questions
            .Where(q => practices.Any(p => p.Id == q.PracticeId))
            .ToList();
        responseModel.LastTestsStats = CalculateTestStats(questions)
            .OrderByDescending(q => practices.First(p => p.Id == q.Id).DateModified)
            .ToList();
        return Result.Success(responseModel);
    }

    private List<TestStatistics> CalculateTestStats(IEnumerable<TestQuestion> questions)
    {
        var groupedPractices = questions.GroupBy(q => q.PracticeId);

        return (from practice in groupedPractices
            let practiceQuestions = practice.ToList()
            select new TestStatistics()
            {
                Id = practice.Key, 
                QuestionsCount = practiceQuestions.Count,
                Correct = practiceQuestions.Count(q => q is { Result: true, AnswerIsReceived: true }), 
                Wrong = practiceQuestions.Count(q => q is { Result: false, AnswerIsReceived: true }),
            }).ToList();
    }

    private int CalculateSuccessRate(List<TestQuestion> questions)
    {
        var practices = questions.GroupBy(p => p.PracticeId);
        var practiceSuccessRates = new List<int>(); 
        foreach (var practice in practices)
        {
            questions = practice
                .Where(q => q.AnswerIsReceived)
                .ToList();
            var correct = questions.Count(q => q.Result);
            var wrong = questions.Count - correct;
            var successRate = correct == 0 
                ? 0
                : (int)Math.Round(100.0 * correct / (wrong + correct));
            practiceSuccessRates.Add(successRate);
        }

        return practiceSuccessRates.Count == 0
            ? 0
            : (int)Math.Round(practiceSuccessRates.Average());
    }
}