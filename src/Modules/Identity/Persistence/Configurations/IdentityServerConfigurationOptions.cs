using Duende.IdentityServer.Models;

namespace Astrum.Identity.Configurations;

public class IdentityServerConfigurationOptions
{
    public IReadOnlyCollection<IdentityResource> IdentityResources { get; set; }
    public IReadOnlyCollection<ApiResource> ApiResources { get; set; }
    public IReadOnlyCollection<Client> Clients { get; set; }
    public IReadOnlyCollection<ApiScope> ApiScopes { get; set; }

    public IdentityServerConfigurationOptions Value { get; }
}