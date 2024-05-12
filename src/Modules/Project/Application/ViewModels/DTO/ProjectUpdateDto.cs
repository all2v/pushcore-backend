using Astrum.Project.ViewModels.DTO;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;

namespace Astrum.Projects.ViewModels.DTO
{
    public class ProjectUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Guid? ProductId { get; set; }
        public List<CustomFieldView>? CustomFields { get; set; }
        public List<MemberRequest>? Members { get; set; }
    }
}
