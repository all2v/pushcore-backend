namespace Astrum.Projects.ViewModels.Requests
{
    public class CustomFieldRequest
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid ProjectId { get; set; }
    }
}
