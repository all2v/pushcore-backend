using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using Astrum.Identity.Models;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.IdentityServer.Domain.Events;
using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.Infrastructure.Services.DbInitializer;
using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Astrum.Identity;

public class IdentityDbContextInitializer : IDbContextInitializer
{
    #region Constructor

    public IdentityDbContextInitializer(
        IdentityDbContext identityContext,
        UserManager<ApplicationUser> userManager,
        ILogger<IdentityDbContextInitializer> logger,
        IMediator mediator,
        IApplicationUserRepository userRepository
)
    {
        _identityContext = identityContext;
        _userManager = userManager;
        _logger = logger;
        _mediator = mediator;
        _userRepository = userRepository;
    }

    #endregion Constructor

    #region IDbInitializerService Members

    /// <summary>
    ///     Execute migrations
    /// </summary>
    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        await _identityContext.Database.MigrateAsync(cancellationToken: cancellationToken);
    }

    public async Task Seed(CancellationToken cancellationToken = default)
    {
        SeedRoles();
        await SeedUsersAsync();
    }

    #endregion

    private void SeedRoles()
    {
        var applicationRoles = _identityContext.Roles;
        if (!applicationRoles.Any())
        {
            applicationRoles.Add(new ApplicationRole
            {
                Name = "Trainee",
                NormalizedName = "TRAINEE"
            });
            applicationRoles.Add(new ApplicationRole
            {
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            });
            applicationRoles.Add(new ApplicationRole
            {
                Name = "Manager",
                NormalizedName = "MANAGER"
            });
            applicationRoles.Add(new ApplicationRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
            applicationRoles.Add(new ApplicationRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            });
            _identityContext.SaveChanges();
        }
    }


    public async Task SeedUsersAsync()
    {
        var superadminExists = (await _userManager.GetUsersInRoleAsync("SuperAdmin")).Any();
        if (!superadminExists)
        {
            var superadmin = new ApplicationUser
            {
                Name = "superadmin",
                UserName = "superadmin",
                Email = "superadmin@email.com",
                EmailConfirmed = true,
                Culture = "rus",
                PhoneNumber = "+8 (880) 555-35-55",
                IsActive = true
            };
            var creationResult = await _userManager.CreateAsync(superadmin, "SuperAdmin");
            if (!creationResult.Succeeded) throw new Exception(creationResult.Errors.First().Description);
            var roleAddingRes = await _userManager.AddToRoleAsync(superadmin, "SuperAdmin");
            if (!roleAddingRes.Succeeded) throw new Exception(roleAddingRes.Errors.First().Description);
            await SeedUserProfiles(superadmin);
            _logger.LogDebug("Superadmin created");
        }
        else
            _logger.LogDebug("Superadmin already exists");
    }

    public async Task SeedUserProfiles(ApplicationUser superadmin)
    {
        var specification = new GetUserByUsernameSpec(superadmin.UserName);
        var user = await _userRepository.FirstOrDefaultAsync(specification);
        var mappedUser = new UserViewModel
        {
            Email = superadmin.UserName,
            Username = superadmin.UserName,
            FirstName = superadmin.UserName,
            LastName = superadmin.UserName,
            Patronymic = superadmin.UserName,
            Id = user.Id
        };
        await _mediator.Publish(new ApplicationUserCreatedEvent(user.Id.ToString()!, 1, mappedUser, superadmin.UserName));
    }
    #region Fields

    private readonly IdentityDbContext _identityContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<IdentityDbContextInitializer> _logger;
    private readonly IMediator _mediator;
    private readonly IApplicationUserRepository _userRepository;

    #endregion Fields
}