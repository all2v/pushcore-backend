using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Contest;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Tasks;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.CodeRev.Domain.Specifications.Interviews;
using Astrum.CodeRev.Domain.Specifications.TaskSolution;
using Astrum.CodeRev.Domain.Specifications.TestTask;
using Astrum.CodeRev.UserService.DomainService.Repositories;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.CodeRev.Application.UserService.Services.Tasks;

public class TaskService : ITaskService
{
    private readonly ITestTaskRepository _testTaskRepository;
    private readonly ITaskSolutionRepository _taskSolutionRepository;
    private readonly IInterviewSolutionRepository _interviewSolutionRepository;
    private readonly IMapper _mapper;

    public TaskService(ITestTaskRepository testTaskRepository,
        ITaskSolutionRepository taskSolutionRepository, IInterviewSolutionRepository interviewSolutionRepository,
        IMapper mapper)
    {
        _testTaskRepository = testTaskRepository;
        _taskSolutionRepository = taskSolutionRepository;
        _interviewSolutionRepository = interviewSolutionRepository;
        _mapper = mapper;
    }

    public async Task<Result<TaskSolutionInfo>> GetTaskSolutionInfo(string authToken, string taskSolutionId)
    {
        var taskSolutionGuid = GuidParser.TryParse(taskSolutionId);
        if (taskSolutionGuid == Guid.Empty)
            return Result<TaskSolutionInfo>.Error("Неверный фромат GUID");
        return await GetTaskSolutionInfo(authToken, taskSolutionGuid);
    }

    public async Task<Result<TaskSolutionInfo>> GetTaskSolutionInfo(string authToken, Guid taskSolutionId)
    {
        var taskSolution =
            await _taskSolutionRepository.FirstOrDefaultAsync(
                new GetTaskSolutionWithInterviewSolution(taskSolutionId));
        if (taskSolution == null)
            return Result<TaskSolutionInfo>.Error($"Нет решения задачи с id {taskSolutionId}");

        return Result<TaskSolutionInfo>.Success(new TaskSolutionInfo
        {
            TaskSolutionId = taskSolution.Id,
            TaskId = taskSolution.TaskId,
            TaskOrder = ' ',
            InterviewSolutionId = taskSolution.InterviewSolutionId,
            FullName = $"{taskSolution.InterviewSolution.Surname} {taskSolution.InterviewSolution.FirstName}",
            Grade = taskSolution.Grade,
            IsDone = taskSolution.IsDone,
            RunAttemptsLeft = taskSolution.RunAttemptsLeft,
            ProgrammingLanguage = (await GetTask(taskSolution.TaskId)).ProgrammingLanguage,
        });
    }

    public async Task<Result<List<TaskSolution>>> GetTaskSolutions(Guid interviewSolutionId)
    {
        return Result<List<TaskSolution>>.Success(_mapper.Map<List<TaskSolution>>(
            await _taskSolutionRepository.ListAsync(
                new GetTestTaskByInterviewSolutionId(interviewSolutionId))));
    }

    public async Task<Result<List<TaskSolutionInfoContest>>> GetTaskSolutionInfosForContest(string interviewSolutionId)
    {
        var interviewSolutionGuid = GuidParser.TryParse(interviewSolutionId);
        if (interviewSolutionGuid == Guid.Empty)
            return Result<List<TaskSolutionInfoContest>>.Error("Неверный фромат GUID");

        var letterOrder = (int)'A';
        var taskInfos = await GetTaskSolutions(interviewSolutionGuid);

        var infoContest = taskInfos.Data
            .Select(solution => _mapper.Map<TaskSolutionInfoContest>(solution))
            .OrderBy(contest => contest.TaskId)
            .Select(t =>
            {
                t.TaskOrder = (char)letterOrder++;
                return t;
            })
            .ToList();
        return Result<List<TaskSolutionInfoContest>>.Success(infoContest);
    }

    private async Task<TaskSolution?> GetTaskSolution(string taskSolutionId)
    {
        var taskSolutionGuid = GuidParser.TryParse(taskSolutionId);
        return await GetTaskSolution(taskSolutionGuid);
    }

    public async Task<Result> TryPutTaskSolutionGrade(string taskSolutionId, Grade grade)
    {
        var taskSolutionGuid = GuidParser.TryParse(taskSolutionId);
        if (taskSolutionGuid == Guid.Empty)
            return Result.Error("Неверный фромат GUID");

        var taskSolution = await GetTaskSolution(taskSolutionGuid);
        if (taskSolution == null)
            return Result.Error($"Решения задачи с id {taskSolutionId} не существует");

        if (taskSolution.IsDone == false)
            return Result.Error($"Решение с id {nameof(taskSolutionId)} еще не закончена");

        taskSolution.Grade = grade;
        await _taskSolutionRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> EndTaskSolution(string taskSolutionId)
    {
        var taskSolution = await GetTaskSolution(taskSolutionId);
        if (taskSolution == null)
            return Result.Error($"Решения задачи с id {taskSolutionId} не существует");

        if (taskSolution.IsDone)
            return Result.Error($"Решение задачи с id {taskSolutionId} уже завершено");

        var interviewSolution =
            await _interviewSolutionRepository.FirstOrDefaultAsync(
                new GetInterviewSolutionById(taskSolution.InterviewSolution.Id));

        if (interviewSolution == null)
            return Result.Error($"Решения с id {taskSolution.InterviewSolution.Id} не существует");

        if (DateTimeOffset.Now.ToUnixTimeMilliseconds() > interviewSolution.EndTimeMs)
            return Result.Error(
                $"Решение уже закончено или еще не начато");

        taskSolution.IsDone = true;
        await _interviewSolutionRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<List<TestTaskDto>>> GetAllTasks(int offset, int limit)
    {
        return Result<List<TestTaskDto>>.Success(
            _mapper
                .Map<List<TestTaskDto>>(await _testTaskRepository
                    .ListAsync(new GetTestTasks(offset, limit))));
    }

    public async Task<Result<int>> TryReduceTaskSolutionAttempt(string taskSolutionId)
    {
        var taskSolutionGuid = GuidParser.TryParse(taskSolutionId);
        if (taskSolutionGuid == Guid.Empty)
            return Result.Error("Неверный фромат GUID");

        var taskSolution = await GetTaskSolution(taskSolutionGuid);
        if (taskSolution == null) return Result<int>.Error($"Решения задачи с id {taskSolutionId} не существует");

        if (taskSolution.RunAttemptsLeft == 0)
            return 0;

        taskSolution.RunAttemptsLeft -= 1;
        await _taskSolutionRepository.UnitOfWork.SaveChangesAsync();
        var runAttemptsLeft = taskSolution.RunAttemptsLeft;
        return Result<int>.Success(runAttemptsLeft);
    }

    public async Task<Result<Guid>> Create(TaskCreationDto taskCreation)
    {
        var task = _mapper.Map<TestTask>(taskCreation);
        task.Id = Guid.NewGuid();

        await _testTaskRepository.AddAsync(task);
        await _testTaskRepository.UnitOfWork.SaveChangesAsync();
        return Result<Guid>.Success(task.Id);
    }

    private async Task<Result<TaskSolutionDto>> CreateSolution(Guid interviewSolutionGuid, Guid taskGuid)
    {
        var taskSolutionGuid = Guid.NewGuid();
        var task = await GetTask(taskGuid);

        var taskSolution = await _taskSolutionRepository.AddAsync(new TaskSolution
        {
            Id = taskSolutionGuid,
            InterviewSolutionId = interviewSolutionGuid,
            TaskId = task.Id,
            IsDone = false,
            Grade = Grade.Zero,
            RunAttemptsLeft = task.RunAttempts,
        });

        await _taskSolutionRepository.UnitOfWork.SaveChangesAsync();

        return _mapper.Map<TaskSolutionDto>(taskSolution);
    }

    private async Task<TestTask?> GetTask(Guid taskId)
    {
        return await _testTaskRepository.FirstOrDefaultAsync(task => task.Id == taskId);
    }

    private async Task<TaskSolution?> GetTaskSolution(Guid taskSolutionId)
    {
        return await _taskSolutionRepository.FirstOrDefaultAsync(
            new GetTaskSolutionByIdWithInterviewSolution(taskSolutionId));
    }

    public async Task<Result<bool>> TryChangeTaskTestsCode(Guid taskId, string testsCode)
    {
        var task = await GetTask(taskId);
        if (task == null)
            return Result<bool>.Success(false);

        task.TestsCode = testsCode;
        await _testTaskRepository.UnitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}