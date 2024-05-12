using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Astrum.Identity.Application.ViewModels
{
    public class GitLabUserForm
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("email")]
        public string? Email { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("bot")]
        public bool IsBot { get; set; }
        [JsonProperty("external")]
        public bool IsExternal { get; set; }
    }
}
