using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Microsoft.AspNetCore.Http;

namespace Astrum.Projects.ViewModels.DTO
{
    public class ProductUpdateDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public IFormFile? CoverImage { get; set; }
        public List<ProjectRequest>? Projects { get; set; } = new List<ProjectRequest>();
    }
}
