using Newtonsoft.Json;

namespace Astrum.IdentityServer.Domain.ViewModels;

public class KeycloakAuthResponse
{
    [JsonProperty(PropertyName = "state")]
    public string State { get; set; }
    [JsonProperty(PropertyName = "session_state")]
    public string SessionState { get; set; }
    [JsonProperty(PropertyName = "code")]
    public string Code { get; set; }
}