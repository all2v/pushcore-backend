using Astrum.CodeRev.Application.CompilerService.ViewModel.DTO;
using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.CompilerService.ViewModel.Requests;

public class ExecutionRequest
{
    public EntryPoint EntryPoint { get; set; }
    public string Code { get; set; }
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
}