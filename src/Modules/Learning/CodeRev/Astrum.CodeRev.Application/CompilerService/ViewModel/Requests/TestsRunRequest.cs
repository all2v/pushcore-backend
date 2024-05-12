namespace Astrum.CodeRev.Application.CompilerService.ViewModel.Requests;

public class TestsRunRequest
{
    public string Code { get; set; }
    public Guid TaskSolutionId { get; set; }
}