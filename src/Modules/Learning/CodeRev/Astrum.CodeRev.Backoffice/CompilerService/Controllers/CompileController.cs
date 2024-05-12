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
public class CompileController : ApiBaseController
{
    private readonly ICompilerResolver _compilerResolver;
    private readonly ILogHttpService _logger;

    public CompileController(ICompilerResolver compilerResolver, ILogHttpService logger)
    {
        _compilerResolver = compilerResolver;
        _logger = logger;
    }

    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<ExecutionResult>))]
    [ProducesResponseType(typeof(ExecutionResult), StatusCodes.Status200OK)]
    [HttpPut("execute")]
    public Result<ExecutionResult> Execute([FromBody] ExecutionRequest req)
    {
        var result = _compilerResolver.GetCompiler(req.ProgrammingLanguage)!.Execute(req.Code, req.EntryPoint);
        _logger.Log(req, result, HttpContext, "Выполнен присылаемый код",
            TypeRequest.PUT, ModuleAstrum.CodeRev);
        return result;
    }
}