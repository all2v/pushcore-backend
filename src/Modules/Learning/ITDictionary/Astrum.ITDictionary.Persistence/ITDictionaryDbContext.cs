using Astrum.ITDictionary.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.ITDictionary;

public class ITDictionaryDbContext: BaseDbContext
{
    public ITDictionaryDbContext(DbContextOptions<ITDictionaryDbContext> options) : base(options)
    {
    }
    
    //To add migration with pmc:
    //Add-Migration Migration -Context ITDictionaryDbContext
    //To update db:
    //Update-Database -Context ITDictionaryDbContext

    public DbSet<Term> Terms { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Practice> Practices { get; set; }

    public DbSet<TestQuestion> TestQuestions { get; set; }

    public DbSet<QuestionAnswerOption> AnswerOptions { get; set; }

    public DbSet<UserTerm> UserTerms { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("ITDictionary");
        
        var currentAssembly = GetType().Assembly;
        builder.ApplyConfigurationsFromAssembly(currentAssembly);
    }
}