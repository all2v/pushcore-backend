using Astrum.Application;
using Astrum.Identity.Contracts;
using Astrum.Identity.Models;
using Astrum.Ordering;
using Astrum.SharedLib.Application.Contracts.Infrastructure;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Application.Extensions;
using Astrum.SharedLib.Domain.Enums;
using Astrum.Tests.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Astrum.Tests;

public class TestBase : IDisposable
{
    protected readonly ApplicationDbContext ApplicationContext;
    protected readonly EventStoreDbContext EventStoreContext;
    protected readonly IdentityDbContext IdentityContext;
    protected readonly OrderingDbContext OrderingContext;
    protected readonly IServiceBus ServiceBus;

    protected readonly IServiceProvider ServiceProvider;

    public TestBase()
    {
        var configuration = BuildConfiguration();
        var serviceProvider = BuildServiceProvider(configuration);
        ServiceProvider = serviceProvider;
        OrderingContext = serviceProvider.GetRequiredService<OrderingDbContext>();
        ApplicationContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        IdentityContext = serviceProvider.GetRequiredService<IdentityDbContext>();
        EventStoreContext = serviceProvider.GetRequiredService<EventStoreDbContext>();
        ServiceBus = serviceProvider.GetRequiredService<IServiceBus>();
    }

    private static List<ApplicationUser> PrepareUsers
    {
        get
        {
            var activeUser = new ApplicationUser
            {
                UserName = "activeUser",
                Name = "Active User",
                Id = Guid.NewGuid(),
                Email = "test@test.it",
                IsActive = true
            };

            var inactiveUser = new ApplicationUser
            {
                UserName = "inactiveUser",
                Name = "Inactive User",
                Id = Guid.NewGuid(),
                Email = "test@test.it",
                IsActive = false
            };

            var superAdminUser = new ApplicationUser
            {
                UserName = "superAdminUser",
                Name = "Super Admin",
                Id = Guid.NewGuid(),
                Email = "test@test.it",
                IsActive = true
            };

            superAdminUser.Roles.Add(new ApplicationUserRole
            {
                Role = new ApplicationRole {Name = RolesEnum.SuperAdmin.ToString()}
            });

            var basicUser = new ApplicationUser
            {
                UserName = "basicUser",
                Name = "Super Admin",
                Id = Guid.NewGuid(),
                Email = "test@test.it",
                IsActive = true
            };

            basicUser.Roles.Add(new ApplicationUserRole
            {
                Role = new ApplicationRole {Name = RolesEnum.Trainee.ToString()}
            });

            var users = new List<ApplicationUser>();
            users.Add(activeUser);
            users.Add(inactiveUser);
            users.Add(superAdminUser);
            users.Add(basicUser);
            return users;
        }
    }

    #region IDisposable Members

    public void Dispose()
    {
        ApplicationContext.Database.EnsureDeleted();
        ApplicationContext.Dispose();

        IdentityContext.Database.EnsureDeleted();
        IdentityContext.Dispose();

        EventStoreContext.Database.EnsureDeleted();
        EventStoreContext.Dispose();
    }

    #endregion

    /// <summary>
    ///     Build services for testing
    /// </summary>
    /// <returns></returns>
    private IServiceProvider BuildServiceProvider(IConfiguration configuration)
    {
        IServiceCollection services = new ServiceCollection();
        // services.AddInfrastructureLayer(configuration);
        services.AddApplicationServices();
        services.AddTransient<IAuthenticatedUserService, MockAuthenticatedUserService>();
        services.AddScoped<SignInManager<ApplicationUser>>(_ => InitializeSignInManager());
        services.AddScoped<UserManager<ApplicationUser>>(_ => InitializeUserManager());

        services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("IdentityDb"));
        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
        services.AddDbContext<EventStoreDbContext>(options => options.UseInMemoryDatabase("EventStoreDb"));

        //services.AddSingleton<ILogger<ApplicationUserService>, NullLogger<ApplicationUserService>>();
        services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
        services.AddLogging();

        var localizationService = new Mock<ILocalizationService>();
        services.AddScoped(_ => localizationService.Object);

        return services.BuildServiceProvider();
    }

    private IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();
        configuration["UseInMemoryDatabase"] = "true";
        return configuration;
    }

    /// <summary>
    ///     Seeds dummy data for the application
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public void AddApplicationData(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
    }

    /// <summary>
    ///     Seeds dummy data for identity
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public void AddIdentityData(IdentityDbContext context)
    {
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
    }

    /// <summary>
    ///     Seeds dummy data for the event store.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public void AddEventStoreData(EventStoreDbContext context)
    {
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
    }

    /// <summary>
    ///     Initializes a <see cref="FakeUserManager" />.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static FakeUserManager InitializeUserManager()
    {
        var fakeUserManager = new Mock<FakeUserManager>();
        var users = PrepareUsers;
        var availableRoles = Enum.GetValues(typeof(RolesEnum)).Cast<RolesEnum>();

        fakeUserManager.Setup(x => x.Users)
            .Returns(users.AsQueryable());

        fakeUserManager.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser user, string role) =>
                user.Roles.Any(r => r.Role.Name == role));

        fakeUserManager.Setup(x => x.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
            .Callback<ApplicationUser, IEnumerable<string>>((au, roles) =>
            {
                var userRoles = au.Roles.ToList();
                userRoles.RemoveAll(ur => roles.Any(r => r == ur.Role.Name));
            })
            .ReturnsAsync(IdentityResult.Success);

        fakeUserManager.Setup(x => x.AddToRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
            .Callback<ApplicationUser, IEnumerable<string>>((au, roles) =>
            {
                foreach (var role in roles)
                    au.Roles.Add(new ApplicationUserRole {Role = new ApplicationRole {Name = role}});
            })
            .ReturnsAsync(IdentityResult.Success);

        fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>()))
            .Callback<ApplicationUser>(u => users.Remove(u))
            .ReturnsAsync(IdentityResult.Success);

        fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .Callback<ApplicationUser, string>((u, p) =>
            {
                if (string.IsNullOrWhiteSpace(p))
                    throw new ArgumentNullException();
                if (u == null || string.IsNullOrWhiteSpace(u.UserName))
                    throw new ArgumentNullException();
                users.Add(u);
            }).ReturnsAsync(IdentityResult.Success);

        fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .Callback<ApplicationUser>(u =>
            {
                users.RemoveAll(x => x.UserName == u.UserName);
                users.Add(u);
            }).ReturnsAsync(IdentityResult.Success);

        fakeUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string username) => users.FirstOrDefault(u => u.UserName == username));

        fakeUserManager.Setup(x => x.FindByNameAsync("throwsException")).Throws<Exception>();

        return fakeUserManager.Object;
    }

    /// <summary>
    ///     Initializes a <see cref="FakeSignInManager" />
    /// </summary>
    /// <returns></returns>
    private static FakeSignInManager InitializeSignInManager()
    {
        var signInManager = new Mock<FakeSignInManager>();
        signInManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Success);
        return signInManager.Object;
    }
}