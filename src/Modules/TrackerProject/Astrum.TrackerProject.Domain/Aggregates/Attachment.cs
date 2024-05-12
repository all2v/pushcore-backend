using Astrum.SharedLib.Domain.Entities;

namespace Astrum.TrackerProject.Domain.Aggregates
{
    public class Attachment : AggregateRootBase<string>
    {
        public string? Name { get; set; }
        public long? Size { get; set; }
        public string? Extension { get; set; }
        public string? Charset { get; set; }
        public string? MimeType { get; set; }
        public string? MetaData { get; set; }
        public string? Base64Content { get; set; }
        public string? Url { get; set; }
    }
}
