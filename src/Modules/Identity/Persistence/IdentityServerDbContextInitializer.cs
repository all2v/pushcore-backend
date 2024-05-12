using Astrum.Identity.Configurations;
using Astrum.Infrastructure.Services.DbInitializer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Astrum.Identity;

public class IdentityServerDbContextInitializer : IDbContextInitializer
{
    #region Constructor

    public IdentityServerDbContextInitializer(
        PersistedGrantDbContext persistedGrantDbContext,
        ConfigurationDbContext configurationDbContext,
        IOptions<IdentityServerConfigurationOptions> options,
        ILogger<IdentityServerDbContextInitializer> logger)
    {
        _persistedGrantDbContext = persistedGrantDbContext;
        _configurationDbContext = configurationDbContext;
        _options = options;
        _logger = logger;
    }

    #endregion Constructor

    #region Fields

    private readonly PersistedGrantDbContext _persistedGrantDbContext;
    private readonly ConfigurationDbContext _configurationDbContext;
    private readonly IOptions<IdentityServerConfigurationOptions> _options;
    private readonly ILogger<IdentityServerDbContextInitializer> _logger;

    #endregion Fields

    #region Public Methods

    /// <summary>
    ///     Execute migrations
    /// </summary>
    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("PersistedGrantDbContext migrating is starting");
     await    _persistedGrantDbContext.Database.MigrateAsync(cancellationToken);
        _logger.LogInformation("PersistedGrantDbContext migrating finished");

        _logger.LogInformation("ConfigurationDbContext migrating is starting");
    await    _configurationDbContext.Database.MigrateAsync(cancellationToken);
        _logger.LogInformation("ConfigurationDbContext migrating finished");
        
    }

    public async Task Seed(CancellationToken cancellationToken = default)
    {
        SeedConfigurationDbContext();
    }

    private void SeedConfigurationDbContext()
    {
        _logger.LogInformation("ConfigurationDbContext seeding is starting");

        if (!_configurationDbContext.Clients.Any())
        {
            //TODO to _options.Value.Clients
            foreach (var client in IdentityServerConfig.Clients)
                _configurationDbContext.Clients.Add(client.ToEntity());
            _configurationDbContext.SaveChanges();
        }

        if (!_configurationDbContext.IdentityResources.Any())
        {
            //TODO to _options.Value.IdentityResources
            foreach (var resource in IdentityServerConfig.IdentityResources)
                _configurationDbContext.IdentityResources.Add(resource.ToEntity());
            _configurationDbContext.SaveChanges();
        }

        if (!_configurationDbContext.ApiScopes.Any())
        {
            // TODO to _options.Value.ApiScopes
            foreach (var resource in _options.Value.ApiScopes)
                _configurationDbContext.ApiScopes.Add(resource.ToEntity());

            _configurationDbContext.SaveChanges();
        }

        if (!_configurationDbContext.ApiResources.Any())
        {
            // TODO to _options.Value.ApiResources
            foreach (var resource in IdentityServerConfig.ApiResources)
                _configurationDbContext.ApiResources.Add(resource.ToEntity());
            _configurationDbContext.SaveChanges();
        }

        _logger.LogInformation("ConfigurationDbContext seeding finished");
    }

    #endregion Public Methods
}