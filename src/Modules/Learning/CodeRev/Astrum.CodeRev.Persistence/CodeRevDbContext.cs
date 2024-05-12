using Astrum.CodeRev.Domain.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.CodeRev.Persistence;

public class CodeRevDbContext : BaseDbContext
{
    public CodeRevDbContext(DbContextOptions<CodeRevDbContext> options) : base(options)
    {
    }
    //public DbSet<User> Users { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    //public DbSet<InterviewTask> InterviewTasks { get; set; }
    public DbSet<TestTask> Tasks { get; set; }
    public DbSet<InterviewSolution> InterviewSolutions { get; set; }
    public DbSet<TaskSolution> TaskSolutions { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<ReviewerDraft> ReviewerDrafts { get; set; }
    public DbSet<InterviewLanguage> InterviewLanguages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("CodeRev");

        modelBuilder.Entity<ReviewerDraft>()
            .Property(reviewerDraft => reviewerDraft.Draft)
            .HasColumnType("jsonb");
    }
}