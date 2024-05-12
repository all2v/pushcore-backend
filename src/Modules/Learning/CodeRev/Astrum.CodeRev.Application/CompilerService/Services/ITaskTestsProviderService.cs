namespace Astrum.CodeRev.Application.CompilerService.Services;

public interface ITaskTestsProviderService
{
    Task<string> GetTaskTestsCodeBySolutionId(Guid taskSolutionId);

}