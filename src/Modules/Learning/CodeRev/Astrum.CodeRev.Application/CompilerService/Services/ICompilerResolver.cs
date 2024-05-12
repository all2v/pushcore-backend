using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.CompilerService.Services;

public interface ICompilerResolver
{
    ICompilerService? GetCompiler(ProgrammingLanguage language);
}
