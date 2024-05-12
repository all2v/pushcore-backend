using Astrum.CodeRev.Application.UserService.Services.Tasks;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Contest;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Interviews;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.CodeRev.Domain.Specifications.Interviews;
using Astrum.CodeRev.UserService.DomainService.Repositories;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using static System.String;

namespace Astrum.CodeRev.Application.UserService.Services.Interviews;

public class InterviewService : IInterviewService
{
    private const long TimeToCheckInterviewSolutionMs = 604800000; // == 1 week //todo make config setting

    private readonly IInterviewRepository _interviewRepository;
    private readonly IInterviewSolutionRepository _interviewSolutionRepository;
    private readonly ITestTaskRepository _testTaskRepository;
    private readonly IMapper _mapper;
    private readonly ITaskService _taskService;
    private readonly IInterviewLanguageService _interviewLanguageService;

    public InterviewService(ITaskService taskService
        , IInterviewLanguageService interviewLanguageService,
        IInterviewSolutionRepository interviewSolutionRepository,
        IInterviewRepository interviewRepository, ITestTaskRepository testTaskRepository, IMapper mapper)
    {
        this._taskService = taskService;
        this._interviewLanguageService = interviewLanguageService;
        _interviewSolutionRepository = interviewSolutionRepository;
        _interviewRepository = interviewRepository;
        _testTaskRepository = testTaskRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<InterviewDto>>> GetAllInterviews(int offset, int limit)
    {
        var interviews = await _interviewRepository.ListAsync(new GetInterviews(offset, limit));
        var result = new List<InterviewDto>(interviews.Count);
        foreach (var interview in interviews)
        {
            result.Add(new InterviewDto
            {
                Id = interview.Id,
                Vacancy = interview.Vacancy,
                InterviewText = interview.InterviewText,
                InterviewDurationMs = interview.InterviewDurationMs,
                CreatedByUsername = interview.CreatedByUsername,
                InterviewLanguages = await _interviewLanguageService.GetInterviewLanguages(interview.Id),
            });
        }


        return Result<List<InterviewDto>>.Success(result);
    }

    public async Task<Result<List<string>>> GetAllVacancies(int offset, int limit)
    {
        var result = await _interviewRepository
            .GetDistinctVacanciesAsync(offset, limit);
        return Result<List<string>>.Success(result);
    }

    public async Task<Result<InterviewCreationResponse>> CreateInterview(InterviewCreationDto interviewCreation,
        string creatorUsername)
    {
        var interview = _mapper.Map<Interview>(interviewCreation);
        interview.CreatedByUsername = creatorUsername;
        interview.Id = Guid.NewGuid();
        interview.Tasks = new List<TestTask>();
        interview.ProgrammingLanguages = new HashSet<InterviewLanguage>();

        foreach (var taskId in interviewCreation.TaskIds)
        {
            var task = await _testTaskRepository.FirstOrDefaultAsync(task => task.Id == taskId);
            if (task == null)
                return Result<InterviewCreationResponse>.Error($"Нет задачи с id {taskId}");
            interview.Tasks.Add(task);
            interview.ProgrammingLanguages.Add(new InterviewLanguage
            {
                InterviewId = interview.Id,
                ProgrammingLanguage = task.ProgrammingLanguage
            });
        }

        await _interviewRepository.AddAsync(interview);
        await _interviewRepository.UnitOfWork.SaveChangesAsync();
        return Result<InterviewCreationResponse>.Success(new InterviewCreationResponse
        {
            InterviewId = interview.Id
        });
    }

    public async Task<Result> TryPutInterviewSolutionGrade(string interviewSolutionId, Grade grade)
    {
        var interviewSolutionGuid = GuidParser.TryParse(interviewSolutionId);
        if (interviewSolutionGuid == Guid.Empty)
            return Result.Error("Неверный фромат GUID");

        var interviewSolution = await GetInterviewSolution(interviewSolutionGuid);
        if (interviewSolution == null)
            return Result.Error($"Нет решения с id {interviewSolutionId}");
        interviewSolution.AverageGrade = grade;
        await _interviewSolutionRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> TryPutInterviewSolutionResult(string interviewSolutionId, InterviewResult interviewResult)
    {
        var interviewSolutionGuid =
            GuidParser.TryParse(interviewSolutionId);
        if (interviewSolutionGuid == Guid.Empty)
            return Result.Error("Неверный фромат GUID");

        var interviewSolution = await GetInterviewSolution(interviewSolutionGuid);
        if (interviewSolution == null)
            return Result.Error($"Нет решения с id {interviewSolutionId}");
        interviewSolution.InterviewResult = interviewResult;
        await _interviewSolutionRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> TryPutInterviewSolutionComment(string interviewSolutionId, string reviewerComment)
    {
        reviewerComment ??= Empty;

        var interviewSolutionGuid = GuidParser.TryParse(interviewSolutionId);
        if (interviewSolutionGuid == Guid.Empty)
            return Result.Error("Неверный фромат GUID");
        var interviewSolution = await GetInterviewSolution(interviewSolutionGuid);
        if (interviewSolution == null)
            return Result.Error($"Нет решения с id {interviewSolutionId}");
        interviewSolution.ReviewerComment = reviewerComment;
        await _interviewSolutionRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> TryPutInterviewSolutionReview(InterviewSolutionReview interviewSolutionReview)
    {
        if (interviewSolutionReview.ReviewerComment == null)
            throw new ArgumentException($"{nameof(interviewSolutionReview.ReviewerComment)} не может быть null");

        var interviewSolutionId = interviewSolutionReview.InterviewSolutionId;
        var interviewSolutionGuid = GuidParser.TryParse(interviewSolutionId);
        if (interviewSolutionGuid == Guid.Empty)
            return Result.Error("Неверный фромат GUID");

        var interviewSolution = await GetInterviewSolution(interviewSolutionGuid);
        if (interviewSolution == null)
            return Result.Error($"Нет решения с id {interviewSolutionId}");

        foreach (var taskSolutionReview in interviewSolutionReview.TaskSolutionsReviews)
            await _taskService.TryPutTaskSolutionGrade(taskSolutionReview.TaskSolutionId, taskSolutionReview.Grade);

        interviewSolution.ReviewerComment = interviewSolutionReview.ReviewerComment;
        interviewSolution.AverageGrade = interviewSolutionReview.AverageGrade;
        interviewSolution.InterviewResult = interviewSolutionReview.InterviewResult;
        await _interviewSolutionRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    private async Task<Interview?> GetInterview(Guid interviewId)
    {
        return await _interviewRepository.FirstOrDefaultAsync(interview => interview.Id == interviewId);
    }

    public async Task<Result<Interview>> GetInterviewWithTasks(Guid interviewId)
    {
        var interview = await _interviewRepository.FirstOrDefaultAsync(new GetInterviewByIdWithTasks(interviewId));
        if (interview == null)
            return Result<Interview>.Error($"Несуществует интервью с id {interviewId}");
        return Result<Interview>.Success(interview);
    }

    private async Task<InterviewSolution?> GetInterviewSolutionByUsername(Guid userId)
    {
        return await _interviewSolutionRepository.FirstOrDefaultAsync(new GetInterviewSolutionByUserId(userId));
    }

    public async Task<InterviewSolution?> GetInterviewSolution(Guid interviewSolutionId)
    {
        return await _interviewSolutionRepository.FirstOrDefaultAsync(
            new GetInterviewSolutionById(interviewSolutionId));
    }

    public async Task<Result<InterviewSolution>> GetInterviewSolution(string interviewSolutionId)
    {
        var interviewSolutionGuid = GuidParser.TryParse(interviewSolutionId);
        if (interviewSolutionGuid == Guid.Empty)
            return Result<InterviewSolution>.Error("Неверный фромат GUID");
        var solution = await GetInterviewSolution(interviewSolutionGuid);
        if (solution == null)
            return Result<InterviewSolution>.Error(
                $"Нет решения с id {interviewSolutionId}");
        return Result<InterviewSolution>.Success(solution);
    }

    // InterviewSolution создается когда пользователь заходит в систему
    public async Task<Result<InterviewSolutionDto>> GetInterviewSolutionInfo(string token, string interviewSolutionId)
    {
        var interviewSolutionGuid = GuidParser.TryParse(interviewSolutionId);
        if (interviewSolutionGuid == Guid.Empty)
            return Result<InterviewSolutionDto>.Error("Неверный фромат GUID");
        var interviewSolution = await GetInterviewSolution(interviewSolutionGuid);
        if (interviewSolution == null)
            return Result<InterviewSolutionDto>.Error($"Нет решения с id {interviewSolutionId}");
        var interviewSolutionInfo = _mapper.Map<InterviewSolutionDto>(interviewSolution);
        interviewSolutionInfo.ProgrammingLanguages =
            await _interviewLanguageService.GetInterviewLanguages(interviewSolution.Interview.Id);

        var taskSolutions = await _taskService.GetTaskSolutions(interviewSolution.Id);
        if (!taskSolutions.IsSuccess)
            return Result<InterviewSolutionDto>.Error(taskSolutions.MessageWithErrors);
        var taskSolutionsInfos = new List<TaskSolutionInfo>(taskSolutions.Data.Count);
        foreach (var taskSolution in taskSolutions.Data)
        {
            var info = await _taskService.GetTaskSolutionInfo(token, taskSolution.Id);
            if (info.IsSuccess)
                taskSolutionsInfos.Add(info.Data);
            else
                return Result<InterviewSolutionDto>.Error(info.MessageWithErrors);
        }

        var letterOrder = (int)'A';
        interviewSolutionInfo.TaskSolutionsInfos = taskSolutionsInfos
            .OrderBy(t => t.TaskId)
            .Select(t =>
            {
                t.TaskOrder = (char)letterOrder++;
                return t;
            })
            .ToList();

        return Result<InterviewSolutionDto>.Success(interviewSolutionInfo);
    }

    public async Task<Result> StartInterviewSolution(string interviewSolutionId)
    {
        var interviewSolutionResult = await GetInterviewSolution(interviewSolutionId);

        if (!interviewSolutionResult.IsSuccess)
            return Result.Error(interviewSolutionResult.MessageWithErrors);

        var interviewSolution = interviewSolutionResult.Data;

        if (interviewSolution.StartTimeMs >= 0)
            return Result.Error($"Решение с id {nameof(interviewSolution)} уже начато");

        var interview = await GetInterview(interviewSolution.Interview.Id);
        if (interview == null)
            return Result.Error($"Не существует интервью с id {interviewSolution.Interview.Id}");

        var nowTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        var endTime = nowTime + interview.InterviewDurationMs;
        interviewSolution.StartTimeMs = nowTime;
        interviewSolution.EndTimeMs = endTime;
        interviewSolution.TimeToCheckMs = endTime + TimeToCheckInterviewSolutionMs;
        await _interviewSolutionRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> EndInterviewSolution(string interviewSolutionId)
    {
        var interviewSolutionResult = await GetInterviewSolution(interviewSolutionId);
        if (!interviewSolutionResult.IsSuccess) return Result.Error(interviewSolutionResult.MessageWithErrors);

        var interviewSolution = interviewSolutionResult.Data;

        var nowTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        if (nowTime > interviewSolution.EndTimeMs || interviewSolution.IsSubmittedByCandidate)
            return Result.Error(
                $"Решение с id {nameof(interviewSolution)} уже завершено или еще не начато");

        var interview = await GetInterview(interviewSolution.Interview.Id);
        if (interview == null) return Result.Error($"Не существует интервью с id {interviewSolution.Interview.Id}");

        interviewSolution.EndTimeMs = nowTime;
        interviewSolution.TimeToCheckMs = nowTime + TimeToCheckInterviewSolutionMs;
        interviewSolution.IsSubmittedByCandidate = true;
        await _interviewSolutionRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<InterviewSolutionInfo>> GetInterviewSolutionInfo(Guid userId)
    {
        var interviewSolution = await GetInterviewSolutionByUsername(userId);
        if (interviewSolution == null)
            return Result<InterviewSolutionInfo>.Error($"Не существует решения у пользователя");
        var interview = await GetInterview(interviewSolution.Interview.Id);
        if (interview == null)
            return Result<InterviewSolutionInfo>.Error($"Не существует интервью с id {interviewSolution.Interview.Id}");

        return Result<InterviewSolutionInfo>.Success(new InterviewSolutionInfo
        {
            Id = interviewSolution.Id,
            Vacancy = interview.Vacancy,
            InterviewText = interview.InterviewText,
            InterviewDurationMs = interview.InterviewDurationMs,
            StartTimeMs = interviewSolution.StartTimeMs,
            EndTimeMs = interviewSolution.EndTimeMs,
            IsStarted = interviewSolution.StartTimeMs >= 0,
            IsSubmittedByCandidate = interviewSolution.IsSubmittedByCandidate,
            ProgrammingLanguages = (await _interviewLanguageService.GetInterviewLanguages(interview.Id)).Data,
            IsSynchronous = interviewSolution.IsSynchronous,
        });
    }
}