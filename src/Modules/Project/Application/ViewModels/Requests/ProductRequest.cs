
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Astrum.Projects.ViewModels.Requests
{
    public class ProductRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid CustomerId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public IFormFile CoverImage { get; set; }
        public List<ProjectRequest> Projects { get; set; } = new List<ProjectRequest>();
    }
}
