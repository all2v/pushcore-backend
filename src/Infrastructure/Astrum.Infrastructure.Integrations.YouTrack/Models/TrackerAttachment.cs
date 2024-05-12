using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Infrastructure.Integrations.YouTrack.Models
{
    public class TrackerAttachment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long? Size { get; set; }
        public string Extension { get; set; }
        public string Charset { get; set; }
        public string MimeType { get; set; }
        public string MetaData { get; set; }
        public string Base64Content { get; set; }
        public string Url { get; set; }
    }
}
