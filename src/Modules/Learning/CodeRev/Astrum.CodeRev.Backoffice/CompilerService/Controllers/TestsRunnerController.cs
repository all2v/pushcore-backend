using Astrum.CodeRev.Application.CompilerService.Services;
using Astrum.CodeRev.Application.CompilerService.ViewModel.Requests;
using Astrum.CodeRev.Application.CompilerService.ViewModel.Response;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.CodeRev.Backoffice.CompilerService.Controllers;

[Area("CodeRev")]
[Route("[controller]")]
public class TestsRunnerController : ApiBaseController
{
    private readonly IAssemblyTestingService _assemblyTestingService;
    private readonly ITaskTestsProviderService _taskTestsProviderService;
    private readonly ILogHttpService _logger;

    public TestsRunnerController(IAssemblyTestingService assemblyTestingService,
        ITaskTestsProviderService taskTestsProviderService, ILogHttpService logger)
    {
        _assemblyTestingService = assemblyTestingService;
        _taskTestsProviderService = taskTestsProviderService;
        _logger = logger;
    }

    [HttpPost("run")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<TestsRunResponse>))]
    [ProducesResponseType(typeof(TestsRunResponse), StatusCodes.Status200OK)]
    public async Task<Result<TestsRunResponse>> RunTests([FromBody] TestsRunRequest req)
    {
        var testsCode = await _taskTestsProviderService.GetTaskTestsCodeBySolutionId(req.TaskSolutionId);
        if (testsCode == string.Empty)
            return Result.Error("Нет тестов");
        var result = _assemblyTestingService.RunTests(req.Code, testsCode);
        _logger.Log(req, result, HttpContext, "Выполнены тесты",
            TypeRequest.POST, ModuleAstrum.CodeRev);
        return result;
    }
}