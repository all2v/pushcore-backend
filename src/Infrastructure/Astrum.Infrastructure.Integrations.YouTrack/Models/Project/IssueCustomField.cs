using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.JsonConverter;
using Newtonsoft.Json;

namespace Astrum.Infrastructure.Integrations.YouTrack.Models.Project
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class IssueCustomField
    {
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }
        [JsonProperty(PropertyName = "value")]
        [JsonConverter(typeof(CustomFieldConverter))]
        public List<IssueCustomFieldValue> Value { get; set; }
        //[JsonProperty(PropertyName = "value")]
        //public List<IssueCustomFieldValue> Values { get; set; }
    }
    
    public class IssueCustomFieldValue
    {
        public string? RingId { get; set; }
        public string? LocalizedName { get; set; }
    }
}
