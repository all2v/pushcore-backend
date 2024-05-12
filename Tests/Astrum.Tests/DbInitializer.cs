using Astrum.Application;
using Astrum.Application.Aggregates;
using Astrum.Application.EventStore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Astrum.Tests;

internal class DbInitializer
{
    #region Application

    public static void Initialize(ApplicationDbContext context)
    {
        if (context.ApplicationConfigurations.Any()) return;

        Seed(context);
    }

    private static void Seed(ApplicationDbContext context)
    {
        // Seed additional data according to your application here
        var appConfigs = new List<ApplicationConfiguration>
        {
            new("dummyvalue1",
                "dummydesc1")
            {
                Id = "dummy1"
            },
            new("dummyvalue2",
                "dummydesc2")
            {
                Id = "dummy2"
            },
            new("dummyvalue3",
                "dummydesc3")
            {
                Id = "dummy3"
            },
            new("dummyvalue3",
                "dummydesc3")
            {
                Id = "dummy4"
            }
        };

        context.ApplicationConfigurations.AddRange(appConfigs);
        context.SaveChanges();
    }

    #endregion Application

    #region Identity

    public static void Initialize(IdentityDbContext context)
    {
    }

    private static void Seed(IdentityDbContext context)
    {
    }

    #endregion Identity

    #region EventStore

    public static void Initialize(EventStoreDbContext context)
    {
        if (context.Events.Any()) return;

        Seed(context);
    }

    private static void Seed(EventStoreDbContext context)
    {
    }

    #endregion EventStore
}