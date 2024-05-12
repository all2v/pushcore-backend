using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Astrum.Application.Aggregates;
using Astrum.Application.EventStore;
using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Astrum.Application;

internal class DbContextInitializer : IDbContextInitializer
{
    #region Constructor

    public DbContextInitializer(
        ApplicationDbContext applicationContext,
        EventStoreDbContext eventStoreContext,
        ILogger<DbContextInitializer> logger)
    {
        _applicationContext = applicationContext;
        _eventStoreContext = eventStoreContext;
        _logger = logger;
    }

    #endregion Constructor

    #region Fields

    private const string User = "DbInitializer";
    private readonly ApplicationDbContext _applicationContext;
    private readonly EventStoreDbContext _eventStoreContext;
    private readonly ILogger<DbContextInitializer> _logger;

    #endregion Fields

    #region Public Methods

    /// <summary>
    ///     Execute migrations
    /// </summary>
    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ApplicationContext migrating is starting");
        await _applicationContext.Database.MigrateAsync(cancellationToken);
        _logger.LogInformation("ApplicationContext migrating finished");

        _logger.LogInformation("EventStoreContext migrating is starting");
        await _eventStoreContext.Database.MigrateAsync(cancellationToken);
        _logger.LogInformation("EventStoreContext migrating finished");
    }

    public async Task Seed(CancellationToken cancellationToken = default)
    {
        SeedApplicationDbContext();
    }

    private void SeedApplicationDbContext()
    {
        _logger.LogInformation("ApplicationContext seeding is starting");
        var appConfigurations = _applicationContext.ApplicationConfigurations;
        if (!appConfigurations.Any())
        {
            appConfigurations.Add(new ApplicationConfiguration("50",
                "The number of events after which a snapshot in the event store will be taken")
            {
                Id = "EventStoreSnapshotFrequency",
                CreatedBy = User
            });
            _applicationContext.SaveChanges();
        }

        _logger.LogInformation("ApplicationContext seeding finished");
    }

    #endregion Public Methods
}