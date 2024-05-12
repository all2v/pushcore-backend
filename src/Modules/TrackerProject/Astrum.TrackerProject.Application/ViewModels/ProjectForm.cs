using Astrum.Account.Features.Profile;

namespace Astrum.TrackerProject.Application.ViewModels
{
    public class ProjectForm
    {
        public string Id { get; set; }
        public string YoutrackId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LeaderId { get; set; }
        public UserProfileSummary Leader { get; set; }
        public List<IssueForm> Issues { get; set; }
        //public string FromEmail { get; set; }
        //public string ReplyToEmail { get; set; }
        public string IconUrl { get; set; }
        public List<UserProfileSummary> Members { get; set; }
        public int Articles { get; set; }
    }
}
