namespace Astrum.Infrastructure.Integrations.YouTrack.Models.Project
{
    public class TrackerProjectTeam
    {
        public TrackerProjectTeam(string id, string name, List<string> members, string projectId)
        {
            Id = id;
            Name = name;
            Members = members;
            ProjectId = projectId;
        }

        public string Id { get; set; }
        public string RingId { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public List<string> Members { get; set; }
    }

}
