namespace Astrum.Infrastructure.Integrations.YouTrack.Models.TrackerDTO.IssueDTO
{
    public class Change
    {
        public string Name { get; set; }

        public string CurrentState { get; set; }

        public string OldState { get; set; }
    }
}
