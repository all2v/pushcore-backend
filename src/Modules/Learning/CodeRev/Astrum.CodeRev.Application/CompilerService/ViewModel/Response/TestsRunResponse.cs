namespace Astrum.CodeRev.Application.CompilerService.ViewModel.Response;

public class TestsRunResponse
{
    public List<string> PassedTestCases { get; set; } = new();
    public Dictionary<string, string> FailedTestCases { get; set; } = new();
    public bool IsCompiledSuccessfully { get; set; }
}