using Astrum.CodeRev.Application.CompilerService.ViewModel.DTO;

namespace Astrum.CodeRev.Application.CompilerService.ViewModel.Response;

public class ExecutionResult
{
    public bool Success { get; set; }
    public IEnumerable<string> Output { get; set; }
    public IEnumerable<CompilationError> Errors { get; set; }

}