
namespace Astrum.Projects.ViewModels.Views
{
    public class ProjectView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Guid? ProductId { get; set; }
        public List<CustomFieldView>? CustomFields { get; set; }
        public List<MemberView>? Members { get; set; }
        public bool IsDeletable { get; set; }
    }
}
