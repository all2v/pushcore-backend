using Astrum.CodeRev.Application.CompilerService.ViewModel.DTO;
using Astrum.CodeRev.Application.CompilerService.ViewModel.Response;
using Astrum.SharedLib.Common.Results;
using Jint;

namespace Astrum.CodeRev.Application.CompilerService.Services;

public class JsCompilerService : ICompilerService
{
    public Result<ExecutionResult> Execute(string code, EntryPoint entryPoint)
    {
        var output = new List<string>();
        var engine = new Engine()
            .SetValue("log", new Action<object>(obj => output.Add(obj.ToString() ?? "null")));

        try
        {
            engine.Execute(code);
        }
        catch (Esprima.ParserException parserException)
        {
            return Result<ExecutionResult>.Success(new ExecutionResult
            {
                Success = false,
                Errors = new[] { new CompilationError(parserException) }
            });
        }
        catch (Jint.Runtime.JavaScriptException javaScriptException)
        {
            return Result<ExecutionResult>.Success(new ExecutionResult
            {
                Success = false,
                Errors = new[] { new CompilationError(javaScriptException) }
            });
        }

        return Result<ExecutionResult>.Success(new ExecutionResult
        {
            Success = true,
            Output = output
        });
    }
}