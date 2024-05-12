using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Contest;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Tasks;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services.Tasks;

public interface ITaskService
{
    Task<Result<TaskSolutionInfo>> GetTaskSolutionInfo(string authToken, string taskSolutionId);
    Task<Result<TaskSolutionInfo>> GetTaskSolutionInfo(string authToken, Guid taskSolutionId);
    Task<Result<List<TaskSolution>>> GetTaskSolutions(Guid interviewSolutionId);
    Task<Result<List<TaskSolutionInfoContest>>> GetTaskSolutionInfosForContest(string interviewSolutionId);
    Task<Result> TryPutTaskSolutionGrade(string taskSolutionId, Grade grade);
    Task<Result> EndTaskSolution(string taskSolutionId);
    Task<Result<List<TestTaskDto>>> GetAllTasks(int offset, int limit);
    Task<Result<int>> TryReduceTaskSolutionAttempt(string taskSolutionId);

    Task<Result<Guid>> Create(TaskCreationDto taskCreation);

    //Task<TaskSolutionDto> CreateSolution(Guid interviewSolutionGuid, Guid taskGuid);
    // Task<TestTaskDto> GetTask(Guid taskId);
    Task<Result<bool>> TryChangeTaskTestsCode(Guid taskId, string testsCode);
}