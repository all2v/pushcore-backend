namespace Astrum.Infrastructure.Integrations.YouTrack.Models.TrackerDTO
{
    public class Comment
    {
        public string Text { get; set; }

        public string Url { get; set; }

        public User CreatedBy { get; set; }
    }
}
