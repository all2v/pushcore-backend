using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace Astrum.Identity.Configurations;

public static class IdentityServerConfig
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResources.Phone(),
            new()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            },
            new("color", new[] {"favorite_color"})
        };


    public static IEnumerable<Client> Clients =>
        new[]
        {
            // machine to machine client (from quickstart 1)
            new Client
            {
                Enabled = true,
                ClientId = "AstrumAPI",
                ClientName = "The Astrum API",
                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = {"astrum.api"}
            },
            // interactive ASP.NET Core Web App
            new Client
            {
                Enabled = true,
                ClientId = "api_swagger",
                ClientName = "Swagger UI for Astrum API",
                ClientSecrets = {new Secret("secret".Sha256())},

                AllowedGrantTypes = GrantTypes.Code,

                // where to redirect to after login
                RedirectUris = {"https://localhost:50010/swagger/oauth2-redirect.html"},
                AllowedCorsOrigins = {"https://localhost:50010"},
                // where to redirect to after logout
                PostLogoutRedirectUris = {"https://localhost:50010/swagger"},
                AllowedScopes = new List<string>
                {
                    // IdentityServerConstants.StandardScopes.OpenId,
                    // IdentityServerConstants.StandardScopes.Profile,
                    // "verification",
                    "astrum.api"
                }
            },
            // NextJs client
            new Client
            {
                ClientId = "react_site",
                ClientName = "Astrum Portal",
                ClientSecrets = {new Secret("secret".Sha256())},
                RequireClientSecret = false,
                AllowedGrantTypes = new[]
                {
                    GrantType.AuthorizationCode,
                    GrantType.ResourceOwnerPassword // Add this to allow the client to use ROPC to authorize**
                },

                AllowOfflineAccess = true, // Add this to recieve the refresh token after login

                // where to redirect to after login
                RedirectUris = {"http://localhost:3000/api/auth/callback/sample-identity-server"},
                // where to redirect to after logout
                PostLogoutRedirectUris = {"http://localhost:3000"},
                AllowedCorsOrigins = {"https://localhost:3000"},

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "astrum.api"
                }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("astrum.api", "Astrum API"),
            // invoice API specific scopes
            new("invoice.read", "Reads your invoices."),
            new("invoice.pay", "Pays your invoices."),

            // customer API specific scopes
            new("customer.read", "Reads you customers information."),
            new("customer.write", "Reads you customers information."),
            new("customer.contact", "Allows contacting one of your customers."),

            // shared scope
            new("manage", "Provides administrative access to invoice and customer data.")
        };


    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("astrum", "Astrum API")
            {
                Scopes = {"astrum.api", "invoice.pay", "manage"}
            },
            new("resource1", "Resource #1", new[]
            {
                "resource1.scope1",
                "shared.scope"
            })
        };
}