using System.Text.Json;
using IdentityModel.Client;

namespace Astrum.Identity.Examples;

public class client
{
    public async Task Main()
    {
        var accessToken = await RequestAccessTokenFromApi();
        if (accessToken == null)
            return;
        await CallApi(accessToken);
    }

    public async Task<string?> RequestAccessTokenFromApi()
    {
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
        if (disco.IsError)
        {
            Console.WriteLine(disco.Error);
            return null;
        }

        // request token
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,

            ClientId = "client",
            ClientSecret = "secret",
            Scope = "AstrumAPI"
        });

        if (tokenResponse.IsError)
        {
            Console.WriteLine(tokenResponse.Error);
            return null;
        }

        Console.WriteLine(tokenResponse.AccessToken);
        return tokenResponse.AccessToken;
    }

    public async Task CallApi(string accessToken)
    {
        // call api
        var apiClient = new HttpClient();
        apiClient.SetBearerToken(accessToken);

        var response = await apiClient.GetAsync("https://localhost:6001/identity");
        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
        {
            var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions {WriteIndented = true}));
        }
    }
}