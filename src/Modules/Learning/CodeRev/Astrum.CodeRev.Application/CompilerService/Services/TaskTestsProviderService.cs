using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.CodeRev.Domain.Specifications.TaskSolution;


namespace Astrum.CodeRev.Application.CompilerService.Services;

public class TaskTestsProviderService : ITaskTestsProviderService
{
    private readonly ITaskSolutionRepository _taskSolutionRepository;
    private readonly ITestTaskRepository _testTaskRepository;

    public TaskTestsProviderService(ITaskSolutionRepository taskSolutionRepository,
        ITestTaskRepository testTaskRepository)
    {
        _taskSolutionRepository = taskSolutionRepository;
        _testTaskRepository = testTaskRepository;
    }

    public async Task<string> GetTaskTestsCodeBySolutionId(Guid taskSolutionId)
    {
        var taskId = (await GetTaskSolution(taskSolutionId))?.TaskId ?? Guid.Empty;
        if (taskId.Equals(Guid.Empty))
            return string.Empty;
        return (await GetTask(taskId))?.TestsCode ?? string.Empty;
    }

    private async Task<TaskSolution?> GetTaskSolution(Guid taskSolutionId)
    {
        return await _taskSolutionRepository.FirstOrDefaultAsync(
            new GetTaskSolutionByIdWithInterviewSolution(taskSolutionId));
    }

    private async Task<TestTask?> GetTask(Guid taskId)
    {
        return await _testTaskRepository.FirstOrDefaultAsync(t => t.Id == taskId);
    }
}