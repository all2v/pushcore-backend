using Astrum.CodeRev.Application.CompilerService.ViewModel.Response;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.CompilerService.Services;

public interface IAssemblyTestingService
{
    Result<TestsRunResponse> RunTests(string code, string testsCode);
}