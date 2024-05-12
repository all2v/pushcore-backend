using System.ComponentModel.DataAnnotations;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Tasks;

public class TestsCodeDto
{
    [Required]
    public string TestsCode { get; set; }
}