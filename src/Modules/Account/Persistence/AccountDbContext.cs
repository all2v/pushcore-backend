using System.Reflection.Emit;
using Astrum.Account.Aggregates;
using Astrum.Account.Domain.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Account;

public class AccountDbContext : BaseDbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }

    //To add migration with pmc:
    //Add-Migration Migration -Context AccountDbContext
    //To update db:
    //Update-Database -Context AccountDbContext

    //public DbSet<Astrum.Account.Aggregates.Account> Accounts { get; set; }
    public DbSet<UserProfile> UsersProfiles { get; set; }
    public DbSet<SocialNetworks> SocialNetworks { get; set; }
    public DbSet<RegistrationApplication> RegistrationApplications { get; set; }
    public DbSet<UserAchievement> UsersAchievements { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<AccessTimeline> Timelines { get; set; }
    public DbSet<AccessTimelineInterval> TimelineIntervals { get; set; }
    public DbSet<CustomField> CustomFields { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Account");


        builder.Entity<UserProfile>(b =>
        {
            b.HasOne(up => up.Position)
            .WithMany(p => p.UserProfiles)
            .HasForeignKey(up => up.PositionId);
        });
    }
}