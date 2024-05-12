using Astrum.CodeRev.Domain.Aggregates.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.CodeRev.Application.CompilerService.Services;

public class CompilerResolver : ICompilerResolver
{
    private readonly IServiceProvider _serviceProvider;

    public CompilerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICompilerService? GetCompiler(ProgrammingLanguage language)
    {
        return language switch
        {
            ProgrammingLanguage.CSharp => _serviceProvider.GetService<CSharpCompilerService>(),
            ProgrammingLanguage.JavaScript => _serviceProvider.GetService<JsCompilerService>(),
            _ => throw new ArgumentOutOfRangeException($"Unsupported programming language: {language}")
        };
    }
}