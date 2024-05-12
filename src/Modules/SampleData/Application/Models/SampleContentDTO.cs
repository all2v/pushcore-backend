using Microsoft.AspNetCore.Http;

namespace Astrum.SampleData.Models;

public class SampleContentDTO
{
    public string ContextName { get; set; }
    public IFormFile File { get; set; }
}
