namespace Astrum.Projects.ViewModels.Views
{
    public class ProductView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public CustomerView Customer { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string? CoverUrl { get; set; }
        public List<ProjectShortView> Projects { get; set; }
    }
}
