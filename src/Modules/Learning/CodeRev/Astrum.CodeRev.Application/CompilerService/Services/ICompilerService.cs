using Astrum.CodeRev.Application.CompilerService.ViewModel.DTO;
using Astrum.CodeRev.Application.CompilerService.ViewModel.Response;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.CompilerService.Services;

public interface ICompilerService
{
    Result<ExecutionResult> Execute(string code, EntryPoint entryPoint);
}