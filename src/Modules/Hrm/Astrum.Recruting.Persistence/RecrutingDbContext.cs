using Astrum.Recruting.Domain.Aggregates;
using Astrum.Recruting.Domain.Aggregates.Board;
using Astrum.Recruting.Domain.Aggregates.Candidate;
using Astrum.Recruting.Domain.Aggregates.Practice;
using Astrum.Recruting.Domain.Aggregates.Vacancy;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Recruting.Persistence;

public class RecrutingDbContext : BaseDbContext
{
    public RecrutingDbContext(DbContextOptions<RecrutingDbContext> options) : base(options)
    {
    }

    public DbSet<KanbanBoard> KanbanBoards { get; set; }
    public DbSet<CandidateLink> CandidateLinks { get; set; }
    public DbSet<CandidateLinkHistory> CandidateLinkHistories { get; set; }
    public DbSet<Stage> Stages { get; set; }
    public DbSet<StageEvent> StageEvents { get; set; }
    public DbSet<StageCard> StageCards { get; set; }
    public DbSet<CandidateResume> CandidateResumes { get; set; }
    public DbSet<HrmUser> HrmUsers { get; set; }
    public DbSet<JobExperience> JobExperiences { get; set; }
    public DbSet<UserComment> UserComments { get; set; }
    public DbSet<PracticeRequest> PracticeRequests { get; set; }
    public DbSet<PracticeSkill> PracticeSkills { get; set; }
    public DbSet<PracticeVacancy> PracticeVacancies { get; set; }
    public DbSet<Responsibility> Responsibilities { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<VacancyArea> VacancyAreas { get; set; }
    public DbSet<VacancyCandidate> VacancyCandidates { get; set; }
    public DbSet<VacancyRole> VacancyRoles { get; set; }
    public DbSet<VacancyType> VacancyTypes { get; set; }
    public DbSet<WorkType> WorkTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Recruting");
    }
}