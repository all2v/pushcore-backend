using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Astrum.Calendar.Services;

public class AccessorService : IAccessorService
{
    private readonly IConfiguration configuration;
    private readonly IDataStore dataStore = new FileDataStore(GoogleWebAuthorizationBroker.Folder);
    private readonly IWebHostEnvironment _hostingEnvironment;

    public AccessorService(IConfiguration config, IWebHostEnvironment hostingEnvironment)
    {
        configuration = config;
        _hostingEnvironment = hostingEnvironment;
    }

    #region IAccessorService Members

    public async Task<CalendarService> GetAccess(CancellationToken cancellationToken)
    {
        var clientSecrets = new ClientSecrets
        {
            ClientId = configuration.GetValue<string>("Google-Auth:client_id"),
            ClientSecret = configuration.GetValue<string>("Google-Auth:client_secret")
        };
        var scopes = new[] { CalendarService.Scope.Calendar };
        //var credPath = "token.json";
        //var userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, scopes, "user", cancellationToken,
        //        dataStore);
        var clientEmail = configuration.GetValue<string>("Google-Auth:client_email");
        var privateKey = configuration.GetValue<string>("Google-Auth:private_key");
        var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(clientEmail)
        {
            Scopes = new string[] { CalendarService.Scope.Calendar }
        }.FromPrivateKey(privateKey));
        return new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "Calendar API Integration"
        });
    }

    #endregion
}